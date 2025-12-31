using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkTransform : ComponentBase {
        private bool isSetPointSync = false;
        private bool isSetRotaSync = false;
        private bool isSetScaleSync = false;
        private bool isFirstPoint = true;
        private bool isFirstRota = true;
        private bool isFirstScale = true;
        private bool isShowDebug = false;
        private Vector3 targetPosition; // 当前的目标位置
        private Vector3 lastPosition; // 上一次的位置
        private Vector3 tartetScale = Vector3.one; // 当前的目标缩放
        private Vector3 lastScale = Vector3.one; // 上一次的缩放
        private Quaternion targetRotation; // 当前的目标旋转
        private Quaternion lastRotation; // 上一次的旋转
        private float pointLerpTime = 0.0f;
        private float rotaLerpTime = 0.0f;
        private float scaleLerpTime = 0.0f;
        private float lerpDuration = 0.15f; // 插值持续时间
        private int pointDistance = 20; // 位置差 大于 多少不使用插值直接移动
        private bool isStop = false;

        protected override void OnAwake() {
            mySystem.Register<CE_GPO.Event_SetTransformPointRota>(OnSetTransformPointRota);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_GPO.Event_SetTransformPointRota>(OnSetTransformPointRota);
            RemoveProtoCallBack(Proto_Network.Rpc_TransformPointRota.ID, OnGetTransformPointRota);
            RemoveProtoCallBack(Proto_Network.Rpc_TransformPoint.ID, OnGetTransformPoint);
            RemoveProtoCallBack(Proto_Network.Rpc_TransformRota.ID, OnGetTransformRota);
            RemoveProtoCallBack(Proto_Network.Rpc_TransformScale.ID, OnSetTransformScale);
            RemoveProtoCallBack(Proto_Network.Rpc_TransformFull.ID, OnSetTransformFull);
            RemoveProtoCallBack(Proto_Network.Rpc_SyncTransformPoint.ID, OnSyncTransformPoint);
            RemoveProtoCallBack(Proto_Network.Rpc_SyncTransformRota.ID, OnSyncTransformRota);
            RemoveProtoCallBack(Proto_Network.Rpc_SyncTransformScale.ID, OnSyncTransformScale);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Network.Rpc_TransformPointRota.ID, OnGetTransformPointRota);
            AddProtoCallBack(Proto_Network.Rpc_TransformPoint.ID, OnGetTransformPoint);
            AddProtoCallBack(Proto_Network.Rpc_TransformRota.ID, OnGetTransformRota);
            AddProtoCallBack(Proto_Network.Rpc_TransformScale.ID, OnSetTransformScale);
            AddProtoCallBack(Proto_Network.Rpc_TransformFull.ID, OnSetTransformFull);
            AddProtoCallBack(Proto_Network.Rpc_SyncTransformPoint.ID, OnSyncTransformPoint);
            AddProtoCallBack(Proto_Network.Rpc_SyncTransformRota.ID, OnSyncTransformRota);
            AddProtoCallBack(Proto_Network.Rpc_SyncTransformScale.ID, OnSyncTransformScale);
        }

        public void ShowDebug(bool isTrue) {
            isShowDebug = isTrue;
        }

        public void Stop() {
            isStop = true;
        }

        private void OnSetTransformPointRota(ISystemMsg body, CE_GPO.Event_SetTransformPointRota ent) {
            SetPoint(ent.Point);
            SetRota(ent.Rota);
        }

        private void OnSyncTransformPoint(INetwork network, IProto_Doc proto) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_SyncTransformPoint)proto;
            iEntity.SetPoint(data.point);
        }

        private void OnSyncTransformRota(INetwork network, IProto_Doc proto) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_SyncTransformRota)proto;
            iEntity.SetRota(data.rota);
        }

        private void OnSyncTransformScale(INetwork network, IProto_Doc proto) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_SyncTransformScale)proto;
            iEntity.SetPoint(data.scale);
        }

        private void OnGetTransformPointRota(INetwork network, IProto_Doc docData) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_TransformPointRota)docData;
            SetPoint(data.point);
            SetRota(data.rota);
        }

        private void OnGetTransformPoint(INetwork network, IProto_Doc docData) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_TransformPoint)docData;
            SetPoint(data.point);
        }

        private void OnGetTransformRota(INetwork network, IProto_Doc docData) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_TransformRota)docData;
            SetRota(data.rota);
        }

        private void OnSetTransformScale(INetwork network, IProto_Doc docData) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_TransformScale)docData;
            SetScale(data.scale);
        }
        
        private void OnSetTransformFull(INetwork network, IProto_Doc docData) {
            if (isStop) {
                return;
            }
            var data = (Proto_Network.Rpc_TransformFull)docData;
            SetPoint(data.point);
            SetScale(data.scale);
            SetRota(data.rota);
        }

        private void SetPoint(Vector3 point) {
            var distance = Vector3.Distance(iEntity.GetPoint(), point);
            if (isFirstPoint || distance > pointDistance) {
                isFirstPoint = false;
                iEntity.SetPoint(point);
            } else {
                if (point != targetPosition) {
                    pointLerpTime = 0f;
                    isSetPointSync = true;
                }
            }
            lastPosition = iEntity.GetPoint();
            targetPosition = point;
        }

        private void SetRota(Quaternion rota) {
            if (isFirstRota) {
                isFirstRota = false;
                iEntity.SetRota(rota);
            } else {
                if (rota != targetRotation) {
                    rotaLerpTime = 0f;
                    isSetRotaSync = true;
                }
            }
            lastRotation = iEntity.GetRota();
            targetRotation = rota;
        }

        private void SetScale(Vector3 scale) {
            if (isFirstScale) {
                isFirstScale = false;
                iEntity.SetLocalScale(scale);
            } else {
                if (scale != tartetScale) {
                    scaleLerpTime = 0f;
                    isSetScaleSync = true;
                }
            }
            lastScale = iEntity.GetLocalScale();
            tartetScale = scale;
        }

        private void OnUpdate(float delta) {
            if (isStop) {
                return;
            }
            UpdateMove();
            UpdateRota();
            UpdateScale();
        }

        private void UpdateMove() {
            if (isSetPointSync == false) {
                return;
            }
            pointLerpTime += Time.deltaTime / lerpDuration;
            if (pointLerpTime >= 1.0f) {
                pointLerpTime = 1.0f;
                lastPosition = targetPosition;
                iEntity.SetPoint(targetPosition);
                isSetPointSync = false;
            } else {
                iEntity.SetPoint(Vector3.Lerp(lastPosition, targetPosition, pointLerpTime));
            }
        }

        private void UpdateRota() {
            if (isSetRotaSync == false) {
                return;
            }
            rotaLerpTime += Time.deltaTime / lerpDuration;
            if (rotaLerpTime >= 1.0f) {
                rotaLerpTime = 1.0f;
                lastRotation = targetRotation;
                iEntity.SetRota(targetRotation);
                isSetRotaSync = false;
            } else {
                iEntity.SetRota(Quaternion.Slerp(lastRotation, targetRotation, rotaLerpTime));
            }
        }

        private void UpdateScale() {
            if (isSetScaleSync == false) {
                return;
            }
            scaleLerpTime += Time.deltaTime / lerpDuration;
            if (scaleLerpTime >= 1.0f) {
                scaleLerpTime = 1.0f;
                lastScale = tartetScale;
                iEntity.SetLocalScale(tartetScale);
                isSetScaleSync = false;
            } else {
                iEntity.SetLocalScale(Vector3.Lerp(lastScale, tartetScale, scaleLerpTime));
            }
        }
    }
}