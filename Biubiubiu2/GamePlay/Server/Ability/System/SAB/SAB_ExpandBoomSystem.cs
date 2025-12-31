using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_ExpandBoomSystem : S_Ability_Base {
        public AbilityIn_ExpandBoom useInData;
        public AbilityM_ExpandBoom useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useInData = (AbilityIn_ExpandBoom)InData;
            useMData = (AbilityM_ExpandBoom)MData;
        }

        protected override void OnStart() {
            AddComponents();
            if (useMData.GetConfigId() > 0) {
                RPCAbility(new Proto_AbilityAB_Auto.Rpc_PlayEffect() {
                    configId = ConfigID,
                    rowId = RowId,
                    startPoint = useInData.In_StartPoint,
                    lifeTime = (ushort)Mathf.CeilToInt(useInData.In_LifeTime * 10f)
                });
            }
        }

        protected override void AddComponents() {
            AddComponent<ServerAbilitySync>();
            AddComponent<AbilityExpandBoom>();
            AddComponent<ServerAbilityLifeCycle>(new ServerAbilityLifeCycle.InitData {
                LifeTime = useInData.In_LifeTime,
                EndTimeCallBack = null,
            });
            AddComponent<ServerAbilityHurtGPO>(new ServerAbilityHurtGPO.InitData {
                Power = useInData.In_ATK,
                WeaponItemId = 0,
            });
        }
    }
}
