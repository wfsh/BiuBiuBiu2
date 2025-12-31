using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class ColliderEnterCheck : MonoBehaviour {
        private Action<Collider> OnHitCallBack;
        private Action<Collider> OnStayCallBack;
        private Action<Collider> OnExitCallBack;

        public void Init(Action<Collider> OnHit, Action<Collider> OnStay, Action<Collider> OnExit) {
            OnHitCallBack = OnHit;
            OnStayCallBack = OnStay;
            OnExitCallBack = OnExit;
        }

        void OnDestroy() {
            OnHitCallBack = null;
            OnStayCallBack = null;
            OnExitCallBack = null;
        }

        private void OnTriggerEnter(Collider other) {
            OnHitCallBack?.Invoke(other);
        }

        private void OnTriggerStay(Collider other) {
            OnStayCallBack?.Invoke(other);
        }

        private void OnTriggerExit(Collider other) {
            OnExitCallBack?.Invoke(other);
        }
    }
}