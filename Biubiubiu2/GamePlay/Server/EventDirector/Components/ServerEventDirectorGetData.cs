using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerEventDirectorGetData : ComponentBase {
        private List<EventDirectorData.Data> dataList = new List<EventDirectorData.Data>();

        protected override void OnStart() {
            dataList = EventDirectorData.GetEventList(ModeData.MatchId);
            mySystem.Dispatcher(new SE_EventDirector.SetEventList {
                List = dataList,
            });
        }

        protected override void OnClear() {
            dataList = null;
        }
    }
}