using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientNetworkSerialize : ComponentBase {
        private ByteBuffer writeByteBuffer = ByteBuffer.Allocate();
        private ByteBuffer readByteBuffer = ByteBuffer.Allocate();
        protected override void OnAwake() {
            MsgRegister.Register<M_Network.CMDSerialize>(OnCMDSerializeCallBack);
            MsgRegister.Register<M_Network.RPCUnSerialize>(OnRPCUnSerializeCallBack);
            MsgRegister.Register<M_Network.RPCSyncUnSerialize>(OnRPCSyncUnSerializeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Network.CMDSerialize>(OnCMDSerializeCallBack);
            MsgRegister.Unregister<M_Network.RPCUnSerialize>(OnRPCUnSerializeCallBack);
            MsgRegister.Unregister<M_Network.RPCSyncUnSerialize>(OnRPCSyncUnSerializeCallBack);
        }

        private void OnCMDSerializeCallBack(M_Network.CMDSerialize ent) {
            byte[] sendBytes;
            using (writeByteBuffer) {
                ent.ProtoDoc.Serialize(writeByteBuffer);
                sendBytes = new byte[writeByteBuffer.WriteIndex];
                Array.Copy(writeByteBuffer.RawBuffer, 0, sendBytes, 0, writeByteBuffer.WriteIndex);
            }
            PerfAnalyzerAgent.NetworkOut(ent.ProtoDoc.GetID(), sendBytes.Length);
            ent.CallBack(ent.Channel, sendBytes);
        }

        private void OnRPCUnSerializeCallBack(M_Network.RPCUnSerialize ent) {
            IProto_Doc protoDoc = null;
            try {
                using (readByteBuffer) {
                    readByteBuffer.OnWrap(ent.Datas);
                    protoDoc = UnSerializeBuffer(readByteBuffer);
                }
                PerfAnalyzerAgent.NetworkIn(protoDoc.GetID(), ent.Datas.Length);
            } catch (Exception e) {
                Debug.LogError("Client 反序列化失败:" + e);
            }
            ent.CallBack(ent.ConnID, protoDoc);
        }

        private void OnRPCSyncUnSerializeCallBack(M_Network.RPCSyncUnSerialize ent) {
            IProto_Doc protoDoc = null;
            try {
                using (readByteBuffer) {
                    readByteBuffer.OnWrap(ent.Datas);
                    protoDoc = UnSerializeBuffer(readByteBuffer);
                }
                PerfAnalyzerAgent.NetworkIn(protoDoc.GetID(), ent.Datas.Length);
            } catch (Exception e) {
                Debug.LogError("Client 反序列化失败:" + e);
            }
            ent.CallBack(ent.ConnID, ent.SpawnType, protoDoc);
        }

        public static IProto_Doc UnSerializeBuffer(ByteBuffer buffer) {
            IProto_Doc protoDoc = null;
            var modId = buffer.ReadByte();
            var funcId = buffer.ReadByte();
            switch (modId) {
                case Proto_Login.ModID:
                    protoDoc = Proto_Login.ReadRpcBuffer(funcId);
                    break;
                case Proto_Character.ModID:
                    protoDoc = Proto_Character.ReadRpcBuffer(funcId);
                    break;
                case Proto_Ability.ModID:
                    protoDoc = Proto_Ability.ReadRpcBuffer(funcId);
                    break;
                case Proto_Weapon.ModID:
                    protoDoc = Proto_Weapon.ReadRpcBuffer(funcId);
                    break;
                case Proto_Network.ModID:
                    protoDoc = Proto_Network.ReadRpcBuffer(funcId);
                    break;
                case Proto_AI.ModID:
                    protoDoc = Proto_AI.ReadRpcBuffer(funcId);
                    break;
                case Proto_GPO.ModID:
                    protoDoc = Proto_GPO.ReadRpcBuffer(funcId);
                    break;
                case Proto_Item.ModID:
                    protoDoc = Proto_Item.ReadRpcBuffer(funcId);
                    break;
                case Proto_Scene.ModID:
                    protoDoc = Proto_Scene.ReadRpcBuffer(funcId);
                    break;
                case Proto_Mode.ModID:
                    protoDoc = Proto_Mode.ReadRpcBuffer(funcId);
                    break;
                case Proto_Wirebug.ModID:
                    protoDoc = Proto_Wirebug.ReadRpcBuffer(funcId);
                    break;
                case Proto_Drive.ModID:
                    protoDoc = Proto_Drive.ReadRpcBuffer(funcId);
                    break;
                case Proto_Skill.ModID:
                    protoDoc = Proto_Skill.ReadRpcBuffer(funcId);
                    break;
                case Proto_AbilityAB_Auto.ModID:
                    protoDoc = Proto_AbilityAB_Auto.ReadRpcBuffer(funcId);
                    break;
                case Proto_AbilityAE_Auto.ModID:
                    protoDoc = Proto_AbilityAE_Auto.ReadRpcBuffer(funcId);
                    break;
                case Proto_GPOSpawnerWave.ModID:
                    protoDoc = Proto_GPOSpawnerWave.ReadRpcBuffer(funcId);
                    break;
                case Proto_EventDirector.ModID:
                    protoDoc = Proto_EventDirector.ReadRpcBuffer(funcId);
                    break;
                default:
                    Debug.LogError("[Client] RPC 没有注册对应的反序列化:" + modId);
                    return null;
            }
            protoDoc.UnSerialize(buffer);
            return protoDoc;
        }
    }
}