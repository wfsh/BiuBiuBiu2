using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerPlatformMovementGPOEnter : ComponentBase {
        // private PlatformMovementEntity entity;
        // private List<ServerGPO> checkList;
        // private S_Ability_Base abSystem;
        // private Vector3 prevPoint;
        // private Vector3 platformMovement;
        // private Dictionary<int, bool> gpoDic = new Dictionary<int, bool>(10);
        //
        // protected override void OnSetEntityObj(IEntity iEntity) {
        //     base.OnSetEntityObj(iEntity);
        //     abSystem = (S_Ability_Base)mySystem;
        //     entity = (PlatformMovementEntity)iEntity;
        //     checkList = new List<ServerGPO>();
        //     AddUpdate(OnUpdate);
        // }
        //
        // protected override void OnClear() {
        //     base.OnClear();
        //     RemoveUpdate(OnUpdate);
        //     gpoDic.Clear();
        //     checkList.Clear();;
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
        // private void SendPlatformMovement() {
        //     for (int i = 0; i < checkList.Count; i++) {
        //         var gpo = checkList[i];
        //         gpo.Dispatcher(new SE_Ability.PlatformMovement() {
        //             move = platformMovement
        //         });
        //     }  
        // }
        //
        // private void CheckGPOStay() {
        //     var triggerTran = entity.TriggerTransform;
        //     var point = triggerTran.position;
        //     var lossyScale = triggerTran.lossyScale;
        //     var angle = Quaternion.Euler(0f, -triggerTran.eulerAngles.y, 0f);
        //     var count = checkList.Count;
        //     gpoDic.Clear();
        //     for (int i = count - 1; i >= 0; i--) {
        //         var gpo = checkList[i];
        //         if (IsPointInRotatedRect(gpo.GetPoint(), point, lossyScale, angle) == false) {
        //             checkList.RemoveAt(i);
        //             gpoDic.Add(gpo.GetGpoID(), true);
        //         }
        //     }
        //     var gpoList = abSystem.GPOList;
        //     for (int i = 0; i < gpoList.Count; i++) {
        //         var gpo = (ServerGPO)gpoList[i];
        //         if (gpo.GetGPOType() == GPOData.GPOType.Role || gpoDic.ContainsKey(gpo.GetGpoID())) {
        //             continue;
        //         }
        //         if (GpoHasStay(gpo.GetGpoID()) == false && IsPointInRotatedRect(gpo.GetPoint(), point, lossyScale, angle)) {
        //             checkList.Add(gpo);
        //         }
        //     }
        // }
        //
        // private bool GpoHasStay(int gpoId) {
        //     var count = checkList.Count;
        //     for (int i = count - 1; i >= 0; i--) {
        //         var gpo = checkList[i];
        //         if (gpo.GetGpoID() == gpoId) {
        //             return true;
        //         }
        //     }
        //     return false;
        // }
        //
        // private bool IsPointInRotatedRect(Vector3 point, Vector3 rectPosition, Vector3 rectSize, Quaternion angle) {
        //     // 将角色的世界坐标转换为矩形的本地坐标系中
        //     var localPos = point - rectPosition;
        //     localPos = angle * localPos;
        //     // 检查坐标是否在矩形范围内
        //     var halfWidth = rectSize.x / 2f;
        //     var halfHeight = rectSize.y / 2f;
        //     return Mathf.Abs(localPos.x) <= halfWidth && Mathf.Abs(localPos.z) <= halfWidth &&
        //            Mathf.Abs(localPos.y) <= halfHeight;
        // }
    }
}