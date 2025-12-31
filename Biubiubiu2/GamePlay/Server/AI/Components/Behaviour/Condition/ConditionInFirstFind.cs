using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否第一次发现敌人")]
    [TaskCategory("AI")]
    public class ConditionInFirstFind : ConditionComponent {
        private bool inFirstFind = true;
        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_InFirstFind>(OnInFirstFindCallBack);
        }

        public override void OnStart() {
            base.OnStart();
            iGPO.Dispatcher(new SE_Behaviour.Event_GetInFirstFind {
                CallBack = InFirstFind
            });
        }

        protected override void OnClear() {
            iGPO.Register<SE_Behaviour.Event_InFirstFind>(OnInFirstFindCallBack);
        }

        public void OnInFirstFindCallBack(ISystemMsg body, SE_Behaviour.Event_InFirstFind ent) {
            InFirstFind(ent.IsTrue);
        }

        public void InFirstFind(bool isTrue) {
            this.inFirstFind = isTrue;
        }

        public override TaskStatus OnUpdate() {
            if (inFirstFind) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}