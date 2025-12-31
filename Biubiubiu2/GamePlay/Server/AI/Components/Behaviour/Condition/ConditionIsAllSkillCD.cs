using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否所有技能都 CD 中")]
    [TaskCategory("AI")]
    public class ConditionIsAllSkillCD : ConditionComponent {
        private bool isAllSkillCD = false;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_IsAllSkillCD>(OnIsAllSkillCDCallBack);
        }

        override public void OnStart() {
            base.OnStart();
            iGPO.Dispatcher(new SE_Behaviour.Event_GetIsAllSkillCD() {
                CallBack = IsAllSkillCD
            });
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_IsAllSkillCD>(OnIsAllSkillCDCallBack);
        }

        public void OnIsAllSkillCDCallBack(ISystemMsg body, SE_Behaviour.Event_IsAllSkillCD ent) {
            IsAllSkillCD(ent.IsTrue);
        }

        public void IsAllSkillCD(bool isTrue) {
            this.isAllSkillCD = isTrue;
        }

        public override TaskStatus OnUpdate() {
            if (isAllSkillCD) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}