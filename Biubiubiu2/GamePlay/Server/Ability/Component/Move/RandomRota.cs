using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class RandomRota : ComponentBase {
        private Quaternion targetRota = Quaternion.identity;
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            AddUpdate(OnUpdate);
            base.OnClear();
        }
        
        void OnUpdate(float delta) {
            var angle = Quaternion.Angle(iEntity.GetRota(), targetRota);
            if (angle <= 5) {
                GetTarget();
            }
            iEntity.SetRota(Quaternion.Lerp(iEntity.GetRota(), targetRota, 3 * delta));
        }

        private void GetTarget() {
            var randY = Random.Range(0f, 360f);
            targetRota = Quaternion.Euler(new Vector3(0, randY, 0));
        }
    }
}
