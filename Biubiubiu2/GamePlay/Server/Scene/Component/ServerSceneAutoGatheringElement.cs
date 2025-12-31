using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneAutoGatheringElement : ComponentBase {
        private const float GatheringRange = 2f;
        private List<SE_Scene.ElementCreateData> resourceList = new List<SE_Scene.ElementCreateData>();
        private List<IGPO> gpoList = new List<IGPO>();
        private float checkGatheringDelayTime = 0.2f;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Scene.SendElementCreateList>(OnSendElementCreateListCallBack);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = list => {
                    this.gpoList = list;
                }
            });
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Scene.SendElementCreateList>(OnSendElementCreateListCallBack);
            resourceList = null;
        }

        private void OnUpdate(float deltaTime) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            CheckGathering();
        }

        private void OnSendElementCreateListCallBack(ISystemMsg body, SE_Scene.SendElementCreateList ent) {
            this.resourceList = ent.ElementCreateDataList;
        }

        private void CheckGathering() {
            if (checkGatheringDelayTime > 0) {
                checkGatheringDelayTime -= Time.deltaTime;
                return;
            }
            checkGatheringDelayTime = 0.2f;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.IsDead()) {
                    continue;
                }
                var createId = CanGathering(gpo);
                if (createId > 0) {
                    MsgRegister.Dispatcher(new SM_SceneElement.Event_StartGathering {
                        IGpo = gpo, CreateId = createId,
                    });
                }
            }
        }

        // 根据 GPO 所在的位置和 resourceList 中的位置判断是否可以自动采集
        private int CanGathering(IGPO gpo) {
            for (int i = 0; i < resourceList.Count; i++) {
                var data = resourceList[i];
                var modeData = data.ModeData;
                if (modeData.HarvestTime <= 0f && data.GatheringCount > 0) {
                    var gpoPos = gpo.GetPoint();
                    var resourcePos = data.Point;
                    if (Vector3.Distance(gpoPos, resourcePos) <= GatheringRange) {
                        if (CheckCanPlayAbility(data.Element, gpo) == false) {
                            return 0;
                        }
                        return data.CreateID;
                    }
                }
            }
            return 0;
        }
        
        private bool CheckCanPlayAbility(SceneData.ElementEnum elementType, IGPO gpo) {
            var isTrue = true;
            mySystem.Dispatcher(new SE_Scene.CanPlayAbilityForElementType {
                ElementType = elementType,
                PickGPO = gpo,
                CallBack = (result) => {
                    isTrue = result;
                }
            });
            return isTrue;
        }
    }
}