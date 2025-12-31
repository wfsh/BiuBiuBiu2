using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Grpc;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SE_GPO {
        public struct Event_UpHP : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public int UpHp;
        }

        public struct Event_DownHP : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DownHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public int DownHp;
            public IGPO AttackGPO;
            public IGPO DownHpGPO;
            public int AttackItemId;
            public bool IsHead;
        }
        
        public struct Event_GetHPInfo : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHPInfo>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<int,int> CallBack;
        }
        
        public struct Event_AfterDownHP : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AfterDownHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public float DownHp;
            public float NowHp;
            public IGPO AttackGPO;
            public int AttackItemId;
        }

        public struct Event_MaxHPChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MaxHPChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public int MaxHp;
        }

        public struct Event_GPOHurt : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GPOHurt>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Hurt;
            public IGPO AttackGPO;
            public int AttackItemId; // 近战伤害: -1, 技能伤害: 0, 枪械伤害: ItemId
            public bool IsHead;
            public DamageType DamageType;
            public float HeadAddPowerRatio;
        }
        
        public struct Event_TypeDamageCount : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TypeDamageCount>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO AttackGPO;
            public int Damage;
            public int AttackItemId;
            public DamageType DamageType;
        }

        public struct Event_GetGPOAttacDuration : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGPOAttacDuration>();

            public int GetID() => _id;
            // 下面写你的参数
            public int AttackGPOId;
            public Action<float> CallBack;
        }
        
        public struct Event_GetGPOShortTimeDamage : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetGPOShortTimeDamage>();

            public int GetID() => _id;
            // 下面写你的参数
            public int AttackGPOId;
            public Action<int> CallBack;
        }


        public struct Event_SetHurtTOGpo : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetHurtTOGpo>();

            public int GetID() => _id;
            // 下面写你的参数
            public int HurtValue;
            public IGPO AttackGPO;
            public IGPO HurtGPO;
            public bool IsHead;
            public DamageType DamageType;
            public int AttackItemId;
        }

        public struct Event_ATKChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ATKChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public int ATK;
        }

        public struct Event_HPChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HPChange>();

            public int GetID() => _id;
            // 下面写你的参数
            public int HP;
        }
        public struct Event_GetATK : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetATK>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }        
        public struct Event_GetHeadHurtRate : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHeadHurtRate>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct Event_GetRandomATK : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetRandomATK>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }

        public struct Event_GetRandomDiffusionReduction : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetRandomDiffusionReduction>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct Event_GetAttributeData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAttributeData>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<GPOData.AttributeData> CallBack;
        }
        public struct Event_GetHurtValueForDamageType : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHurtValueForDamageType>();

            public int GetID() => _id;
            // 下面写你的参数
            public DamageType Type;
            public int DamageValue;
            public Action<float> CallBack;
        }
        public struct Event_UpdateAttribute : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateAttribute>();
            public int GetID() => _id;

            // 下面写你的参数
            public GPOData.AttributeData Data;
        }
        public struct Event_UpdateSpeed : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateSpeed>();
            public int GetID() => _id;

            // 下面写你的参数
            public float Speed;
        }
        public struct Event_GetAttackRange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAttackRange>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct Event_GetAddEffectFireDistanceRate : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAddEffectFireDistanceRate>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float> CallBack;
        }
        public struct Event_SetIsDead : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetIsDead>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsDead;
            public IGPO DeadGpo;
        }

        // 是否已经死亡
        public struct Event_GetIsDead : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsDead>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct Event_SetMonsterAerocraftSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetMonsterAerocraftSign>();

            public int GetID() => _id;
            // 下面写你的参数
            public string AerocraftSign;
        }

        public struct Event_SetPackAerocraftSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetPackAerocraftSign>();

            public int GetID() => _id;
            // 下面写你的参数
            public string AerocraftSign;
        }

        public struct Event_KillGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_KillGPO>();

            public int GetID() => _id;
            // 下面写你的参数
            public IGPO AttackGPO;
            public IGPO DeadGPO;
        }

        public struct Event_MovePointEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MovePointEnd>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        // GPO 击退计算
        public struct Event_KnockbackGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_KnockbackGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Dir;  // 朝向
            public float Speed;  // 击飞速度
            public float Duration;  // 击飞持续时间
        }

        // GPO 击退移动
        public struct Event_KnockbackMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_KnockbackMovePoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 KnockbackMove;
        }

        // GPO 击飞计算
        public struct Event_StrikeFlyGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StrikeFlyGPO>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Dir;   // 朝向
            public float Force;   // 击飞力度
            public float Duration; // 击飞持续时间
        }

        // GPO 击飞
        public struct Event_StrikeFlyMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StrikeFlyMovePoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 StrikeFlyMove;
        }

        public struct Event_IsGodMode : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsGodMode>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsTrue;
        }
        public struct GetAttackPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetAttackPoint>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<Vector3> CallBack;
        }
        // 当前移动朝向
        public struct Event_MoveDir: GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MoveDir>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 MoveDir;
        }

        // 重生
        public struct Event_ReLife : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ReLife>();

            public int GetID() => _id;
            // 下面写你的参数
            public int UpHp;
        }

        // 直接死亡
        public struct Event_OnSetDead : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnSetDead>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        // 直接死亡
        public struct Event_CheckOneShotKillTwoTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CheckOneShotKillTwoTime>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct SetWeaponBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetWeaponBullet>();
            public int GetID() => _id;

            // 下面写你的参数
            public int BulletCount;
            public int UseItemId;
        }

        public struct Event_OnUseBullet : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnUseBullet>();
            public int GetID() => _id;

            // 下面写你的参数
            public int BulletId;
        }

        public struct Event_OnGunFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnGunFire>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 StartPoint;
            public Vector3 TargetPoint;
        }
        
        public struct Event_GunReload : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GunReload>();
            public int GetID() => _id;

            // 下面写你的参数
        }
        
        public struct Event_GunFireEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GunFireEnd>();
            public int GetID() => _id;

            // 下面写你的参数
        }


        // 通知别人我当前使用的武器是什么
        public struct UseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public IWeapon Weapon;
        }

        // 设置需要使用的武器
        public struct SetUseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetUseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public int UseGPOId;
            public IWeapon Weapon;
            public Action<WeaponData.UseBulletData> FireCallBack;
            public Action<IWeapon> PutAwakeWeaponCallBack;
        }

        public struct GetUseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetUseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<IWeapon> CallBack;
        }

        // 设置需要卸下使用的武器
        public struct SetCanceUseWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetCanceUseWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public int WeaponId;
        }

        // 武器背包是否还能够容纳武器
        public struct Event_CanInWeaponPack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_CanInWeaponPack>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct AddWeaponPack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AddWeaponPack>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AddWeaponId;
            public int AddWeaponSkinId;
        }

        public struct AddWeaponInPackSuccess : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AddWeaponInPackSuccess>();
            public int GetID() => _id;

            // 下面写你的参数
            public IWeapon Weapon;
        }

        public struct Event_RemoveAllWeaponPack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveAllWeaponPack>();
            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_RemoveWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RemoveWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public int WeaponId;
        }

        public struct Event_GetPackWeaponList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetPackWeaponList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<IWeapon>> CallBack;
        }

        public struct Event_OnEquipPackWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnEquipPackWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public int WeaponId; // 武器 ID 填 0 默认装备第一把武器
        }

        public struct Event_OnTakeBackWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnTakeBackWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public int WeaponId;
        }

        public struct Event_OnFire1 : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnFire1>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsDown;
        }

        public struct Event_OnFire2 : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnFire2>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsDown;
        }

        public struct Event_GroundDistance : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GroundDistance>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
            public float GroundDis;
        }

        public struct Event_Fall : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_Fall>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FallValue;
        }

        public struct Event_JumpTypeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_JumpTypeChange>();
            public int GetID() => _id;

            // 下面写你的参数
            public CharacterData.JumpType JumpType;
            public float jumpHeight;
        }


        public struct Event_Jump : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_Jump>();
            public int GetID() => _id;
            // 下面写你的参数
            public int GPOId;
            public CharacterData.JumpType JumpType;
        }

        public struct Event_Slide : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_Slide>();
            public int GetID() => _id;
            // 下面写你的参数
            public int GPOId;
            public bool IsSlide;
        }
        
        public struct Event_GetState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetState>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool,bool,bool,CharacterData.JumpType> CallBack;
        }
        public struct Event_ContinueKillNum : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_ContinueKillNum>();
            public int GetID() => _id;
            // 下面写你的参数
            public IGPO GPO;
            public int ContinueKillNum;
        }
        public struct Event_GetContinueKillState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetContinueKillState>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool,bool,int> CallBack;
        }

        public struct Event_MoveDistance : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_MoveDistance>();
            public int GetID() => _id;
            // 下面写你的参数
            public int GPOId;
            public float moveDistance;
        }

        public struct Event_FlyTypeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FlyTypeChange>();
            public int GetID() => _id;

            // 下面写你的参数
            public CharacterData.FlyType FlyType;
        }

        public struct Event_FallToGrounded : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FallToGrounded>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct Event_IsGround : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsGround>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
        }
        // 滑产
        public struct Event_SlideMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SlideMove>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 SlideVelocity;
            public bool IsSlide;
        }

        // 驾驶员状态
        public struct Event_PlayerDrive : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayerDrive>();
            public int GetID() => _id;

            // 下面写你的参数
            public IGPO DriveGPO; // 驾驶的 GPO
            public bool IsDrive;
        }

        public struct Event_BecomeTargetProtectTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_BecomeTargetProtectTime>();
            public int GetID() => _id;

            // 下面写你的参数
            public float ProtectTime;
        }

        public struct Event_GetHitRatio : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHitRatio>();

            public int GetID() => _id;
            // 下面写你的参数
            public GPOData.GPOType HitGpoType;
            public Action<float> CallBack;
        }
        
        public struct Event_GetHitHeadAccuracy : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetHitHeadAccuracy>();

            public int GetID() => _id;
            // 下面写你的参数
            public GPOData.GPOType HitGpoType;
            public Action<float> CallBack;
        }

        public struct Event_SetAIConfig : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetAIConfig>();

            public int GetID() => _id;
            // 下面写你的参数
            public AiConfig Config;
        }

        public struct Event_GetAISlideMoveActionRatioTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAISlideMoveActionRatioTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float, float> CallBack;
        }

        public struct Event_SetAISlideMoveActionRatioTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetAISlideMoveActionRatioTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public float SlideMinIntervalTime;
            public float SlideMaxIntervalTime;
        }

        public struct Event_GetAIJumpActionRatioTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAIJumpActionRatioTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<float, float> CallBack;
        }

        public struct Event_SetAIJumpActionRatioTime : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetAIJumpActionRatioTime>();

            public int GetID() => _id;
            // 下面写你的参数
            public float JumpMinIntervalTime;
            public float JumpMaxIntervalTime;
        }
        // 删除 GPO
        public struct Event_StartRemoveGPO : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StartRemoveGPO>();

            public int GetID() => _id;
            // 下面写你的参数
        }
        
        // 删除 GPO
        public struct Event_OnGetFollowPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnGetFollowPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public GPOData.FollowPointType pointType;
            public IGPO useGPO;
            public Action<GameObject> CallBack;
        }
        
        public struct Event_OnBackFollowPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_OnBackFollowPoint>();

            public int GetID() => _id;
            // 下面写你的参数
            public GPOData.FollowPointType pointType;
            public IGPO useGPO;
        }

        public struct Event_SetCheckInRoomEnable : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetCheckInRoomEnable>();

            public int GetID() => _id;
            // 下面写你的参数
            public bool IsEnable;
        }

        public struct Event_GetIsInRoom : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetIsInRoom>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<bool> CallBack;
        }
        
        public struct Event_IsCanUseCallAISkill : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_IsCanUseCallAISkill>();

            public int GetID() => _id;
            // 下面写你的参数
            public float Width;
            public float Height;
            public Action<bool> CallBack;
        }
        public struct Event_SetKillWeaponId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetKillWeaponId>();

            public int GetID() => _id;
            // 下面写你的参数
            public int KillWeaponId;
        }
        public struct Event_RoundEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_RoundEnd>();

            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_SetMaxHP : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetMaxHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public int MaxHp;
            public bool IsSyncSetHp;
        }

        public struct Event_SetHP : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetHP>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Hp;
        }

        public struct Event_PlayAnimIdEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayAnimIdEnd>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AnimId;
        }

        public struct Event_PlayAnimId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayAnimId>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AnimId;
        }

        public struct Event_PlayWarReportAnimIdStart : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayWarReportAnimIdStart>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AnimId;
        }
        
        public struct Event_WarReportBossFightFail : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_WarReportBossFightFail>();
            public int GetID() => _id;
        }

        public struct Event_GetDropBoxId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetDropBoxId>();
            public int GetID() => _id;
            // 下面写你的参数
            public Action<int> CallBack;
        }

        // 获取最近目标的距离
        public struct Event_SetLevel : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetLevel>();

            public int GetID() => _id;
            // 下面写你的参数
            public int Level;
        }
        
        public struct Event_GetLastAttackGpo : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetLastAttackGpo>();

            public int GetID() => _id;
            // 下面写你的参数
            public Action<IGPO> CallBack;
        }
        
        public struct Event_PlayTurn : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayTurn>();
            public int GetID() => _id;
        }        
        
        
        public struct Event_PlayAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlayAttack>();
            public int GetID() => _id;
        }
    }
}