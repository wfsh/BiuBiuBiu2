using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Subject_PlayerTeam : EventDirectorSubject {
        private List<IGPO> gpoList = new List<IGPO>();
        private List<int> tempList = new List<int>();

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
            tempList.Clear();
            gpoList = null;
        }

        public override List<int> GetSubjectIds() {
            tempList.Clear();
            foreach (var gpo in gpoList) {
                var teamId = gpo.GetTeamID();
                if (teamId > 0 && !tempList.Contains(teamId)) {
                    tempList.Add(teamId);
                }
            }
            return tempList;
        }
    }
}
