using System;
using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public partial class ClientAbilityManager : ManagerBase {
        private List<C_Ability_Base> abilityList;
        private Dictionary<string, List<C_Ability_Base>> abilityDic;
        private Dictionary<string, List<C_Ability_Base>> lookGpoAbilityDic;
        private INetwork network;
        private ClientAbilitySystem system;
        private int perf_AbilityCount = PerfAnalyzerKey.StringToHash("Ability 数量");
        private float perfDeltaTime = 0.0f;
        public bool isPlayAbility = true;
        private IGPO lookGPO = null;

        protected override void OnAwake() {
            base.OnAwake();
            abilityList = new List<C_Ability_Base>();
            abilityDic = new Dictionary<string, List<C_Ability_Base>>();
            MsgRegister.Register<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Register<CM_Ability.RemoveAbility>(OnRemoveAbilityCallBack);
            MsgRegister.Register<M_Game.HideAbility>(OnSetHideAbility);
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }
        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new CM_GPO.GetLookGPO {
                CallBack = gpo => {
                    lookGPO = gpo;
                }
            });
        }

        protected override void OnUpdate() {
            base.OnUpdate();
            SavePerfanalyzer();
        }

        private void SavePerfanalyzer() {
            if (NetworkData.IsStartServer) {
                return;
            }
            if (perfDeltaTime >= 0f) {
                perfDeltaTime -= Time.deltaTime;
                return;
            }
            perfDeltaTime = 0.3f;
            PerfAnalyzerAgent.SetCustomRecorder(perf_AbilityCount, abilityList.Count);
        }

        private void OnSetHideAbility(M_Game.HideAbility ent) {
            isPlayAbility = !isPlayAbility;
        }


        protected override void OnClear() {
            base.OnClear();
            network?.RemoveProtoCallBack(Proto_Ability.TargetRpc_InWorldAbilityList.ID, TargetRpcInWorldAbilityList);
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Unregister<CM_Ability.RemoveAbility>(OnRemoveAbilityCallBack);
            MsgRegister.Unregister<M_Network.SetNetwork>(SetNetwork);
            MsgRegister.Unregister<CM_Ability.RemoveAbility>(OnRemoveAbilityCallBack);
            ClearSystem();
            network = null;
            lookGPO = null;
            if (system != null) {
                system.Clear();
                system = null;
            }
        }

        private void SetNetwork(M_Network.SetNetwork ent) {
            network = ent.iNetwork;
            network.AddProtoCallBack(Proto_Ability.TargetRpc_InWorldAbilityList.ID, TargetRpcInWorldAbilityList);
            if (system == null) {
                system = AddSystem(delegate(ClientAbilitySystem abilitySystem) {
                    abilitySystem.SetNetwork(network);
                });
                system.Register<CE_Ability.PlayAbility>(OnPlayAbilityCallBack);
                system.Register<CE_Ability.RemoveAbility>(OnRemoveAbilityEventCallBack);
            } else {
                system.SetNetwork(network);
            }
            UpdateNetwork();
        }
        
        private void TargetRpcInWorldAbilityList(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_Ability.TargetRpc_InWorldAbilityList)data;
            var abList = rpcData.abList;
            for (int i = 0; i < abilityList.Count; i++) {
                var ab = abilityList[i];
                ab.Dispatcher(new CE_Ability.HasConnAbility {
                    CallBack = () => {
                        var abilityId = ab.AbilityId;
                        if (abList.Contains(abilityId) == false) {
                            RemoveAbility(abilityId);
                        }
                    }
                });
            }
        }

        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            lookGPO = ent.LookGPO;
        }
        
        private void OnPlayAbilityCallBack(ISystemMsg body, CE_Ability.PlayAbility entData) {
            if (isPlayAbility == false) {
                return;
            }
            if (entData.abilityData is IAbilityABCreateRpc) {
                SetAbilityABConfigBase((IAbilityABCreateRpc)entData.abilityData, delegate(IAbilityMData data) {
                    PlayAbility(entData, data);
                });
            } else if (entData.abilityData is IAbilityAECreateRpc) {
                SetAbilityAEConfigBase((IAbilityAECreateRpc)entData.abilityData, delegate(IAbilityEffectMData data) {
                    PlayAbility(entData, data);
                });
            } else {
                PlayAbility(entData, null);
            }
        }

        private void PlayAbility(CE_Ability.PlayAbility entData, IAbilityMData data) {
            if (HashAbility(entData.abilityId)) {
                Debug.LogError("重复的 Ability ID:" + entData.abilityId + " Ability Data:" + entData.abilityData);;
                return;
            }
            var system = GetSystem(entData.sign, delegate(C_Ability_Base systemBase) {
                systemBase.SetConnID(entData.connId);
                systemBase.SetAbilityData(entData.abilityId, entData.fireGpoId, entData.abilityData, data);
                systemBase.SetNetwork(network);
                abilityList.Add(systemBase);
            });
            if (system != null) {
                system.Register<CE_Ability.RemoveAbility>(OnRemoveAbilityEventCallBack);
            }
            if (lookGPO == null || lookGPO.GetGpoID() != entData.fireGpoId) {
                AddSystemDic(entData.sign, system);
            } else {
                AddLookGPOSystemDic(entData.sign, system);
            }
        }

        /// <summary>
        /// 部分 AB 使用频率较高，需要增加对象池设定
        /// </summary>
        /// <param name="sign"></param>
        /// <param name="system"></param>
        private void AddSystemDic(string sign,  C_Ability_Base system) {
            List<C_Ability_Base> systemList;
            if (abilityDic.TryGetValue(sign, out systemList) == false) {
                systemList = new List<C_Ability_Base>();
                abilityDic.Add(sign, systemList);
            }
            systemList.Add(system);
            CheckSystemDic(sign, systemList);
        }
        
        private void AddLookGPOSystemDic(string sign,  C_Ability_Base system) {
            List<C_Ability_Base> systemList;
            if (lookGpoAbilityDic.TryGetValue(sign, out systemList) == false) {
                systemList = new List<C_Ability_Base>();
                lookGpoAbilityDic.Add(sign, systemList);
            }
            systemList.Add(system);
            CheckSystemDic(sign, systemList);
        }

        private void CheckSystemDic(string sign, List<C_Ability_Base> systemList) {
            switch (sign) {
                case Proto_Ability.Rpc_BulletFire.ID:
                case Proto_Ability.Rpc_BulletFireDecal.ID:
                case Proto_Ability.Rpc_BulletFireWithStartPoint.ID:
                case Proto_Ability.Rpc_BulletFireDecalWithStartPoint.ID:
                case Proto_Ability.Rpc_RangeFlash.ID:
                    if (systemList.Count > QualityData.GetOtherBulletCount()) {
                        var firstSystem = systemList[0];
                        systemList.RemoveAt(0);
                        RemoveAbility(firstSystem.AbilityId);
                    }
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayBloodSplatter.ID:
                    if (systemList.Count > QualityData.GetBloodSplatterCount()) {
                        var firstSystem = systemList[0];
                        systemList.RemoveAt(0);
                        RemoveAbility(firstSystem.AbilityId);
                    }
                    break;
            }
        }

        private void UpdateNetwork() {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                system.SetNetwork(network);
            }
        }
        private void OnRemoveAbilityEventCallBack(ISystemMsg body, CE_Ability.RemoveAbility ent) {
            RemoveAbility(ent.AbilityId);
        }

        private void OnRemoveAbilityCallBack(CM_Ability.RemoveAbility ent) {
            RemoveAbility(ent.AbilityID);
        }

        private void RemoveAbility(int abilityId) {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                if (system.AbilityId == abilityId) {
                    var sign = system.InData.GetID();
                    system.Clear();
                    abilityList.RemoveAt(i);
                    List<C_Ability_Base> systemList;
                    if (abilityDic.TryGetValue(sign, out systemList)) {
                        systemList.Remove(system);
                    }
                    return;
                }
            }
        }

        private bool HashAbility(int abilityId) {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                if (system.AbilityId == abilityId) {
                    return true;
                }
            }
            return false;
        }

        private void SetAbilityABConfigBase(IAbilityABCreateRpc proto, Action<IAbilityMData> callBack) {
            IAbilityMData mData = null;
            switch (proto.GetID()) {
                case Proto_Ability.Rpc_BulletFire.ID:
                case Proto_Ability.Rpc_BulletFireDecal.ID:
                    mData = AbilityM_Bullet.CreateForID(proto.GetRowID());
                    break;
            }
            if (mData != null) {
                mData.Select(() => {
                    callBack(mData);
                });
            } else {
                AbilityConfig.GetAbilityConfig(proto.GetConfigID(), proto.GetRowID(), callBack);
            }
        }

        private void SetAbilityAEConfigBase(IAbilityAECreateRpc proto, Action<IAbilityEffectMData> callBack) {
            AbilityEffectConfig.GetAbilityEffectConfig(proto.GetConfigID(), proto.GetRowID(), callBack);
        }

        private C_Ability_Base GetSystem(string sign, Action<C_Ability_Base> callBack) {
            var system = HandleAbilityABType(sign, callBack);
            if (system == null) {
                system = HandleAbilityAEType(sign, callBack);
            }
            if (system != null) {
                return system;
            }
            switch (sign) {
                case Proto_Ability.Rpc_BulletFire.ID:
                case Proto_Ability.Rpc_BulletFireDecal.ID:
                    system = AddSystem<CAB_BulletSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_ThreadGrenade.ID:
                    if (NetworkData.IsStartServer == false) {
                        system = AddSystem<CAB_GrenadeSystem>(callBack);
                    }
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayEffect.ID:
                    system = AddSystem<CAB_PlayEffectSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayBloodSplatter.ID:
                    system = AddSystem<CAB_BloodSplatterSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_ThreadMonsterBall.ID:
                    if (NetworkData.IsStartServer == false) {
                        system = AddSystem<CAB_AIBallSystem>(callBack);
                    }
                    break;
                case Proto_Ability.Rpc_PenetratorGrenade.ID:
                    system = AddSystem<CAB_PenetratorGrenadeSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_PlayDynamicScalingEffect.ID:
                    system = AddSystem<CAB_PlayDynamicScalingEffectSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_PlayRotatingEffect.ID:
                    system = AddSystem<CAB_PlayRotatingEffectSystem>(callBack);
                    break;
                case AbilityData.CAB_PlatformMovement:
                    system = AddSystem<CAB_PlatformMovementSystem>(callBack);
					break;
                case Proto_Ability.TargetRpc_PlayGPOEffect.ID:
                    system = AddSystem<CAE_PlayGPOEffectSystem>(callBack);
                    break;
                // 常驻类型 AB 编写范例。支持重连，单独类型同步 (测试)
                case Proto_Ability.TargetRpc_PlayTestFire.ID:
                    system = AddSystem<CAB_TestFire>(callBack);
                    break;
                case Proto_Ability.Rpc_GiantDaDaPlayDropBugKeyboardEffect.ID:
                    system = AddSystem<CAB_GiantDaDaPlayDropbugKeyboardEffectSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayEffectWithFullDimensionScale.ID:
                    system = AddSystem<CAB_PlayEffectWithFullDimensionScaleSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_PlayRotatingRayEffect.ID:
                    system = AddSystem<CAB_PlayRotatingRayEffectSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_PlayDynamicScalingRippleEffect.ID:
                    system = AddSystem<CAB_PlayDynamicScalingRippleEffectSystem>(callBack);
                    break;
                case Proto_Ability.TargetRpc_GoldDashFightBossRange.ID:
                    system = AddSystem<CAB_TargetGiantDaDaFightRangeSystem>(callBack);
                    break;
                case Proto_Ability.TargetRpc_Missile.ID:
                    system = AddSystem<CAB_MissileSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_MissileBomb.ID:
                    system = AddSystem<CAB_MissileBombSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_BulletFireWithStartPoint.ID:
                case Proto_Ability.Rpc_BulletFireDecalWithStartPoint.ID:
                    system = AddSystem<CAB_BulletWithStartPointSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_SplitCone.ID:
                    system = AddSystem<CAB_SplitConeSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_EffectFollowServerTransform.ID:
                    system = AddSystem<CAB_EffectFollowServerTransformSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_RangeFlash.ID:
                    system = AddSystem<CAB_RangeFlashSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_PlayFillScaleEffect.ID:
                    system = AddSystem<CAB_PlayFillScaleEffectSystem>(callBack);
                    break;
                case Proto_Ability.Rpc_SyncTransformEffect.ID:
                    system = AddSystem<CAB_SyncTransformEffect>(callBack);
                    break;
                case Proto_Ability.Rpc_PlayWarningEffect.ID:
                    system = AddSystem<CAB_PlayWarningEffectSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_PlayRay.ID:
                    system = AddSystem<CAB_PlayRaySystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_GoldJokerFloatingGun.ID:
                    system = AddSystem<CAB_GoldJokerFloatingGunSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_GoldJokerDollBomb.ID:
                    system = AddSystem<CAB_GoldJokerDollBombSystem>(callBack);
                    break;
                case Proto_AbilityAB_Auto.Rpc_GoldJokerFollowEffect.ID:
                    system = AddSystem<CAB_GoldJokerFollowEffect>(callBack);
                    break;
                default:
                    Debug.LogError("缺少 Ability 协议:" + sign);
                    break;
            }
            return system;
        }

        private void ClearSystem() {
            for (int i = 0; i < abilityList.Count; i++) {
                var system = abilityList[i];
                system.Clear();
            }
            abilityList.Clear();
            abilityList = null;
            abilityDic.Clear();
            abilityDic = null;
        }
    }
}