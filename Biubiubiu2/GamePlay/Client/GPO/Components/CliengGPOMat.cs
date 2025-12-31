using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CliengGPOMat : ComponentBase {
        private Dictionary<string, byte> matTypeDic = new Dictionary<string, byte>();
        private Dictionary<string, byte> protoMatTypeDic = new Dictionary<string, byte>();
        private GameObject gpoObject;
        private Dictionary<string, byte> teamIdMatTypeDic = new Dictionary<string, byte>();

        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnClear() {
            RemoveProtoCallBack(Proto_GPO.TargetRpc_GpoMat.ID, OnTargetRpcGpoMat);
            RemoveProtoCallBack(Proto_GPO.Rpc_GpoMat.ID, OnRpcGpoMat);
            matTypeDic.Clear();
            foreach (var item in teamIdMatTypeDic) {
                var teamId = item.Key;
                var matType = item.Value;
                MsgRegister.Dispatcher(new CM_Sausage.Event_SetGpoMatType {
                    GpoId = iGPO.GetGpoID(),
                    TeamId = teamId,

                    MatType = matType,
                    IsForward = false,
                    GpoObject = gpoObject
                });
            }
            protoMatTypeDic.Clear();
            gpoObject = null;
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entityBase = (EntityBase)iEntity;
            gpoObject = entityBase.gameObject;
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_GPO.TargetRpc_GpoMat.ID, OnTargetRpcGpoMat);
            AddProtoCallBack(Proto_GPO.Rpc_GpoMat.ID, OnRpcGpoMat);
        }

        private void OnTargetRpcGpoMat(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_GPO.TargetRpc_GpoMat)cmdData;
            protoMatTypeDic[data.teamId] = data.matType;
        }

        private void OnRpcGpoMat(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_GPO.Rpc_GpoMat)cmdData;
            protoMatTypeDic[data.teamId] = data.matType;
        }

        private void OnUpdate(float deltaTime) {
            if (isSetEntityObj == false) {
                return;
            }
            if (protoMatTypeDic.Count > 0) {
                foreach (var item in protoMatTypeDic) {
                    var teamId = item.Key;
                    var matType = item.Value;
                    var isSend = true;
                    byte saveMatType = 0;
                    var sendMatType = matType;
                    if (matTypeDic.TryGetValue(teamId, out saveMatType) == false) {
                        matTypeDic.Add(teamId, matType);
                    } else {
                        if (saveMatType == matType) {
                            isSend = false;
                        } else {
                            if (matType == 0) {
                                sendMatType = matTypeDic[teamId];
                            }
                            matTypeDic[teamId] = matType;
                        }
                    }
                    if (isSend) {
                        var isForward = matType != 0;
                        MsgRegister.Dispatcher(new CM_Sausage.Event_SetGpoMatType {
                            GpoId = iGPO.GetGpoID(),
                            TeamId = teamId,
                            MatType = sendMatType,
                            IsForward = matType != 0,
                            GpoObject = gpoObject
                        });
                        if (isForward == false) {
                            if (teamIdMatTypeDic.ContainsKey(teamId)) {
                                teamIdMatTypeDic.Remove(teamId);
                            }
                        } else {
                            teamIdMatTypeDic[teamId] = sendMatType;
                        }
                    }
                }
                protoMatTypeDic.Clear();
            }
        }
    }
}