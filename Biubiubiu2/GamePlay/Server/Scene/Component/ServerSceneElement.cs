using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneElement : ComponentBase {
        public List<ElementSpawnPoint> elementSpawnPoints;

        // 采集者数据
        public class GatheringData {
            public int CreateID; // 生成 ID
            public IGPO GatheringGPO; // 采集 GPO
            public float EndGatheringTime; // 采集结束时间
        }

        private List<SE_Scene.ElementCreateData> resourceList = new List<SE_Scene.ElementCreateData>();
        private List<SE_Scene.ElementCreateData> nextResetList = new List<SE_Scene.ElementCreateData>();
        private List<GatheringData> gatheringDataList = new List<GatheringData>();
        private float delayGatheringTime = 0f;
        private int IDIndex = 0;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Scene.SendElementSpawnPointList>(OnSendElementSpawnPointListCallBack);
            MsgRegister.Register<SM_Character.CharacterLogin>(OnCharacterLogin);
            MsgRegister.Register<SM_SceneElement.Event_StartGathering>(OnStartGathering);
            MsgRegister.Register<SM_SceneElement.Event_CancelGathering>(OnCancelGathering);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Scene.SendElementSpawnPointList>(OnSendElementSpawnPointListCallBack);
            MsgRegister.Unregister<SM_SceneElement.Event_StartGathering>(OnStartGathering);
            MsgRegister.Unregister<SM_SceneElement.Event_CancelGathering>(OnCancelGathering);
            MsgRegister.Unregister<SM_Character.CharacterLogin>(OnCharacterLogin);
            resourceList.Clear();
            nextResetList.Clear();
            gatheringDataList.Clear();
            elementSpawnPoints = null;
        }

        private void OnUpdate(float deltaTime) {
            if (delayGatheringTime > 0f) {
                delayGatheringTime -= deltaTime;
                return;
            }
            delayGatheringTime = 0.1f;
            GatheringUpdate();
            ResetResourceGatheringCount();
        }

        private void OnCharacterLogin(SM_Character.CharacterLogin ent) {
            if (ent.INetwork == null) {
                Debug.LogError("networkBase 没有正确赋值");
                return;
            }
            for (int i = 0; i < resourceList.Count; i++) {
                var data = resourceList[i];
                if (data.GatheringCount > 0) {
                    TargetRpc(ent.INetwork, new Proto_Scene.TargetRpc_AddSceneElement() {
                        element = (ushort)data.Element, createID = data.CreateID, point = data.Point,
                    });
                }
            }
        }

        private void OnSendElementSpawnPointListCallBack(ISystemMsg body, SE_Scene.SendElementSpawnPointList ent) {
            elementSpawnPoints = ent.ElementSpawnPoints;
            for (int i = 0; i < elementSpawnPoints.Count; i++) {
                var element = elementSpawnPoints[i];
                for (int j = 0; j < element.PointList.Count; j++) {
                    var pointData = element.PointList[j];
                    AddSceneElement(element, pointData.Point);
                }
            }
            mySystem.Dispatcher(new SE_Scene.SendElementCreateList {
                ElementCreateDataList = resourceList,
            });
        }

        private void AddSceneElement(ElementSpawnPoint modeData, Vector3 point) {
            var data = new SE_Scene.ElementCreateData();
            data.CreateID = ++IDIndex;
            if (modeData.SpawnDelayTime > 0) {
                data.NextResetTime = Time.realtimeSinceStartup + modeData.SpawnDelayTime;
                data.GatheringCount = 0;
                nextResetList.Add(data);
            } else {
                data.GatheringCount = modeData.GatheringLimit;
                data.NextResetTime = 0f;
            }
            data.Point = point;
            data.ModeData = modeData;
            CreateElement(data);
            Rpc(new Proto_Scene.Rpc_AddSceneElement {
                element = (ushort)data.Element, createID = data.CreateID, point = data.Point,
            });
            resourceList.Add(data);
        }

        private void OnStartGathering(SM_SceneElement.Event_StartGathering ent) {
            var data = GetResourceData(ent.CreateId);
            var gatheringData = GatheringResource(ent.IGpo, data);
            if (gatheringData == null) {
                NotifyGatheringState(ent.IGpo, false, 0);
            } else {
                NotifyGatheringState(ent.IGpo, data.ModeData.HarvestTime > 0, data.GatheringCount);
            }
        }

        private void OnCancelGathering(SM_SceneElement.Event_CancelGathering ent) {
            CancelGathering(ent.IGpo);
        }

        private void CreateElement(SE_Scene.ElementCreateData data) {
            if (data.ModeData.HarvestElements.Count <= 0) {
                Debug.LogError($"没有配置采集物品 {data.ModeData.GroupName}");
                return;
            }
            data.Element = GetElementForWeight(data.ModeData.HarvestElements);
        }

        private SceneData.ElementEnum GetElementForWeight(List<HarvestElementData> list) {
            if (list == null || list.Count <= 0) {
                Debug.LogError("采集配置数据错误 <= 0");
                return SceneData.ElementEnum.None;
            }
            if (list.Count == 1) {
                return list[0].Element;
            }
            var weight = 0;
            for (int i = 0; i < list.Count; i++) {
                var elementData = list[i];
                if (elementData.Weight == 0) {
                    Debug.LogError($"{elementData.Element} 采集物品权重为 0");
                }
                weight += list[i].Weight;
            }
            var random = Random.Range(0, weight);
            var sum = 0;
            for (int i = 0; i < list.Count; i++) {
                var data = list[i];
                sum += data.Weight;
                if (random < sum) {
                    return data.Element;
                }
            }
            Debug.LogError($"没有找到对应的物品 {random}");
            return SceneData.ElementEnum.None;
        }

        // 采集资源
        public GatheringData GatheringResource(IGPO gpo, SE_Scene.ElementCreateData data) {
            if (data == null) {
                Debug.LogError("没有找到对应的资源");
                return null;
            }
            if (data.GatheringCount <= 0) {
                Debug.LogError("资源采集次数已用完");
                return null;
            }
            if (CheckGPOIsGathering(gpo)) {
                Debug.LogError("该 GPO 正在采集其他资源");
                return null;
            }
            var gatheringData = new GatheringData();
            gatheringData.CreateID = data.CreateID;
            gatheringData.GatheringGPO = gpo;
            gatheringData.EndGatheringTime = Time.realtimeSinceStartup + data.ModeData.HarvestTime;
            gatheringDataList.Add(gatheringData);
            return gatheringData;
        }

        // 中途取消采集
        public void CancelGathering(IGPO gpo) {
            for (int i = gatheringDataList.Count - 1; i >= 0; i--) {
                var data = gatheringDataList[i];
                if (data.GatheringGPO == gpo) {
                    gatheringDataList.RemoveAt(i);
                    NotifyGatheringState(data.GatheringGPO, false, 0);
                }
            }
        }

        private void GatheringUpdate() {
            var len = gatheringDataList.Count;
            for (int i = len - 1; i >= 0; i--) {
                var data = gatheringDataList[i];
                if (Time.realtimeSinceStartup >= data.EndGatheringTime) {
                    var elementData = GetResourceData(data.CreateID);
                    if (elementData.GatheringCount <= 0) {
                        gatheringDataList.RemoveAt(i);
                        return;
                    }
                    elementData.GatheringCount--;
                    var modeData = elementData.ModeData;
                    GetElement(elementData.Element, data.GatheringGPO);
                    var isGathering = modeData.HarvestTime > 0 && elementData.GatheringCount > 0;
                    NotifyGatheringState(data.GatheringGPO, isGathering, elementData.GatheringCount);
                    gatheringDataList.RemoveAt(i);
                    if (elementData.GatheringCount <= 0) {
                        if (modeData.RespawnCooldown > 0f) {
                            elementData.NextResetTime = Time.realtimeSinceStartup + modeData.RespawnCooldown;
                            nextResetList.Add(elementData);
                        }
                        Rpc(new Proto_Scene.Rpc_RemoveSceneElement {
                            createID = elementData.CreateID,
                        });
                    }
                }
            }
        }

        private void GetElement(SceneData.ElementEnum elementType, IGPO gpo) {
            mySystem.Dispatcher(new SE_Scene.PlayAbilityForElementType {
                ElementType = elementType, PickGPO = gpo,
            });
        }

        // 根据重置时间重置资源使用次数
        private void ResetResourceGatheringCount() {
            var len = nextResetList.Count;
            for (int i = len - 1; i >= 0; i--) {
                var data = nextResetList[i];
                if (Time.realtimeSinceStartup >= data.NextResetTime) {
                    nextResetList.RemoveAt(i);
                    if (data.GatheringCount <= 0) {
                        data.GatheringCount = data.ModeData.GatheringLimit;
                        CreateElement(data);
                        Rpc(new Proto_Scene.Rpc_AddSceneElement {
                            element = (ushort)data.Element, createID = data.CreateID, point = data.Point,
                        });
                    }
                }
            }
        }

        // 检测一个 GPO 是否同时采集多个物品
        private bool CheckGPOIsGathering(IGPO gpo) {
            for (int i = 0; i < gatheringDataList.Count; i++) {
                var data = gatheringDataList[i];
                if (data.GatheringGPO == gpo) {
                    return true;
                }
            }
            return false;
        }

        // 发送采集状态消息给采集者
        private void NotifyGatheringState(IGPO gpo, bool state, int count) {
            var character = gpo as ServerGPO;
            if (character == null || character.GetGPOType() != GPOData.GPOType.Role) {
                return;
            }
            gpo.Dispatcher(new SE_Character.Event_GatheringState {
                Count = count, State = state,
            });
        }

        private SE_Scene.ElementCreateData GetResourceData(int createID) {
            for (int i = 0; i < resourceList.Count; i++) {
                var data = resourceList[i];
                if (data.CreateID == createID) {
                    return data;
                }
            }
            return null;
        }
    }
}