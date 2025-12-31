using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class MachineGunBulletSelectGPO : BulletSelectGPO {

        protected override int Raycast(Vector3 point, Vector3 forward, RaycastHit[] raycastHit, float distance, int layerMask) {
            return Physics.SphereCastNonAlloc(point, 0.1f, forward, raycastHit, distance, layerMask);
        }
    }
}