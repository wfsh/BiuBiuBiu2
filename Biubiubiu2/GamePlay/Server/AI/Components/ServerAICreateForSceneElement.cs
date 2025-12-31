using System;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAICreateForSceneElement : ComponentBase {
        public class AreaData {
            public float area;
            public Vector3 point;
            public PatrolPointData patrolPoint;
            public string monsterSign;
            public List<Vector3> createPointList;
            public float triggerDistance; // 触发加载的距离
            public IGPO gpo;
        }

        private List<AreaData> monsterList = new List<AreaData>();
        private List<IGPO> roleGpoList = new List<IGPO>();
        private float refreshTime = 0.0f;
        private float terrainSyncYOffset = 7.5f;

        protected override void OnAwake() {
            MsgRegister.Register<M_Game.LoginGameScene>(OnLoginGameSceneCallBack);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            MsgRegister.Dispatcher(new SM_GPO.GetGPOListForGpoType {
                GpoType = GPOData.GPOType.Role,
                CallBack = delegate(List<IGPO> list) {
                    roleGpoList = list;
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Game.LoginGameScene>(OnLoginGameSceneCallBack);
            monsterList.Clear();
        }

        private void OnLoginGameSceneCallBack(M_Game.LoginGameScene ent) {
            refreshTime = 0f;
            MsgRegister.Dispatcher(new SM_Scene.GetMonsterPointData {
                CallBack = OnGetSceneElementListCallBack
            });
        }

        private void OnGetSceneElementListCallBack(List<AISpawnPoint> list) {
            for (int i = 0; i < list.Count; i++) {
                var sceneArea = list[i];
                if (sceneArea.IsEnable == false) {
                    continue;
                }
                for (int j = 0; j < sceneArea.SpawnCount; j++) {
                    var areaData = new AreaData();
                    areaData.area = sceneArea.SpawnRadius;
                    var addSign = GetMonsterForWeight(sceneArea.AIList);
                    areaData.monsterSign = addSign;
                    areaData.patrolPoint = sceneArea.PatrolPoint;
                    if (sceneArea.PatrolPoint.PatrolType == SceneConfig.PatrolType.Range) {
                        areaData.createPointList = sceneArea.PatrolPoint.PointList.ToList();
                    } else {
                        areaData.createPointList = new List<Vector3>();
                    }
                    areaData.createPointList.Add(sceneArea.Point);
                    var startPointIndex = Random.Range(0, areaData.createPointList.Count);
                    areaData.point = areaData.createPointList[startPointIndex];
                    if(CheckCreatePointValid(areaData.point, addSign) == false) {
#if UNITY_EDITOR
                        Debug.LogError($"已经在当前坐标生成过 AI [{addSign}]，忽略相同坐标的 AI Point {areaData.point}");
#endif
                        continue;
                    }
                    var data = GpoSet.GetGPOMData(addSign, ModeData.MatchId);
                    if (data == null || data.GetId() == 0) {
                        data = GpoSet.GetGPOMData(addSign);
                    }
                    if (data.GetQuality() >= (int)AIData.AIQuality.Elite) {
                        areaData.triggerDistance = -1f; // 精英及以上品质，使用全局触发距离
                    } else {
                        areaData.triggerDistance = NetworkData.GetBehaviourSyncDistance();
                    }
                    monsterList.Add(areaData);
                }
            }
            refreshTime = 0f;
        }
        
        private bool CheckCreatePointValid(Vector3 point, string aiSign) {
            for (int i = 0; i < monsterList.Count; i++) {
                var data = monsterList[i];
                if (data.monsterSign == aiSign && Vector3.Distance(point, data.point) < 0.5f) {
                    return false;
                }
            }
            return true;
        }

        private void OnUpdate(float delta) {
            if (refreshTime > 0f) {
                refreshTime -= delta;
                return;
            }
            refreshTime = 1f;
            for (int i = 0; i < monsterList.Count; i++) {
                var data = monsterList[i];
                if (data.triggerDistance <= -1f) {
                    if (data.gpo == null) {
                        CreateMonster(data);
                    }
                } else {
                    if (data.gpo != null) {
                        var hasNearbyPlayer = IsAnyPlayerNearby(data.gpo.GetPoint(), data.triggerDistance * 1.5f);
                        if (!hasNearbyPlayer) {
                            RemoveMonstersInNearby(data);
                        }
                    } else {
                        var hasNearbyPlayer = IsAnyPlayerNearby(data.point, data.triggerDistance);
                        if (hasNearbyPlayer) {
                            CreateMonster(data);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 删除区域内所有已加载的怪物（根据每个AI的实际位置判断）
        /// </summary>
        private void RemoveMonstersInNearby(AreaData areaData) { 
            var monster = areaData.gpo;
            // 怪物已被清除，直接从列表移除
            if (monster == null) {
                return;
            }
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = monster.GetGpoID(),
            });
            areaData.gpo = null;
        }

        /// <summary>
        /// 检测是否有玩家在指定位置的触发范围内
        /// </summary>
        private bool IsAnyPlayerNearby(Vector3 monsterPoint, float triggerDistance) {
            var sqrTriggerDistance = triggerDistance * triggerDistance;
            for (int i = 0; i < roleGpoList.Count; i++) {
                var gpo = roleGpoList[i];
                if (gpo == null || gpo.IsClear()) {
                    continue;
                }
                var network = gpo.GetNetwork();
                if (network == null || network.IsDestroy()) {
                    continue;
                }
                var playerPos = gpo.GetPoint();
                if (monsterPoint.y < terrainSyncYOffset) {
                    if (playerPos.y > terrainSyncYOffset) {
                        continue;
                    }
                } else {
                    if (playerPos.y < terrainSyncYOffset) {
                        continue;
                    }
                }
                var sqrDistance = (playerPos - monsterPoint).sqrMagnitude;
                if (sqrDistance <= sqrTriggerDistance) {
                    return true;
                }
            }
            return false;
        }

        private void CreateMonster(AreaData areaData) {
            MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                OR_CallBack = delegate(IAI monster) {
                    areaData.gpo = monster.GetGPO();
                    monster.Dispatcher(new SE_AI.Event_SetPatrolPoint {
                        PatrolPointData = areaData.patrolPoint,
                        Area = areaData.area,
                    });
                    monster.Register<SE_GPO.Event_SetIsDead>(OnMonsterDead);
                },
                AISign = areaData.monsterSign,
                StartPoint = areaData.point,
                OR_GpoType = GPOData.GPOType.AI
            });
        }
        
        private void OnMonsterDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            for (int i = 0; i < monsterList.Count; i++) {
                var areaData = monsterList[i];
                if (areaData.gpo != null && areaData.gpo.GetGpoID() == body.GetGPOId()) {
                    monsterList.RemoveAt(i);
                    break;
                }
            }
        }

        // 根据权重 List<MonsterWeight> 加载怪物
        private string GetMonsterForWeight(List<AIWeight> monsterSigns) {
            var totalWeight = 0;
            for (int i = 0; i < monsterSigns.Count; i++) {
                totalWeight += monsterSigns[i].Weight;
            }
            var randomWeight = Random.Range(0, totalWeight);
            for (int i = 0; i < monsterSigns.Count; i++) {
                var data = monsterSigns[i];
                randomWeight -= data.Weight;
                if (randomWeight <= 0) {
                    return data.AISign;
                }
            }
            return "";
        }
    }
}