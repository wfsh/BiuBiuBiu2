using System;
using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerAbilityManager : ManagerBase {
        private int abilityIdIndex = 0;
        private List<S_Ability_Base> abilityList = new List<S_Ability_Base>();
        private INetwork network;
        private int perf_AbilityCount = PerfAnalyzerKey.StringToHash("Ability 数量");
        private float perfDeltaTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Ability.PlayAbilityOld>(OnPlayAbilityOldCallBack);
            MsgRegister.Register<SM_Ability.PlayAbility>(OnPlayAbilityCallBack);
            MsgRegister.Register<SM_Ability.PlayAbilityEffect>(OnPlayAbilityEffectCallBack);
            MsgRegister.Register<SM_Ability.RemoveAbility>(OnRemoveAbilityCallBack);
            MsgRegister.Register<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLogin);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Ability.PlayAbilityOld>(OnPlayAbilityOldCallBack);
            MsgRegister.Unregister<SM_Ability.PlayAbility>(OnPlayAbilityCallBack);
            MsgRegister.Unregister<SM_Ability.PlayAbilityEffect>(OnPlayAbilityEffectCallBack);
            MsgRegister.Unregister<SM_Ability.RemoveAbility>(OnRemoveAbilityCallBack);
            MsgRegister.Unregister<SM_Network.SetWorldNetwork>(OnSetWorldNetworkCallBack);
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLogin);
            network = null;
            ClearSystem();
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            SavePerfanalyzer();
        }

        private void SavePerfanalyzer() {
            if (perfDeltaTime >= 0f) {
                perfDeltaTime -= Time.deltaTime;
                return;
            }
            perfDeltaTime = 0.3f;
            PerfAnalyzerAgent.SetCustomRecorder(perf_AbilityCount, abilityList.Count);
        }

        private void OnCharacterLogin(SM_Character.CharacterLogin ent) {
            if (ent.INetwork == null) {
                Debug.LogError("networkBase 没有正确赋值");
                return;
            }
            var ids = new List<int>(abilityList.Count);
            for (int i = 0; i < abilityList.Count; i++) {
                var ab = abilityList[i];
                ab.Dispatcher(new SE_Ability.HasConnAbility {
                    CallBack = () => {
                        ids.Add(ab.AbilityId);
                    }
                });
            }
            network.TargetRpc(ent.INetwork, new Proto_Ability.TargetRpc_InWorldAbilityList {
                abList = ids
            });
        }

        private void OnSetWorldNetworkCallBack(SM_Network.SetWorldNetwork ent) {
            this.network = ent.network;
            OnSetNetworkCallBack();
        }

        private void OnPlayAbilityOldCallBack(SM_Ability.PlayAbilityOld ent) {
            var modData = AbilityConfig.GetAbilityModData(ent.AbilityMData.GetConfigId());
            ent.AbilityMData.SetConfigData(modData);
            var sign = $"PlayAbility:{modData}";
            PerfAnalyzerAgent.BeginSample(sign);
            var id = abilityIdIndex++;
            var system = GetAbilityAB(ent.AbilityMData, delegate(S_Ability_Base system) {
                system.SetData(id, ent.AbilityMData, null);
                system.SetNetwork(this.network);
                system.SetFireGPO(ent.FireGPO);
                abilityList.Add(system);
            });
            ent.CallBack?.Invoke(system);
            PerfAnalyzerAgent.EndSample(sign);
        }

        private void OnPlayAbilityCallBack(SM_Ability.PlayAbility ent) {
            var id = abilityIdIndex++;
            ent.MData.Select(() => {
                var sign = $"PlayAbility:{ent.MData}";
                PerfAnalyzerAgent.BeginSample(sign);
                var system = GetAbilityAB(ent.MData, delegate(S_Ability_Base system) {
                    system.SetData(id, ent.MData, ent.InData);
                    system.SetNetwork(this.network);
                    system.SetFireGPO(ent.FireGPO);
                    system.SetParentAB(ent.OR_ParentAB);
                    abilityList.Add(system);
                });
                ent.OR_CallBack?.Invoke(system);
                PerfAnalyzerAgent.EndSample(sign);
            });
        }

        private void OnPlayAbilityEffectCallBack(SM_Ability.PlayAbilityEffect ent) {
            var id = abilityIdIndex++;
            ent.MData.Select(() => {
                var system = TargetGPOHasSystemForConfigId(ent.TargetGPO, ent.MData.GetConfigId());
                if (system != null) {
                    system.SetFireGPO(ent.FireGPO);
                } else {
                    var sign = $"PlayAbilityEffect:{ent.MData.GetTypeID()}";
                    PerfAnalyzerAgent.BeginSample(sign);
                    system = GetAbilityAE(ent.MData, delegate(S_Ability_Base system) {
                        system.SetData(id, ent.MData, ent.InData);
                        system.SetNetwork(this.network);
                        system.SetFireGPO(ent.FireGPO);
                        system.SetTargetGPO(ent.TargetGPO);
                        system.SetParentAB(ent.OR_ParentAB);
                        abilityList.Add(system);
                    });
                    ent.OR_CallBack?.Invoke(system);
                    PerfAnalyzerAgent.EndSample(sign);
                }
                system.ResetEffect();
                ent.OR_CallBack?.Invoke(system);
            });
        }

        private void OnRemoveAbilityCallBack(SM_Ability.RemoveAbility ent) {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                if (system.AbilityId == ent.AbilityId) {
                    system.Clear();
                    abilityList.RemoveAt(i);
                    return;
                }
            }
        }

        private void OnSetNetworkCallBack() {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                system.SetNetwork(this.network);
            }
        }

        private S_Ability_Base GetAbilityAB(IAbilityMData abilityMData, Action<S_Ability_Base> callBack) {
            var system = HandleAbilityABType(abilityMData.GetTypeID(), callBack);
            if (system != null) {
                return system;
            }
            switch (abilityMData.GetTypeID()) {
                case AbilityData.SAB_RPG:
                    system = AddSystem<SAB_BulletRPGSystem>(callBack);
                    break;
                case AbilityData.SAB_Updraft:
                    system = AddSystem<SAB_UpdraftSystem>(callBack);
                    break;
                case AbilityData.SAB_Grenade:
                    system = AddSystem<SAB_GrenadeSystem>(callBack);
                    break;
                case AbilityData.SAB_PlatformMovement:
                    system = AddSystem<SAB_PlatformMovementSystem>(callBack);
                    break;
                case AbilityData.SAB_SlipRope:
                    system = AddSystem<SAB_SlipRope>(callBack);
                    break;
                case AbilityData.SAB_AIBall:
                    system = AddSystem<SAB_AIBallSystem>(callBack);
                    break;
                case AbilityData.SAB_HurtRangeAttack:
                    system = AddSystem<SAB_HurtRangeAttackSystem>(callBack);
                    break;
                case AbilityData.SAB_RexKingFire:
                    system = AddSystem<SAB_RexKinFireSystem>(callBack);
                    break;
                case AbilityData.SAB_BatLoopAttack:
                    system = AddSystem<SAB_BatLoopAttack>(callBack);
                    break;
                case AbilityData.SAB_BatEndAttack:
                    system = AddSystem<SAB_BatEndAttack>(callBack);
                    break;
                case AbilityData.SAB_PlungerAttackLoop:
                    system = AddSystem<SAB_PlungerLoopAttack>(callBack);
                    break;
                case AbilityData.SAB_PlungerEndAttack:
                    system = AddSystem<SAB_PlungerEndAttack>(callBack);
                    break;
                case AbilityData.SAB_UpHPForFireRole:
                    system = AddSystem<SAB_UpHPForFireRoleSystem>(callBack);
                    break;
                case AbilityData.SAB_PenetratorGrenade:
                    system = AddSystem<SAB_PenetratorGrenadeSystem>(callBack);
                    break;
                case AbilityData.SAB_FireGunFire:
                    system = AddSystem<SAB_FireGunFireSystem>(callBack);
                    break;
                case AbilityData.SAB_SausageBullet:
                    system = AddSystem<SAB_SausageBulletSystem>(callBack);
                    break;
                case AbilityData.SAB_GiantDaDaRollingStone:
                    system = AddSystem<SAB_GiantDaDaRollingStoneSystem>(callBack);
                    break;
                case AbilityData.SAB_GiantDaDaSurge:
                    system = AddSystem<SAB_GiantDaDaSurgeSystem>(callBack);
                    break;
                case AbilityData.SAB_GiantDaDaLightning:
                    system = AddSystem<SAB_GiantDaDaLightningSystem>(callBack);
                    break;
                case AbilityData.SAB_GiantDaDaLightningWarning:
                    system = AddSystem<SAB_GiantDaDaLightningWarningSystem>(callBack);
                    break;
                case AbilityData.SAB_SausageBomb:
                    system = AddSystem<SAB_SausageBombSystem>(callBack);
                    break;
                // 常驻类型 AB 编写范例。支持重连，单独类型同步 (测试)
                case AbilityData.SAB_TestFire:
                    system = AddSystem<SAB_TestFire>(callBack);
                    break;
                case AbilityData.SAB_PlayEffectWithFullDimensionScale:
                    system = AddSystem<SAB_PlayEffectWithFullDimensionScaleSystem>(callBack);
                    break;
                case AbilityData.SAB_PlayRotatingEffect:
                    system = AddSystem<SAB_PlayRotatingEffectSystem>(callBack);
                    break;
                case AbilityData.SAB_PlayRotatingRayEffect:
                    system = AddSystem<SAB_PlayRotatingRayEffectSystem>(callBack);
                    break;
                case AbilityData.SAB_Missile:
                    system = AddSystem<SAB_MissileSystem>(callBack);
                    break;
                case AbilityData.SAB_MissileBomb:
                    system = AddSystem<SAB_MissileBombSystem>(callBack);
                    break;
                case AbilityData.SAB_SplitCone:
                    system = AddSystem<SAB_SplitConeSystem>(callBack);
                    break;
                case AbilityData.SAB_BulletWithStartPoint:
                    system = AddSystem<SAB_BulletWithStartPointSystem>(callBack);
                    break;
                case AbilityData.SAB_TrackingMissle:
                    system = AddSystem<SAB_TrackingMissleSystem>(callBack);
                    break;
                case AbilityData.SAB_RangeFlash:
                    system = AddSystem<SAB_RangeFlashSystem>(callBack);
                    break;
                case AbilityData.SAB_DragonFullScreenAOE:
                    system = AddSystem<SAB_DragonFullScreenAOESystem>(callBack);
                    break;
                case AbilityData.SAB_PlayFillScaleEffect:
                    system = AddSystem<SAB_PlayFillScaleEffectSystem>(callBack);
                    break;
                case AbilityData.SAB_PlayWarningEffect:
                    system = AddSystem<SAB_PlayWarningEffectSystem>(callBack);
                    break;
                case AbilityData.SAB_AuroraDragonDelayBlast:
                    system = AddSystem<SAB_AuroraDragonDelayBlastSystem>(callBack);
                    break;
                default:
                    Debug.LogError("缺少 Ability 标识:" + abilityMData.GetTypeID());
                    break;
            }
            return system;
        }

        private S_Ability_Base GetAbilityAE(IAbilityMData abilityMData, Action<S_Ability_Base> callBack) {
            var system = HandleAbilityAEType(abilityMData.GetTypeID(), callBack);
            if (system != null) {
                return system;
            }
            Debug.LogError("缺少 Ability 标识:" + abilityMData.GetTypeID());
            return system;
        }

        private void ClearSystem() {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                system.Clear();
            }
            abilityList.Clear();
        }

        private S_Ability_Base TargetGPOHasSystemForConfigId(IGPO targetGpo, ushort configId) {
            if (targetGpo == null) {
                return null;
            }
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                if (system.ConfigID == configId &&
                    system.TargetGPO.GetGpoID() == targetGpo.GetGpoID()) {
                    return system;
                }
            }
            return null;
        }
    }
}