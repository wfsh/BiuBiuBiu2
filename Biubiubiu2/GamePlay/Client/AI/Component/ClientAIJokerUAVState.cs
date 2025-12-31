using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIJokerUAVState : ComponentBase {
        private float targetValue;
        private float fillValue;
        private AIBehaviourData.FightStateEnum fightState;
        private EntityBase entity = null;
        private Transform[] effects;
        private Transform[] alerts;
        private Renderer renderer;
        private GameObject alert;
        private float timer;

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_JokerUAVState.ID, OnRpcJokerUAVState);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            RemoveProtoCallBack(Proto_AI.Rpc_JokerUAVState.ID, OnRpcJokerUAVState);
        }

        private void OnUpdate(float deltaTime) {
            if (alert != null) {
                timer -= deltaTime;
                if (timer <= 0 && fightState != AIBehaviourData.FightStateEnum.Warning) {
                    alert.SetActive(false);
                    alert = null;
                }
            }
            if (renderer != null && fightState == AIBehaviourData.FightStateEnum.Warning) {
                var offset = renderer.material.mainTextureOffset;
                var offsetY = Mathf.Lerp(offset.y, targetValue, Time.deltaTime);
                renderer.material.mainTextureOffset = new Vector2(offset.x, offsetY);
            }
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            var list = iEntity.GetBodyTranList(GPOData.PartEnum.Object);
            for (int i = 0; i < list.Count; i++) {
                var tran = list[i];
                var name = tran.name;
                if (name.Contains("Effect")) {
                    effects = new Transform[tran.childCount];
                    for (int j = 0; j < tran.childCount; j++) {
                        effects[j] = tran.GetChild(j);
                    }
                } else if (name.Contains("Alert")) {
                    alerts = new Transform[tran.childCount];
                    for (int j = 0; j < tran.childCount; j++) {
                        alerts[j] = tran.GetChild(j);
                    }
                    renderer = alerts[0].GetChild(1).GetComponent<Renderer>();
                }
            }
            if (fightState == AIBehaviourData.FightStateEnum.None) {
                UpdateState(AIBehaviourData.FightStateEnum.Idle);
            }
        }

        private void OnRpcJokerUAVState(INetwork network, IProto_Doc docData) {
            if (entity == null) {
                return;
            }
            var rpcData = (Proto_AI.Rpc_JokerUAVState)docData;
            UpdateState((AIBehaviourData.FightStateEnum)rpcData.state);
            fillValue = rpcData.fillValue / 100f;
            targetValue = Mathf.Lerp(0.45f, 0.1f, fillValue);
        }

        private void UpdateState(AIBehaviourData.FightStateEnum state) {
            if (this.fightState == state) {
                return;
            }
            if (effects == null) {
                return;
            }
            foreach (var effect in effects) {
                effect.gameObject.SetActiveEx(false);
            }
            if (alert != null) {
                alert.SetActiveEx(false);
                alert = null;
            }
            this.fightState = state;
            switch (state) {
                case AIBehaviourData.FightStateEnum.Idle:
                    effects[0].gameObject.SetActive(true);
                    break;
                case AIBehaviourData.FightStateEnum.Alert:
                    effects[1].gameObject.SetActive(true);
                    alert = alerts[2].gameObject;
                    break;
                case AIBehaviourData.FightStateEnum.Warning:
                    effects[1].gameObject.SetActive(true);
                    alert = alerts[0].gameObject;
                    break;
                case AIBehaviourData.FightStateEnum.Fight:
                    effects[2].gameObject.SetActive(true);
                    alert = alerts[1].gameObject;
                    break;
            }
            if (alert != null) {
                timer = 2f;
                alert.SetActive(true);
            }
        }
    }
}