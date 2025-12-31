using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientEventActionNotifier : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_EventDirector.Rpc_WaitAction.ID, OnRpcWaitActionCallBack);
            AddProtoCallBack(Proto_EventDirector.Rpc_EnterAction.ID, OnRpcEnterActionCallBack);
            AddProtoCallBack(Proto_EventDirector.Rpc_QuitAction.ID, OnRpcQuitActionCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_EventDirector.Rpc_WaitAction.ID, OnRpcWaitActionCallBack);
            RemoveProtoCallBack(Proto_EventDirector.Rpc_EnterAction.ID, OnRpcEnterActionCallBack);
            RemoveProtoCallBack(Proto_EventDirector.Rpc_QuitAction.ID, OnRpcQuitActionCallBack);
        }

        private void OnRpcWaitActionCallBack(INetwork network, IProto_Doc doc) {
            var data = (Proto_EventDirector.Rpc_WaitAction) doc;
            var eventData = EventDirectorData.GetEventData(data.eventDataId);
            ShowTip("等待进入", eventData.EnterActionTipType, eventData, (EventDirectorData.ActionType)data.actionType);
        }

        private void OnRpcEnterActionCallBack(INetwork network, IProto_Doc doc) {
            var data = (Proto_EventDirector.Rpc_EnterAction) doc;
            var eventData = EventDirectorData.GetEventData(data.eventDataId);
            ShowTip("进入", eventData.EnterActionTipType, eventData, (EventDirectorData.ActionType)data.actionType);
        }

        private void OnRpcQuitActionCallBack(INetwork network, IProto_Doc doc) {
            var data = (Proto_EventDirector.Rpc_QuitAction) doc;
            var eventData = EventDirectorData.GetEventData(data.eventDataId);
            ShowTip("退出", eventData.QuitActionTipType,  eventData, (EventDirectorData.ActionType)data.actionType);
        }

        private void ShowTip(string title, EventDirectorData.ShowTipType tipType, EventDirectorData.Data data, EventDirectorData.ActionType actionType) {
            if (tipType == EventDirectorData.ShowTipType.None) {
                return;
            }
            var info = $"{title}事件【{data.EventName}】 - 类型:{actionType}";
            if (tipType == EventDirectorData.ShowTipType.DialogTip) {
                MsgRegister.Dispatcher(new CM_UI.ShowDialog {
                    Message = info,
                    BtnSureText = "确认",
                });
            } else {
                MsgRegister.Dispatcher(new CM_UI.ShowToast {
                    Message = info,
                });
            }
        }
    }
}