using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Grpc;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIAttribute : ServerGPOAttribute {
        public struct InitData : SystemBase.IComponentInitData {
            public int MaxHp;
            public int ATK;
            public float AttackRange;
        }
        public float toRoleHitRate = 100f; // 命中率
        public float toMonsterHitRate = 100f; // 命中率
        public S_AI_Base AISystem;
        public AiConfig aiConfig;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_GetHitRatio>(OnGetHitRatioCallBack);
            mySystem.Register<SE_GPO.Event_SetAIConfig>(OnSetAIConfigCallBack);
            mySystem.Register<SE_GPO.Event_GetAISlideMoveActionRatioTime>(OnGetAISlideMoveActionRatioTimeCallBack);
            mySystem.Register<SE_GPO.Event_GetAIJumpActionRatioTime>(OnGetAIJumpActionRatioTimeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_GetHitRatio>(OnGetHitRatioCallBack);
            mySystem.Unregister<SE_GPO.Event_SetAIConfig>(OnSetAIConfigCallBack);
            mySystem.Unregister<SE_GPO.Event_GetAISlideMoveActionRatioTime>(OnGetAISlideMoveActionRatioTimeCallBack);
            mySystem.Unregister<SE_GPO.Event_GetAIJumpActionRatioTime>(OnGetAIJumpActionRatioTimeCallBack);
            attributeData = null;
        }

        protected override GPOData.AttributeData CreateAttribute() {
            var initData = (InitData)initDataBase;
            var monsterData = new GPOData.AttributeData();
            AISystem = (S_AI_Base)mySystem;
            if (AISystem.AttributeData != null) {
                monsterData = AISystem.AttributeData;
                return monsterData;
            }
            monsterData.Level = 1;
            if (WarData.TestMaxHpInput > 0) {
                monsterData.maxHp = WarData.TestMaxHpInput;
            } else {
                monsterData.maxHp = initData.MaxHp;
            }
            monsterData.ATK = initData.ATK;
            monsterData.AttackRange = initData.AttackRange;
            monsterData.nowHp = monsterData.maxHp;
            return monsterData;
        }

        private void OnSetAIConfigCallBack(ISystemMsg body, SE_GPO.Event_SetAIConfig ent) {
            aiConfig = ent.Config;
            toRoleHitRate = aiConfig.WeaponHitRate;
            toMonsterHitRate = aiConfig.SuperWeaponHitRate;
            iGPO.Dispatcher(new SE_GPO.Event_SetAISlideMoveActionRatioTime {
                SlideMinIntervalTime = aiConfig.SlideMinIntervalTime,
                SlideMaxIntervalTime = aiConfig.SlideMaxIntervalTime
            });
            iGPO.Dispatcher(new SE_GPO.Event_SetAIJumpActionRatioTime {
                JumpMinIntervalTime = aiConfig.JumpMinIntervalTime, JumpMaxIntervalTime = aiConfig.JumpMaxIntervalTime
            });
        }

        private void OnGetAISlideMoveActionRatioTimeCallBack(ISystemMsg body, SE_GPO.Event_GetAISlideMoveActionRatioTime ent) {
            if (aiConfig != null) {
                ent.CallBack(aiConfig.SlideMinIntervalTime, aiConfig.SlideMaxIntervalTime);
            }
        }

        private void OnGetAIJumpActionRatioTimeCallBack(ISystemMsg body, SE_GPO.Event_GetAIJumpActionRatioTime ent) {
            if (aiConfig != null) {
                ent.CallBack(aiConfig.JumpMinIntervalTime, aiConfig.JumpMaxIntervalTime);
            }
        }
        
        virtual protected void OnGetHitRatioCallBack(ISystemMsg body, SE_GPO.Event_GetHitRatio ent) {
            if (ent.HitGpoType == GPOData.GPOType.Role || ent.HitGpoType == GPOData.GPOType.RoleAI) {
                ent.CallBack(toRoleHitRate);
                return;
            }
            ent.CallBack(toMonsterHitRate);
        }

        protected override int ATK() {
            if (useWeapon != null) {
                var atk = 0;
                useWeapon.Dispatcher(new SE_Weapon.Event_GetATK {
                    CallBack = value => {
                        atk = value;
                    }
                });
                return atk;
            }
            return attributeData.ATK;
        }

        protected override void UpdateSpeed() {
            var useSpeed = baseSpeed * (1 + ability_SpeedRatio);
            attributeData.Speed = useSpeed;
            mySystem.Dispatcher(new SE_GPO.Event_UpdateSpeed() {
                Speed = useSpeed
            });
        }
    }
}