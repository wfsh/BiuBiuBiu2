using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterSceneElement : ServerCharacterComponent {
        private bool isGathering = false;
        protected override void OnAwake() {
            mySystem.Register<SE_Character.Event_GatheringState>(OnGatheringStateCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Character.Cmd_MoveDir.ID, OnMoveDirCallBack);
            AddProtoCallBack(Proto_Scene.Cmd_GatheringElement.ID, OnGatheringResourceCallBack);
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_Character.Event_GatheringState>(OnGatheringStateCallBack);
            RemoveProtoCallBack(Proto_Scene.Cmd_GatheringElement.ID, OnGatheringResourceCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_MoveDir.ID, OnMoveDirCallBack);
        }

        private void OnGatheringStateCallBack(ISystemMsg body, SE_Character.Event_GatheringState ent) {
            if (isGathering == ent.State) {
                return;
            }
            isGathering = ent.State;
            TargetRpc(networkBase, new Proto_Scene.TargetRpc_GatheringElement() {
                state = ent.State, count = ent.Count,
            });
        }

        private void OnGatheringResourceCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            var data = (Proto_Scene.Cmd_GatheringElement)protoDoc;
            if (isGathering) {
                return;
            }
            MsgRegister.Dispatcher(new SM_SceneElement.Event_StartGathering {
                IGpo = iGPO, CreateId = data.createId
            });
        }

        private void OnMoveDirCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            if (isGathering == false) {
                return;
            }
            MsgRegister.Dispatcher(new SM_SceneElement.Event_CancelGathering {
                IGpo = iGPO,
            });
        }
    }
}