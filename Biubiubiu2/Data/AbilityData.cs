using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public interface IAbilityMData {
        string GetTypeID();
        byte GetRowID();
        ushort GetConfigId();
        string GetEffectSign();
        void SetConfigData(IAbilityMData modMData);
        public void Select(Action callBack);
    }

    public interface IAbilityInData {
    }

    // 技能模板基类
#if UNITY_EDITOR
    public abstract class AbilityMData : ScriptableObject, IAbilityMData {
#else
    public abstract class AbilityMData : IAbilityMData {
#endif
        public string AbilityTypeId; // 对应AbilityType
        public ushort ConfigId; // 技能唯一ID
        public bool IsSyncCreateCAB = false;
        public bool IsEditorUseSO = false; // 仅编辑器使用SO，发布版本不使用SO
        
        virtual public string GetTypeID() => AbilityTypeId;
        virtual public ushort GetConfigId() => ConfigId;
        virtual public string GetEffectSign() => "";
        virtual public byte GetRowID() => 0;
        virtual public string GetRowSign() => "";

        public void SetConfigData(IAbilityMData modMData) {}

        virtual public void Select(Action callBack) {
            callBack?.Invoke();
        }
    }

    public partial class AbilityData {

        /// <summary>
        /// 目前可以使用的技能 - 服务端使用
        /// </summary>
        public const string SAB_RPG = "SAB_RPG";
        public const string SAB_AIBall = "SAB_MonsterBall";
        public const string SAB_Grenade = "SAB_Grenade";
        public const string SAB_HurtRangeAttack = "SAB_HurtRangeAttack";
        public const string SAB_RexKingFire = "SAB_RexKingFire";
        public const string SAB_Updraft = "SAB_Updraft";
        public const string SAB_PlatformMovement = "SAB_PlatformMovement";
        public const string SAB_SlipRope = "SAB_SlipRope";
        public const string SAB_BatLoopAttack = "SAB_BatLoopAttack";
        public const string SAB_BatEndAttack = "SAB_BatEndAttack";
        public const string SAB_PlungerAttackLoop = "SAB_PlungerAttackLoop";
        public const string SAB_PlungerEndAttack = "SAB_PlungerEndAttack";
        public const string SAB_UpHPForFireRole = "SAB_UpHPForFireRole";
        public const string SAB_PenetratorGrenade = "SAB_PenetratorGrenade";
        public const string SAB_FireGunFire = "SAB_FireGunFire";
        public const string SAB_TestFire = "SAB_TestFire";
        public const string SAB_Missile = "SAB_Missile";
        public const string SAB_MissileBomb = "SAB_MissileBomb";
        public const string SAB_SplitCone = "SAB_SplitCone";
        public const string SAB_BulletWithStartPoint = "SAB_BulletWithStartPoint";
        public const string SAB_TrackingMissle = "SAB_TrackingMissle";
        public const string SAB_RangeFlash = "SAB_RangeFlash";
        public const string SAB_GiantDaDaRollingStone = "SAB_GiantDaDaRollingStone";
        public const string SAB_GiantDaDaSurge = "SAB_GiantDaDaSurge";
        public const string SAB_GiantDaDaLightning = "SAB_GiantDaDaLightning";
        public const string SAB_GiantDaDaLightningWarning = "SAB_GiantDaDaLightningWarning";
        public const string SAB_PlayEffectWithFullDimensionScale = "SAB_PlayEffectWithFullDimensionScale";
        public const string SAB_PlayRotatingEffect = "SAB_PlayRotatingEffect";
        public const string SAB_PlayRotatingRayEffect = "SAB_PlayRotatingRayEffect";

        public const string SAB_SausageBomb = "SAB_SausageBomb";
        public const string SAB_SausageBullet = "SAB_SausageBullet";

        public const string SAB_AuroraDragonDelayBlast = "SAB_AuroraDragonDelayBlast";
        public const string SAB_DragonFullScreenAOE = "SAB_DragonFullScreenAOE";
        public const string SAB_PlayFillScaleEffect = "SAB_PlayFillScaleEffect";
        public const string SAB_PlayWarningEffect = "SAB_PlayWarningEffect";
        
        public const string SAB_Bullet = "SAB_Bullet";
        public const string SAB_BloodSplatter = "SAB_BloodSplatter";
        public const string SAB_PlayEffect = "SAB_PlayEffect";
        
        
        
        /// <summary>
        /// 目前可以使用的技能 - 客户端使用
        /// </summary>
        public const string CAB_PlatformMovement = "CAB_PlatformMovement";
    }
}