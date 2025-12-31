using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using BehaviorDesigner.Runtime.Tasks;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("是否在使用技能")]
    [TaskCategory("AI")]
    public class ConditionIsUseSkill : ConditionComponent {
        private bool isUseSkill = false;

        override public void OnAwake() {
            base.OnAwake();
            iGPO.Register<SE_Behaviour.Event_IsUseSkill>(OnIsUseSkillCallBack);
        }

        override public void OnStart() {
            base.OnStart();
            iGPO.Dispatcher(new SE_Behaviour.Event_GetIsUseSkill() {
                CallBack = IsUseSkill
            });
        }

        protected override void OnClear() {
            iGPO.Unregister<SE_Behaviour.Event_IsUseSkill>(OnIsUseSkillCallBack);
        }

        public void OnIsUseSkillCallBack(ISystemMsg body, SE_Behaviour.Event_IsUseSkill ent) {
            IsUseSkill(ent.IsTrue);
        }

        public void IsUseSkill(bool isTrue) {
            this.isUseSkill = isTrue;
        }

        public override TaskStatus OnUpdate() {
            if (isUseSkill) {
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }
    }
}