using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class CollisionEnterCheck : MonoBehaviour {
        private Action<Collision> OnHitCallBack;
        private Action<Collision> OnStayCallBack;
        private Action<Collision> OnExitCallBack;
        
        public void Init(Action<Collision> OnHit, Action<Collision> OnStay, Action<Collision> OnExit) {
            OnHitCallBack = OnHit;
            OnStayCallBack = OnStay;
            OnExitCallBack = OnExit;
        }

        void OnDestroy() {
            OnHitCallBack = null;
            OnStayCallBack = null;
            OnExitCallBack = null;
        }

        private void OnCollisionEnter(Collision other) {
            OnHitCallBack?.Invoke(other);
        }        
        
        private void OnCollisionStay(Collision other) {
            OnStayCallBack?.Invoke(other);
        } 
        
        private void OnCollisionExit(Collision other) {
            OnExitCallBack?.Invoke(other);
        }
    }
}