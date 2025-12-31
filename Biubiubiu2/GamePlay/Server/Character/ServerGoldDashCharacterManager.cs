using System.Collections;
using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashCharacterManager : ManagerBase {
        private int perf_RoleCount = PerfAnalyzerKey.StringToHash("在线角色数量");
        private int perf_OffLineRoleCount = PerfAnalyzerKey.StringToHash("断线角色数量");
        private float perfDeltaTime = 0.0f;
        private List<ServerGoldDashCharacterSystem> characterList = new List<ServerGoldDashCharacterSystem>();
        
        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Register<SM_Character.AddCharacterGoldDash>(OnCmdLoginWarCallBack);
            MsgRegister.Register<SM_Character.ChangeLockSausageRole>(OnChangeLockPlayer);
            MsgRegister.Register<SM_Character.GetCharacterForePlayerId>(OnGetCharacterForePlayerIdCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Character.AddCharacterGoldDash>(OnCmdLoginWarCallBack);
            MsgRegister.Unregister<SM_Character.ChangeLockSausageRole>(OnChangeLockPlayer);
            MsgRegister.Unregister<SM_Character.GetCharacterForePlayerId>(OnGetCharacterForePlayerIdCallBack);
            RemoveAllRole();
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
            var roleCount = 0;
            var offLineRoleCount = 0;
            for (int i = 0; i < characterList.Count; i++) {
                var role = characterList[i];
                if (role.network != null) {
                    roleCount++;
                } else {
                    offLineRoleCount++;
                }
            }
            PerfAnalyzerAgent.SetCustomRecorder(perf_RoleCount, roleCount);
            PerfAnalyzerAgent.SetCustomRecorder(perf_OffLineRoleCount, offLineRoleCount);
        }

        public void OnCmdLoginWarCallBack(SM_Character.AddCharacterGoldDash ent) {
            Debug.Log("登录服务器：" + ent.Name + " PID:" + ent.PlayerId);
            var character = GetCharacter(ent.PlayerId);
            if (character == null) {
                var teamId = GetTeamId(ent.TeamId);
                var gpoMId = GPOM_CharacterSet.Id_Character;
                var mData = GpoSet.GetGPOMData(gpoMId);
                character = AddSystem(delegate(ServerGoldDashCharacterSystem character) {
                    character.SetCharacterData(ent.PlayerId,  ent.Name, teamId, mData, null);
                    character.SetNetwork(ent.INetwork);
                    characterList.Add(character);
                });
            } else {
                var oldNetwork = character.network;
                if (oldNetwork != null && oldNetwork.IsDestroy() == false) {
                    Debug.LogError("旧的链接还存在，断开旧的链接：" + character.NickName);
                    oldNetwork.ConnectionToClientDisconnect();
                }
                character.SetNetwork(ent.INetwork);
                Debug.Log("老玩家 队伍ID： " + character.GetGPO().GetTeamID());
            }
            var igpo = character.GetGPO();
            ent.INetwork.TargetRpc(ent.INetwork, new Proto_Login.TargetRpc_RoleInfo {
                 gpoId = igpo.GetGpoID(), 
                 teamId = igpo.GetTeamID(), 
                 point = igpo.GetPoint(), 
                 rota = igpo.GetRota()
            });
            Debug.Log("[GoldDash]成功登录服务器 创建服务器映射角色：" + ent.Name + " GPOID:" + igpo.GetGpoID() + " ConnId:" +ent.INetwork.ConnId() + " point" + igpo.GetPoint());
            MsgRegister.Dispatcher(new SM_Character.CharacterLogin {
                INetwork = ent.INetwork,
                GpoID = igpo.GetGpoID(),
            });
        }
        
        public void OnChangeLockPlayer(SM_Character.ChangeLockSausageRole ent) {
            Debug.Log("登录服务器：" + ent.PlayerId + " PID:" + ent.LockPlayerId);
            var lockCharacter = GetCharacter(ent.LockPlayerId);
            if (lockCharacter == null) {
                Debug.LogError($"Error lockCharacter Null {ent.LockPlayerId}");
                return;
            }
            var character = GetCharacter(ent.PlayerId);
            if (character != null) {
                character.Dispatcher(new SE_Character.SetSausageLockGPO() {
                    LockGpo = lockCharacter.GetGPO(),
                });
            }
        }
        
        public void OnGetCharacterForePlayerIdCallBack(SM_Character.GetCharacterForePlayerId ent) {
            var character = GetCharacter(ent.PlayerId);
            if (character != null) {
                ent.CallBack?.Invoke(character.GetGPO());
            } else {
                ent.CallBack?.Invoke(null);
            }
        }

        private ServerGoldDashCharacterSystem GetCharacter(long playerId) {
            for (int i = 0; i < characterList.Count; i++) {
                var character = characterList[i];
                if (character.PlayerId == playerId) {
                    return character;
                }
            }
            return null;
        }

        private void RemoveAllRole() {
            for (int i = 0; i < characterList.Count; i++) {
                var role = characterList[i];
                role.Clear();
            }
            characterList.Clear();
        }
    }
}