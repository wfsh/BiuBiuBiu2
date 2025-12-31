using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerNetworkSerialize : ComponentBase {
        private ByteBuffer writeByteBuffer = ByteBuffer.Allocate();
        private ByteBuffer readByteBuffer = ByteBuffer.Allocate();

        protected override void OnAwake() {
            MsgRegister.Register<M_Network.RPCSerialize>(OnRPCSerializeCallBack);
            MsgRegister.Register<M_Network.TargetRPCSerialize>(OnTargetRPCSerializeCallBack);
            MsgRegister.Register<M_Network.CMDUnSerialize>(OnCMDUnSerializeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.RPCSerialize>(OnRPCSerializeCallBack);
            MsgRegister.Unregister<M_Network.TargetRPCSerialize>(OnTargetRPCSerializeCallBack);
            MsgRegister.Unregister<M_Network.CMDUnSerialize>(OnCMDUnSerializeCallBack);
        }

        private void OnRPCSerializeCallBack(M_Network.RPCSerialize ent) {
            byte[] sendBytes;
            using (writeByteBuffer) {
                ent.ProtoDoc.Serialize(writeByteBuffer);
                sendBytes = new byte[writeByteBuffer.WriteIndex];
                Array.Copy(writeByteBuffer.RawBuffer, 0, sendBytes, 0, writeByteBuffer.WriteIndex);
            }
            ent.CallBack(ent.Channel, ent.ConnID, sendBytes);
        }

        private void OnTargetRPCSerializeCallBack(M_Network.TargetRPCSerialize ent) {
            // Debug.Log(ent.ProtoDoc.GetID());
            byte[] sendBytes;
            using (writeByteBuffer) {
                ent.ProtoDoc.Serialize(writeByteBuffer);
                sendBytes = new byte[writeByteBuffer.WriteIndex];
                Array.Copy(writeByteBuffer.RawBuffer, 0, sendBytes, 0, writeByteBuffer.WriteIndex);
            }
            ent.CallBack(ent.Channel, ent.ConnID, sendBytes, ent.TargetNetwork, ent.ProtoDoc.GetID(), true);
        }

        private void OnCMDUnSerializeCallBack(M_Network.CMDUnSerialize ent) {
            IProto_Doc protoDoc = null;
            try {
                using (readByteBuffer) {
                    readByteBuffer.OnWrap(ent.Datas);
                    protoDoc = UnSerialize(readByteBuffer);
                }
                if (NetworkData.IsStartClient == false) {
                    PerfAnalyzerAgent.NetworkIn(protoDoc.GetID(), ent.Datas.Length);
                }
            } catch (Exception e) {
                Debug.LogError("Client 反序列化失败:" + e);
            }
            ent.CallBack(protoDoc);
        }

        public IProto_Doc UnSerialize(ByteBuffer buffer) {
            IProto_Doc protoDoc = null;
            var modId = buffer.ReadByte();
            var funcId = buffer.ReadByte();
            switch (modId) {
                case Proto_Login.ModID:
                    protoDoc = Proto_Login.ReadCmdBuffer(funcId);
                    break;
                case Proto_Character.ModID:
                    protoDoc = Proto_Character.ReadCmdBuffer(funcId);
                    break;
                case Proto_Ability.ModID:
                    protoDoc = Proto_Ability.ReadCmdBuffer(funcId);
                    break;
                case Proto_Weapon.ModID:
                    protoDoc = Proto_Weapon.ReadCmdBuffer(funcId);
                    break;
                case Proto_Network.ModID:
                    protoDoc = Proto_Network.ReadCmdBuffer(funcId);
                    break;
                case Proto_AI.ModID:
                    protoDoc = Proto_AI.ReadCmdBuffer(funcId);
                    break;
                case Proto_Item.ModID:
                    protoDoc = Proto_Item.ReadCmdBuffer(funcId);
                    break;
                case Proto_Scene.ModID:
                    protoDoc = Proto_Scene.ReadCmdBuffer(funcId);
                    break;
                case Proto_Mode.ModID:
                    protoDoc = Proto_Mode.ReadCmdBuffer(funcId);
                    break;
                case Proto_Wirebug.ModID:
                    protoDoc = Proto_Wirebug.ReadCmdBuffer(funcId);
                    break;
                case Proto_Drive.ModID:
                    protoDoc = Proto_Drive.ReadCmdBuffer(funcId);
                    break;
                case Proto_Skill.ModID:
                    protoDoc = Proto_Skill.ReadCmdBuffer(funcId);
                    break;
                case Proto_GPOSpawnerWave.ModID:
                    protoDoc = Proto_GPOSpawnerWave.ReadCmdBuffer(funcId);
                    break;
                case Proto_EventDirector.ModID:
                    protoDoc = Proto_EventDirector.ReadCmdBuffer(funcId);
                    break;
                default:
                    Debug.LogError("[Server] ICMD modId 没有注册对应的反序列化:" + modId);
                    return null;
            }
            protoDoc.UnSerialize(buffer);
            return protoDoc;
        }
    }
}