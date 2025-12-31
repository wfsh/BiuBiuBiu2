using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterManager : ManagerBase {
        private bool isSceneSerialized;
        private int perf_RoleCount = PerfAnalyzerKey.StringToHash("在线角色数量");
        private int perf_OffLineRoleCount = PerfAnalyzerKey.StringToHash("断线角色数量");
        private float perfDeltaTime = 0.0f;
        private List<ServerCharacterSystem> characterList = new List<ServerCharacterSystem>();
        private List<SM_Character.AddCharacter> addCharacterList = new List<SM_Character.AddCharacter>();
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Character.AddCharacter>(OnCmdLoginWarCallBack);
            MsgRegister.Register<SM_Scene.SceneSerialized>(OnSceneSerializedCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Character.AddCharacter>(OnCmdLoginWarCallBack);
            MsgRegister.Unregister<SM_Scene.SceneSerialized>(OnSceneSerializedCallBack);
            RemoveAllRole();
        }

        public void OnCmdLoginWarCallBack(SM_Character.AddCharacter ent) {
            var loginData = (Proto_Login.Cmd_Login_Stage)ent.ICmdData;
            if (isSceneSerialized) {
                AddCharacter(ent);
            } else {
                for (int i = 0; i < addCharacterList.Count; i++) {
                    var oldLoginData = (Proto_Login.Cmd_Login_Stage)ent.ICmdData;
                    if (oldLoginData.playerId == loginData.playerId) {
                        addCharacterList[i] = ent;
                        return;
                    }
                }
                addCharacterList.Add(ent);
            }
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
                if (role.network != null && role.network.IsOnline()) {
                    roleCount++;
                } else {
                    offLineRoleCount++;
                }
            }
            PerfAnalyzerAgent.SetCustomRecorder(perf_RoleCount, roleCount);
            PerfAnalyzerAgent.SetCustomRecorder(perf_OffLineRoleCount, offLineRoleCount);
        }

        private ServerCharacterSystem GetCharacter(long playerId) {
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

        private void AddCharacter(SM_Character.AddCharacter ent) {
            var loginData = (Proto_Login.Cmd_Login_Stage)ent.ICmdData;
            Debug.Log("登录服务器：" + loginData.name + " PID:" + loginData.playerId);
            PerfAnalyzerAgent.SetLog("登录服务器：" + loginData.name + " PID:" + loginData.playerId);
            var character = GetCharacter(loginData.playerId);
            if (character == null) {
                var teamId = 0;
                MsgRegister.Dispatcher(new SM_Mode.GetTeamId {
                    PlayerId = loginData.playerId,
                    CallBack = modeTeamId => {
                        teamId = modeTeamId;
                    }
                });
                IGPOInData inData = null;
                var gpoMId = GPOM_CharacterSet.Id_Character;
                MsgRegister.Dispatcher(new SM_Character.GetCharacterDataByPlayerId() {
                    PlayerId = loginData.playerId,
                    CallBack = (data) => {
                        gpoMId = data.GPOMId;
                        inData = data.InData;
                    }
                });
                var mData = GpoSet.GetGPOMData(gpoMId);
                character = AddSystem(delegate(ServerCharacterSystem character) {
                    character.SetCharacterData(loginData.playerId,  loginData.name, teamId, mData, inData);
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
                rota = igpo.GetRota(),
            });
            Debug.Log("成功登录服务器 创建服务器映射角色：" + loginData.name + " GPOID:" + igpo.GetGpoID() + " ConnId:" +ent.INetwork.ConnId());
            MsgRegister.Dispatcher(new SM_Character.CharacterLogin {
                INetwork = ent.INetwork,
                GpoID = igpo.GetGpoID(),
            });
        }

        private void OnSceneSerializedCallBack(SM_Scene.SceneSerialized ent) {
            isSceneSerialized = true;
            foreach (var character in addCharacterList) {
                AddCharacter(character);
            }
            addCharacterList.Clear();
        }
    }
}