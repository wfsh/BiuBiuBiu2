using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EventDirectorSubject : EventDirectorBase, IEventDirectorSubject {
        protected EventDirectorData.SubjectType subjectType = EventDirectorData.SubjectType.None;
        
        public void SetSubjectData(EventDirectorData.SubjectType type) {
            subjectType = type;
        }
        
        virtual public List<int> GetSubjectIds() {
            return new List<int>();
        }
    }
}
