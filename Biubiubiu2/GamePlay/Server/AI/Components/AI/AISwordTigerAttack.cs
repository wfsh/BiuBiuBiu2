using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AISwordTigerAttack : ComponentBase {
        // ------- DragonCar
        private float DragonCarCD = 30f;
        private float InDragonCarTime = 5f;
        private float CountDragonCarCD = 0f;
        private float inDragonCarTime = 0.0f;
        // ------- Bellow
        private float BellowCD = 20f;
        private float InBellowTime = 3f;
        private float CountBellowCD = 0f;
        private float inBellowTime = 0.0f;
        // ------- Attack
        private float AttackCD = 3f;
        private float InAttackTime = 1f;
        private float CountAttackCD = 0f;
        private float inAttackTime = 0.0f;
        // ------ 其他
        private bool isUseSkill = false;
        private bool isAllCD = false;
        private Transform attackBoxTran;
        private IGPO attackTarget;
        private Vector3 fireBallAttackPoint = Vector3.zero;
        private float skillCD = 0.0f;
        private Vector3 dragonCarEndPoint = Vector3.zero;

        protected override void OnAwake() {
            mySystem.Register<SE_Behaviour.Event_InFirstFind>(OnInFirstFindGPOCallBack);
            mySystem.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            mySystem.Register<SE_GPO.GetAttackPoint>(OnGetAttackPointCallBack);
            mySystem.Register<SE_AI_RexKing.Event_Attack>(OnAttackPointCallBack);
            mySystem.Register<SE_AI_RexKing.Event_DragonCar>(OnDragonCarCallBack);
            mySystem.Register<SE_AI_RexKing.Event_Bellow>(OnBellowCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Behaviour.Event_InFirstFind>(OnInFirstFindGPOCallBack);
            mySystem.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            mySystem.Unregister<SE_GPO.GetAttackPoint>(OnGetAttackPointCallBack);
            mySystem.Unregister<SE_AI_RexKing.Event_Attack>(OnAttackPointCallBack);
            mySystem.Unregister<SE_AI_RexKing.Event_DragonCar>(OnDragonCarCallBack);
            mySystem.Unregister<SE_AI_RexKing.Event_Bellow>(OnBellowCallBack);
            fireBallAttackPoint = Vector3.zero;
            attackBoxTran = null;
            RemoveUpdate(OnUpdate);
        }        
        
        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            this.attackTarget = ent.TargetGPO;
        }
        
        private void OnAttackPointCallBack(ISystemMsg body, SE_AI_RexKing.Event_Attack ent) {
            if (isUseSkill || HasTarget() == false) {
                return;
            }
            UseAttack();
        }
        private void OnDragonCarCallBack(ISystemMsg body, SE_AI_RexKing.Event_DragonCar ent) {
            if (isUseSkill || HasTarget() == false) {
                return;
            }
            UseDragonCar();
        }
        private void OnBellowCallBack(ISystemMsg body, SE_AI_RexKing.Event_Bellow ent) {
            if (isUseSkill || HasTarget() == false) {
                return;
            }
            UseBellow();
        }
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (AIEntity)iEntity;
            attackBoxTran = entity.AttackTran;
            AddUpdate(OnUpdate);
        }

        private bool IsAllCD() {
            return CountDragonCarCD > 0f && 
                   CountBellowCD > 0f && CountAttackCD > 0f;
        }
        
        private void OnUpdate(float deltaTime) {
            CheckIsUseSkill();
            if (isAllCD != IsAllCD()) {
                isAllCD = IsAllCD();
                mySystem.Dispatcher(new SE_Behaviour.Event_IsAllSkillCD {
                    IsTrue = isAllCD
                });
            }
        }

        private void CheckIsUseSkill() {
            var useSkill = UseSkill();
            if (isUseSkill != useSkill) {
                isUseSkill = useSkill;
                mySystem.Dispatcher(new SE_Behaviour.Event_IsUseSkill {
                    IsTrue = isUseSkill
                });
                if (useSkill == true) {
                    iGPO.Dispatcher(new SE_Behaviour.Event_StopMove());
                }
            }
        }

        private bool HasTarget() {
            if (this.attackTarget == null || this.attackTarget.IsClear()) {
                return false;
            }
            return true;
        }

        private void OnGetAttackPointCallBack(ISystemMsg body, SE_GPO.GetAttackPoint ent) {
            ent.CallBack(attackBoxTran.position);
        }

        private bool UseSkill() {
            if (InAttack() || InUseBellow() || InUseDragonCar()) {
                return true;
            }
            DownUseBellowCD();
            DownUseDragonCarCD();
            DownAttackCD();
            return false;
        }
        
        private void LookForTarget() {
            var targetPoint = this.attackTarget.GetPoint();
            var point = Vector3.zero;
            point.x = targetPoint.x;
            point.y = iEntity.GetPoint().y;
            point.z = targetPoint.z;
            iEntity.LookAT(point);
        }
        

        // --------------------------  普通攻击 ----------------------------------------
        private bool UseAttack() {
            if (CountAttackCD <= 0) {
                OnAttack();
                inAttackTime = InAttackTime;
                CountAttackCD = AttackCD;
                CheckIsUseSkill();
                return true;
            }
            return false;
        }

        private void OnAttack() {
            LookForTarget();
            mySystem.Dispatcher(new SE_AI.Event_PlayAttackAnim {
                IsTrue = true
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = iGPO, 
                AbilityMData = new AbilityData.PlayAbility_HurtRangeAttack {
                    ConfigId = AbilityConfig.RexKingAttack,
                    In_FirePoint = attackBoxTran.position
                }, 
                CallBack = null,
            });
        }

        private bool InAttack() {
            if (inAttackTime > 0f) {
                inAttackTime -= Time.deltaTime;
                if (inAttackTime <= 0) {
                    mySystem.Dispatcher(new SE_AI.Event_PlayAttackAnim {
                        IsTrue = false
                    });
                } else {
                    return true;
                }
            }
            return false;
        }

        private void DownAttackCD() {
            if (CountAttackCD > 0) {
                CountAttackCD -= Time.deltaTime;
            }
        }

        // --------------------------  吼叫 ----------------------------------------

        private bool UseBellow() {
            if (CountBellowCD <= 0) {
                OnBellowAttack();
                CheckIsUseSkill();
                return true;
            }
            return false;
        }

        private bool InUseBellow() {
            if (inBellowTime > 0f) {
                inBellowTime -= Time.deltaTime;
                if (inBellowTime <= 0) {
                    mySystem.Dispatcher(new SE_AI.Event_PlayBellowAnim {
                        IsTrue = false
                    });
                } else {
                    return true;
                }
            }
            return false;
        }

        private void DownUseBellowCD() {
            if (CountBellowCD > 0) {
                CountBellowCD -= Time.deltaTime;
            }
        }

        private void OnInFirstFindGPOCallBack(ISystemMsg body, SE_Behaviour.Event_InFirstFind ent) {
            if (ent.IsTrue == true) {
                return;
            }
            CountDragonCarCD = 25f;
            CountBellowCD = 15f;
            CountAttackCD = 2f;
            OnBellowAttack();
        }

        private void OnBellowAttack() {
            inBellowTime = InBellowTime;
            CountBellowCD = BellowCD;
            mySystem.Dispatcher(new SE_AI.Event_PlayBellowAnim {
                IsTrue = true
            });
            UpdateRegister.AddInvoke(InvokePlayBellow, 1f);
        }

        private void InvokePlayBellow() {
            if (iEntity == null) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO, 
                MData = AbilityM_BellowAttack.CreateForID(AbilityM_BellowAttack.ID_SwordTigerBellowAttack),
                InData = new AbilityIn_BellowAttack {
                    In_StartPoint = iEntity.GetPoint(),
                },
                OR_CallBack = null,
            });
        }


        // -------------------------- 龙车 ----------------------------------------

        private bool UseDragonCar() {
            if (CountDragonCarCD <= 0) {
                OnDragonCar();
                inDragonCarTime = InDragonCarTime;
                CountDragonCarCD = DragonCarCD;
                CheckIsUseSkill();
                return true;
            }
            return false;
        }

        private bool InUseDragonCar() {
            if (inDragonCarTime > 0f) {
                inDragonCarTime -= Time.deltaTime;
                if (inDragonCarTime <= 0) {
                    mySystem.Dispatcher(new SE_AI.Event_DragonCarAnim_Run {
                        IsTrue = false
                    });
                } else {
                    return true;
                }
            }
            return false;
        }

        private void DownUseDragonCarCD() {
            if (CountDragonCarCD > 0) {
                CountDragonCarCD -= Time.deltaTime;
            }
        }

        private void OnDragonCar() {
            LookForTarget();
            mySystem.Dispatcher(new SE_AI.Event_DragonCarAnim());
            dragonCarEndPoint = attackTarget.GetPoint();
            UpdateRegister.AddInvoke(InvokePlayDragonCar, 2.0f);
        }

        private void InvokePlayDragonCar() {
            if (mySystem == null) {
                return;
            }
            mySystem.Dispatcher(new SE_AI.Event_DragonCarAnim_Run {
                IsTrue = true
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO, 
                MData = AbilityM_RexKingDragonCar.CreateForID(AbilityM_RexKingDragonCar.ID_SwordTigerDragonCar),
                InData = new AbilityIn_RexKingDragonCar {
                    In_StartPoint = iEntity.GetPoint(),
                    In_EndPoint = dragonCarEndPoint
                },
            });
        }
    }
}