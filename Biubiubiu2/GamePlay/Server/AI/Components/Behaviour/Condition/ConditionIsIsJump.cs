using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否跳跃")]
    [TaskCategory("AI")]
    public class ConditionIsIsJump : ConditionComponent {
        private CharacterData.JumpType JumpType = CharacterData.JumpType.None;
        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_GPO.Event_JumpTypeChange>(OnJumpTypeChangeCallBack);
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_GPO.Event_JumpTypeChange>(OnJumpTypeChangeCallBack);
        }
        
        public override void OnStart() {
        }
        
        private void OnJumpTypeChangeCallBack(ISystemMsg body, SE_GPO.Event_JumpTypeChange ent) {
            JumpType = ent.JumpType;
        }

        public override TaskStatus OnUpdate() {
            return JumpType == CharacterData.JumpType.None ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}