using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Subject_GpoMID : EventDirectorSubject {
        public struct InitData : IEventDirectorData {
            public int GpoMID;
            public void Serialize (string value) {
                GpoMID = int.Parse(value);
            }
        }
        private List<IGPO> gpoList = new List<IGPO>();
        private List<int> gpoMList = new List<int>();
        private InitData useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

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
            gpoMList.Clear();
            gpoList = null;
        }

        public override List<int> GetSubjectIds() {
            gpoMList.Clear();
            foreach (var gpo in gpoList) {
                if (gpo.GetGpoMID() == useMData.GpoMID) {
                    gpoMList.Add(gpo.GetGpoID());
                }
            }
            return gpoMList;
        }
    }
}
