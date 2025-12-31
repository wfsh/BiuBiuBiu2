using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityDragonDelayBlastSpawner : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_DragonDelayBlastSpawner Param;
        }
        private SAB_DragonDelayBlastSpawnerSystem abSystem;
        private AbilityM_DragonDelayBlastSpawner useMData;
        private AbilityIn_DragonDelayBlastSpawner useInData;
        private float timer;
        private int index;
        private int attackNum;
        private bool isEndAnim;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_DragonDelayBlastSpawnerSystem)mySystem;
            var initData = (InitData)initDataBase;
            useMData = initData.Param;
            useInData = (AbilityIn_DragonDelayBlastSpawner)abSystem.InData;
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            abSystem = null;
            useMData = null;
        }

        protected override void OnStart() {
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = useMData.GetTypeID(),
            });
            #endregion
            attackNum = Mathf.Max(1, useMData.M_AttackNum);
            index = 0;
            timer = 0;
            isEndAnim = false;
            abSystem.FireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DelayBlastStartAnim {});
            AddUpdate(OnUpdate);
        }
        
        private void OnUpdate(float deltaTime) {
            if (abSystem.FireGPO == null || abSystem.FireGPO.IsClear()) {
                return;
            }

            timer += deltaTime;
            if (!isEndAnim && timer > 2.667f) {
                isEndAnim = true;
                InvokeAnimEnd();
            }
            
            if (index < attackNum && timer >= index * useMData.M_CreateInterval) {
                var rotationOffset = 360f / attackNum;
                var startRotation = Random.value * rotationOffset;
                InvokeCreateDelayBlast(startRotation + rotationOffset * index);
                index++;
            }
        }

        private void InvokeCreateDelayBlast(float eulerAngleY) {
            if (abSystem == null || abSystem.FireGPO == null || abSystem.FireGPO.IsClear() || abSystem.FireGPO.IsDead()) {
                return;
            }
            
            var randomPos = (Quaternion.Euler(0,eulerAngleY,0)) * new Vector3(0f,0f,Random.Range(useMData.M_MinDistance,useMData.M_MaxDistance));
            var createCenter = new Vector3(0, useMData.M_GroundY, 0);
            abSystem.FireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData {
                CallBack = (Vector3 center, float radius, float endTime, bool blockDamage) => {
                    createCenter = center;
                    createCenter.y = useMData.M_GroundY;
                }
            });
            
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = abSystem.FireGPO,
                AbilityMData = new AbilityData.PlayAbility_AuroraDragonDelayBlast() {
                    ConfigId = AbilityConfig.AuroraDragonDelayBlast,
                    In_StartPos = createCenter + new Vector3(0,0.1f,0) + randomPos,
                    IsAncientBoss = useInData.In_MonsterSign == GPOM_AuroraDragonSet.Sign_AncientDragon,
                    In_Param = useMData
                },
                CallBack = null,
            });
        }

        private void InvokeAnimEnd() {
            if (abSystem == null || abSystem.FireGPO == null || abSystem.FireGPO.IsClear()) {
                return;
            }
            abSystem.FireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_DelayBlastEndAnim{});
        }
    }
}