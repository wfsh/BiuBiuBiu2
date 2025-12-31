using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerEventDirectorCore : ComponentBase {
        private const float UPDATE_INTERVAL = 1F;
        private int eventIndex = 0;
        private List<EventDirectorData.Data> dataList = new List<EventDirectorData.Data>();
        private List<EventDirectorInstance> events = new List<EventDirectorInstance>();
        private float updateInterval = 0f;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_EventDirector.SetEventList>(OnSetEventListCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            dataList = null;
            mySystem.Unregister<SE_EventDirector.SetEventList>(OnSetEventListCallBack);
            ClearInstance();
        }
        
        private void OnSetEventListCallBack(ISystemMsg body, SE_EventDirector.SetEventList ent) {
            dataList = ent.List;
            CreateInstance();
        }

        private void OnUpdate(float deltaTime) {
            updateInterval += deltaTime;
            if (updateInterval >= UPDATE_INTERVAL) {
                for (int i = 0; i < events.Count; i++) {
                    var instance = events[i];
                    instance.Update(updateInterval);
                }
                updateInterval = 0f;
            }
        }
        
        private void CreateInstance () {
            for (int i = 0; i < dataList.Count; i++) {
                eventIndex++;
                var data = dataList[i];
                var instance = new EventDirectorInstance();
                instance.Init(data, eventIndex);
                events.Add(instance);
            }
        }
        
        private void ClearInstance () {
            for (int i = 0; i < events.Count; i++) {
                var instance = events[i];
                instance.Clear();
            }
            events.Clear();
            eventIndex = 0;
        }
    }
}