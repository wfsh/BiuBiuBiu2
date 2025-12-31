using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientPlatformMovementGPOEnter : ComponentBase {
        // private PlatformMovementEntity entity;
        // private C_Ability_Base abSystem;
        // private Vector3 prevPoint;
        // private Vector3 platformMovement;
        // private bool isStay = false;
        // private IGPO localGPO;
        //
        //
        // protected override void OnAwake() {
        //     MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        // }
        //
        // protected override void OnStart() {
        //     base.OnStart();
        //     MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
        //         CallBack = gpo => {
        //             localGPO = gpo;
        //         }
        //     });
        // }
        //
        // protected override void OnSetEntityObj(IEntity entity) {
        //     base.OnSetEntityObj(entity);
        //     abSystem = (C_Ability_Base)mySystem;
        //     this.entity = (PlatformMovementEntity)iEntity;
        //     AddUpdate(OnUpdate);
        // }
        //
        // protected override void OnClear() {
        //     base.OnClear();
        //     MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        //     RemoveUpdate(OnUpdate);
        //     abSystem = null;
        //     entity = null;
        // }
        //
        // private void OnUpdate(float delayTime) {
        //     CheckGPOStay();
        //     platformMovement = iEntity.GetPoint() - prevPoint;
        //     prevPoint = iEntity.GetPoint();
        //     SendPlatformMovement();
        // }
        //
        // private void OnAddLocalGPOCallBack(CM_GPO.AddLocalGPO ent) {
        //     localGPO = ent.LocalGPO;
        // }
        //
        // private void SendPlatformMovement() {
        //     if (isStay) {
        //         localGPO.Dispatcher(new CE_Ability.PlatformMovement() {
        //             entity = entity,
        //             point = platformMovement,
        //         });
        //     }
        // }
        //
        // private void CheckGPOStay() {
        //     if (localGPO == null) {
        //         isStay = false;
        //         return;
        //     }
        //     if (isStay) {
        //         if (IsPointInRotatedRect(localGPO.GetPoint(), entity.TriggerTransform) == false) {
        //             isStay = false;
        //             // localGPO.Dispatcher(new CE_Ability.StayPlatformMovement() {
        //             //     elementId = entity.ElementIndex,
        //             //     isStay = isStay,
        //             // });
        //         }
        //     }
        //     if (isStay == false && IsPointInRotatedRect(localGPO.GetPoint(), entity.TriggerTransform)) {
        //         isStay = true;
        //         // localGPO.Dispatcher(new CE_Ability.StayPlatformMovement() {
        //         //     elementId = entity.ElementIndex,
        //         //     isStay = isStay,
        //         // });
        //     }
        // }
        //
        // private bool IsPointInRotatedRect(Vector3 point, Transform rect) {
        //     // 获取矩形的位置、大小和旋转
        //     var rectPosition = rect.position;
        //     var rectSize = rect.lossyScale;
        //     var rectRotation = rect.eulerAngles.y;
        //     // 将角色的世界坐标转换为矩形的本地坐标系中
        //     var localPos = point - rectPosition;
        //     localPos = Quaternion.Euler(0f, -rectRotation, 0f) * localPos;
        //     // 检查坐标是否在矩形范围内
        //     var halfWidth = rectSize.x / 2f;
        //     var halfHeight = rectSize.y / 2f;
        //     return Mathf.Abs(localPos.x) <= halfWidth && Mathf.Abs(localPos.z) <= halfWidth &&
        //            Mathf.Abs(localPos.y) <= halfHeight;
        // }
    }
}