using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ConditionComponent : Conditional {
        protected IEntity iEntity { get; private set; }
        protected IGPO iGPO { get; private set; }

        public override void OnAwake() {
            base.OnAwake();
            iEntity = gameObject.GetComponent<IEntity>();
            iGPO = iEntity.GetGPO();
        }

        public void Clear() {
            OnClear();
            iEntity = null;
            iGPO = null;
        }

        virtual protected void OnClear() {
        }
    }
}