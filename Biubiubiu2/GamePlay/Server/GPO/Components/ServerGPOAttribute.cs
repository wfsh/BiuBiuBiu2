using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOAttribute : ServerNetworkComponentBase {
        protected float randomDiffusionReduction;
        protected float AddEffectFireDistanceRate = 0f;
        protected S_Weapon_Base useWeapon;
        protected GPOData.AttributeData attributeData;
        protected int randomATK;
        protected float ability_SpeedRatio;
        protected float ability_MaxHpRatio;
        protected Dictionary<DamageType,float> ability_ATKRatio = new Dictionary<DamageType,float>();
        private bool isShortcutToolLockBlood = false;
        protected S_GPO_Base gpoSystem;
        protected float baseSpeed = 0f;
        protected int baseMaxHp = 0;
        protected int level = 1;

        protected override void OnAwake() {
            gpoSystem = (S_GPO_Base)mySystem;
            mySystem.Register<SE_GPO.Event_GetATK>(OnGetATKCallBack);
            mySystem.Register<SE_GPO.Event_GetATK>(OnGetATKCallBack);
            mySystem.Register<SE_GPO.Event_GetRandomATK>(OnGetRandomATKCallBack);
            mySystem.Register<SE_GPO.Event_GetAttackRange>(OnGetAttackRangeCallBack);
            mySystem.Register<SE_GPO.Event_GetAddEffectFireDistanceRate>(OnGetEffectFireDistanceCallBack);
            mySystem.Register<SE_GPO.Event_GetRandomDiffusionReduction>(OnGetRandomDiffusionReductionCallBack);
            mySystem.Register<SE_GPO.Event_DownHP>(OnDownLifeCallBack);
            mySystem.Register<SE_GPO.Event_UpHP>(OnUpLifeCallBack);
            mySystem.Register<SE_GPO.Event_GetAttributeData>(OnGetAttributeDataCallBack);
            mySystem.Register<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Register<SE_GPO.Event_GetHPInfo>(OnGetHp);
            mySystem.Register<SE_GPO.Event_SetLevel>(OnSetLevelCallBack);
            mySystem.Register<SE_GPO.Event_SetMaxHP>(OnSetMaxHpCallBack);
            mySystem.Register<SE_GPO.Event_SetHP>(OnSetHpCallBack);
            mySystem.Register<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            mySystem.Register<SE_GPO.Event_GetHurtValueForDamageType>(OnGetHurtValueForDamageTypeCallBack);
            MsgRegister.Register<SM_ShortcutTool.Event_CharacterLockBlood>(OnShortcutToolLockBloodCallBack);
            attributeData = CreateAttribute();
            OnCreateAttribute(attributeData, gpoSystem.MData);
            gpoSystem.SetAttributeData(attributeData);
            baseSpeed = attributeData.Speed;
            baseMaxHp = attributeData.maxHp;
        }
        
        virtual protected GPOData.AttributeData CreateAttribute() {
            randomATK = 0;
            randomDiffusionReduction = 0;
            var data = new GPOData.AttributeData();
            data.Level = 1;
            if (WarData.TestMaxHpInput > 0) {
                data.maxHp = WarData.TestMaxHpInput;
            } else {
                data.maxHp = 1000;
            }
            data.nowHp = data.maxHp;
            data.ATK = 0;
            data.AttackRange = 1f;
            ability_ATKRatio.Add(DamageType.Normal,0);
            ability_ATKRatio.Add(DamageType.Explosive,0);
            ability_ATKRatio.Add(DamageType.Burn,0);
            ability_ATKRatio.Add(DamageType.Flash,0);
            return data;
        }

        virtual protected void OnCreateAttribute(GPOData.AttributeData data, IGPOM gpoMData) {
        }

        protected override void OnStart() {
            base.OnStart();
            SetLevel(attributeData.Level);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_GetATK>(OnGetATKCallBack);
            mySystem.Unregister<SE_GPO.Event_GetAttackRange>(OnGetAttackRangeCallBack);
            mySystem.Unregister<SE_GPO.Event_GetRandomATK>(OnGetRandomATKCallBack);
            mySystem.Unregister<SE_GPO.Event_GetRandomDiffusionReduction>(OnGetRandomDiffusionReductionCallBack);
            mySystem.Unregister<SE_GPO.Event_DownHP>(OnDownLifeCallBack);
            mySystem.Unregister<SE_GPO.Event_UpHP>(OnUpLifeCallBack);
            mySystem.Unregister<SE_GPO.Event_GetAttributeData>(OnGetAttributeDataCallBack);
            mySystem.Unregister<SE_GPO.UseWeapon>(OnUseWeaponCallBack);
            mySystem.Unregister<SE_GPO.Event_GetAddEffectFireDistanceRate>(OnGetEffectFireDistanceCallBack);
            mySystem.Unregister<SE_GPO.Event_GetHPInfo>(OnGetHp);
            mySystem.Unregister<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            mySystem.Unregister<SE_GPO.Event_SetLevel>(OnSetLevelCallBack);
            mySystem.Unregister<SE_GPO.Event_SetMaxHP>(OnSetMaxHpCallBack);
            mySystem.Unregister<SE_GPO.Event_SetHP>(OnSetHpCallBack);
            mySystem.Unregister<SE_GPO.Event_GetHurtValueForDamageType>(OnGetHurtValueForDamageTypeCallBack);
            MsgRegister.Unregister<SM_ShortcutTool.Event_CharacterLockBlood>(OnShortcutToolLockBloodCallBack);
        }
        
        private void OnSetLevelCallBack(ISystemMsg body, SE_GPO.Event_SetLevel ent) {
            SetLevel(ent.Level);
        }

        private void SetLevel(int level) {
            this.level = level;
            ChangeATK();
            ChangeMaxHp();
            ChangeHP();
        }
        
        private void OnShortcutToolLockBloodCallBack(SM_ShortcutTool.Event_CharacterLockBlood ent) {
            if (ent.GpoId > 0 && ent.GpoId != GpoID) {
                return;
            }
            isShortcutToolLockBlood = ent.IsLocked;
        }

        private void OnGetHp(ISystemMsg body, SE_GPO.Event_GetHPInfo ent) {
            ent.CallBack.Invoke(attributeData.nowHp,attributeData.maxHp);
        }

        private void OnGetAttackRangeCallBack(ISystemMsg body, SE_GPO.Event_GetAttackRange ent) {
            if (useWeapon == null) {
                ent.CallBack.Invoke(attributeData.AttackRange);
            } else {
                var range = useWeapon.GetData().AttackRange;
                useWeapon.Dispatcher(new SE_Weapon.Event_GetAttackRange {
                    CallBack = value => {
                        range = value;
                    } 
                });
                ent.CallBack.Invoke(range);
            }
        }
        
        private void OnUseWeaponCallBack(ISystemMsg body, SE_GPO.UseWeapon ent) {
            useWeapon = (S_Weapon_Base)ent.Weapon;
            if (useWeapon == null) {
                randomATK = 0;
                randomDiffusionReduction = 0;
                attributeData.ATK = 0;
            } else {
                useWeapon.Dispatcher(new SE_Weapon.Event_GetATK {
                    CallBack = value => {
                        attributeData.ATK = value;
                    } 
                });
                useWeapon.Dispatcher(new SE_Weapon.Event_GetAddEffectFireDistanceRate {
                    CallBack = value => {
                        AddEffectFireDistanceRate = value;
                    } 
                });
                useWeapon.Dispatcher(new SE_Weapon.Event_GetRandomATK {
                    CallBack = value => {
                        randomATK = value;
                    }
                });
                useWeapon.Dispatcher(new SE_Weapon.Event_GetRandomDiffusionReduction {
                    CallBack = value => {
                        randomDiffusionReduction = value;
                    }
                });
                if (attributeData.ATK <= 0) {
                    //Debug.LogError($"{iGPO.GetName()} ATK <= 0 ");
                }  
            }
            ChangeATK();
        }

        private void OnGetAttributeDataCallBack(ISystemMsg body, SE_GPO.Event_GetAttributeData ent) {
            ent.CallBack(attributeData);
        }
        
        protected void UpdateAttribute() {
            mySystem.Dispatcher(new SE_GPO.Event_UpdateAttribute {
                Data = attributeData
            });
        }
// --------------------------- AddEffectFireDistanceRate ---------------------------------------------------------------
        private void OnGetEffectFireDistanceCallBack(ISystemMsg body, SE_GPO.Event_GetAddEffectFireDistanceRate ent) {
            ent.CallBack.Invoke(AddEffectFireDistanceRate);
        }

// --------------------------- ATK ---------------------------------------------------------------
        virtual protected void ChangeATK() {
            mySystem.Dispatcher(new SE_GPO.Event_ATKChange {
                ATK = ATK()
            });
            UpdateAttribute();
        }

        private void OnGetATKCallBack(ISystemMsg body, SE_GPO.Event_GetATK ent) {
            ent.CallBack.Invoke(ATK());
        }

        private void OnGetRandomATKCallBack(ISystemMsg body, SE_GPO.Event_GetRandomATK ent) {
            ent.CallBack.Invoke(randomATK);
        }

        private void OnGetHurtValueForDamageTypeCallBack(ISystemMsg body, SE_GPO.Event_GetHurtValueForDamageType ent) {
            var ratio = 0f;
            ability_ATKRatio.TryGetValue(ent.Type, out ratio);
            var hurtValue = (int)(ent.DamageValue * (1 + ratio));
            ent.CallBack.Invoke(hurtValue);
        }

        private void OnGetRandomDiffusionReductionCallBack(ISystemMsg body, SE_GPO.Event_GetRandomDiffusionReduction ent) {
            ent.CallBack.Invoke(randomDiffusionReduction);
        }
        virtual protected int ATK() {
            return attributeData.ATK;
        }

// --------------------------- HP ---------------------------------------------------------------
        public void OnDownLifeCallBack(ISystemMsg body, SE_GPO.Event_DownHP ent) {
            if (isShortcutToolLockBlood || WarData.TestIsLocalSelfHp) {
                return;
            }
            var lastHp = this.attributeData.nowHp;
            attributeData.nowHp -= ent.DownHp;
            if (attributeData.nowHp <= 0) {
                attributeData.nowHp = 0;
            }
            Dispatcher(new SE_Behaviour.Event_HateAttackTarget() {
                AttackGPO = ent.AttackGPO
            });
            ChangeHP();
            mySystem.Dispatcher(new SE_GPO.Event_AfterDownHP() {
                AttackGPO = ent.AttackGPO,
                AttackItemId = ent.AttackItemId,
                DownHp = lastHp - this.attributeData.nowHp,
                NowHp = this.attributeData.nowHp,
            });
            MsgRegister.Dispatcher(new SM_GPO.Event_AfterDownHP() {
                AttackGPO = ent.AttackGPO,
                TargetGPO = iGPO,
                ActualDownHp = lastHp - this.attributeData.nowHp,
                IsHead = ent.IsHead,
                AttackItemId = ent.AttackItemId,
            });
            Rpc(new Proto_GPO.Rpc_AfterDownHP {
                attackerGPOId = ent.AttackGPO != null ? ent.AttackGPO.GetGpoID() : 0,
                DownHp = ent.DownHp,
                isHead = ent.IsHead,
            });
        }

        public void OnUpLifeCallBack(ISystemMsg body, SE_GPO.Event_UpHP ent) {
            UpLife(ent.UpHp);
        }

        public void UpLife(int upHp) {
            attributeData.nowHp += upHp;
            if (attributeData.nowHp >= attributeData.maxHp) {
                attributeData.nowHp = attributeData.maxHp;
            }
            OnUpHp(upHp);
            ChangeHP();
        }   

        public void OnSetHpCallBack(ISystemMsg body, SE_GPO.Event_SetHP ent) {
            attributeData.nowHp = ent.Hp;
            if (attributeData.nowHp >= attributeData.maxHp) {
                attributeData.nowHp = attributeData.maxHp;
            }
            ChangeHP();
        }  

        public void OnSetMaxHpCallBack(ISystemMsg body, SE_GPO.Event_SetMaxHP ent) {
            baseMaxHp = ent.MaxHp;
            if (ent.IsSyncSetHp == true) {
                attributeData.nowHp = ent.MaxHp;
            }
            ChangeMaxHp();
        }     
             
        virtual protected void OnUpHp(int upHp) {
        }

        virtual protected void ChangeMaxHp() {
            var maxHp = baseMaxHp * (1+ ability_MaxHpRatio);
            attributeData.maxHp = (int)maxHp;
            mySystem.Dispatcher(new SE_GPO.Event_MaxHPChange {
                MaxHp = attributeData.maxHp
            });
            Rpc(new Proto_GPO.Rpc_ChangeMaxHP {
                nowMaxHP = this.attributeData.maxHp,
            });
            if (attributeData.nowHp >= attributeData.maxHp) {
                attributeData.nowHp = attributeData.maxHp;
                ChangeHP();
            }
            UpdateAttribute();
        }

        virtual protected void ChangeHP() {
            Rpc(new Proto_GPO.Rpc_ChangeHP {
                nowHP = this.attributeData.nowHp,
            });
            mySystem.Dispatcher(new SE_GPO.Event_HPChange {
                HP = attributeData.nowHp
            });
            UpdateAttribute();
        }
        virtual protected void OnUpdateEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_UpdateEffect env) {
            switch (env.Effect) {
                case AbilityEffectData.Effect.GpoMoveSpeedRate:
                    ability_SpeedRatio = env.Value;
                    UpdateSpeed();
                    break;
                case AbilityEffectData.Effect.GpoMaxHpRate:
                    ability_MaxHpRatio = env.Value;
                    ChangeMaxHp();
                    break;
                case AbilityEffectData.Effect.GpoHurtValueRate:
                    ability_ATKRatio[DamageType.DamageTypeNull] = env.Value;
                    break;
            }
        }
        virtual protected void UpdateSpeed() {
            var useSpeed = baseSpeed * (1 + ability_SpeedRatio);
            attributeData.Speed = useSpeed;
            mySystem.Dispatcher(new SE_GPO.Event_UpdateSpeed() {
                Speed = useSpeed
            });
        }
    }
}