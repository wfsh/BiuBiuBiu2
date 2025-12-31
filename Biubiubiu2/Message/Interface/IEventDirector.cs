using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    
    public interface IEventDirectorData {
        void Serialize(string value);
    }
    
    public interface IEventDirectorBase {
        void SetData(string value);
        void Awake();
        int GetID();
    }
    public interface IEventDirectorTime {
        bool CheckInTime();
    }
    public interface IEventDirectorSubject {
        List<int> GetSubjectIds();
    }

    public interface IEventDirectorCondition {
        void SetID(int gpo, int subjectType);
        List<IGPO> GetGPOList();
        bool CheckCondition();
    }

    public interface IEventDirectorAction {
        void SetGpoList(List<IGPO> gpoList);
        void SetSubject(int subjectType, int subjectID);
        int GetSubjectID();
        int GetSubjectType();
        List<IGPO> GetGPOList();
    }
}
