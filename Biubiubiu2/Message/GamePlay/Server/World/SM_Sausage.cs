using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public class SM_Sausage {
        public struct SausageFireBullet : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageFireBullet>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public AbilityData.PlayAbility_SausageBullet BulletData;
        }
        public struct SausagePlayAbility : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausagePlayAbility>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public IAbilityMData MData;
            public IAbilityInData InData;
        }

        public struct SausagePlayAbilityEffect : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausagePlayAbilityEffect>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public IGPO TargetGPO;
            public IAbilityEffectMData MData;
            public IAbilityEffectInData InData;
        }

        public struct SausageTakeMeleeDamageToMonster : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageTakeMeleeDamageToMonster>();

            public int GetID() => _id;

            // 下面写你的参数
            public long AttackerPlayerId;
            public int TargetGPOId;
            public int Hurt;
        }

        public struct AttackSausageCharacterDownHp : GamePlayEvent.IWorldEvent {
            // TODO: [ViE] 在更多伤害类型接入后需要调整枚举到正式的位置
            public enum AttackSourceType {
                Undefined,
                Gun,
                Ability,
            }

            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AttackSausageCharacterDownHp>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public int MonsterId;
            public AIData.AIQuality AIQuality;
            public float DownHp;
            public Vector3 AttackSourcePos;
            public bool IsHead;
            public AttackSourceType AttackType;
        }
        public struct AttackSausageAICharacterDownHp : GamePlayEvent.IWorldEvent {
            // TODO: [ViE] 在更多伤害类型接入后需要调整枚举到正式的位置
            public enum AttackSourceType {
                Undefined,
                Gun,
                Ability,
            }

            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AttackSausageAICharacterDownHp>();

            public int GetID() => _id;

            // 下面写你的参数
            public int AiId;
            public int MonsterId;
            public AIData.AIQuality AIQuality;
            public float DownHp;
            public Vector3 AttackSourcePos;
            public AttackSourceType AttackType;
        }
        
        public struct GetGroundExceptWater : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetGroundExceptWater>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 NowPoint;
            public Action<Vector3> CallBack;
        }

        public struct GetIsPointInRectangleIgnoreY : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetIsPointInRectangleIgnoreY>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 Pos;
            public Vector3[] Corners;
            public Action<bool> CallBack;
        }
        
        public struct HideGlodDashBossWall : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<HideGlodDashBossWall>();
            public int GetID() => _id;
            // 下面写你的参数
        }
        
        public struct MonsterKilledByRole : GamePlayEvent.IWorldEvent {
            // TODO: [ViE] 在更多伤害类型接入后需要调整枚举到正式的位置
            public enum AttackSourceType {
                Undefined,
                Gun,
                Ability,
                Melee,
            }

            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MonsterKilledByRole>();

            public int GetID() => _id;

            // 下面写你的参数
            public long AttackPlayerId;
            public IAI Iai;
            public int MonsterId;
            public string MonsterName;
            public float TimeUsedToDead; // 从第一次受到伤害到死亡的时间
            public Vector3 MonsterPos;
            public AttackSourceType AttackType;
            public AIData.AIQuality AIQuality;
            public List<IGPO> hurtHistoryGPOSet;
            public int AttackItemId;
        }
        
        public struct GetIsMonsterRayHit : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetIsMonsterRayHit>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 FirePos;
            public Vector3 FireDir;
            public Vector3 EndPoint;
            public Vector3 RolePos;
            public float AttackHeight;
            public float AttackWidth;
            public float THeight;
            public float TWidth;
            public float TLength;
            public Action<bool> CallBack;
        }
        
        public struct GetAttackBlock : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetAttackBlock>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 AttackPos;
            public Vector3 RolePos;
            public float AddOffsetY;
            public Action<bool> CallBack;
        }

        public struct SausageSetKnockDownStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageSetKnockDownStatus>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public bool IsKnockedDown;
        }

        public struct SausageSetIsBeRatBuffNoNeedFindByBoss : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageSetIsBeRatBuffNoNeedFindByBoss>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public bool IsBeRatBuffNoNeedFindByBoss;
        }
        
        public struct GetLerpForward : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetLerpForward>();
            public int GetID() => _id;
            // 下面写你的参数
            public Vector3 NowForward;
            public Vector3 TargetForward;
            public float LerpValue;
            public Action<Vector3> CallBack;
        }
        
        public struct SausageDead : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageDead>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
        }

        public struct SausageReLife : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageReLife>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public int Hp;
        }

        // TODO: [ViE] 测试用事件，后续需要排除正式环境
        public struct SausageSwitchAllBehavior : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageSwitchAllBehavior>();

            public int GetID() => _id;

            // 下面写你的参数
            public bool isEnabled;
        }

        public struct SausageSwitchAISight : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageSwitchAISight>();
            
            public int GetID() => _id;

            public long PlayerId;
            public float ReduceValue;
        }
        
        public struct SausageHit : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageHit>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO AttackGPO;
            public IGPO TargetGPO;
            public int GunAutoItemId;
            public bool IsHead;
            public float Distance;
            public int BuffDamage;
        }

        public struct BossReleaseAbility : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<BossReleaseAbility>();

            public int GetID() => _id;

            // 下面写你的参数
            public string SourceAbilityType;
        }

        public struct MonsterAbilityHit : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MonsterAbilityHit>();

            public int GetID() => _id;

            // 下面写你的参数
            public IGPO FireGPO;
            public string SourceAbilityType;
        }

        public struct GetSausageAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetSausageAI>();
            public int GetID() => _id;
            // 下面写你的参数
            public float Range;
            public Vector3 NowPoint;
            public Action<List<Vector3>, List<int>> CallBack;
        }

        public struct GetIsBlindingShield : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetIsBlindingShield>();
            public int GetID() => _id;

            public int GpoId;
            public Action<bool> Callback;
        }

        public struct StartKittyRadar : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<StartKittyRadar>();

            public int GetID() => _id;

            // 下面写你的参数
            public string SausageTeamId;
            public int KittyRadarId;
            public long AttackPlayerId;
            public float Range;
        }

        public struct EndKittyRadar : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<EndKittyRadar>();

            public int GetID() => _id;

            // 下面写你的参数
            public string SausageTeamId;
            public int KittyRadarId;
        }

        public struct BossFightFailed : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<BossFightFailed>();

            public int GetID() => _id;

            // 下面写你的参数
            public int BossGPOId;
        }

        public struct GoldDashFightRangeRemoved : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GoldDashFightRangeRemoved>();

            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct CreateBeatBackRangeCallBack : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CreateBeatBackRangeCallBack>();

            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 CenterPoint;
            public float BeatBackRadius;
        }

        public struct MonsterTriggerFightStatus : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<MonsterTriggerFightStatus>();

            public int GetID() => _id;

            // 下面写你的参数
            public IAI Iai;
            public bool IsTrigger;
        }

        public struct SaveWarReportCreatePVEAI : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveWarReportCreatePVEAI>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public Vector3 Point;
            public string MonsterSign;
            public int TeamId;
            public int GpoType;
            public int Level;
        }
        
        public struct GetCurTimestampMilliseconds : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetCurTimestampMilliseconds>();

            public int GetID() => _id;

            // 下面写你的参数
            public Action<long> CallBack;
        }
        
        public struct SaveWarReportBossFightFail : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveWarReportBossFightFail>();

            public int GetID() => _id;
            public int GpoId;
        }
        
        public struct SaveWarReportSetPointRota : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveWarReportSetPointRota>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public Vector3 Point;
            public Quaternion Rota;
        }

        public struct SaveWarReportPlayAnimId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveWarReportPlayAnimId>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public int AnimId;
        }

        public struct SaveWarReportSetHpChange : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveWarReportSetHpChange>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public float NowHp;
        }

        public struct SaveWarReportSetIsDead : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SaveWarReportSetIsDead>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
        }

        public struct LoadWarReportMoveToPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadWarReportMoveToPoint>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public Vector3 Point;
        }

        public struct LoadWarReportMoveToRota : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadWarReportMoveToRota>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public Quaternion Rota;
        }

        public struct LoadWarReportSetNowHp : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadWarReportSetNowHp>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public int NowHp;
        }

        public struct LoadWarReportSetIsDead : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadWarReportSetIsDead>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
        }

        public struct LoadWarReportPlayAnimId : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<LoadWarReportPlayAnimId>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public int AnimId;
        }

        public struct SausageSummerMasterMonster : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageSummerMasterMonster>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public string MonsterSign;
        }
        
        public struct RemoveFightRangeAbility : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveFightRangeAbility>();
            public int GetID() => _id;
        }
        
        public struct SausageStandType : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageStandType>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public CharacterData.StandType StandType;
        }
        
        public struct AddSausageRoleMoveForce : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AddSausageRoleMoveForce>();
            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public Vector3 CenterPoint;
        }
        
        public struct GetSausageRoleIsWeak : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetSausageRoleIsWeak>();
            public int GetID() => _id;
            // 下面写你的参数
            public long PlayerId;
            public Action<bool> Callback;
        }

        public struct GetSausageRoleIsFriendBubbleTarget : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetSausageRoleIsFriendBubbleTarget>();
            public int GetID() => _id;

            public long PlayerId;
            public Action<bool> Callback;
        }
        
        public struct SyncGoldJokerBossState : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SyncGoldJokerBossState>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
            public byte State;
            public Vector3 Pos;
            public int Time;
        }
        
        public struct GetSausageMonsterHateReduceDisValue : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<GetSausageMonsterHateReduceDisValue>();
            public int GetID() => _id;

            public long PlayerId;
            public Action<float> Callback;
        }
        
        public struct AttackMonster : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<AttackMonster>();

            public int GetID() => _id;

            // 下面写你的参数
            public long AttackPlayerId;
            public bool IsHead;
            public IGPO TargetGPO;
        }
        
        public struct RemoveHighLevelMonster : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveHighLevelMonster>();

            public int GetID() => _id;

            // 下面写你的参数
            public int GpoId;
        }
        
        public struct SausageTakeGunDamageToMonster : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageTakeGunDamageToMonster>();

            public int GetID() => _id;

            // 下面写你的参数
            public long AttackerPlayerId;
            public int TargetGPOId;
            public int AttackItemId;
            public int Hurt;
            public bool IsHead;
            public float HeadAddPowerRatio;
        }
        public struct SausageUpHp : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<SausageUpHp>();

            public int GetID() => _id;

            // 下面写你的参数
            public long PlayerId;
            public int UpHp;
        }

        public struct CutDownBlindingShield : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CutDownBlindingShield>();
            public int GetID() => _id;

            public IGPO attackGpo;
            public int gpoId;
            public float downValue;
        }

        public struct CheckBlindingShieldPoint : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<CheckBlindingShieldPoint>();
            public int GetID() => _id;

            public int gpoId;
            public Action<Vector3, Quaternion> Callback;
        }
    }
}