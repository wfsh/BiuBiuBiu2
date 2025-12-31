using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientResetTrailRenderer : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Ability.SetEntityStartPoint>(OnSetEntityStartPointCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Ability.SetEntityStartPoint>(OnSetEntityStartPointCallBack);
        }
        
        private void OnSetEntityStartPointCallBack(ISystemMsg body, CE_Ability.SetEntityStartPoint ent) {
            ResetTrailRenderer();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            ResetTrailRenderer();
        }

        private void ResetTrailRenderer() {
            var entity = (EntityBase)iEntity;
            var trailRenderers = entity.GetComponentsInChildren<TrailRenderer>();
            for (int i = 0; i < trailRenderers.Length; i++) {
                var trail = trailRenderers[i];
                trail.Clear();
            }
        }
    }
}