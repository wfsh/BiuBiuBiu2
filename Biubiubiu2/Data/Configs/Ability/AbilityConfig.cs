using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityConfig {
        /// <summary>
        /// 技能标签，通过跟换模版数据可以使用不同的效果
        /// </summary>
        public const ushort BulletRPG = 4;
        public const ushort PenetratorGrenadeBullet = 8;
        public const ushort BulletSplitCone = 11;
        public const ushort BulletSplitSubBullet = 12;
        public const ushort SausageBullet = 14;
        public const ushort PlayEffect_LightningEndpoint = 28;
        public const ushort UAVTrackingMissle = 33;
        public const ushort RexKingAttack = 38;
        public const ushort RexKingFire = 41;
        public const ushort Missile = 43;
        public const ushort MissileBomb = 44;
        public const ushort FlashGunExplosive = 47;
        public const ushort SceneUpdraft = 52;
        public const ushort MonsterBall = 53;
        public const ushort BatLoopAttack = 54;
        public const ushort BatEndAttack = 55;
        public const ushort PlungerAttackLoop = 57;
        public const ushort PlungerEndAttack = 58;
        public const ushort ChangeFireGunFireState = 59;
        public const ushort TestFire = 60;
        public const ushort PlatformMovement = 61;
        public const ushort SlipRope = 62;
        public const ushort GiantDaDaRollingStone = 70;
        public const ushort GiantDaDaLightning = 75;
        public const ushort GiantDaDaSurge = 74;
        public const ushort GiantDaDaLightningWarning = 76;
        public const ushort PlayRotatingRayEffect_Lightning = 81;
        public const ushort DragonFullScreenAOE = 84;
        public const ushort PlayRectWarningEffect = 85;
        public const ushort AuroraDragonFireTornadoEffect = 96;
        public const ushort AuroraDragonDelayBlast = 104;    
        public const ushort AncientDragonFireTornadoEffect = 114;
        public const ushort PlayCircleWarningEffect = 128;
        public const ushort GoldJokerFloatingGunBullet = 138;
        public const ushort AceJokerFloatingGunBullet = 139;

        /// <summary>
        /// 技能模版 [废弃了，不要在用了]
        /// </summary>
        public static List<IAbilityMData> datas = new List<IAbilityMData>() {
            new AbilityData.PlayAbility_SplitCone {
                ConfigId = BulletSplitCone, M_EffectSign = "FX_SplitGun_Bomb", M_AudioSign = "WP_NG_SP_SplitGun_Blast", M_ConeAngle = 15, M_BulletSpreadPoints = new List<Vector2> {
                    new Vector2(0.02f, 0.86f), new Vector2(0.69f, -0.36f), new Vector2(-0.65f, -0.42f), new Vector2(-0.82f, 0.28f), new Vector2(0.38f, 0.45f), new Vector2(0.00f, 0.00f), new Vector2(0.35f, 0.02f), new Vector2(0.80f, 0.24f), new Vector2(0.01f, -0.54f), new Vector2(-0.20f, 0.38f), new Vector2(-0.28f, -0.23f), new Vector2(-0.47f, 0.06f), new Vector2(0.11f, 0.33f), new Vector2(0.30f, -0.38f),  
                }
            },
            new AbilityData.PlayAbility_BulletWithStartPos {
                ConfigId = BulletSplitSubBullet, M_EffectSign = "FX_SplitGun_Trail2", M_Power = 100, M_HitEffect = "BulletDecalGun", M_ATK = 200, M_BulletAttnMap = new List<WeaponData.BulletAttnMap> {
                    new WeaponData.BulletAttnMap { MinDistanceRatio = 0, MaxDistanceRatio = 1, MinHurtRatio = 1, MaxHurtRatio = 1 },
                }
            },
            new AbilityData.PlayAbility_BulletWithStartPos {
                ConfigId = GoldJokerFloatingGunBullet, M_EffectSign = "GoldJoker/Sfx_GoldDash_BossSupremeJoker_FloatingGun_Bullet_Option", M_Power = 100, M_HitEffect = "BulletDecalGun", M_ATK = 100, M_HitSplatter = AbilityM_PlayBloodSplatter.ID_GoldJokerFloatingGunHit
            },
            new AbilityData.PlayAbility_BulletWithStartPos {
                ConfigId = AceJokerFloatingGunBullet, M_EffectSign = "AceJoker/Sfx_GoldDash_BossAceJoker_FloatingGun_Bullet_Option", M_Power = 100, M_HitEffect = "BulletDecalGun", M_ATK = 100, M_HitSplatter = AbilityM_PlayBloodSplatter.ID_AceJokerFloatingGunHit
            },
            new AbilityData.PlayAbility_TrackingMissle() {
                ConfigId = UAVTrackingMissle, M_EffectSign = "UAVMissle", M_Power = 100, M_HitAbility = AbilityM_Explosive.ID_UAVMissleExplosive,M_TrackSpeed = 20,M_LockSpeed = 20,M_MoveDistance = 200,M_StopTrackDistance = 5
            },
            // 子弹 -------------------------------------
            new AbilityData.PlayAbility_SausageBullet() {
                ConfigId = SausageBullet,
            },
            new AbilityData.PlayAbility_RangeFlash() { 
                ConfigId = FlashGunExplosive, M_EffectSign = "fx_flashgun_electricchain_loop", M_LifeTime = 2.0f, In_Range = 20,In_DelayCheckTime = 0.1f
            },
            new AbilityData.PlayAbility_Updraft() {
                ConfigId = SceneUpdraft, M_LifeTime = -1f, M_RangeXZ = 3f, M_RangeY = 20f,  M_Power = 30f,
            },
            new AbilityData.PlayAbility_AIBall() {
                ConfigId = MonsterBall, M_EffectSign = "MonsterBall", M_LifeTime = 3.0f, M_Speed = 40f,
            },
            new AbilityData.PlayAbility_HurtRangeAttack() {
                ConfigId = RexKingAttack, M_LifeTime = 0.5f, M_Power = 50, M_Range = 10,
            },
            new AbilityData.PlayAbility_BatLoopAttack() {
                ConfigId = BatLoopAttack, M_Power = 25, M_LifeTime = 10f, M_EffectSign = "BatLoopAttack"
            },
            new AbilityData.PlayAbility_BatEndAttack() {
                ConfigId = BatEndAttack, M_Power = 80, M_LifeTime = 0.5f, M_EffectSign = "BatEndAttack"
            },
            new AbilityData.PlayAbility_PlungerAttackLoop() {
                ConfigId = PlungerAttackLoop, M_Power = 35, M_LifeTime = 10f, M_EffectSign = "PlungerLoopAttack"
            },
            new AbilityData.PlayAbility_PlungerEndAttack() {
                ConfigId = PlungerEndAttack, M_Power = 90, M_LifeTime = 0.5f, M_EffectSign = "PlungerEndAttack"
            },
            new AbilityData.PlayAbility_PenetratorGrenade() {
                ConfigId = PenetratorGrenadeBullet, M_EffectSign = "PenetratorGrenadeBullet", M_LifeTime = 10.0f, M_Speed = 65, M_Power = 50, M_BombTime = 3.0f, M_BombRange = 2
            },
            new AbilityData.PlayAbility_GiantDaDaRollingStone() {
                ConfigId = GiantDaDaRollingStone, M_EffectSign = "BossDaDa/Sfx_GoldDash_BossDaDa_SkillPhase1_RockRoll_Rocktrial_02_Option"
            },
            new AbilityData.PlayAbility_GiantDaDaSurge() {
                ConfigId = GiantDaDaSurge, M_EffectSign = "BossDaDa/GoldDashBossDaDa_SurgeRing"
            },
            new AbilityData.PlayAbility_GiantDaDaLightning() {
                ConfigId = GiantDaDaLightning, M_EffectSign = "BossDaDa/Sfx_GoldDash_BossDaDa_SkillPhase2_LighteningLaser_Laser_01_Option"
            },
            new AbilityData.PlayAbility_GiantDaDaLightningWarning() {
                ConfigId = GiantDaDaLightningWarning, M_EffectSign = "BossDaDa/Sfx_GoldDash_BossDaDa_SkillPhase2_LighteningLaser_LaserWarning_Option"
            },
            new AbilityData.PlayAbility_PlayRotatingEffect() {
                ConfigId = PlayEffect_LightningEndpoint, M_EffectSign = "BossDaDa/Sfx_GoldDash_BossDaDa_SkillPhase2_LighteningLaser_Laser_02_Option",
            },
            new AbilityData.PlayAbility_PlayRotatingRayEffect() {
                ConfigId = PlayRotatingRayEffect_Lightning, M_EffectSign = "BossDaDa/Sfx_GoldDash_BossDaDa_SkillPhase2_LighteningLaser_Laser_01_Option",
            },
            new AbilityData.PlayAbility_Missile {
                ConfigId = Missile, M_EffectSign = "fx_missile_beacon", M_Speed = 20f, M_BombingDuration = 20f, M_DelayBombingDuration = 0f, M_AreaRadius = 7.5f, M_BombSpawnHeight = 50f, M_VehicleHurtRatio = 3f
            },
            new AbilityData.PlayAbility_MissileBomb {
                ConfigId = MissileBomb, M_EffectSign = "fx_missile_bomb", M_LifeTime = 5.0f, M_Speed = 80f, M_IsHitBome = true,  M_Power = 100,
            },
            new AbilityData.PlayAbility_FireGunFire() { 
                ConfigId = ChangeFireGunFireState, M_EffectSign = "Sfx_Weapon_FireGun", M_Power = 100, M_HitEffect = "BulletDecalGun",M_Radius = 0.2f, M_HitAbilityEffect = AbilityM_HurtGPOByTime.ID_Burn
            },
            new AbilityData.PlayAbility_TestFire { 
                ConfigId = TestFire, M_EffectSign = "BoomEffect", M_Power = 60, M_DeltaTime = 1.0f
            },
            
            new AbilityData.PlayAbility_DragonFullScreenAOE() {
                ConfigId = DragonFullScreenAOE
            },
            new AbilityData.PlayAbility_PlayWarningEffect() {
                ConfigId = PlayRectWarningEffect, M_EffectSign = "BossDaDa/Sfx_GoldDash_BossDaDa_SkillPhase1_RockRoll_Rocktrial_01"
            },
            new AbilityData.PlayAbility_PlayWarningEffect() {
                ConfigId = PlayCircleWarningEffect, M_EffectSign = "CircleWarningEffect"
            },
            new AbilityData.PlayAbility_PlayFillScaleEffect() {
                ConfigId = AuroraDragonFireTornadoEffect, M_EffectSign = "AuroraDragon/Sfx_GoldDash_BossAncientDragon_FireTornado_Blue_Option"
            },
            new AbilityData.PlayAbility_PlayFillScaleEffect() {
                ConfigId = AncientDragonFireTornadoEffect, M_EffectSign = "AncientDragon/Sfx_GoldDash_BossAncientDragon_FireTornado_Red_Option"
            },
            new AbilityData.PlayAbility_AuroraDragonDelayBlast () {
                ConfigId = AuroraDragonDelayBlast 
            },
            new AbilityData.PlayAbility_RexKingFire() { 
                ConfigId = RexKingFire, M_Power = 50, M_LifeTime = 5.0f, M_PlayEffectAbility = AbilityM_PlayEffect.ID_RexKingFire
            },
        };

        public static IAbilityMData GetAbilityModData(ushort modId){
            for (int i = 0; i < datas.Count; i++) {
                var data = datas[i];
                if (data == null) {
                    continue;
                }
                if (data.GetConfigId() == modId) {
                    return data;
                }
            }
            Debug.LogError("没有找到技能模版数据:" + modId);
            return null;
        }
        
        /// <summary>
        /// 获取技能数据 按 ID 检查 AB 和 AE
        /// </summary>
        public static IAbilityMData GetAbilityData(ushort configId, byte rowId) {
            IAbilityMData abilityData = null;
            if (configId < AbilityEffectConfig.StartIndex) {
                abilityData = AbilityConfig.GetAbilityConfig(configId, rowId);
            } else {
                abilityData = AbilityEffectConfig.GetAbilityEffectConfig(configId, rowId);
            }
            return abilityData;
        }
        
        /// <summary>
        /// 获取技能数据 按 Sign 检查 AB 和 AE
        /// </summary>
        public static IAbilityMData GetAbilityData(string configSign, string rowSign) {
            IAbilityMData abilityData = null;
            abilityData = AbilityConfig.GetAbilityConfig(configSign, rowSign);
            if (abilityData == null) {
                abilityData = AbilityEffectConfig.GetAbilityEffectConfig(configSign, rowSign);
            }
            return abilityData;
        }
    }
}