using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Message {
    public interface IHitType {
        GPOData.PartEnum GetPart();
        Transform GetTransform();
    }
    public interface IEntity {
        Vector3 GetPoint();
        Vector3 GetForward();
        Quaternion GetRota();
        Vector3 GetEulerAngles();
        void SetPoint(Vector3 point);
        void SetRota(Quaternion rota);
        void SetLocalScale(Vector3 scale);
        Vector3 GetLocalScale();
        void SetEulerAngles(Vector3 eulerAngles);
        void LookAT(Vector3 point);
        void SetGpo(IGPO iGPO);
        void SetConnID(int connId);
        IGPO GetGPO();
        int GetGPOID();
        int GetTeamID();
        string GetName();
        void Clear();
        bool IsClear();
        void StartDebug();
        void SetParent(Transform parent);
        Transform GetParent();
        Transform GetBodyTran(GPOData.PartEnum gpoType);
        List<Transform> GetBodyTranList(GPOData.PartEnum gpoType);
    }
}
