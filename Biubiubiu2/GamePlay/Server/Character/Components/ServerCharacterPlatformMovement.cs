using System;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterPlatformMovement : ServerCharacterComponent {
        // private PlatformMovementEntity sceneElement;
        // private ServerCharacterSystem system;
        // private Vector3 localPoint = Vector3.zero;
        // private int elementId = 0;
        // private bool isStay = false;
        //
        // protected override void OnAwake() {
        //     system = (ServerCharacterSystem)mySystem;
        // }
        //
        // protected override void OnStart() {
        //     base.OnStart();
        //     AddUpdate(OnUpdate);
        // }
        //
        // protected override void OnClear() {
        //     RemoveUpdate(OnUpdate);
        //     characterNetwork.RemoveProtoCallBack(Proto_Character.Cmd_StayPlatformMovement.ID, OnStayPlatformMovementCallBack);
        //     characterNetwork.RemoveProtoCallBack(Proto_Character.Cmd_PlatformMovement.ID, OnPlatformMovementCallBack);
        //     this.system = null;
        //     this.sceneElement = null;
        //     base.OnClear();
        // }
        //
        // protected override void OnSetNetwork() {
        //     AddProtoCallBack(Proto_Character.Cmd_StayPlatformMovement.ID, OnStayPlatformMovementCallBack);
        //     AddProtoCallBack(Proto_Character.Cmd_PlatformMovement.ID, OnPlatformMovementCallBack);
        // }
        //
        // private void OnUpdate(float delta) {
        //     if (characterNetwork == null || characterNetwork.IsDestroy()) {
        //         return;
        //     }
        //     SetSync();
        //     if (isStay == false || this.characterNetwork.IsLocalPlayer()) {
        //         return;
        //     }
        //     var point = sceneElement.TransformPoint(localPoint);
        //     iEntity.SetPoint(point);
        // }
        //
        // private void SetSync() {
        //     if (characterSync.IsStayPlatformMovement() != isStay) {
        //         characterSync.IsStayPlatformMovement(isStay);
        //     }
        //     if (characterSync.StayElementId() != elementId) {
        //         characterSync.StayElementId(elementId);
        //     }
        //     if (characterSync.StayLocalPoint() != localPoint) {
        //         characterSync.StayLocalPoint(localPoint);
        //     }
        // }
        //
        // public void OnStayPlatformMovementCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
        //     var data = (Proto_Character.Cmd_StayPlatformMovement)cmdData;
        //     isStay = data.isStay;
        //     if (isStay) {
        //         elementId = data.elementId;
        //         // MsgRegister.Dispatcher(new SM_SceneElement.GetSceneElement {
        //         //     ElementId = data.elementId,
        //         //     CallBack = SetSceneElement
        //         // });
        //     } else {
        //         localPoint = Vector3.zero;
        //         sceneElement = null;
        //         elementId = 0;
        //     }
        // }
        //
        // public void SetSceneElement(IEntity iEntity) {
        //     sceneElement = (PlatformMovementEntity)iEntity;
        // }
        //
        // public void OnPlatformMovementCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
        //     var data = (Proto_Character.Cmd_PlatformMovement)cmdData;
        //     localPoint = data.localPoint;
        // }
    }
}