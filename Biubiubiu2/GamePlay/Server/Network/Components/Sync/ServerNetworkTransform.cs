using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

// 同步 AB 的位置，旋转
namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerNetworkTransform : ServerNetworkComponentBase {
        private Vector3 lastPoint = Vector3.zero;
        private Quaternion lastRota = Quaternion.identity;
        private Vector3 lastScale = Vector3.one;
        private float positionSensitivity = 0.1f;
        private float rotationSensitivity = 1f;
        private float scaleSensitivity = 0.01f;
        private float syncTime = 0.1f;
        private float constSyncTime = 0.0f;
        private bool pointChanged = true;
        private bool rotaChanged = true;
        private bool scaleChanged = true;
        private bool syncPoint = true;
        private bool syncRota = true;
        private bool syncScale = true;
        private bool isEnabled = true;

        protected override void OnAwake() {
            mySystem.Register<SE_Entity.SyncRotaToNetworkTransform>(OnSyncRotaToNetworkTransformCallBack);
            mySystem.Register<SE_Entity.SyncPointToNetworkTransform>(OnSyncPointToNetworkTransformallBack);
            mySystem.Register<SE_Entity.SyncScaleToNetworkTransform>(OnSyncScaleToNetworkTransformCallBack);
            mySystem.Register<SE_Entity.SyncPointAndRota>(OnSyncPointAndRotaCallBack);
            mySystem.Register<SE_Entity.SyncPoint>(OnSyncPointCallBack);
            mySystem.Register<SE_Entity.SyncRota>(OnSyncRotaCallBack);
            mySystem.Register<SE_Network.Event_EnabledSyncNetworkTransform>(OnEnabledSyncNetworkTransformCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }
        
        // /// <summary>
        // /// 提供全量同步（玩家进入视野/登录时触发）
        // /// </summary>
        protected override ITargetRpc SyncData() {
            return new Proto_Network.Rpc_TransformFull {
                point = lastPoint,
                scale = lastScale,
                rota = lastRota
            };
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Entity.SyncRotaToNetworkTransform>(OnSyncRotaToNetworkTransformCallBack);
            mySystem.Unregister<SE_Entity.SyncPointToNetworkTransform>(OnSyncPointToNetworkTransformallBack);
            mySystem.Unregister<SE_Entity.SyncScaleToNetworkTransform>(OnSyncScaleToNetworkTransformCallBack);
            mySystem.Unregister<SE_Entity.SyncPointAndRota>(OnSyncPointAndRotaCallBack);
            mySystem.Unregister<SE_Entity.SyncPoint>(OnSyncPointCallBack);
            mySystem.Unregister<SE_Entity.SyncRota>(OnSyncRotaCallBack);
            mySystem.Unregister<SE_Network.Event_EnabledSyncNetworkTransform>(OnEnabledSyncNetworkTransformCallBack);
        }

        public void SetSync(bool syncPoint, bool syncRota, bool syncScale) {
            this.syncPoint = syncPoint;
            this.syncRota = syncRota;
            this.syncScale = syncScale;
        }

        public void SetSyncTime(float time) {
            this.syncTime = time;
        }

        private void OnSyncPointCallBack(ISystemMsg body, SE_Entity.SyncPoint ent) {
            iEntity.SetPoint(ent.Point);
        }

        private void OnSyncRotaCallBack(ISystemMsg body, SE_Entity.SyncRota ent) {
            iEntity.SetRota(ent.Rota);
        }

        private void OnSyncPointAndRotaCallBack(ISystemMsg body, SE_Entity.SyncPointAndRota ent) {
            iEntity.SetPoint(ent.Point);
            iEntity.SetRota(ent.Rota);
            if (ent.OR_IsSync) {
                lastRota = iEntity.GetRota();
                lastPoint = iEntity.GetPoint();
                Rpc(new Proto_Network.Rpc_TransformPointRota {
                    point = lastPoint, rota = lastRota,
                });
            }
        }

        private void OnEnabledSyncNetworkTransformCallBack(ISystemMsg body, SE_Network.Event_EnabledSyncNetworkTransform ent) {
            this.isEnabled = ent.IsTrue;
        }

        public void OnSyncRotaToNetworkTransformCallBack(ISystemMsg body, SE_Entity.SyncRotaToNetworkTransform ent) {
            iEntity.SetRota(ent.Rota);
            if (ent.OR_IsSync) {
                lastRota = iEntity.GetRota();
                Rpc(new Proto_Network.Rpc_SyncTransformRota() {
                    rota = ent.Rota,
                });
            }
        }

        public void OnSyncScaleToNetworkTransformCallBack(ISystemMsg body, SE_Entity.SyncScaleToNetworkTransform ent) {
            iEntity.SetLocalScale(ent.Scale);
            if (ent.OR_IsSync) {
                lastScale = iEntity.GetLocalScale();
                Rpc(new Proto_Network.Rpc_SyncTransformScale() {
                    scale = ent.Scale,
                });
            }
        }

        public void OnSyncPointToNetworkTransformallBack(ISystemMsg body, SE_Entity.SyncPointToNetworkTransform ent) {
            iEntity.SetPoint(ent.Point);
            if (ent.OR_IsSync) {
                lastPoint = iEntity.GetPoint();
                Rpc(new Proto_Network.Rpc_SyncTransformPoint() {
                    point = ent.Point,
                });
            }
        }

        private void OnUpdate(float delta) {
            if (this.isEnabled == false) {
                return;
            }
            if (constSyncTime >= 0f) {
                constSyncTime -= delta;
                return;
            }
            constSyncTime = syncTime;
            var isTrue = Compare();
            if (isTrue) {
                lastPoint = iEntity.GetPoint();
                lastRota = iEntity.GetRota();
                lastScale = iEntity.GetLocalScale();
                if (pointChanged && rotaChanged) {
                    Rpc(new Proto_Network.Rpc_TransformPointRota {
                        point = lastPoint, rota = lastRota,
                    });
                } else if (pointChanged) {
                    Rpc(new Proto_Network.Rpc_TransformPoint {
                        point = lastPoint,
                    });
                } else if (rotaChanged) {
                    Rpc(new Proto_Network.Rpc_TransformRota {
                        rota = lastRota,
                    });
                }
                
                if (scaleChanged) {
                    Rpc(new Proto_Network.Rpc_TransformScale {
                        scale = lastScale
                    });
                }
            }
        }

        private bool Compare() {
            pointChanged = syncPoint && Vector3.SqrMagnitude(iEntity.GetPoint() - lastPoint) >
                positionSensitivity * positionSensitivity;
            var angle = Quaternion.Angle(lastRota, iEntity.GetRota());
            rotaChanged = syncRota && angle > rotationSensitivity;
            scaleChanged = syncScale && Vector3.SqrMagnitude(iEntity.GetLocalScale() - lastScale) >
                scaleSensitivity * scaleSensitivity;
            return (pointChanged || rotaChanged || scaleChanged);
        }
    }
}