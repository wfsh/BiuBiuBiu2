using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIBehaviorManager : ManagerBase {
        private BehaviorManager behaviorManager;
        private const float AI_UPDATE_INTERVAL = 1F;
        private float updateInterval = 0f;
        private bool isEnabled;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Behaviour.Event_StartBehavior>(OnStartBehaviorCallBack);
            MsgRegister.Register<SM_Behaviour.Event_DisableBehavior>(OnDisableBehaviorCallBack);
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            isEnabled = true;
            AddAIBehaviorManager();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Behaviour.Event_StartBehavior>(OnStartBehaviorCallBack);
            MsgRegister.Unregister<SM_Behaviour.Event_DisableBehavior>(OnDisableBehaviorCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            if (behaviorManager != null) {
                GameObject.Destroy(behaviorManager.gameObject);
                behaviorManager = null;
            }
        }

        private void AddAIBehaviorManager() {
            var gameObject = new GameObject("AIBehaviorManager");
            behaviorManager = gameObject.AddComponent<BehaviorManager>();
            behaviorManager.UpdateInterval = UpdateIntervalType.Manual;
        }

        private void OnStartBehaviorCallBack(SM_Behaviour.Event_StartBehavior ent) {
            behaviorManager.EnableBehavior(ent.UseBehavior);
        }

        private void OnDisableBehaviorCallBack(SM_Behaviour.Event_DisableBehavior ent) {
            behaviorManager.DisableBehavior(ent.UseBehavior);
        }

        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            isEnabled = ent.isEnabled;
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            if (!isEnabled) {
                return;
            }
            if (behaviorManager == null) {
                return;
            }
            if (updateInterval > 0) {
                updateInterval -= Time.deltaTime;
                return;
            }
            updateInterval = AI_UPDATE_INTERVAL;
            PerfAnalyzerAgent.BeginSample("behaviorManager.Tick()");
            behaviorManager.Tick();
            PerfAnalyzerAgent.EndSample("behaviorManager.Tick()");
        }
    }
}