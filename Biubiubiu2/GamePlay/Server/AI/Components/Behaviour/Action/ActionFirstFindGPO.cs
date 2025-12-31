using System;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("第一次发现目标")]
    [TaskCategory("AI")]
    public class ActionFirstFindGPO : ActionComponent {
        private bool isFirstFind = true;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
            iGPO.Register<SE_Behaviour.Event_GetInFirstFind>(OnGetInFirstFindCallBack);
        }

        override protected void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
            iGPO.Unregister<SE_Behaviour.Event_GetInFirstFind>(OnGetInFirstFindCallBack);
        }

        public void OnGetInFirstFindCallBack(ISystemMsg body, SE_Behaviour.Event_GetInFirstFind ent) {
            ent.CallBack.Invoke(isFirstFind);
        }

        public void OnComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            if (ent.IsTrue == false) {
                isFirstFind = true;
                iGPO.Dispatcher(new SE_Behaviour.Event_InFirstFind {
                    IsTrue = true
                });
            }
        }

        public override TaskStatus OnUpdate() {
            if (isFirstFind) {
                iGPO.Dispatcher(new SE_Behaviour.Event_InFirstFind {
                    IsTrue = false
                });
                isFirstFind = false;
            }
            return TaskStatus.Success;
        }
    }
}