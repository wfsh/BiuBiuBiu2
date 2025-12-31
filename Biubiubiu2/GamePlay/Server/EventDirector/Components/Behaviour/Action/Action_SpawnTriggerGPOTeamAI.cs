using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Action_SpawnTriggerGPOTeamAI : EventDirectorAction {
        public struct InitData : IEventDirectorData {
            public int SpawnGpoId;
            public Vector3 Point;
            public void Serialize (string value) {
                var arr = value.Split('&');
                SpawnGpoId = int.Parse(arr[0]);
                var pointStr = arr[1];
                var pointArr = pointStr.Split(';');
                Point = new Vector3(float.Parse(pointArr[0]), float.Parse(pointArr[1]), float.Parse(pointArr[2]));
            }
        }
        private InitData useMData;
        private List<IGPO> monsterGPOs = new List<IGPO>();
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

        protected override void OnClear() {
            base.OnClear();
            ClearAllMonster();
            monsterGPOs = null;
        }

        protected override void OnEnterAction() {
            var gpoData = GpoSet.GetGpoById(useMData.SpawnGpoId);
            if (TriggerGPO == null) {
                QuitAction();
                return;
            }
            if (subjectType == EventDirectorData.SubjectType.World) {
                var spawnPoint = useMData.Point;
                if (spawnPoint == Vector3.zero) {
                    spawnPoint = TriggerGPO.GetPoint();
                }
                MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                    AISign = gpoData.Sign,
                    StartPoint = spawnPoint,
                    OR_TeamId = TriggerGPO.GetTeamID(),
                    OR_GpoType = GPOData.GPOType.AI,
                    OR_CallBack = SetMonster
                });
            } else {
                for (int i = 0; i < gpoList.Count; i++) {
                    var gpo = gpoList[i];
                    MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                        AISign = gpoData.Sign,
                        StartPoint = gpo.GetPoint() + useMData.Point,
                        OR_GpoType = GPOData.GPOType.AI,
                        OR_TeamId = gpo.GetTeamID(),
                        OR_CallBack = SetMonster
                    });
                }
            }
        }

        private void SetMonster(IAI iAi) {
            iAi.Register<SE_GPO.Event_StartRemoveGPO>(OnStartRemoveGPOCallBack);
            monsterGPOs.Add(iAi.GetGPO());
        }

        private void OnStartRemoveGPOCallBack(ISystemMsg body, SE_GPO.Event_StartRemoveGPO ent) {
            for (int i = 0; i < monsterGPOs.Count; i++) {
                if (monsterGPOs[i].GetGpoID() == body.GetGPOId()) {
                    monsterGPOs.RemoveAt(i);
                    break;
                }
            }
            Debug.LogError("剩余 GPO 数量:" + monsterGPOs.Count);
            if (monsterGPOs.Count <= 0) {
                QuitAction();
            }
        }

        private void ClearAllMonster() {
            for (int i = 0; i < monsterGPOs.Count; i++) {
                var gpo = monsterGPOs[i];
                gpo.Dispatcher(new SE_AI.Event_OnRemoveAI());
            }
            monsterGPOs.Clear();
        }
    }
}
