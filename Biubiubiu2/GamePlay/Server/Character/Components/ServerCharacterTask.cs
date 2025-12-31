using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterTask : ServerCharacterComponent {
        private bool isSliding = false;
        private bool isMove = false;
        private bool isJump = false;
        private bool isContinueKillReload = false;
        private bool isContinueKillOneShotKillTwo = false;
        private float delayCheckKillTime = -10f;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private Vector3 prePoint = Vector3.zero;
        private float delayCheckMoveTime;
        private int bulletDamageCount;
        private int burnDamageCount;
        private int flashDamageCount;
        private int explosiveDamageCount;
        private int lastBulletDamageWeapon;
        private int lastBurnDamageWeapon;
        private int lastFlashDamageWeapon;
        private int lastExplosiveDamageWeapon;
        private float delayCheckDamageTime = 5;

        private int killWeaponItemId = -1;

        private GPOData.AttributeData attributeData;
        private Dictionary<int, int> shortTimeHurtValue = new Dictionary<int, int>();
        private float checkOneShootKillTime = -10;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_Jump>(OnJump);
            mySystem.Register<SE_GPO.Event_Slide>(OnSlide);
            mySystem.Register<SE_GPO.Event_GetState>(OnGetState);
            mySystem.Register<SE_GPO.Event_GetContinueKillState>(OnGetContinueKillState);
            mySystem.Register<SE_GPO.Event_CheckOneShotKillTwoTime>(OnCheckOneShotKillTwoTime);
            mySystem.Register<SE_GPO.Event_SetHurtTOGpo>(OnSetHuetGPO);
            mySystem.Register<SE_GPO.Event_GunReload>(OnGunReload);
            mySystem.Register<SE_GPO.Event_RoundEnd>(OnRoundEnd);
            mySystem.Register<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            mySystem.Register<SE_GPO.Event_GetGPOShortTimeDamage>(OnGetGPOShortTimeDamage);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<SE_GPO.Event_SetKillWeaponId>(OnSetKillWeaponId);
        }

        protected override void OnStart() {
            base.OnStart();
            mySystem.Dispatcher(new SE_GPO.Event_GetAttributeData {
                CallBack = GetAttributeCallBack
            });
            AddUpdate(OnUpdate);
        }

        public void GetAttributeCallBack(GPOData.AttributeData data) {
            attributeData = data;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_Jump>(OnJump);
            mySystem.Unregister<SE_GPO.Event_Slide>(OnSlide);
            mySystem.Unregister<SE_GPO.Event_GetState>(OnGetState);
            mySystem.Unregister<SE_GPO.Event_GetContinueKillState>(OnGetContinueKillState);
            mySystem.Unregister<SE_GPO.Event_CheckOneShotKillTwoTime>(OnCheckOneShotKillTwoTime);
            mySystem.Unregister<SE_GPO.Event_SetHurtTOGpo>(OnSetHuetGPO);
            mySystem.Unregister<SE_GPO.Event_GunReload>(OnGunReload);
            mySystem.Unregister<SE_GPO.Event_RoundEnd>(OnRoundEnd);
            mySystem.Unregister<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            mySystem.Unregister<SE_GPO.Event_GetGPOShortTimeDamage>(OnGetGPOShortTimeDamage);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_SetKillWeaponId>(OnSetKillWeaponId);
            shortTimeHurtValue.Clear();
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            DelayCheckMoveTime(deltaTime);

            DelayCheckKillTime(deltaTime);

            DelayCheckDamageTime(deltaTime);

            DelayCheckOneShootTime(deltaTime);
        }

        private void DelayCheckMoveTime(float deltaTime) {
            if (delayCheckMoveTime <= 0) {
                delayCheckMoveTime = 0.1f;
                if (prePoint == Vector3.zero) {
                    prePoint = iEntity.GetPoint();
                    return;
                }

                var newPos = iEntity.GetPoint();
                if (!prePoint.Equals(newPos)) {
                    isMove = true;
                } else {
                    isMove = false;
                }

                prePoint = newPos;
            } else {
                delayCheckMoveTime -= deltaTime;
            }
        }

        private void DelayCheckKillTime(float deltaTime) {
            if (delayCheckKillTime > -5) {
                if (delayCheckKillTime > 0) {
                    delayCheckKillTime -= deltaTime;
                } else {
                    delayCheckKillTime = -10f;
                }
            }
        }

        private void DelayCheckDamageTime(float deltaTime) {
            if (delayCheckDamageTime < 0) {
                delayCheckDamageTime = 5f;
                DispatcherDamage();
            } else {
                delayCheckDamageTime -= deltaTime;
            }
        }

        private void DelayCheckOneShootTime(float deltaTime) {
            if (checkOneShootKillTime > -5) {
                if (checkOneShootKillTime > 0) {
                    checkOneShootKillTime -= deltaTime;
                } else {
                    checkOneShootKillTime = -10;
                    shortTimeHurtValue.Clear();
                }
            }
        }

        private void OnSetKillWeaponId(ISystemMsg body, SE_GPO.Event_SetKillWeaponId ent) {
            killWeaponItemId = ent.KillWeaponId;
        }

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (!ent.IsDead) {
                return;
            }

            // killWeaponItemId = -1;
            checkOneShootKillTime = -10;
            shortTimeHurtValue.Clear();
        }

        private void OnGPOHurtCallBack(ISystemMsg body, SE_GPO.Event_GPOHurt ent) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }

            if (iGPO.IsGodMode() || iGPO.IsDead()) {
                return;
            }

            var fireGPO = (ServerGPO)ent.AttackGPO;
            // 这边可以做伤害相关公式，比如 hurt - def  或者穿透啥的
            var downHp = Mathf.Max(1, ent.Hurt);
            if (shortTimeHurtValue.Count < 1) {
                checkOneShootKillTime = 0.05f;
                shortTimeHurtValue.Add(fireGPO.GpoID, downHp);
            } else {
                if (shortTimeHurtValue.ContainsKey(fireGPO.GpoID)) {
                    shortTimeHurtValue[fireGPO.GpoID] += downHp;
                } else {
                    checkOneShootKillTime = 0.05f;
                    shortTimeHurtValue.Clear();
                    shortTimeHurtValue.Add(fireGPO.GpoID, downHp);
                }
            }
        }

        private void OnGetGPOShortTimeDamage(ISystemMsg body, SE_GPO.Event_GetGPOShortTimeDamage ent) {
            if (shortTimeHurtValue.ContainsKey(ent.AttackGPOId)) {
                ent.CallBack.Invoke(shortTimeHurtValue[ent.AttackGPOId]);
            } else {
                ent.CallBack.Invoke(0);
            }
        }

        private void OnRoundEnd(ISystemMsg body, SE_GPO.Event_RoundEnd ent) {
            DispatcherDamage();
        }

        private void OnGunReload(ISystemMsg body, SE_GPO.Event_GunReload ent) {
            isContinueKillReload = true;
        }

        private void OnSetHuetGPO(ISystemMsg body, SE_GPO.Event_SetHurtTOGpo ent) {
            switch (ent.DamageType) {
                case DamageType.Burn:
                    burnDamageCount += ent.HurtValue;
                    lastBurnDamageWeapon = ent.AttackItemId;
                    break;
                case DamageType.Flash:
                    flashDamageCount += ent.HurtValue;
                    lastFlashDamageWeapon = ent.AttackItemId;
                    break;
                case DamageType.Normal:
                    bulletDamageCount += ent.HurtValue;
                    lastBulletDamageWeapon = ent.AttackItemId;
                    break;
                case DamageType.Explosive:
                    explosiveDamageCount += ent.HurtValue;
                    lastExplosiveDamageWeapon = ent.AttackItemId;
                    break;
            }
        }

        private void DispatcherDamage() {
            DispatcherNormalDamage();
            DispatcherBurnDamage();
            DispatcherExplosiveDamage();
            DispatcherFlashDamage();
        }

        private void DispatcherNormalDamage() {
            if (bulletDamageCount > 0) {
                mySystem.Dispatcher(new SE_GPO.Event_TypeDamageCount() {
                    AttackGPO = iGPO,
                    DamageType = DamageType.Normal,
                    Damage = bulletDamageCount,
                    AttackItemId = lastBulletDamageWeapon
                });
                bulletDamageCount = 0;
            }
        }

        private void DispatcherFlashDamage() {
            if (flashDamageCount > 0) {
                mySystem.Dispatcher(new SE_GPO.Event_TypeDamageCount() {
                    AttackGPO = iGPO,
                    DamageType = DamageType.Flash,
                    Damage = flashDamageCount,
                    AttackItemId = lastFlashDamageWeapon
                });
                flashDamageCount = 0;
            }
        }

        private void DispatcherBurnDamage() {
            if (burnDamageCount > 0) {
                mySystem.Dispatcher(new SE_GPO.Event_TypeDamageCount() {
                    AttackGPO = iGPO,
                    DamageType = DamageType.Burn,
                    Damage = burnDamageCount,
                    AttackItemId = lastBurnDamageWeapon
                });
                burnDamageCount = 0;
            }
        }

        private void DispatcherExplosiveDamage() {
            if (explosiveDamageCount > 0) {
                mySystem.Dispatcher(new SE_GPO.Event_TypeDamageCount() {
                    AttackGPO = iGPO,
                    DamageType = DamageType.Explosive,
                    Damage = explosiveDamageCount,
                    AttackItemId = lastExplosiveDamageWeapon
                });
                explosiveDamageCount = 0;
            }
        }

        private void OnGetContinueKillState(ISystemMsg body, SE_GPO.Event_GetContinueKillState ent) {
            ent.CallBack.Invoke(isContinueKillReload, isContinueKillOneShotKillTwo,killWeaponItemId);
            isContinueKillReload = false;
        }

        private void OnCheckOneShotKillTwoTime(ISystemMsg body, SE_GPO.Event_CheckOneShotKillTwoTime ent) {
            if (delayCheckKillTime < -5) {
                isContinueKillOneShotKillTwo = false;
                delayCheckKillTime = 0.1f;
            }

            if (delayCheckKillTime > 0) {
                isContinueKillOneShotKillTwo = true;
            }
        }

        private void OnGetState(ISystemMsg body, SE_GPO.Event_GetState ent) {
            ent.CallBack.Invoke(isJump, isSliding, isMove, jumpType);
        }

        private void OnJump(ISystemMsg body, SE_GPO.Event_Jump ent) {
            jumpType = ent.JumpType;
            switch (ent.JumpType) {
                case CharacterData.JumpType.Jump:
                case CharacterData.JumpType.AirJump:
                    isJump = true;
                    break;
                case CharacterData.JumpType.None:
                case CharacterData.JumpType.Fall:
                    isJump = false;
                    break;
                default:
                    isJump = false;
                    break;
            }
        }

        private void OnSlide(ISystemMsg body, SE_GPO.Event_Slide ent) {
            isSliding = ent.IsSlide;
        }
    }
}