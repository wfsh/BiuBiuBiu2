using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientRippleEffectDraw : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float ScaleChangeSpeed;
            public float PlayTimestamp;
        }

        private float scaleChangeSpeed = 1.0f;
        private float playTimestamp = 0.0f;
        
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            scaleChangeSpeed = initData.ScaleChangeSpeed;
            playTimestamp = initData.PlayTimestamp;
        }
        
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (EntityBase)iEntity;
            // 获取电涌表现 mono 组件
            var ripple = entity.GetComponentInChildren<RippleEffectDraw>(true);
            // 设置扩散速度
            ripple.PlaySpeed = scaleChangeSpeed;
            ripple.SetCurFrame(Mathf.FloorToInt(0.001f * (TimeUtil.GetCurUTCTimestamp() - playTimestamp) * 60));
        }
    }
}
