using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIHateTarget : ComponentBase {
        public struct HateData {
            public IGPO HateGPO;
            public float HateValue;
        }

        private List<HateData> hateTargetList = new List<HateData>();
        private RaycastHit[] raycastHit;
        private float refreshMaxHateTime = 3;
        private IGPO maxHateGpo;
        private IGPO baseHateGPO = null;
        private List<IGPO> GPOList;
        private float deltaCheckDistanceTime = 0f;
        private float minDistanceGPO = -1f;
        private bool isMaxHateGPOObstacle = false;
        private float checkHasObstacleTime = 0f;
        private bool isEntityLoadEnd = false;

        protected override void OnAwake() {
            mySystem.Register<SE_Behaviour.Event_HateLockTarget>(OnHateTargetCallBack);
            mySystem.Register<SE_Behaviour.Event_HateFindTarget>(OnHateFindTargetCallBack);
            mySystem.Register<SE_Behaviour.Event_HateAttackTarget>(OnHateAttackTargetCallBack);
            mySystem.Register<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
            mySystem.Register<SE_Behaviour.Event_GetMaxHateTarget>(OnGetMaxHateTargetCallBack);
            mySystem.Register<SE_Behaviour.Event_ClearAllHateTarget>(OnClearAllHateTargetCallBack);
            mySystem.Register<SE_AI.Event_GetMinDistanceGPO>(OnGetMinDistanceGPOCallBack);
            mySystem.Register<SE_AI.Event_SetBaseHateTarget>(OnSetBaseHateTargetCallBack);
        }
        
        protected override void OnStart() {
            raycastHit = new RaycastHit[10];
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = OnGetGPOListCallBack
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            isEntityLoadEnd = true;
        }

        private void OnGetGPOListCallBack(List<IGPO> list) {
            this.GPOList = list;
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            hateTargetList.Clear();
            maxHateGpo = null;
            mySystem.Unregister<SE_Behaviour.Event_HateLockTarget>(OnHateTargetCallBack);
            mySystem.Unregister<SE_Behaviour.Event_HateFindTarget>(OnHateFindTargetCallBack);
            mySystem.Unregister<SE_Behaviour.Event_HateAttackTarget>(OnHateAttackTargetCallBack);
            mySystem.Unregister<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
            mySystem.Unregister<SE_Behaviour.Event_GetMaxHateTarget>(OnGetMaxHateTargetCallBack);
            mySystem.Unregister<SE_Behaviour.Event_ClearAllHateTarget>(OnClearAllHateTargetCallBack);
            mySystem.Unregister<SE_AI.Event_GetMinDistanceGPO>(OnGetMinDistanceGPOCallBack);
            mySystem.Unregister<SE_AI.Event_SetBaseHateTarget>(OnSetBaseHateTargetCallBack);
            GPOList = null;
        }

        void OnUpdate(float delta) {
            DownHateValue();
            RefreshMaxHate(delta);
            CheckObstacle();
        }

        public void OnComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            if (ent.IsTrue == false) {
                ClearAllHate();
            }
        }
        
        private void OnSetBaseHateTargetCallBack(ISystemMsg body, SE_AI.Event_SetBaseHateTarget ent) {
            baseHateGPO = ent.TargetGPO;
        }

        private void OnHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_HateLockTarget ent) {
            if (ent.TargetGPO.GetTeamID() == iGPO.GetTeamID()) {
                return;
            }
            var hate = Mathf.Max(1, 100 - ent.Distance);
            SetHateGPOAndValue(ent.TargetGPO, hate);
        }

        private void OnHateFindTargetCallBack(ISystemMsg body, SE_Behaviour.Event_HateFindTarget ent) {
            if (ent.TargetGPO.GetTeamID() == iGPO.GetTeamID()) {
                return;
            }
            var hate = Mathf.Max(1, 100 - ent.Distance);
            SetHateGPOAndValue(ent.TargetGPO, hate);
        }

        private void OnHateAttackTargetCallBack(ISystemMsg body, SE_Behaviour.Event_HateAttackTarget ent) {
            if (ent.AttackGPO.GetTeamID() == iGPO.GetTeamID()) {
                return;
            }
            SetHateGPOAndValue(ent.AttackGPO, 100f);
        }

        private void ClearAllHate() {
            hateTargetList.Clear();
            maxHateGpo = baseHateGPO;
            Dispatcher(new SE_Behaviour.Event_MaxHateTarget {
                TargetGPO = maxHateGpo
            });
        }

        private void OnGetMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_GetMaxHateTarget ent) {
            ent.CallBack(maxHateGpo);
        }

        private void SetHateGPOAndValue(IGPO target, float value) {
            HateData data;
            var index = GetHateForGPOID(target.GetGpoID(), out data);
            if (index < 0) {
                data.HateGPO = target;
                target.Register<SE_Behaviour.Event_ClearGPOSelfHate>(OnClearGPOSelfHate);
                hateTargetList.Add(data);
                index = hateTargetList.Count - 1;
            }
            data.HateValue = Mathf.Max(data.HateValue, value);
            hateTargetList[index] = data;
            RefreshMaxHate();
        }

        private void OnClearGPOSelfHate(ISystemMsg body, SE_Behaviour.Event_ClearGPOSelfHate ent) {
            HateData data;
            var index = GetHateForGPOID(ent.GpoID, out data);
            if (index >= 0) {
                data.HateValue = 0;
                hateTargetList[index] = data;
            }
        }

        private int GetHateForGPOID(int gpoID, out HateData data) {
            data = new HateData();
            for (int i = 0; i < hateTargetList.Count; i++) {
                var hateData = hateTargetList[i];
                if (hateData.HateGPO.GetGpoID() == gpoID) {
                    data = hateData;
                    return i;
                }
            }
            return -1;
        }

        private IGPO GetMaxHateTarget() {
            IGPO maxGPO = baseHateGPO;
            float value = 0;
            for (int i = 0; i < hateTargetList.Count; i++) {
                var hateData = hateTargetList[i];
                if (hateData.HateValue > value) {
                    value = hateData.HateValue;
                    maxGPO = hateData.HateGPO;
                }
            }
            return maxGPO;
        }

        private void DownHateValue() {
            for (int i = 0; i < hateTargetList.Count; i++) {
                var hateData = hateTargetList[i];
                hateData.HateValue -= Time.deltaTime;
                if (hateData.HateGPO.IsClear() || hateData.HateGPO.IsDead() || hateData.HateValue <= 0) {
                    hateTargetList.RemoveAt(i);
                } else {
                    hateTargetList[i] = hateData;
                }
            }
        }

        private void RefreshMaxHate(float delta) {
            if (refreshMaxHateTime > 0) {
                refreshMaxHateTime -= delta;
                return;
            }
            refreshMaxHateTime = 3f;
            RefreshMaxHate();
        }

        private void RefreshMaxHate() {
            maxHateGpo = GetMaxHateTarget();
            Dispatcher(new SE_Behaviour.Event_MaxHateTarget {
                TargetGPO = maxHateGpo
            });
        }

        private void OnGetMinDistanceGPOCallBack(ISystemMsg body, SE_AI.Event_GetMinDistanceGPO ent) {
            if (baseHateGPO != null) {
                ent.CallBack(0f);
                return;
            }
            if (Time.realtimeSinceStartup - deltaCheckDistanceTime > 1f) {
                deltaCheckDistanceTime = Time.realtimeSinceStartup;
                minDistanceGPO = GetMinDistanceGPO();
            }
            ent.CallBack(minDistanceGPO);
        }

        private void OnClearAllHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_ClearAllHateTarget ent) {
            ClearAllHate();
        }

        // 获取距离当前位置最近的 GPO 距离
        private float GetMinDistanceGPO() {
            var point = iEntity.GetPoint();
            float minDistance = float.MaxValue;
            for (int i = 0; i < GPOList.Count; i++) {
                var gpo = GPOList[i];
                if (gpo.IsClear() || gpo.IsGodMode() || gpo.IsDead() || gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }
                var distance = Vector3.Distance(point, gpo.GetPoint());
                if (distance < minDistance) {
                    minDistance = distance;
                }
            }
            return minDistance;
        }

        // 检查和 GPO 之间是否有障碍物
        private void CheckObstacle() {
            if (maxHateGpo == null || maxHateGpo.IsClear() || isEntityLoadEnd == false) {
                return;
            }
            if (checkHasObstacleTime > 0) {
                checkHasObstacleTime -= Time.deltaTime;
                return;
            }
            checkHasObstacleTime = 0.5f;
            var startPoint = CheckGPOPoint(iGPO);
            var endPoint = CheckGPOPoint(maxHateGpo);
            var forwart = (endPoint - startPoint).normalized;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forwart, raycastHit, distance,
                ~(LayerData.ClientLayerMask));
            if (count > 0) {
                var isHit = CheckObstacleRaycastHit(count);
                SetIsTargetObstacle(isHit);
            } else {
                SetIsTargetObstacle(false);
            }
            Debug.DrawLine(startPoint, endPoint, this.isMaxHateGPOObstacle ? Color.red : Color.green);
        }

        public void SetIsTargetObstacle(bool isObstacle) {
            if (isObstacle != this.isMaxHateGPOObstacle) {
                this.isMaxHateGPOObstacle = isObstacle;
                Dispatcher(new SE_Behaviour.Event_isMaxHateGPOObstacle {
                    IsObstacle = isObstacle
                });
            }
        }

        public bool CheckObstacleRaycastHit(int count) {
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    continue;
                }
                return true;
            }
            return false;
        }

        private Vector3 CheckGPOPoint(IGPO igpo) {
            var tran = igpo.GetBodyTran(GPOData.PartEnum.Head);
            if (tran == null) {
                tran = igpo.GetBodyTran(GPOData.PartEnum.Body);
            }
            if (tran == null) {
                return iEntity.GetPoint();
            }
            return tran.position;
        }
    }
}