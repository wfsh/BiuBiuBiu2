using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否掉血")]
    [TaskCategory("Monster")]
    public class ConditionIsBeHurt : ConditionComponent {
        private bool isBeHurt;
        public override void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_GPO.Event_DownHP>(OnDownHP);
            iGPO.Register<SE_Behaviour.Event_ComeBack>(ComeBackCallBack);
            iGPO.Register<SE_AI.Event_TriggerFightStatus>(TriggerFightStatusCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_GPO.Event_DownHP>(OnDownHP);
            iGPO.Unregister<SE_Behaviour.Event_ComeBack>(ComeBackCallBack);
            iGPO.Unregister<SE_AI.Event_TriggerFightStatus>(TriggerFightStatusCallBack);
        }

        private void TriggerFightStatusCallBack(ISystemMsg arg1, SE_AI.Event_TriggerFightStatus ent) {
            isBeHurt = false;
        }

        private void ComeBackCallBack(ISystemMsg arg1, SE_Behaviour.Event_ComeBack ent) {
            isBeHurt = false;
        }

        private void OnDownHP(ISystemMsg arg1, SE_GPO.Event_DownHP obj) {
            if (obj.DownHpGPO.GetGpoID() != iGPO.GetGpoID()) {
                return;
            }

            isBeHurt = true;
        }

        public override TaskStatus OnUpdate() {
            var status = isBeHurt ? TaskStatus.Success : TaskStatus.Failure;
            return status;
        }
    }
}