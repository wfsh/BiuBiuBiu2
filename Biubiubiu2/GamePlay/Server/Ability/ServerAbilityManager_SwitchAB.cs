using System;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerAbilityManager {
        protected S_Ability_Base HandleAbilityABType(string abilityTypeId, Action<S_Ability_Base> callBack) {
            S_Ability_Base system = null;
            switch (abilityTypeId) {
                case AbilityM_AncientDragonFlyFlame.AbilityTypeID:
                    system = AddSystem<SAB_AncientDragonFlyFlameSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonDragonCar.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonDragonCarSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonDragonCarFlame.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonDragonCarFlameSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonFire.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonFireSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonFireBall.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonFireBallSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonFireBallSpawner.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonFireBallSpawnerSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonFireEffect.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonFireEffectSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonTread.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonTreadSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonTreadFire.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonTreadFireSystem>(callBack);
                    break;
                case AbilityM_AuroraDragonTreadSpawner.AbilityTypeID:
                    system = AddSystem<SAB_AuroraDragonTreadSpawnerSystem>(callBack);
                    break;
                case AbilityM_BossFightMusic.AbilityTypeID:
                    system = AddSystem<SAB_BossFightMusicSystem>(callBack);
                    break;
                case AbilityM_Bullet.AbilityTypeID:
                    system = AddSystem<SAB_BulletSystem>(callBack);
                    break;
                case AbilityM_DragonDelayBlastSpawner.AbilityTypeID:
                    system = AddSystem<SAB_DragonDelayBlastSpawnerSystem>(callBack);
                    break;
                case AbilityM_DragonFireTornado.AbilityTypeID:
                    system = AddSystem<SAB_DragonFireTornadoSystem>(callBack);
                    break;
                case AbilityM_DragonFullScreenAOESpawner.AbilityTypeID:
                    system = AddSystem<SAB_DragonFullScreenAOESpawnerSystem>(callBack);
                    break;
                case AbilityM_ExpandBoom.AbilityTypeID:
                    system = AddSystem<SAB_ExpandBoomSystem>(callBack);
                    break;
                case AbilityM_Explosive.AbilityTypeID:
                    system = AddSystem<SAB_ExplosiveSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaAirBillow.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaAirBillowSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaAirBillowHitGround.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaAirBillowHitGroundSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaBug.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaBugSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaBugSpawner.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaBugSpawnerSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaDropBug.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaDropBugSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaDropElectricArc.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaDropElectricArcSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaElectricArc.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaElectricArcSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaLightningWithBug.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaLightningWithBugSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaStoneSpawner.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaStoneSpawnerSystem>(callBack);
                    break;
                case AbilityM_GiantDaDaSurgeIncoming.AbilityTypeID:
                    system = AddSystem<SAB_GiantDaDaSurgeIncomingSystem>(callBack);
                    break;
                case AbilityM_GoldDashFightBossRange.AbilityTypeID:
                    system = AddSystem<SAB_GoldDashFightBossRangeSystem>(callBack);
                    break;
                case AbilityM_GoldJokerCardTrickSpawner.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerCardTrickSpawnerSystem>(callBack);
                    break;
                case AbilityM_GoldJokerDollBomb.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerDollBombSystem>(callBack);
                    break;
                case AbilityM_GoldJokerDollBombSpawner.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerDollBombSpawnerSystem>(callBack);
                    break;
                case AbilityM_GoldJokerFlash.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerFlashSystem>(callBack);
                    break;
                case AbilityM_GoldJokerFloatingGun.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerFloatingGunSystem>(callBack);
                    break;
                case AbilityM_GoldJokerFloatingGunSpawner.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerFloatingGunSpawnerSystem>(callBack);
                    break;
                case AbilityM_GoldJokerFollowEffect.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerFollowEffectSystem>(callBack);
                    break;
                case AbilityM_GoldJokerRocketBomb.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerRocketBombSystem>(callBack);
                    break;
                case AbilityM_GoldJokerRocketBombSpawner.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerRocketBombSpawnerSystem>(callBack);
                    break;
                case AbilityM_GoldJokerSurpriseBoom.AbilityTypeID:
                    system = AddSystem<SAB_GoldJokerSurpriseBoomSystem>(callBack);
                    break;
                case AbilityM_MoveRangeHurt.AbilityTypeID:
                    system = AddSystem<SAB_MoveRangeHurtSystem>(callBack);
                    break;
                case AbilityM_PlayBloodSplatter.AbilityTypeID:
                    system = AddSystem<SAB_PlayBloodSplatterSystem>(callBack);
                    break;
                case AbilityM_PlayEffect.AbilityTypeID:
                    system = AddSystem<SAB_PlayEffectSystem>(callBack);
                    break;
                case AbilityM_PlayEffectWithFullDimensionScale.AbilityTypeID:
                    system = AddSystem<SAB_PlayEffectWithFullDimensionScaleSystem>(callBack);
                    break;
                case AbilityM_PlayHollowCircleWarningEffect.AbilityTypeID:
                    system = AddSystem<SAB_PlayHollowCircleWarningEffectSystem>(callBack);
                    break;
                case AbilityM_PlayMovingEffect.AbilityTypeID:
                    system = AddSystem<SAB_PlayMovingEffectSystem>(callBack);
                    break;
                case AbilityM_PlayRay.AbilityTypeID:
                    system = AddSystem<SAB_PlayRaySystem>(callBack);
                    break;
                case AbilityM_PlayWWiseAudio.AbilityTypeID:
                    system = AddSystem<SAB_PlayWWiseAudioSystem>(callBack);
                    break;
                case AbilityM_SausageBomb.AbilityTypeID:
                    system = AddSystem<SAB_SausageBombSystem>(callBack);
                    break;
                case AbilityM_UpHPForFireRole.AbilityTypeID:
                    system = AddSystem<SAB_UpHPForFireRoleSystem>(callBack);
                    break;
                case AbilityM_BellowAttack.AbilityTypeID:
                    system = AddSystem<SAB_BellowAttackSystem>(callBack);
                    break;
                case AbilityM_RexKingDragonCar.AbilityTypeID:
                    system = AddSystem<SAB_RexKingDragonCarSystem>(callBack);
                    break;
                case AbilityM_Grenade.AbilityTypeID:
                    system = AddSystem<SAB_GrenadeSystem>(callBack);
                    break;
                case AbilityM_SniperAlertEffect.AbilityTypeID:
                    system = AddSystem<SAB_SniperAlertEffectSystem>(callBack);
                    break;
                case AbilityM_SniperLaserEffect.AbilityTypeID:
                    system = AddSystem<SAB_SniperLaserEffectSystem>(callBack);
                    break;
                case AbilityM_BoxRectAttack.AbilityTypeID:
                    system = AddSystem<SAB_BoxRectAttackSystem>(callBack);
                    break;
            }
            return system;
        }
    }
}
