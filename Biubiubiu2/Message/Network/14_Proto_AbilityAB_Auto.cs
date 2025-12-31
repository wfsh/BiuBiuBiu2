using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    // 自动生成的代码，不要修改
    public partial class Proto_AbilityAB_Auto {
        public const byte ModID = 14;
        public const byte Rpc_AncientDragonFlyFlame_FuncID = 1;
        public const byte Rpc_AuroraDragonDragonCar_FuncID = 2;
        public const byte Rpc_AuroraDragonDragonCarFlame_FuncID = 3;
        public const byte Rpc_AuroraDragonFire_FuncID = 4;
        public const byte Rpc_AuroraDragonFireBall_FuncID = 5;
        public const byte Rpc_AuroraDragonFireBallSpawner_FuncID = 6;
        public const byte Rpc_AuroraDragonFireEffect_FuncID = 7;
        public const byte Rpc_AuroraDragonTread_FuncID = 8;
        public const byte Rpc_AuroraDragonTreadFire_FuncID = 9;
        public const byte Rpc_AuroraDragonTreadSpawner_FuncID = 10;
        public const byte Rpc_BellowAttack_FuncID = 11;
        public const byte Rpc_BossFightMusic_FuncID = 12;
        public const byte Rpc_BoxRectAttack_FuncID = 13;
        public const byte Rpc_Bullet_FuncID = 14;
        public const byte Rpc_DragonDelayBlastSpawner_FuncID = 15;
        public const byte Rpc_DragonFireTornado_FuncID = 16;
        public const byte Rpc_DragonFullScreenAOESpawner_FuncID = 17;
        public const byte Rpc_ExpandBoom_FuncID = 18;
        public const byte Rpc_Explosive_FuncID = 19;
        public const byte Rpc_GiantDaDaAirBillow_FuncID = 20;
        public const byte Rpc_GiantDaDaAirBillowHitGround_FuncID = 21;
        public const byte Rpc_GiantDaDaBug_FuncID = 22;
        public const byte Rpc_GiantDaDaBugSpawner_FuncID = 23;
        public const byte Rpc_GiantDaDaDropBug_FuncID = 24;
        public const byte Rpc_GiantDaDaDropElectricArc_FuncID = 25;
        public const byte Rpc_GiantDaDaElectricArc_FuncID = 26;
        public const byte Rpc_GiantDaDaLightningWithBug_FuncID = 27;
        public const byte Rpc_GiantDaDaStoneSpawner_FuncID = 28;
        public const byte Rpc_GiantDaDaSurgeIncoming_FuncID = 29;
        public const byte Rpc_GoldDashFightBossRange_FuncID = 30;
        public const byte Rpc_GoldJokerCardTrickSpawner_FuncID = 31;
        public const byte Rpc_GoldJokerDollBomb_FuncID = 32;
        public const byte Rpc_GoldJokerDollBombSpawner_FuncID = 33;
        public const byte Rpc_GoldJokerFlash_FuncID = 34;
        public const byte Rpc_GoldJokerFloatingGun_FuncID = 35;
        public const byte Rpc_GoldJokerFloatingGunSpawner_FuncID = 36;
        public const byte Rpc_GoldJokerFollowEffect_FuncID = 37;
        public const byte Rpc_GoldJokerRocketBomb_FuncID = 38;
        public const byte Rpc_GoldJokerRocketBombSpawner_FuncID = 39;
        public const byte Rpc_GoldJokerSurpriseBoom_FuncID = 40;
        public const byte Rpc_Grenade_FuncID = 41;
        public const byte Rpc_MoveRangeHurt_FuncID = 42;
        public const byte Rpc_PlayBloodSplatter_FuncID = 43;
        public const byte Rpc_PlayEffect_FuncID = 44;
        public const byte Rpc_PlayEffectWithFullDimensionScale_FuncID = 45;
        public const byte Rpc_PlayHollowCircleWarningEffect_FuncID = 46;
        public const byte Rpc_PlayMovingEffect_FuncID = 47;
        public const byte Rpc_PlayRay_FuncID = 48;
        public const byte Rpc_PlayWWiseAudio_FuncID = 49;
        public const byte Rpc_RexKingDragonCar_FuncID = 50;
        public const byte Rpc_SausageBomb_FuncID = 51;
        public const byte Rpc_SniperAlertEffect_FuncID = 52;
        public const byte Rpc_SniperLaserEffect_FuncID = 53;
        public const byte Rpc_UpHPForFireRole_FuncID = 54;

        public static IProto_Doc ReadRpcBuffer(byte funcId) {
            IProto_Doc doc = null;
            switch (funcId) {
                case Rpc_AncientDragonFlyFlame_FuncID:
                    doc = new Rpc_AncientDragonFlyFlame();
                    break;
                case Rpc_AuroraDragonDragonCar_FuncID:
                    doc = new Rpc_AuroraDragonDragonCar();
                    break;
                case Rpc_AuroraDragonDragonCarFlame_FuncID:
                    doc = new Rpc_AuroraDragonDragonCarFlame();
                    break;
                case Rpc_AuroraDragonFire_FuncID:
                    doc = new Rpc_AuroraDragonFire();
                    break;
                case Rpc_AuroraDragonFireBall_FuncID:
                    doc = new Rpc_AuroraDragonFireBall();
                    break;
                case Rpc_AuroraDragonFireBallSpawner_FuncID:
                    doc = new Rpc_AuroraDragonFireBallSpawner();
                    break;
                case Rpc_AuroraDragonFireEffect_FuncID:
                    doc = new Rpc_AuroraDragonFireEffect();
                    break;
                case Rpc_AuroraDragonTread_FuncID:
                    doc = new Rpc_AuroraDragonTread();
                    break;
                case Rpc_AuroraDragonTreadFire_FuncID:
                    doc = new Rpc_AuroraDragonTreadFire();
                    break;
                case Rpc_AuroraDragonTreadSpawner_FuncID:
                    doc = new Rpc_AuroraDragonTreadSpawner();
                    break;
                case Rpc_BellowAttack_FuncID:
                    doc = new Rpc_BellowAttack();
                    break;
                case Rpc_BossFightMusic_FuncID:
                    doc = new Rpc_BossFightMusic();
                    break;
                case Rpc_BoxRectAttack_FuncID:
                    doc = new Rpc_BoxRectAttack();
                    break;
                case Rpc_Bullet_FuncID:
                    doc = new Rpc_Bullet();
                    break;
                case Rpc_DragonDelayBlastSpawner_FuncID:
                    doc = new Rpc_DragonDelayBlastSpawner();
                    break;
                case Rpc_DragonFireTornado_FuncID:
                    doc = new Rpc_DragonFireTornado();
                    break;
                case Rpc_DragonFullScreenAOESpawner_FuncID:
                    doc = new Rpc_DragonFullScreenAOESpawner();
                    break;
                case Rpc_ExpandBoom_FuncID:
                    doc = new Rpc_ExpandBoom();
                    break;
                case Rpc_Explosive_FuncID:
                    doc = new Rpc_Explosive();
                    break;
                case Rpc_GiantDaDaAirBillow_FuncID:
                    doc = new Rpc_GiantDaDaAirBillow();
                    break;
                case Rpc_GiantDaDaAirBillowHitGround_FuncID:
                    doc = new Rpc_GiantDaDaAirBillowHitGround();
                    break;
                case Rpc_GiantDaDaBug_FuncID:
                    doc = new Rpc_GiantDaDaBug();
                    break;
                case Rpc_GiantDaDaBugSpawner_FuncID:
                    doc = new Rpc_GiantDaDaBugSpawner();
                    break;
                case Rpc_GiantDaDaDropBug_FuncID:
                    doc = new Rpc_GiantDaDaDropBug();
                    break;
                case Rpc_GiantDaDaDropElectricArc_FuncID:
                    doc = new Rpc_GiantDaDaDropElectricArc();
                    break;
                case Rpc_GiantDaDaElectricArc_FuncID:
                    doc = new Rpc_GiantDaDaElectricArc();
                    break;
                case Rpc_GiantDaDaLightningWithBug_FuncID:
                    doc = new Rpc_GiantDaDaLightningWithBug();
                    break;
                case Rpc_GiantDaDaStoneSpawner_FuncID:
                    doc = new Rpc_GiantDaDaStoneSpawner();
                    break;
                case Rpc_GiantDaDaSurgeIncoming_FuncID:
                    doc = new Rpc_GiantDaDaSurgeIncoming();
                    break;
                case Rpc_GoldDashFightBossRange_FuncID:
                    doc = new Rpc_GoldDashFightBossRange();
                    break;
                case Rpc_GoldJokerCardTrickSpawner_FuncID:
                    doc = new Rpc_GoldJokerCardTrickSpawner();
                    break;
                case Rpc_GoldJokerDollBomb_FuncID:
                    doc = new Rpc_GoldJokerDollBomb();
                    break;
                case Rpc_GoldJokerDollBombSpawner_FuncID:
                    doc = new Rpc_GoldJokerDollBombSpawner();
                    break;
                case Rpc_GoldJokerFlash_FuncID:
                    doc = new Rpc_GoldJokerFlash();
                    break;
                case Rpc_GoldJokerFloatingGun_FuncID:
                    doc = new Rpc_GoldJokerFloatingGun();
                    break;
                case Rpc_GoldJokerFloatingGunSpawner_FuncID:
                    doc = new Rpc_GoldJokerFloatingGunSpawner();
                    break;
                case Rpc_GoldJokerFollowEffect_FuncID:
                    doc = new Rpc_GoldJokerFollowEffect();
                    break;
                case Rpc_GoldJokerRocketBomb_FuncID:
                    doc = new Rpc_GoldJokerRocketBomb();
                    break;
                case Rpc_GoldJokerRocketBombSpawner_FuncID:
                    doc = new Rpc_GoldJokerRocketBombSpawner();
                    break;
                case Rpc_GoldJokerSurpriseBoom_FuncID:
                    doc = new Rpc_GoldJokerSurpriseBoom();
                    break;
                case Rpc_Grenade_FuncID:
                    doc = new Rpc_Grenade();
                    break;
                case Rpc_MoveRangeHurt_FuncID:
                    doc = new Rpc_MoveRangeHurt();
                    break;
                case Rpc_PlayBloodSplatter_FuncID:
                    doc = new Rpc_PlayBloodSplatter();
                    break;
                case Rpc_PlayEffect_FuncID:
                    doc = new Rpc_PlayEffect();
                    break;
                case Rpc_PlayEffectWithFullDimensionScale_FuncID:
                    doc = new Rpc_PlayEffectWithFullDimensionScale();
                    break;
                case Rpc_PlayHollowCircleWarningEffect_FuncID:
                    doc = new Rpc_PlayHollowCircleWarningEffect();
                    break;
                case Rpc_PlayMovingEffect_FuncID:
                    doc = new Rpc_PlayMovingEffect();
                    break;
                case Rpc_PlayRay_FuncID:
                    doc = new Rpc_PlayRay();
                    break;
                case Rpc_PlayWWiseAudio_FuncID:
                    doc = new Rpc_PlayWWiseAudio();
                    break;
                case Rpc_RexKingDragonCar_FuncID:
                    doc = new Rpc_RexKingDragonCar();
                    break;
                case Rpc_SausageBomb_FuncID:
                    doc = new Rpc_SausageBomb();
                    break;
                case Rpc_SniperAlertEffect_FuncID:
                    doc = new Rpc_SniperAlertEffect();
                    break;
                case Rpc_SniperLaserEffect_FuncID:
                    doc = new Rpc_SniperLaserEffect();
                    break;
                case Rpc_UpHPForFireRole_FuncID:
                    doc = new Rpc_UpHPForFireRole();
                    break;
                default:
                    Debug.LogError("Proto_Ability:ReadRpcBuffer 没有注册对应 ID:" + funcId);
                    return null;
            }

            return doc;
        }
    }
}
