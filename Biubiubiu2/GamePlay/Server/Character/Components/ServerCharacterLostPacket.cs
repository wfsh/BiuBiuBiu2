using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterLostPacket : ServerCharacterComponent {
        private string uid;
        private bool isStartOutLossData = false;
        protected override void OnClear() {
            base.OnClear();
            PerfAnalyzerAgent.RemoveListenerOutLossData(uid);
            uid = "";
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            var characterSystem = (ServerCharacterSystem)mySystem;
            uid = PlayerData.PlayerIDToUID(characterSystem.PlayerId);
            Debug.Log("OnSetNetwork " + characterSystem.PlayerId + " " + uid);
            AddProtoCallBack(Proto_Character.Cmd_ToServerPacketLossRate.ID, OnCmdToServerPacketLossRate);
            AddProtoCallBack(Proto_Character.Cmd_ToClientPacketLossRate.ID, OnCmdToClientPacketLossRate);
            if (isStartOutLossData == false) {
                PerfAnalyzerAgent.AddListenerOutLossData(uid, OutToClientLossData);
                isStartOutLossData = true;
            }
        }

        private void OutToClientLossData(ushort outIndex) {
            TargetRpc(networkBase, new Proto_Character.TargetRpc_ToClientPacketLossRate {
                index = outIndex
            });
        }

        private void OnCmdToServerPacketLossRate(INetwork network, IProto_Doc data) {
            var cmdData = (Proto_Character.Cmd_ToServerPacketLossRate)data;
            PerfAnalyzerAgent.SetInNetIndex(uid, cmdData.index);
            TargetRpc(network, new Proto_Character.TargetRpc_ToServerPacketLossRate {
                index = cmdData.index
            });
        }

        private void OnCmdToClientPacketLossRate(INetwork network, IProto_Doc data) {
            var cmdData = (Proto_Character.Cmd_ToClientPacketLossRate)data;
            PerfAnalyzerAgent.SetOutLossIndex(uid, cmdData.index);
        }
    }
}