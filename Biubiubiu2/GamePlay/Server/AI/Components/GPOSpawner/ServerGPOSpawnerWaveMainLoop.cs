using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSpawnerWaveMainLoop : ServerNetworkComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public string configSign;
        }
        
        public class SpawnGPOData {
            public IGPO SpawnGPO;
            public ScenePoint SpawnPoint;
        }
        
        private AIGpoSpawnerConfig config = null;
        private SpawnAIWaveGroup nextWaveGroup = null;
        private SpawnAIWaveGroup currentWaveGroup = null;
        private List<SpawnAIWaveGroup> waveGroups = new List<SpawnAIWaveGroup>();
        private List<ScenePoint> spawnPoints = new List<ScenePoint>();
        private List<SpawnAIWaveInfo> waitSpawnWaveInfos = new List<SpawnAIWaveInfo>();
        private Dictionary<int, bool> spawnPointUseDic = new Dictionary<int, bool>();
        private List<SpawnGPOData> allSpawnedGpos = new List<SpawnGPOData>();
        private IGPO targetGpo;
        private float timeSinceLastSpawn = 0f;
        private int nextWaveGroupIndex = 0;
        private int currentWaveGroupIndex = 0;
        private bool isStart = false;
        private string configSign = "";
        private float deltaCheckAllDeadTime = 1f;
        
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPOSpawner.Event_AddSpawnerGPOEnd>(OnAddSpawnerGPOEndCallBack);
            var initData = (InitData)initDataBase;
            configSign = initData.configSign;
            targetGpo = iGPO;
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            LoadConfig();
        }

        protected override ITargetRpc SyncData() {
            return new Proto_GPOSpawnerWave.TargetRpc_SyncData {
                currentWaveIndex = (byte)currentWaveGroupIndex,
                nextWaveIndex = (byte)nextWaveGroupIndex,
                gpoIds = GetAllSpawnedGpoIds(),
                gpoMIds = GetAllSpawnedGpoMIds(),
            };
        }

        protected override void OnClear() {
            base.OnClear();
            WaveEnd(false);
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_GPOSpawner.Event_AddSpawnerGPOEnd>(OnAddSpawnerGPOEndCallBack);
            ClearAllSpawnedGpo();
            config = null;
            waveGroups.Clear();
            Debug.LogError(" GPO波次生成器被清理 ");
        }
        
        private void LoadConfig() {
            if (string.IsNullOrEmpty(configSign)) {
                return;
            }
            if (config != null) {
                return;
            }
            AssetManager.LoadAISO($"GpoSpawner/{configSign}", so => {
                if (isClear || config != null) {
                    return;
                }
                config = (AIGpoSpawnerConfig)so;
                waveGroups.AddRange(config.WaveGroups);
                isStart = waveGroups.Count > 0;
                nextWaveGroupIndex = 0;
                currentWaveGroupIndex = 0;
                Debug.Log(" 总共波次: " + waveGroups.Count );
                IntoNextWave();
            });
        }
        
        private void OnUpdate(float deltaTime) {
            if (isStart == false) {
                return;
            }
            CheckAllGpoDead();
            if (waitSpawnWaveInfos.Count <= 0) {
                if (nextWaveGroup == null || nextWaveGroupIndex >= waveGroups.Count) {
                    if (IsAllGpoDead()) {
                        WaveEnd(true);
                    }
                } else {
                    CheckGenerateWave();
                }
            } else {
                if (timeSinceLastSpawn >= nextWaveGroup.DelaySpawnTime) {
                    timeSinceLastSpawn = 0f;
                    IntoWave();
                } else {
                    var prevTime = timeSinceLastSpawn + deltaTime;
                    var isChange = Mathf.FloorToInt(prevTime) != Mathf.FloorToInt(timeSinceLastSpawn);
                    timeSinceLastSpawn = prevTime;
                    if (isChange) {
                        if (nextWaveGroup.NeedPrevSpawnGPOAllDead == false) {
                            if (IsAllGpoDead() && (nextWaveGroup.DelaySpawnTime - timeSinceLastSpawn) > 5f) {
                                timeSinceLastSpawn = nextWaveGroup.DelaySpawnTime - 5f;
                            }
                        }
                        SendTimeSinceLastSpawnToClient();
                    }
                }
            }
        }
        
        private void SendTimeSinceLastSpawnToClient() {
            var lostTime = Mathf.Max(0f, nextWaveGroup.DelaySpawnTime - timeSinceLastSpawn);
            Rpc(new Proto_GPOSpawnerWave.Rpc_DelayWaveSpawnTime {
                time = (ushort)(lostTime)
            });
        }

        private void OnAddSpawnerGPOEndCallBack(ISystemMsg msg, SE_GPOSpawner.Event_AddSpawnerGPOEnd ent) {
            targetGpo = ent.SpawnerGPO;
        }
        
        private void CheckGenerateWave() {
            // 如果当前波次需要所有敌人死亡后才进入下一波，跳过生成
            if (nextWaveGroup.NeedPrevSpawnGPOAllDead && IsAllGpoDead() == false) {
                return;
            }
            IntoNextWave();
        }
        
        private void IntoNextWave() {
            nextWaveGroup = waveGroups[nextWaveGroupIndex];
            nextWaveGroupIndex++;
            Debug.Log("准备进入下一个波次 : " + nextWaveGroupIndex + " / " + waveGroups.Count);
            GenerateWave(nextWaveGroup);
            mySystem.Dispatcher(new SE_GPOSpawner.Event_IntoNextWave {
                NextWaveIndex = nextWaveGroupIndex,
                NextWaveGroup = nextWaveGroup
            });
            Rpc(new Proto_GPOSpawnerWave.Rpc_IntoNextWave {
                currentWaveIndex = (byte)nextWaveGroupIndex
            });
        }

        private void WaveEnd(bool isTrue) {
            if (isStart == false) {
                return;
            }
            isStart = false;
            Debug.Log(" 波次结束 " + isTrue + " 当前波次: " + currentWaveGroupIndex + " / " + waveGroups.Count);
            Rpc(new Proto_GPOSpawnerWave.Rpc_WaveEnd {
                waveIndex = currentWaveGroupIndex,
                maxWaveIndex = waveGroups.Count,
                isWin = isTrue
            });
            mySystem.Dispatcher(new SE_GPOSpawner.Event_WaveEnd {
                WaveIndex = currentWaveGroupIndex,
                MaxWaveIndex = waveGroups.Count,
                IsWin = isTrue
            });
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = GpoID,
            });
        }

        private void GenerateWave(SpawnAIWaveGroup waveGroup) {
            // 过滤符合条件的生成点
            FilterSpawnPoints(waveGroup);
            waitSpawnWaveInfos.AddRange(waveGroup.WaveInfos);
            Debug.Log("创建波次数据: " + waveGroup.GroupSign + " 波次怪物数量: " + waveGroup.WaveInfos.Count + " 符合条件的生成点数量: " + spawnPoints.Count);
        }

        private void IntoWave() {
            if (currentWaveGroupIndex != nextWaveGroupIndex) {
                Debug.Log(" 进入波次 : " + nextWaveGroupIndex + " / " + waveGroups.Count);
                currentWaveGroupIndex = nextWaveGroupIndex;
                currentWaveGroup = nextWaveGroup;
                mySystem.Dispatcher(new SE_GPOSpawner.Event_IntoWaveEnd {
                    WaveIndex = currentWaveGroupIndex,
                    WaveGroup = nextWaveGroup
                });
                Rpc(new Proto_GPOSpawnerWave.Rpc_IntoWave {
                    currentWaveIndex = (byte)currentWaveGroupIndex
                });
            }
            for (int i = 0; i < waitSpawnWaveInfos.Count; i++) {
                var waveInfo = waitSpawnWaveInfos[i];
                if (waveInfo.DelayTime > 0) {
                    waveInfo.DelayTime -= Time.deltaTime;
                    continue;
                }
                // 如果完成，从列表中移除
                waitSpawnWaveInfos.RemoveAt(i);
                i--;
                SpawnAIWaveInfo(waveInfo);
            }
        }
        
        private void FilterSpawnPoints(SpawnAIWaveGroup waveGroup) {
            spawnPoints.Clear();
            MsgRegister.Dispatcher(new SM_Scene.GetRangeScenePoint {
                Point = iEntity.GetPoint(),
                MaxRange = waveGroup.MaxRange,
                MinRange = waveGroup.MinRange,
                PointTypes = waveGroup.SpawnPointTypeIds,
                CallBack = points => {
                    if (points.Count <= 0) {
                        Debug.Log($"波次：{waveGroup.GroupSign} Point: {iEntity.GetPoint()}  Range:{waveGroup.MinRange}/{waveGroup.MaxRange} PointTypes:{string.Join(",", waveGroup.SpawnPointTypeIds)} 没有符合条件的生成点");
                    } else {
                        spawnPoints.AddRange(GetMaxPointForRate(waveGroup, points));
                    }
                }
            });
        }

        private List<ScenePoint> GetMaxPointForRate(SpawnAIWaveGroup waveGroup, List<ScenePoint> points) {
            var maxCount = Mathf.Max(1, Mathf.CeilToInt(points.Count * waveGroup.MaxPointUsageRate));
            var list = new List<ScenePoint>(points);
            while (list.Count > maxCount) {
                var index = Random.Range(0, list.Count);
                list.RemoveAt(index);
            }
            // Debug.LogError(" 抽取坐标点数: " + list.Count + "/" + points.Count);
            return list;
        }

        private void SpawnAIWaveInfo(SpawnAIWaveInfo waveInfo) {
            if (spawnPoints.Count <= 0) {
                Debug.LogError("没有可用的生成点");
                return;
            }
            // Debug.LogError(" 开始怪物生成: " + waveInfo.AISign + " x " + waveInfo.SpawnCount + " 可以用坐标数量:" + spawnPoints.Count);
            var pointList = new List<ScenePoint>(spawnPoints);
            for (int i = 0; i < waveInfo.SpawnCount; i++) {
                var index = Random.Range(0, pointList.Count);
                var spawnPoint = pointList[index];
                pointList.RemoveAt(index);
                // 如果不允许重生 GPO，检查点位是否已经生成过
                if (config.AllowReSpawnOnPoints == false) {
                    if (spawnPointUseDic.ContainsKey(spawnPoint.ID)) {
                        if (pointList.Count <= 0) {
                            break;
                        } else {
                            continue;
                        }
                    }
                    spawnPointUseDic.Add(spawnPoint.ID, true);
                }
                SpawnAI(waveInfo.AISign, spawnPoint, nextWaveGroup.IsSpawnAIMoveToPoint);
                if (pointList.Count <= 0) {
                    if (config.AllowReSpawnOnPoints == false) {
                        break;
                    } else {
                        pointList.AddRange(spawnPoints);
                    }
                }
            }
        }

        private void SpawnAI(string aiSign, ScenePoint spawnPoint, bool isMoveToPoint) {
            MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                AISign = aiSign,
                StartPoint = spawnPoint.Point,
                OR_GpoType = GPOData.GPOType.AI,
                OR_CallBack = ai => {
                    var igpo = ai.GetGPO();
                    var data = new SpawnGPOData {
                        SpawnGPO = igpo,
                        SpawnPoint = spawnPoint,
                    };
                    allSpawnedGpos.Add(data);
                    if (isMoveToPoint) {
                        ai.Dispatcher(new SE_AI.Event_SetBaseHateTarget {
                            TargetGPO = targetGpo,
                        });
                    }
                    ai.Dispatcher(new SE_GPO.Event_SetLevel {
                        Level = currentWaveGroup.Level,
                    });
                    Rpc(new Proto_GPOSpawnerWave.Rpc_SpawnerGpo {
                        gpoId = igpo.GetGpoID(),
                        gpoMId = igpo.GetGpoMID(),
                    });
                }
            });
        }

        private void CheckAllGpoDead() {
            deltaCheckAllDeadTime -= Time.deltaTime;
            if (deltaCheckAllDeadTime > 0f) {
                return;
            }
            deltaCheckAllDeadTime = 1f;
            var count = allSpawnedGpos.Count;
            for (int i = count - 1; i >= 0; i--) {
                var spawnData = allSpawnedGpos[i];
                var gpo = spawnData.SpawnGPO;
                if (gpo.IsClear() || gpo.IsDead()) {
                    if (config.AllowReSpawnOnPoints == false) {
                        spawnPointUseDic.Remove(spawnData.SpawnPoint.ID);
                    }
                    allSpawnedGpos.RemoveAt(i);
                    Rpc(new Proto_GPOSpawnerWave.Rpc_DeadGpo {
                        gpoId = gpo.GetGpoID()
                    });
                }
            }
        }
        
        private void ClearAllSpawnedGpo() {
            var count = allSpawnedGpos.Count;
            for (int i = count - 1; i >= 0; i--) {
                var spawnData = allSpawnedGpos[i];
                var gpo = spawnData.SpawnGPO;
                gpo?.Dispatcher(new SE_AI.Event_OnRemoveAI());
            }
        }
        
        private bool IsAllGpoDead() {
            return allSpawnedGpos.Count <= 0;
        }
        
        private int[] GetAllSpawnedGpoIds() {
            var ids = new List<int>();
            foreach (var spawnData in allSpawnedGpos) {
                ids.Add(spawnData.SpawnGPO.GetGpoID());
            }
            return ids.ToArray();
        }
        
        private int[] GetAllSpawnedGpoMIds() {
            var ids = new List<int>();
            foreach (var spawnData in allSpawnedGpos) {
                ids.Add(spawnData.SpawnGPO.GetGpoMID());
            }
            return ids.ToArray();
        }
    }
}