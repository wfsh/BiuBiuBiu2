using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIBehaviour : ComponentBase {
         public struct InitData : SystemBase.IComponentInitData {
            public string AIBehaviour;
            public string PetAIBehaviour;
        }
        private Behavior behavior;
        private S_AI_Base aiSystem;
        private GPOData.GPOType driveGpoType = GPOData.GPOType.NULL;
        private bool isLoadingBehavior = false;
        private bool isLoadBehavior = false;
        private bool isEnableBehavior = false;
        private float invokeTime = 0.0f;
        private List<IGPO> gpoList;
        private List<ActionComponent> actionComponents = null;
        private List<ConditionComponent> conditionComponents = null;
        private string aiBehaviour;
        private string petAIBehaviour;
        private IGPO maxHateTargetGPO = null;
        private bool enabledBehavior = true;
        private int enabledDistance = 0;
        
        protected override void OnAwake() {
            aiSystem = (S_AI_Base)mySystem;
            mySystem.Register<SE_AI.Event_DriveState>(OnDriveIngCallBack);
            mySystem.Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            var initData = (InitData)initDataBase;
            aiBehaviour = initData.AIBehaviour;
            petAIBehaviour = initData.PetAIBehaviour;
            enabledDistance = NetworkData.GetBehaviourSyncDistance();
        }

        protected override void OnStart() {
            base.OnStart();
            if (enabledBehavior == false) {
                return;
            }
            MsgRegister.Dispatcher(new SM_GPO.GetGPOListForGpoType {
                GpoType = GPOData.GPOType.Role,
                CallBack = OnGetGPOListCallBack
            });
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            mySystem.Unregister<SE_AI.Event_DriveState>(OnDriveIngCallBack);
            EnableBehavior(false);
            RemoveUpdate(OnUpdate);
            behavior = null;
            actionComponents = null;
            conditionComponents = null;
        }
        
        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            maxHateTargetGPO = ent.TargetGPO;
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            this.gpoList = list;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entityBase = (EntityBase)iEntity;
            behavior = entityBase.gameObject.GetComponent<Behavior>();
            InitBehavior();
        }

        public void OnDriveIngCallBack(ISystemMsg body, SE_AI.Event_DriveState ent) {
            driveGpoType = ent.DriveGpoType;
            InitBehavior();
        }

        private void InitBehavior() {
            if (isLoadingBehavior == true || behavior == null) {
                return;
            }
            isLoadingBehavior = true;
            var hasMaster = aiSystem.MasterGPO != null;
            var behaviorSign = hasMaster ? petAIBehaviour : aiBehaviour;
            if (string.IsNullOrEmpty(behaviorSign)) {
                Debug.LogError("AIBehavior is null :" + iGPO.GetSign());
                return;
            }
            AssetManager.LoadAIBehavior(behaviorSign, LoadAIBehaviorCallBack);
            Dispatcher(new SE_Behaviour.Event_AfterBehaviorConfigInit());
        }

        private void LoadAIBehaviorCallBack(ExternalBehavior data) {
            PerfAnalyzerAgent.BeginSample("AIBehavior:ExternalBehavior");
            behavior.ExternalBehavior = data;
#if UNITY_EDITOR || BUILD_SERVER
            behavior.AsynchronousLoad = true;
#endif
            PerfAnalyzerAgent.EndSample("AIBehavior:ExternalBehavior");
            isLoadBehavior = true;
            actionComponents = behavior.FindTasks<ActionComponent>();
            conditionComponents = behavior.FindTasks<ConditionComponent>();
        }

        private void ClearActionComponent() {
            if (behavior == null) {
                return;
            }
            for (int i = 0; i < actionComponents.Count; i++) {
                var action = actionComponents[i];
                action.Clear();
            }
        }

        private void ClearConditionComponent() {
            if (behavior == null) {
                return;
            }
            for (int i = 0; i < conditionComponents.Count; i++) {
                var condition = conditionComponents[i];
                condition.Clear();
            }
        }

        private void OnUpdate(float deltaTime) {
            if (isLoadBehavior == false || behavior == null) {
                return;
            }
            if (Time.time - invokeTime < 2.0f) {
                return;
            }
            invokeTime = Time.time;
            var sqrDistance = GetMinDistanceGPO();
            var isEnabled = sqrDistance < enabledDistance * enabledDistance;
            EnableBehavior(isEnabled);
        }

        private void EnableBehavior(bool isTrue) {
            if (isEnableBehavior == isTrue) {
                return;
            }
            isEnableBehavior = isTrue;
            if (isTrue) {
                MsgRegister.Dispatcher(new SM_Behaviour.Event_StartBehavior {
                    UseBehavior = behavior,
                });
                mySystem.Dispatcher(new SE_Behaviour.Event_EnabledBehavior {
                    IsEnabled = true,
                });
            } else {
                ClearActionComponent();
                ClearConditionComponent();
                MsgRegister.Dispatcher(new SM_Behaviour.Event_DisableBehavior {
                    UseBehavior = behavior,
                });
                mySystem.Dispatcher(new SE_Behaviour.Event_EnabledBehavior {
                    IsEnabled = false,
                });
            }
        }

        // 获取距离当前位置最近的 GPO 距离
        private float GetMinDistanceGPO() {
            var point = iEntity.GetPoint();
            var minSqrDistance = float.MaxValue;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.IsGodMode() || gpo.IsDead()) {
                    continue;
                }
                if (gpo.GetGPOType() != GPOData.GPOType.Role) {
                    continue;
                }
                var sqrDistance = Vector3.SqrMagnitude(point - gpo.GetPoint());
                if (sqrDistance < minSqrDistance) {
                    minSqrDistance = sqrDistance;
                }
            }
            return minSqrDistance;
        }
    }
}