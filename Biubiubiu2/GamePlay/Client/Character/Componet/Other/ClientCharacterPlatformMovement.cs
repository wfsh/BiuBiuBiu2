using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterPlatformMovement : ClientCharacterComponent {
        // private PlatformMovementEntity sceneElement;
        // private Vector3 localPoint = Vector3.zero;
        // private bool isStay = false;
        // private int elementId = 0;
        //
        // protected override void OnAwake() {
        // }
        //
        // protected override void OnStart() {
        //     base.OnStart();
        //     AddUpdate(OnUpdate);
        // }
        //
        // protected override void OnClear() {
        //     base.OnClear();
        //     RemoveUpdate(OnUpdate);
        //     this.sceneElement = null;
        // }
        //
        // private void OnUpdate(float delta) {
        //     UpdateSync();
        //     if (isStay == false) {
        //         return;
        //     }
        //     localPoint = Vector3.Lerp(localPoint, characterSync.StayLocalPoint(), 6 * Time.deltaTime);
        //     var point = sceneElement.TransformPoint(localPoint);
        //     iEntity.SetPoint(point);
        // }
        //
        // private void UpdateSync() {
        //     if (characterNetwork == null || characterNetwork.IsDestroy()) {
        //         return;
        //     }
        //     if (isStay != characterSync.IsStayPlatformMovement()) {
        //         isStay = characterSync.IsStayPlatformMovement();
        //         if (isStay == false) {
        //             localPoint = Vector3.zero;
        //         }
        //     }
        //     if (elementId != characterSync.StayElementId()) {
        //         elementId = characterSync.StayElementId();
        //         if (elementId > 0) {
        //             Action<IEntity> callback = SetSceneElement;
        //             // MsgRegister.Dispatcher(new CM_SceneElement.GetSceneElement {
        //             //     CallBack = callback,
        //             //     ElementId = elementId
        //             // });
        //         } else {
        //             sceneElement = null;
        //         }
        //     }
        //     if (localPoint == Vector3.zero) {
        //         localPoint = characterSync.StayLocalPoint();
        //     }
        // }
        //
        // public void SetSceneElement(IEntity iEntity) {
        //     sceneElement = (PlatformMovementEntity)iEntity;
        // }
    }
}
