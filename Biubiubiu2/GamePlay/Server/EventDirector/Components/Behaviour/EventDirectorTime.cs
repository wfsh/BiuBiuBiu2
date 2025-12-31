using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EventDirectorTime : EventDirectorBase, IEventDirectorTime {
        protected float StartTime = 0f;
        protected float EndTime = 0f;
        protected EventDirectorData.TimeType useTimeType = EventDirectorData.TimeType.AnyTime;
        
        virtual public bool CheckInTime() {
            return false;
        }
        
        public void SetTimeData(EventDirectorData.TimeType timeType, float startTime, float endTime) {
            StartTime = startTime;
            EndTime = endTime;
            useTimeType = timeType;
        }
    }
}
