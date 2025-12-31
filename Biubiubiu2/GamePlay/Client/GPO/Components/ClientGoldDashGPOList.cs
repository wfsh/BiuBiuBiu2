using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGoldDashGPOList : ClientGPOList {
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<CM_Sausage.GetSausageAI>(OnGetSausageAICallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_Sausage.GetSausageAI>(OnGetSausageAICallBack);
        }

        private void OnGetSausageAICallBack(CM_Sausage.GetSausageAI ent) {
            var pointList = new List<Vector3>();
            var idList = new List<int>();
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetGPOType() == GPOData.GPOType.Role) {
                    continue;
                }
                if (Vector3.Distance(gpo.GetPoint(), ent.NowPoint) < ent.Range) {
                    pointList.Add(gpo.GetPoint());
                    idList.Add(gpo.GetGpoID());
                }
            }
            ent.CallBack.Invoke(pointList, idList);
        }
    }
}