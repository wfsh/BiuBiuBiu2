using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_PlatformMovementSystem : S_Ability_Base {
        // private PlatformMovementEntity entity;
        //
        // protected override void OnAwake() {
        //     base.OnAwake();
        //     AddComponents();
        // }
        //
        // override protected void AddComponents() {
        //     base.AddComponents();
        //     AddNetwork();
        //     AddMove();
        //     AddSelect();
        // }
        //
        // protected override void OnLoadEntityEnd(IEntity iEnter) {
        //     entity = (PlatformMovementEntity)iEnter;
        //     var points = new List<Vector3>();
        //     points.Add(iEntity.GetPoint());
        //     points.Add(entity.EndPoint);
        //     Dispatcher(new SE_Ability.SetMovePointsLoop {
        //         MoveSpeed = entity.MoveSpeed,
        //         Points = points,
        //     });
        //     
        // }
        //
        // protected override void OnClear() {
        //     base.OnClear();
        //     entity = null;
        // }
        //
        // private void AddMove() {
        //     AddComponent<MovePointsLoop>();
        // }
        //
        // private void AddSelect() {
        //     AddComponent<ServerPlatformMovementGPOEnter>();
        // }
        //
        // protected void AddNetwork() {
        //     var sync = AddComponent<ServerNetworkSync>();
        //     sync.SetSpawnProtoFunc(OnSyncSpawnProto);
        //     AddComponent<ServerNetworkTransform>();
        // }
        //
        // void OnSyncSpawnProto(ServerNetworkSync sync) {
        //     // sync.SetSpawnRPC(new Proto_Ability.TargetRpc_ScreenElement {
        //     //     abilityId = AbilityId, playAbility = AbilityData.CAB_PlatformMovement, elementId = entity.ElementIndex,
        //     // });
        // }
    }
}