using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerGPOAbilityEffect : ServerNetworkComponentBase {
        const float TOLERANCE = 0.01f; // 设置一个合理的误差范围
        private Dictionary<AbilityEffectData.Effect, List<IGPOAbilityEffectData>> effectListDatas = 
            new Dictionary<AbilityEffectData.Effect, List<IGPOAbilityEffectData>>();
        private Dictionary<AbilityEffectData.Effect, float> effectData = new Dictionary<AbilityEffectData.Effect, float>();
        private List<AbilityEffectData.Effect> keys = new List<AbilityEffectData.Effect>();
        private float effectCheckDeltaTime = 0.0f;

        protected override void OnAwake() {
            mySystem.Register<SE_AbilityEffect.Event_AddEffect>(OnAddEffectCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_AbilityEffect.Event_AddEffect>(OnAddEffectCallBack);
            RemoveUpdate(OnUpdate);
            RemoveAllEffect();
            effectData.Clear();
        }

        protected override void Sync(List<INetworkCharacter> networks) {
            foreach (var key in effectData.Keys) {
                var value = effectData[key];
                TargetRpcList(networks, new Proto_Ability.TargetRpc_AbilityEffect {
                    abilityEffect = (int)key, value = value,
                });
            }
        }

        private void RemoveAllEffect() {
            foreach (var key in keys) {
                var list = effectListDatas[key];
                for (int i = 0; i < list.Count; i++) {
                    var effectData = (EffectData)list[i];
                    effectData.Remove();
                }
                list.Clear();
            }
            keys.Clear();
            effectListDatas.Clear();
        }

        private void OnUpdate(float delta) {
            UpdateEffectData();
        }

        private void UpdateEffectData() {
            if (effectCheckDeltaTime > 0) {
                effectCheckDeltaTime -= Time.deltaTime;
                return;
            }
            effectCheckDeltaTime = 0.2f;
            for (int i = keys.Count - 1; i >= 0; i--) {
                var key = keys[i];
                var list = effectListDatas[key];
                var value = 0f;
                CheckRemoveForList(list);
                switch (key) {
                    case AbilityEffectData.Effect.UpdraftPoint:
                    case AbilityEffectData.Effect.GpoMoveSpeedRate:
                    case AbilityEffectData.Effect.GpoReloadRate:
                    case AbilityEffectData.Effect.GpoShootIntervalRate:
                    case AbilityEffectData.Effect.GpoHurtValueRate:
                    case AbilityEffectData.Effect.GpoMaxHpRate:
                        value = GetMaxValue(list);
                        break;
                }
                SetEffectData(key, value);
                if (list.Count <= 0) {
                    keys.RemoveAt(i);
                    effectListDatas.Remove(key);
                }
            }
        }

        private void SetEffectData(AbilityEffectData.Effect effect, float value) {
            var oldValue = 0f;
            if (effectData.TryGetValue(effect, out oldValue) == false) {
                effectData.Add(effect, value);
            }
            if (Mathf.Abs(oldValue - value) > TOLERANCE) {
                effectData[effect] = value;
                mySystem.Dispatcher(new SE_AbilityEffect.Event_UpdateEffect() {
                    Effect = effect, Value = value,
                });
                Rpc(new Proto_Ability.Rpc_AbilityEffect {
                    abilityEffect = (int)effect, value = value,
                });
            }
        }

        private void OnAddEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_AddEffect ent) {
            var data = AddEffect(ent.Effect, ent.Value);
            if (ent.CallBack != null) {
                ent.CallBack.Invoke(data);
            }
        }

        private EffectData AddEffect(AbilityEffectData.Effect effect, float value) {
            List<IGPOAbilityEffectData> list;
            if (effectListDatas.TryGetValue(effect, out list) == false) {
                list = new List<IGPOAbilityEffectData>();
                effectListDatas.Add(effect, list);
                keys.Add(effect);
            }
            var effectData = new EffectData();
            effectData.Init(effect, value, iGPO);
            list.Add(effectData);
            return effectData;
        }

        // 计算移动速度效果
        private void CheckRemoveForList(List<IGPOAbilityEffectData> list) {
            for (int i = 0; i < list.Count; i++) {
                var effect = (EffectData)list[i];
                if (effect.IsRemove()) {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        ///  对应效果的计算方式
        /// </summary>
        private float GetMaxValue(List<IGPOAbilityEffectData> list) {
            var value = 0f;
            for (int i = 0; i < list.Count; i++) {
                var saveValue = list[i].GetValue();
                if (i == 0 || value < saveValue) {
                    value = saveValue;
                }
            }
            return value;
        }
    }
}