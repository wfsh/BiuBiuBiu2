using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Subject_PlayerAndAI : EventDirectorSubject {
        private List<IGPO> gpoList = new List<IGPO>();
        private List<int> playerList = new List<int>();

        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = list => {
                    gpoList = list;
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
            playerList.Clear();
            gpoList = null;
        }

        public override List<int> GetSubjectIds() {
            playerList.Clear();
            foreach (var gpo in gpoList) {
                if (gpo.GetGPOType() == GPOData.GPOType.Role || gpo.GetGPOType() == GPOData.GPOType.RoleAI) {
                    playerList.Add(gpo.GetGpoID());
                }
            }
            return playerList;
        }
    }
}
