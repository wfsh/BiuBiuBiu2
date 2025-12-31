using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOMat : ServerNetworkComponentBase {
        public enum MatType {
            None = 0,
            Hades, //冥王
            Kadura, //海王
            Kitty, //猫女
            HideSeek, //躲猫猫
            CaptainCard, //幽灵船长
            OnlyUp //选中
        }
        private const string ALL_TEAM = "ALL_TEAM";
        
        public struct KittyRadarData {
            public int KittyRadarId;
            public IGPO AttackGPO;
            public float Range;
        }
        
        public class TeamMatData {
            public List<KittyRadarData> kittyRadarList = new List<KittyRadarData>();
            public float skirmisherTime = 0f;
            public byte allTeamMatType = 0;
            public MatType matType = MatType.None;
        }
        private Dictionary<string, TeamMatData> matTypeForTeam = new Dictionary<string, TeamMatData>();
        private readonly List<string> reusableRemoveList = new List<string>(7);
        private float updateDeltaTime = 0.0f;
        private float kittyDelayCheckTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Sausage.StartKittyRadar>(OnStartKittyRadarCallBack);
            MsgRegister.Register<SM_Sausage.EndKittyRadar>(OnEndKittyRadarCallBack);
            mySystem.Register<SE_AI.StartSkirmisher>(OnStartSkirmisherCallBack);
            mySystem.Register<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Sausage.StartKittyRadar>(OnStartKittyRadarCallBack);
            MsgRegister.Unregister<SM_Sausage.EndKittyRadar>(OnEndKittyRadarCallBack);
            mySystem.Unregister<SE_AI.StartSkirmisher>(OnStartSkirmisherCallBack);
            mySystem.Unregister<SE_AbilityEffect.Event_UpdateEffect>(OnUpdateEffectCallBack);
            RemoveUpdate(OnUpdate);
        }

        protected override void Sync(List<INetworkCharacter> network) {
            if (network == null || network.Count == 0) {
                Debug.LogWarning("Sync 调用时 network 列表为空");
                return;
            }

            var maxTeamIdLength = 1000;

            foreach (var kv in matTypeForTeam) {
                var teamId = kv.Key;
                var teamData = kv.Value;

                if (teamData.matType == MatType.None) {
                    continue;
                }

                if (teamId.Length > maxTeamIdLength) {
                    Debug.LogError($"teamId 长度超过 {maxTeamIdLength}，可能是错误的 teamId: {teamId}");
                    continue;
                }

                foreach (var iNetWork in network) {
                    if (iNetWork is INetwork net) {
                        var msg = new Proto_GPO.TargetRpc_GpoMat {
                            matType = (byte)teamData.matType,
                            teamId = teamId
                        };
                        TargetRpc(net, msg);
                    } else {
                        Debug.LogError($"network 参数无法转换为 INetwork，实际类型是 {network.GetType()}");
                    }
                }
            }
        }
        
        private void OnUpdate(float delta) {
            if (updateDeltaTime > 0) {
                updateDeltaTime -= delta;
                return;
            }
            updateDeltaTime = 0.5f;
            UpdateMatState();
        }
        
        private void OnStartKittyRadarCallBack(SM_Sausage.StartKittyRadar ent) {
            if (ent.KittyRadarId == 0) {
                Debug.LogError("KittyRadarId 为 0");
                return;
            }
            IGPO attackGPO = null;
            MsgRegister.Dispatcher(new SM_Character.GetCharacterForePlayerId() {
                CallBack = gpo => {
                    attackGPO = gpo;
                },
                PlayerId = ent.AttackPlayerId
            });
            if (attackGPO == null) {
                return;
            }
            TeamMatData teamData;
            if (matTypeForTeam.TryGetValue(ent.SausageTeamId, out teamData) == false) {
                teamData = new TeamMatData();
                matTypeForTeam.Add(ent.SausageTeamId, teamData);
            }
            teamData.kittyRadarList.Add(new KittyRadarData {
                KittyRadarId = ent.KittyRadarId,
                AttackGPO = attackGPO,
                Range = ent.Range,
            });
            UpdateMatState();
        }

        private void OnEndKittyRadarCallBack(SM_Sausage.EndKittyRadar ent) {
            TeamMatData teamData;
            if (matTypeForTeam.TryGetValue(ent.SausageTeamId, out teamData) == false) {
                return;
            }
            var isTrue = false;
            var count = teamData.kittyRadarList.Count - 1;
            for (int i = count; i >= 0; i--) {
                var data = teamData.kittyRadarList[i];
                if (data.KittyRadarId == ent.KittyRadarId) {
                    teamData.kittyRadarList.RemoveAt(i);
                    isTrue = true;
                }
            }
            if (isTrue) {
                UpdateMatState();
            }
        }

        private void OnStartSkirmisherCallBack(ISystemMsg body, SE_AI.StartSkirmisher ent) {
            TeamMatData teamData;
            if (matTypeForTeam.TryGetValue(ent.SausageTeamId, out teamData) == false) {
                teamData = new TeamMatData();
                matTypeForTeam.Add(ent.SausageTeamId, teamData);
            }
            teamData.skirmisherTime = Time.realtimeSinceStartup + ent.LifeTime;
            UpdateMatState();
        }

        private void OnUpdateEffectCallBack(ISystemMsg body, SE_AbilityEffect.Event_UpdateEffect ent) {
            TeamMatData teamData;
            if (matTypeForTeam.TryGetValue(ALL_TEAM, out teamData) == false) {
                teamData = new TeamMatData();
                matTypeForTeam.Add(ALL_TEAM, teamData);
            }
            teamData.allTeamMatType = (byte)ent.Value;
            // 这里需要注意，ALL_TEAM 只会有一个  
            // 没有开发完，等有这个需求在继续
        }
         
        private void UpdateMatState() {
            var nowTime = Time.realtimeSinceStartup;
            reusableRemoveList.Clear();
            foreach (var teamId in matTypeForTeam.Keys) {
                var newMatType = MatType.None;
                var teamMatData = matTypeForTeam[teamId];
                if (teamMatData.kittyRadarList.Count > 0) {
                    for (int i = 0; i < teamMatData.kittyRadarList.Count; i++) {
                        var kittyRadarData = teamMatData.kittyRadarList[i];
                        if (kittyRadarData.AttackGPO.IsClear() == false) {
                            var distance = Vector3.Distance(iEntity.GetPoint(), kittyRadarData.AttackGPO.GetPoint());
                            if (distance < kittyRadarData.Range) {
                                newMatType = MatType.Kitty;
                                break;
                            }
                        }
                    }
                } else if (nowTime <= teamMatData.skirmisherTime) {
                    newMatType = MatType.Kitty;
                }
                if (teamMatData.matType != newMatType) {
                    teamMatData.matType = newMatType;
                    Rpc(new Proto_GPO.Rpc_GpoMat {
                        matType = (byte)teamMatData.matType,
                        teamId = teamId,
                    });
                } else {
                    if (teamMatData.kittyRadarList.Count <= 0 &&teamMatData.matType == MatType.None) {
                        reusableRemoveList.Add(teamId); // 收集待删除项
                    }
                }
            }
            if (reusableRemoveList.Count > 0) {
                // 遍历完成后再统一删除
                foreach (var teamId in reusableRemoveList) {
                    matTypeForTeam.Remove(teamId);
                }
            }
        }
    }
}