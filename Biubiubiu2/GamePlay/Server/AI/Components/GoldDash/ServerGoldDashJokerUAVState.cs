using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashJokerUAVState : ServerNetworkComponentBase {
        private float hateValue;
        private float maxHateValue;
        private bool isInWarningStatus;
        private AIBehaviourData.FightStateEnum fightState;

        protected override void OnAwake() {
            base.OnAwake();
            Register<SE_AI.Event_TriggerAlertStatus>(EnterAlertStatus);
            Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTarget);
            Register<SE_AI.Event_ChangeAIState>(OnChangeMonsterState);
        }

        protected override void OnClear() {
            base.OnClear();
            Unregister<SE_AI.Event_TriggerAlertStatus>(EnterAlertStatus);
            Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTarget);
            Unregister<SE_AI.Event_ChangeAIState>(OnChangeMonsterState);
        }

        private void EnterAlertStatus(ISystemMsg body, SE_AI.Event_TriggerAlertStatus ent) {
            if (ent.isEnabled) {
                UpdateState(AIBehaviourData.FightStateEnum.Alert);
            }
        }

        private void OnMaxHateTarget(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            hateValue = ent.HateValue;
            UpdateState(fightState);
        }

        private void UpdateState(AIBehaviourData.FightStateEnum state) {
            if (this.fightState == state && state != AIBehaviourData.FightStateEnum.Warning) {
                return;
            }
            this.fightState = state;
            ushort fillValue = 0;
            switch (state) {
                case AIBehaviourData.FightStateEnum.Warning:
                    if (maxHateValue > 0) {
                        fillValue = (ushort)(Mathf.Clamp01(hateValue / maxHateValue) * 100);
                    }
                    break;
            }
            Rpc(new Proto_AI.Rpc_JokerUAVState() {
                state = (byte)state, fillValue = fillValue,
            });
        }

        private void OnChangeMonsterState(ISystemMsg body, SE_AI.Event_ChangeAIState ent) {
            if (ent.State == AIBehaviourData.FightStateEnum.Warning) {
                maxHateValue = ent.WarningMaxHate;
            }
            UpdateState(ent.State);
        }
    }
}