using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityConfig {
        // 自动生成的技能 ID
        // 从 10000 开始
        public const ushort StartIndex = 10000;
        public const ushort AncientDragonFlyFlame = 10001;
        public const ushort AuroraDragonDragonCar = 10002;
        public const ushort AuroraDragonDragonCarFlame = 10003;
        public const ushort AuroraDragonFire = 10004;
        public const ushort AuroraDragonFireBall = 10005;
        public const ushort AuroraDragonFireBallSpawner = 10006;
        public const ushort AuroraDragonFireEffect = 10007;
        public const ushort AuroraDragonTread = 10008;
        public const ushort AuroraDragonTreadFire = 10009;
        public const ushort AuroraDragonTreadSpawner = 10010;
        public const ushort BossFightMusic = 10011;
        public const ushort Bullet = 10012;
        public const ushort DragonDelayBlastSpawner = 10013;
        public const ushort DragonFireTornado = 10014;
        public const ushort DragonFullScreenAOESpawner = 10015;
        public const ushort ExpandBoom = 10016;
        public const ushort Explosive = 10017;
        public const ushort GiantDaDaAirBillow = 10018;
        public const ushort GiantDaDaAirBillowHitGround = 10019;
        public const ushort GiantDaDaBug = 10020;
        public const ushort GiantDaDaBugSpawner = 10021;
        public const ushort GiantDaDaDropBug = 10022;
        public const ushort GiantDaDaDropElectricArc = 10023;
        public const ushort GiantDaDaElectricArc = 10024;
        public const ushort GiantDaDaLightningWithBug = 10025;
        public const ushort GiantDaDaStoneSpawner = 10026;
        public const ushort GiantDaDaSurgeIncoming = 10027;
        public const ushort GoldDashFightBossRange = 10028;
        public const ushort GoldJokerCardTrickSpawner = 10029;
        public const ushort GoldJokerDollBomb = 10030;
        public const ushort GoldJokerDollBombSpawner = 10031;
        public const ushort GoldJokerFlash = 10032;
        public const ushort GoldJokerFloatingGun = 10033;
        public const ushort GoldJokerFloatingGunSpawner = 10034;
        public const ushort GoldJokerFollowEffect = 10035;
        public const ushort GoldJokerRocketBomb = 10036;
        public const ushort GoldJokerRocketBombSpawner = 10037;
        public const ushort GoldJokerSurpriseBoom = 10038;
        public const ushort MoveRangeHurt = 10039;
        public const ushort PlayBloodSplatter = 10040;
        public const ushort PlayEffect = 10041;
        public const ushort PlayEffectWithFullDimensionScale = 10042;
        public const ushort PlayHollowCircleWarningEffect = 10043;
        public const ushort PlayMovingEffect = 10044;
        public const ushort PlayRay = 10045;
        public const ushort PlayWWiseAudio = 10046;
        public const ushort SausageBomb = 10047;
        public const ushort UpHPForFireRole = 10048;
        public const ushort BellowAttack = 10049;
        public const ushort RexKingDragonCar = 10050;
        public const ushort Grenade = 10051;
        public const ushort SniperAlertEffect = 10052;
        public const ushort SniperLaserEffect = 10053;
        public const ushort BoxRectAttack = 10054;

        // 自动生成的技能类型标识
        public const string AB_AncientDragonFlyFlame = "AB_AncientDragonFlyFlame";
        public const string AB_AuroraDragonDragonCar = "AB_AuroraDragonDragonCar";
        public const string AB_AuroraDragonDragonCarFlame = "AB_AuroraDragonDragonCarFlame";
        public const string AB_AuroraDragonFire = "AB_AuroraDragonFire";
        public const string AB_AuroraDragonFireBall = "AB_AuroraDragonFireBall";
        public const string AB_AuroraDragonFireBallSpawner = "AB_AuroraDragonFireBallSpawner";
        public const string AB_AuroraDragonFireEffect = "AB_AuroraDragonFireEffect";
        public const string AB_AuroraDragonTread = "AB_AuroraDragonTread";
        public const string AB_AuroraDragonTreadFire = "AB_AuroraDragonTreadFire";
        public const string AB_AuroraDragonTreadSpawner = "AB_AuroraDragonTreadSpawner";
        public const string AB_BossFightMusic = "AB_BossFightMusic";
        public const string AB_Bullet = "AB_Bullet";
        public const string AB_DragonDelayBlastSpawner = "AB_DragonDelayBlastSpawner";
        public const string AB_DragonFireTornado = "AB_DragonFireTornado";
        public const string AB_DragonFullScreenAOESpawner = "AB_DragonFullScreenAOESpawner";
        public const string AB_ExpandBoom = "AB_ExpandBoom";
        public const string AB_Explosive = "AB_Explosive";
        public const string AB_GiantDaDaAirBillow = "AB_GiantDaDaAirBillow";
        public const string AB_GiantDaDaAirBillowHitGround = "AB_GiantDaDaAirBillowHitGround";
        public const string AB_GiantDaDaBug = "AB_GiantDaDaBug";
        public const string AB_GiantDaDaBugSpawner = "AB_GiantDaDaBugSpawner";
        public const string AB_GiantDaDaDropBug = "AB_GiantDaDaDropBug";
        public const string AB_GiantDaDaDropElectricArc = "AB_GiantDaDaDropElectricArc";
        public const string AB_GiantDaDaElectricArc = "AB_GiantDaDaElectricArc";
        public const string AB_GiantDaDaLightningWithBug = "AB_GiantDaDaLightningWithBug";
        public const string AB_GiantDaDaStoneSpawner = "AB_GiantDaDaStoneSpawner";
        public const string AB_GiantDaDaSurgeIncoming = "AB_GiantDaDaSurgeIncoming";
        public const string AB_GoldDashFightBossRange = "AB_GoldDashFightBossRange";
        public const string AB_GoldJokerCardTrickSpawner = "AB_GoldJokerCardTrickSpawner";
        public const string AB_GoldJokerDollBomb = "AB_GoldJokerDollBomb";
        public const string AB_GoldJokerDollBombSpawner = "AB_GoldJokerDollBombSpawner";
        public const string AB_GoldJokerFlash = "AB_GoldJokerFlash";
        public const string AB_GoldJokerFloatingGun = "AB_GoldJokerFloatingGun";
        public const string AB_GoldJokerFloatingGunSpawner = "AB_GoldJokerFloatingGunSpawner";
        public const string AB_GoldJokerFollowEffect = "AB_GoldJokerFollowEffect";
        public const string AB_GoldJokerRocketBomb = "AB_GoldJokerRocketBomb";
        public const string AB_GoldJokerRocketBombSpawner = "AB_GoldJokerRocketBombSpawner";
        public const string AB_GoldJokerSurpriseBoom = "AB_GoldJokerSurpriseBoom";
        public const string AB_MoveRangeHurt = "AB_MoveRangeHurt";
        public const string AB_PlayBloodSplatter = "AB_PlayBloodSplatter";
        public const string AB_PlayEffect = "AB_PlayEffect";
        public const string AB_PlayEffectWithFullDimensionScale = "AB_PlayEffectWithFullDimensionScale";
        public const string AB_PlayHollowCircleWarningEffect = "AB_PlayHollowCircleWarningEffect";
        public const string AB_PlayMovingEffect = "AB_PlayMovingEffect";
        public const string AB_PlayRay = "AB_PlayRay";
        public const string AB_PlayWWiseAudio = "AB_PlayWWiseAudio";
        public const string AB_SausageBomb = "AB_SausageBomb";
        public const string AB_UpHPForFireRole = "AB_UpHPForFireRole";
        public const string AB_BellowAttack = "AB_BellowAttack";
        public const string AB_RexKingDragonCar = "AB_RexKingDragonCar";
        public const string AB_Grenade = "AB_Grenade";
        public const string AB_SniperAlertEffect = "AB_SniperAlertEffect";
        public const string AB_SniperLaserEffect = "AB_SniperLaserEffect";
        public const string AB_BoxRectAttack = "AB_BoxRectAttack";

        public static void GetAbilityConfig(ushort configId, byte rowId, Action<IAbilityMData> callBack) {
            var mData = GetAbilityConfig(configId, rowId);
            if (mData != null) {
                mData.Select(() => {
                    callBack(mData);
                });
            } else {
                Debug.LogError($"没有对应的 AbilityM 配置 configId: {configId}  rowId: {rowId}");
                callBack(null);
            }
        }

        public static void GetAbilityConfig(string configSign, string rowSign, Action<IAbilityMData> callBack) {
            var mData = GetAbilityConfig(configSign, rowSign);
            if (mData != null) {
                mData.Select(() => {
                    callBack(mData);
                });
            } else {
                Debug.LogError($"没有对应的 AbilityM 配置 configSign: {configSign}  rowSign: {rowSign}");
                callBack(null);
            }
        }

        public static IAbilityMData GetAbilityConfig(string configSign, string rowSign) {
            IAbilityMData mData = null;
            switch (configSign) {
                case AB_AncientDragonFlyFlame:
                    mData = AbilityM_AncientDragonFlyFlame.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonDragonCar:
                    mData = AbilityM_AuroraDragonDragonCar.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonDragonCarFlame:
                    mData = AbilityM_AuroraDragonDragonCarFlame.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonFire:
                    mData = AbilityM_AuroraDragonFire.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonFireBall:
                    mData = AbilityM_AuroraDragonFireBall.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonFireBallSpawner:
                    mData = AbilityM_AuroraDragonFireBallSpawner.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonFireEffect:
                    mData = AbilityM_AuroraDragonFireEffect.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonTread:
                    mData = AbilityM_AuroraDragonTread.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonTreadFire:
                    mData = AbilityM_AuroraDragonTreadFire.CreateForSign(rowSign);
                    break;
                case AB_AuroraDragonTreadSpawner:
                    mData = AbilityM_AuroraDragonTreadSpawner.CreateForSign(rowSign);
                    break;
                case AB_BossFightMusic:
                    mData = AbilityM_BossFightMusic.CreateForSign(rowSign);
                    break;
                case AB_Bullet:
                    mData = AbilityM_Bullet.CreateForSign(rowSign);
                    break;
                case AB_DragonDelayBlastSpawner:
                    mData = AbilityM_DragonDelayBlastSpawner.CreateForSign(rowSign);
                    break;
                case AB_DragonFireTornado:
                    mData = AbilityM_DragonFireTornado.CreateForSign(rowSign);
                    break;
                case AB_DragonFullScreenAOESpawner:
                    mData = AbilityM_DragonFullScreenAOESpawner.CreateForSign(rowSign);
                    break;
                case AB_ExpandBoom:
                    mData = AbilityM_ExpandBoom.CreateForSign(rowSign);
                    break;
                case AB_Explosive:
                    mData = AbilityM_Explosive.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaAirBillow:
                    mData = AbilityM_GiantDaDaAirBillow.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaAirBillowHitGround:
                    mData = AbilityM_GiantDaDaAirBillowHitGround.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaBug:
                    mData = AbilityM_GiantDaDaBug.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaBugSpawner:
                    mData = AbilityM_GiantDaDaBugSpawner.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaDropBug:
                    mData = AbilityM_GiantDaDaDropBug.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaDropElectricArc:
                    mData = AbilityM_GiantDaDaDropElectricArc.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaElectricArc:
                    mData = AbilityM_GiantDaDaElectricArc.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaLightningWithBug:
                    mData = AbilityM_GiantDaDaLightningWithBug.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaStoneSpawner:
                    mData = AbilityM_GiantDaDaStoneSpawner.CreateForSign(rowSign);
                    break;
                case AB_GiantDaDaSurgeIncoming:
                    mData = AbilityM_GiantDaDaSurgeIncoming.CreateForSign(rowSign);
                    break;
                case AB_GoldDashFightBossRange:
                    mData = AbilityM_GoldDashFightBossRange.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerCardTrickSpawner:
                    mData = AbilityM_GoldJokerCardTrickSpawner.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerDollBomb:
                    mData = AbilityM_GoldJokerDollBomb.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerDollBombSpawner:
                    mData = AbilityM_GoldJokerDollBombSpawner.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerFlash:
                    mData = AbilityM_GoldJokerFlash.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerFloatingGun:
                    mData = AbilityM_GoldJokerFloatingGun.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerFloatingGunSpawner:
                    mData = AbilityM_GoldJokerFloatingGunSpawner.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerFollowEffect:
                    mData = AbilityM_GoldJokerFollowEffect.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerRocketBomb:
                    mData = AbilityM_GoldJokerRocketBomb.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerRocketBombSpawner:
                    mData = AbilityM_GoldJokerRocketBombSpawner.CreateForSign(rowSign);
                    break;
                case AB_GoldJokerSurpriseBoom:
                    mData = AbilityM_GoldJokerSurpriseBoom.CreateForSign(rowSign);
                    break;
                case AB_MoveRangeHurt:
                    mData = AbilityM_MoveRangeHurt.CreateForSign(rowSign);
                    break;
                case AB_PlayBloodSplatter:
                    mData = AbilityM_PlayBloodSplatter.CreateForSign(rowSign);
                    break;
                case AB_PlayEffect:
                    mData = AbilityM_PlayEffect.CreateForSign(rowSign);
                    break;
                case AB_PlayEffectWithFullDimensionScale:
                    mData = AbilityM_PlayEffectWithFullDimensionScale.CreateForSign(rowSign);
                    break;
                case AB_PlayHollowCircleWarningEffect:
                    mData = AbilityM_PlayHollowCircleWarningEffect.CreateForSign(rowSign);
                    break;
                case AB_PlayMovingEffect:
                    mData = AbilityM_PlayMovingEffect.CreateForSign(rowSign);
                    break;
                case AB_PlayRay:
                    mData = AbilityM_PlayRay.CreateForSign(rowSign);
                    break;
                case AB_PlayWWiseAudio:
                    mData = AbilityM_PlayWWiseAudio.CreateForSign(rowSign);
                    break;
                case AB_SausageBomb:
                    mData = AbilityM_SausageBomb.CreateForSign(rowSign);
                    break;
                case AB_UpHPForFireRole:
                    mData = AbilityM_UpHPForFireRole.CreateForSign(rowSign);
                    break;
                case AB_BellowAttack:
                    mData = AbilityM_BellowAttack.CreateForSign(rowSign);
                    break;
                case AB_RexKingDragonCar:
                    mData = AbilityM_RexKingDragonCar.CreateForSign(rowSign);
                    break;
                case AB_Grenade:
                    mData = AbilityM_Grenade.CreateForSign(rowSign);
                    break;
                case AB_SniperAlertEffect:
                    mData = AbilityM_SniperAlertEffect.CreateForSign(rowSign);
                    break;
                case AB_SniperLaserEffect:
                    mData = AbilityM_SniperLaserEffect.CreateForSign(rowSign);
                    break;
                case AB_BoxRectAttack:
                    mData = AbilityM_BoxRectAttack.CreateForSign(rowSign);
                    break;
            }
            return mData;
         }

        public static IAbilityMData GetAbilityConfig(ushort configId, byte rowId) {
            IAbilityMData mData = null;
            switch (configId) {
                case AncientDragonFlyFlame:
                    mData = AbilityM_AncientDragonFlyFlame.CreateForID(rowId);
                    break;
                case AuroraDragonDragonCar:
                    mData = AbilityM_AuroraDragonDragonCar.CreateForID(rowId);
                    break;
                case AuroraDragonDragonCarFlame:
                    mData = AbilityM_AuroraDragonDragonCarFlame.CreateForID(rowId);
                    break;
                case AuroraDragonFire:
                    mData = AbilityM_AuroraDragonFire.CreateForID(rowId);
                    break;
                case AuroraDragonFireBall:
                    mData = AbilityM_AuroraDragonFireBall.CreateForID(rowId);
                    break;
                case AuroraDragonFireBallSpawner:
                    mData = AbilityM_AuroraDragonFireBallSpawner.CreateForID(rowId);
                    break;
                case AuroraDragonFireEffect:
                    mData = AbilityM_AuroraDragonFireEffect.CreateForID(rowId);
                    break;
                case AuroraDragonTread:
                    mData = AbilityM_AuroraDragonTread.CreateForID(rowId);
                    break;
                case AuroraDragonTreadFire:
                    mData = AbilityM_AuroraDragonTreadFire.CreateForID(rowId);
                    break;
                case AuroraDragonTreadSpawner:
                    mData = AbilityM_AuroraDragonTreadSpawner.CreateForID(rowId);
                    break;
                case BossFightMusic:
                    mData = AbilityM_BossFightMusic.CreateForID(rowId);
                    break;
                case Bullet:
                    mData = AbilityM_Bullet.CreateForID(rowId);
                    break;
                case DragonDelayBlastSpawner:
                    mData = AbilityM_DragonDelayBlastSpawner.CreateForID(rowId);
                    break;
                case DragonFireTornado:
                    mData = AbilityM_DragonFireTornado.CreateForID(rowId);
                    break;
                case DragonFullScreenAOESpawner:
                    mData = AbilityM_DragonFullScreenAOESpawner.CreateForID(rowId);
                    break;
                case ExpandBoom:
                    mData = AbilityM_ExpandBoom.CreateForID(rowId);
                    break;
                case Explosive:
                    mData = AbilityM_Explosive.CreateForID(rowId);
                    break;
                case GiantDaDaAirBillow:
                    mData = AbilityM_GiantDaDaAirBillow.CreateForID(rowId);
                    break;
                case GiantDaDaAirBillowHitGround:
                    mData = AbilityM_GiantDaDaAirBillowHitGround.CreateForID(rowId);
                    break;
                case GiantDaDaBug:
                    mData = AbilityM_GiantDaDaBug.CreateForID(rowId);
                    break;
                case GiantDaDaBugSpawner:
                    mData = AbilityM_GiantDaDaBugSpawner.CreateForID(rowId);
                    break;
                case GiantDaDaDropBug:
                    mData = AbilityM_GiantDaDaDropBug.CreateForID(rowId);
                    break;
                case GiantDaDaDropElectricArc:
                    mData = AbilityM_GiantDaDaDropElectricArc.CreateForID(rowId);
                    break;
                case GiantDaDaElectricArc:
                    mData = AbilityM_GiantDaDaElectricArc.CreateForID(rowId);
                    break;
                case GiantDaDaLightningWithBug:
                    mData = AbilityM_GiantDaDaLightningWithBug.CreateForID(rowId);
                    break;
                case GiantDaDaStoneSpawner:
                    mData = AbilityM_GiantDaDaStoneSpawner.CreateForID(rowId);
                    break;
                case GiantDaDaSurgeIncoming:
                    mData = AbilityM_GiantDaDaSurgeIncoming.CreateForID(rowId);
                    break;
                case GoldDashFightBossRange:
                    mData = AbilityM_GoldDashFightBossRange.CreateForID(rowId);
                    break;
                case GoldJokerCardTrickSpawner:
                    mData = AbilityM_GoldJokerCardTrickSpawner.CreateForID(rowId);
                    break;
                case GoldJokerDollBomb:
                    mData = AbilityM_GoldJokerDollBomb.CreateForID(rowId);
                    break;
                case GoldJokerDollBombSpawner:
                    mData = AbilityM_GoldJokerDollBombSpawner.CreateForID(rowId);
                    break;
                case GoldJokerFlash:
                    mData = AbilityM_GoldJokerFlash.CreateForID(rowId);
                    break;
                case GoldJokerFloatingGun:
                    mData = AbilityM_GoldJokerFloatingGun.CreateForID(rowId);
                    break;
                case GoldJokerFloatingGunSpawner:
                    mData = AbilityM_GoldJokerFloatingGunSpawner.CreateForID(rowId);
                    break;
                case GoldJokerFollowEffect:
                    mData = AbilityM_GoldJokerFollowEffect.CreateForID(rowId);
                    break;
                case GoldJokerRocketBomb:
                    mData = AbilityM_GoldJokerRocketBomb.CreateForID(rowId);
                    break;
                case GoldJokerRocketBombSpawner:
                    mData = AbilityM_GoldJokerRocketBombSpawner.CreateForID(rowId);
                    break;
                case GoldJokerSurpriseBoom:
                    mData = AbilityM_GoldJokerSurpriseBoom.CreateForID(rowId);
                    break;
                case MoveRangeHurt:
                    mData = AbilityM_MoveRangeHurt.CreateForID(rowId);
                    break;
                case PlayBloodSplatter:
                    mData = AbilityM_PlayBloodSplatter.CreateForID(rowId);
                    break;
                case PlayEffect:
                    mData = AbilityM_PlayEffect.CreateForID(rowId);
                    break;
                case PlayEffectWithFullDimensionScale:
                    mData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(rowId);
                    break;
                case PlayHollowCircleWarningEffect:
                    mData = AbilityM_PlayHollowCircleWarningEffect.CreateForID(rowId);
                    break;
                case PlayMovingEffect:
                    mData = AbilityM_PlayMovingEffect.CreateForID(rowId);
                    break;
                case PlayRay:
                    mData = AbilityM_PlayRay.CreateForID(rowId);
                    break;
                case PlayWWiseAudio:
                    mData = AbilityM_PlayWWiseAudio.CreateForID(rowId);
                    break;
                case SausageBomb:
                    mData = AbilityM_SausageBomb.CreateForID(rowId);
                    break;
                case UpHPForFireRole:
                    mData = AbilityM_UpHPForFireRole.CreateForID(rowId);
                    break;
                case BellowAttack:
                    mData = AbilityM_BellowAttack.CreateForID(rowId);
                    break;
                case RexKingDragonCar:
                    mData = AbilityM_RexKingDragonCar.CreateForID(rowId);
                    break;
                case Grenade:
                    mData = AbilityM_Grenade.CreateForID(rowId);
                    break;
                case SniperAlertEffect:
                    mData = AbilityM_SniperAlertEffect.CreateForID(rowId);
                    break;
                case SniperLaserEffect:
                    mData = AbilityM_SniperLaserEffect.CreateForID(rowId);
                    break;
                case BoxRectAttack:
                    mData = AbilityM_BoxRectAttack.CreateForID(rowId);
                    break;
            }
            return mData;
         }
    }
}
