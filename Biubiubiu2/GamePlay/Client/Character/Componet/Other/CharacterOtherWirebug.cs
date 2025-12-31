using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherWirebug : ClientCharacterComponent {
        private CharacterData.WirebugType wirebugType = CharacterData.WirebugType.None;
        private GameObject wirebugGameObj;
        private WirebugLine wirebugLine = null;
        private Transform lHandTran;
        private float currentTime = 0.0f;
        private const float duration = 2f; // 持续时间

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            AssetManager.LoadGamePlayObjAsync("Components/WirebugLine", WirebugLineEndCallBack);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            lHandTran = iEntity.GetBodyTran(GPOData.PartEnum.LeftHand);
        }

        private void OnUpdate(float deltaTime) {
            if (wirebugLine != null) {
                wirebugLine.SetPoint(lHandTran.position);
            }
            ExecuteSkill();
        }

        private void WirebugLineEndCallBack(GameObject perfab) {
            if (perfab != null) {
                wirebugGameObj = perfab;
            } else {
                Debug.LogError("LineRenderer 加载失败:");
            }
        }

        protected override void OnClear() {
            base.OnClear();
            wirebugGameObj = null;
            lHandTran = null;
            RemoveUpdate(OnUpdate);
            ClearWirebug();
            RemoveProtoCallBack(Proto_Wirebug.Rpc_WirebugState.ID, OnRpcWirebugState);
            RemoveProtoCallBack(Proto_Wirebug.Rpc_CreateWirebug.ID, OnRpcCreateWirebug);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Wirebug.Rpc_WirebugState.ID, OnRpcWirebugState);
            AddProtoCallBack(Proto_Wirebug.Rpc_CreateWirebug.ID, OnRpcCreateWirebug);
        }

        private void OnRpcWirebugState(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Wirebug.Rpc_WirebugState)protoDoc;
            SetWirebugType((CharacterData.WirebugType)data.wirebugType);
        }

        private void OnRpcCreateWirebug(INetwork network, IProto_Doc protoDoc) {
            var data = (Proto_Wirebug.Rpc_CreateWirebug)protoDoc;
            CreateWirebug(data.targetPoint);
            DisptcherWirebugMoveTargetPoint(data.targetPoint);
        }

        private void CreateWirebug(Vector3 targetPoint) {
            currentTime = duration;
            var gameObj = GameObject.Instantiate(wirebugGameObj);
            var tran = gameObj.transform;
            MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                layer = StageData.GameWorldLayerType.Character, transform = tran
            });
            wirebugLine = gameObj.GetComponent<WirebugLine>();
            wirebugLine.Init();
            wirebugLine.SetPoint(lHandTran.position);
            wirebugLine.MoveTo(targetPoint);
        }

        void ExecuteSkill() {
            if (wirebugType == CharacterData.WirebugType.None) {
                return;
            }
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f) {
                CanceWirebug();
            }
        }

        private void CanceWirebug() {
            ClearWirebug();
            SetWirebugType(CharacterData.WirebugType.None);
        }

        private void ClearWirebug() {
            if (wirebugLine != null) {
                wirebugLine.Clear();
                GameObject.Destroy(wirebugLine.gameObject);
                wirebugLine = null;
            }
        }

        public void SetWirebugType(CharacterData.WirebugType type) {
            if (wirebugType == type) {
                return;
            }
            wirebugType = type;
            switch (wirebugType) {
                case CharacterData.WirebugType.Start:
                    currentTime = duration;
                    ClearWirebug();
                    break;
                case CharacterData.WirebugType.Drop:
                    DisptcherWirebugMoveTargetPoint(Vector3.zero);
                    break;
                case CharacterData.WirebugType.Glide:
                    ClearWirebug();
                    break;
                case CharacterData.WirebugType.None:
                    CanceWirebug();
                    DisptcherWirebugMoveTargetPoint(Vector3.zero);
                    break;
            }
            mySystem.Dispatcher(new CE_Character.Event_WirebugMoveState {
                State = type
            });
        }

        private void DisptcherWirebugMoveTargetPoint(Vector3 point) {
            Dispatcher(new CE_Character.Event_WirebugMoveTargetPoint {
                MoveTarget = point
            });
        }
    }
}