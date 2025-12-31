using System;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;


namespace Sofunny.BiuBiuBiu2.Message {
    public interface ISystemMsg {
        void Register<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent;
        void Register<T>(Action<ISystemMsg, T> handler, int priority) where T : GamePlayEvent.ISystemEvent;
        void Unregister<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent;
        void Dispatcher<T>(T ent) where T : GamePlayEvent.ISystemEvent;
        IGPO GetGPO();
        int GetGPOId();
        bool IsClear();
    }
    // AI 对象返回，不包含真人玩家
    public interface IAI : ISystemMsg {
        GPOData.AttributeData GetAttributeData();
        IGPO GetMasterGPO();
    }
    
    // 武器返回接口
    public interface IWeapon : ISystemMsg {
        WeaponData.Data GetData();
        IGPO UseGPO();
        int GetWeaponId();
        int GetWeaponItemId();
        int GetWeaponSkinItemId();
        string GetWeaponSign();
        void SetParent(Transform parent);
        WeaponData.WeaponType GetWeaponType();
    }
    
    // 技能返回接口
    public interface IAbilitySystem : ISystemMsg {
        int GetAbilityId();
        Vector3 GetPoint();
        IGPO GetFireGPO();
    }
}
