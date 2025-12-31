using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using UnityEngine.Profiling;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOCheckIsCanCallAI : ComponentBase {
        private RaycastHit[] raycastHit;

        protected override void OnAwake() {
            Register<SE_GPO.Event_IsCanUseCallAISkill>(OnIsCanUseCallAISkill);
        }

        protected override void OnStart() {
            base.OnStart();
            raycastHit = new RaycastHit[10];
        }

        protected override void OnClear() {
            Unregister<SE_GPO.Event_IsCanUseCallAISkill>(OnIsCanUseCallAISkill);
        }


        private void OnIsCanUseCallAISkill(ISystemMsg body, SE_GPO.Event_IsCanUseCallAISkill ent) {
            ent.CallBack.Invoke(CheckIsHit(ent.Width, ent.Height) == false); //&& CheckIsHitWall(ent.Width,ent.Height) == false);
        }

        private bool CheckIsHit(float width, float height) {
            if (width == 0 || height == 0) {
                return false;
            }

            var point = iGPO.GetPoint() + Vector3.up;
            int count = Physics.RaycastNonAlloc(point, iGPO.GetForward(), raycastHit, 1.8f, LayerData.DefaultLayerMask);
            if (count > 0) {
                for (int i = 0; i < raycastHit.Length; i++) {
                    var hit = raycastHit[i];
                    if (hit.collider != null && hit.collider.isTrigger == false) {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckIsHitWall(float width, float height) {
            if (width == 0 || height == 0) {
                return false;
            }
            var point = iGPO.GetPoint() + Vector3.up + iGPO.GetForward() * width;
            var half = new Vector3(width, width, 0.4f);
            int count = Physics.BoxCastNonAlloc(point, half, Vector3.up, raycastHit,Quaternion.identity,height,LayerData.DefaultLayerMask);
            return count > 0;
        }
    }
}