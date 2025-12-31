using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerEventActionNotifier : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_EventDirector.Event_WaitAction>(OnWaitActionCallBack);
            MsgRegister.Register<SM_EventDirector.Event_EnterAction>(OEnterActionCallBack);
            MsgRegister.Register<SM_EventDirector.Event_QuitAction>(OnQuitActionCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_EventDirector.Event_WaitAction>(OnWaitActionCallBack);
            MsgRegister.Unregister<SM_EventDirector.Event_EnterAction>(OEnterActionCallBack);
            MsgRegister.Unregister<SM_EventDirector.Event_QuitAction>(OnQuitActionCallBack);
        }
        
        private void OnWaitActionCallBack(SM_EventDirector.Event_WaitAction ent) {
            Rpc(new Proto_EventDirector.Rpc_WaitAction {
                eventId = ent.ID,
                eventDataId = (ushort)ent.EventData.EventId,
                waitTime = ent.WaitTime,
                actionType = (ushort)ent.ActionType,
                gpoIds = GetGpoIds(ent.GpoList)
            });
        }
        
        private void OEnterActionCallBack(SM_EventDirector.Event_EnterAction ent) {
            Rpc(new Proto_EventDirector.Rpc_EnterAction {
                eventId = ent.ID,
                eventDataId = (ushort)ent.EventData.EventId,
                actionType = (ushort)ent.ActionType,
                gpoIds = GetGpoIds(ent.GpoList)
            });
        }

        private void OnQuitActionCallBack(SM_EventDirector.Event_QuitAction ent) {
            Rpc(new Proto_EventDirector.Rpc_QuitAction {
                eventId = ent.ID,
                eventDataId = (ushort)ent.EventData.EventId,
                actionType = (ushort)ent.ActionType,
                gpoIds = GetGpoIds(ent.GpoList)
            });
        }
        
        private List<int> GetGpoIds (List<IGPO> gpoList) {
            var ids = new List<int>();
            foreach (var gpo in gpoList) {
                ids.Add(gpo.GetGpoID());
            }
            return ids;
        }
    }
}