using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneSerialize : ComponentBase {
        private SceneConfig sceneConfig;
        private List<SM_Scene.GetMonsterPointData> GetMonsterSpawnPointCallBackList =
            new List<SM_Scene.GetMonsterPointData>();
        private List<SM_Scene.GetPlayerPointList> GetPlayerPointListCallBackList =
            new List<SM_Scene.GetPlayerPointList>();
        private List<SM_Scene.GetRoomAreas> GetRoomAreasCallBackList =
            new List<SM_Scene.GetRoomAreas>();

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Register<SM_Scene.GetMonsterPointData>(OnGetMonsterPointDataCallBack);
            MsgRegister.Register<SM_Scene.GetPlayerPointList>(OnGetPlayerPointListCallBack);
            MsgRegister.Register<SM_Scene.GetRoomAreas>(OnGetRoomAreasCallBack);
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
            MsgRegister.Unregister<SM_Scene.GetMonsterPointData>(OnGetMonsterPointDataCallBack);
            MsgRegister.Unregister<SM_Scene.GetPlayerPointList>(OnGetPlayerPointListCallBack);
            MsgRegister.Unregister<SM_Scene.GetRoomAreas>(OnGetRoomAreasCallBack);
        }

        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            SerializeConfig();
        }

        private void SerializeConfig() {
            var id = ModeData.SceneId;
            if (SceneData.TestSceneConfig != null) {
                SetSceneConfig(SceneData.TestSceneConfig);
            } else {
                if (SceneData.HasSceneData(id) == false) {
                    return;
                }
                var sceneData = SceneData.Get(id);
                var elementSign = sceneData.ElementConfig;
                if (String.IsNullOrEmpty(elementSign)) {
                    return;
                }
                AssetManager.LoadScenesConfigs(elementSign, asset => {
                    SetSceneConfig(asset as SceneConfig);
                });
            }
        }
        
        private void SetSceneConfig(SceneConfig config) {
            sceneConfig = config;
            SendMonsterSpawnPointData();
            SendPlayerSpawnPointData();
            SendScenePoints();
            SendElementSpawnPointData();
            SendRoomAreas();
            MsgRegister.Dispatcher(new SM_Scene.SceneSerialized());
        }

        private void OnGetMonsterPointDataCallBack(SM_Scene.GetMonsterPointData ent) {
            if (sceneConfig != null) {
                ent.CallBack.Invoke(sceneConfig.AISpawnPoints);
            } else {
                GetMonsterSpawnPointCallBackList.Add(ent);
            }
        }

        private void SendMonsterSpawnPointData() {
            for (int i = 0; i < GetMonsterSpawnPointCallBackList.Count; i++) {
                var ent = GetMonsterSpawnPointCallBackList[i];
                OnGetMonsterPointDataCallBack(ent);
            }
            GetMonsterSpawnPointCallBackList.Clear();
        }

        private void OnGetPlayerPointListCallBack(SM_Scene.GetPlayerPointList ent) {
            if (sceneConfig != null) {
                var list = new List<PlayerSpawnPoint>();
                for (int i = 0; i < sceneConfig.PlayerSpawnPoints.Count; i++) {
                    var data = sceneConfig.PlayerSpawnPoints[i];
                    if (data.IsEnable && data.Team == ent.TeamIndex) {
                        list.Add(data);
                    }
                }
                ent.CallBack.Invoke(list);
            } else {
                GetPlayerPointListCallBackList.Add(ent);
            }
        }

        private void SendPlayerSpawnPointData() {
            for (int i = 0; i < GetPlayerPointListCallBackList.Count; i++) {
                var ent = GetPlayerPointListCallBackList[i];
                OnGetPlayerPointListCallBack(ent);
            }
            GetPlayerPointListCallBackList.Clear();
        }
        
        private void SendScenePoints() {
            mySystem.Dispatcher(new SE_Scene.SendScenePoints {
                ScenePoints = sceneConfig.ScenePoints
            });
        }

        private void SendElementSpawnPointData() {
            mySystem.Dispatcher(new SE_Scene.SendElementSpawnPointList {
                ElementSpawnPoints = sceneConfig.ElementSpawnPoints,
            });
        }

        private void OnGetRoomAreasCallBack(SM_Scene.GetRoomAreas ent) {
            if (sceneConfig != null) {
                ent.CallBack.Invoke(sceneConfig.RoomAreas);
            } else {
                GetRoomAreasCallBackList.Add(ent);
            }
        }

        private void SendRoomAreas() {
            foreach (var ent in GetRoomAreasCallBackList) {
                OnGetRoomAreasCallBack(ent);
            }
            GetRoomAreasCallBackList.Clear();
        }
    }
}