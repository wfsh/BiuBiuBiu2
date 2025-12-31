using System;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.Message {
    // 玩法交互组件接口
    public interface IGPO {
        string GetName();
        string GetSign();
        Vector3 GetPoint();
        Quaternion GetRota();
        Vector3 GetForward();
        int GetGpoID();
        int GetTeamID();
        Transform GetBodyTran(GPOData.PartEnum gpoType);
        List<Transform> GetBodyTranList(GPOData.PartEnum gpoType);
        GPOData.GPOType GetGPOType();
        bool IsGodMode();
        bool IsClear();
        bool IsDead();
        INetwork GetNetwork();
        void Register<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent;
        void Register<T>(Action<ISystemMsg, T> handler, int priority) where T : GamePlayEvent.ISystemEvent;
        void Unregister<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent;
        void Dispatcher<T>(T ent) where T : GamePlayEvent.ISystemEvent;
        bool IsLocalGPO();
        Transform GetTargetTransform(); // 获取顺序 头 => 身体 > ROOT
        int GetGpoMID();
        int GetGpoMTypeID();
        IGPOM GetMData();
        GPOData.AttributeData GetAttributeData();
        List<IHitType> GetAllCanHitPart();
    }
}