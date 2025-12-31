using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAIHateTarget : ComponentBase {
        public struct HateData {
            public IGPO HateGPO;
            public float HateValue;
        }

        private const float minHateUpperLimit = 1;
        private const float hateRefreshInterval = 3;
        private const float hateLowerLimit = 0;
        private S_AI_Base aiSystem;
        private List<HateData> hateTargetList = new List<HateData>();
        private RaycastHit[] raycastHit;
        private float refreshMaxHateTime = hateRefreshInterval;
        private IGPO maxHateGpo;
        private IGPO baseHateGPO = null;

        private List<IGPO> gpoList;
        private float deltaCheckDistanceTime = 0f;
        private float minDistanceGPO = -1f;
        private bool isMaxHateGPOObstacle = false;
        private float checkHasObstacleTime = 0f;
        private bool isEntityLoadEnd = false;
        private bool hasBehaviorInit;
        private MonsterBehavior goldDashBehavior;

        protected override void OnAwake() {
            aiSystem = (S_AI_Base)mySystem;
            hasBehaviorInit = false;
            mySystem.Register<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            // hit
            mySystem.Register<SE_GPO.Event_AfterDownHP>(AfterDownHPCallBack);
            mySystem.Register<SE_Behaviour.Event_FillHateToValue>(OnFillHateToValueCallBack);
            mySystem.Register<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
            mySystem.Register<SE_Behaviour.Event_GetMaxHateTargetData>(OnGetMaxHateTargetDataCallBack);
            mySystem.Register<SE_Behaviour.Event_GetMaxHateTarget>(OnGetMaxHateTargetCallBack);
            mySystem.Register<SE_AI.Event_GetMinDistanceGPO>(OnGetMinDistanceGPOCallBack);
            mySystem.Register<SE_AI.Event_SetBaseHateTarget>(OnSetBaseHateTargetCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            goldDashBehavior = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
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
            this.gpoList = list;
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            hateTargetList.Clear();
            maxHateGpo = null;
            mySystem.Unregister<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            mySystem.Unregister<SE_GPO.Event_AfterDownHP>(AfterDownHPCallBack);
            mySystem.Unregister<SE_Behaviour.Event_FillHateToValue>(OnFillHateToValueCallBack);
            mySystem.Unregister<SE_Behaviour.Event_ComeBack>(OnComeBackCallBack);
            mySystem.Unregister<SE_Behaviour.Event_GetMaxHateTargetData>(OnGetMaxHateTargetDataCallBack);
            mySystem.Unregister<SE_Behaviour.Event_GetMaxHateTarget>(OnGetMaxHateTargetCallBack);
            mySystem.Unregister<SE_AI.Event_GetMinDistanceGPO>(OnGetMinDistanceGPOCallBack);
            mySystem.Unregister<SE_AI.Event_SetBaseHateTarget>(OnSetBaseHateTargetCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            gpoList = null;
        }

        void OnUpdate(float delta) {
            if (!hasBehaviorInit) {
                return;
            }

            ReduceHateValue();
            PeriodicRefreshMaxHate(delta);
            CheckObstacle();
        }

        private void AfterBehaviorConfigInitCallBack(ISystemMsg body, SE_Behaviour.Event_AfterBehaviorConfigInit ent) {
            hasBehaviorInit = true;
        }

        private void AfterDownHPCallBack(ISystemMsg body, SE_GPO.Event_AfterDownHP ent) {
            TryIncreaseHate(ent.AttackGPO, goldDashBehavior.OnHitHate);
        }

        public void OnComeBackCallBack(ISystemMsg body, SE_Behaviour.Event_ComeBack ent) {
            if (ent.IsTrue == false) {
                ClearAllHate();
            }
        }
        
        private void OnSetBaseHateTargetCallBack(ISystemMsg body, SE_AI.Event_SetBaseHateTarget ent) {
            baseHateGPO = ent.TargetGPO;
            if (maxHateGpo == null) {
                maxHateGpo = baseHateGPO;
            }
        }

        private void OnFillHateToValueCallBack(ISystemMsg body, SE_Behaviour.Event_FillHateToValue ent) {
            TryFillHateToValue(ent.CasterGPO, ent.Value);
        }

        private void TryIncreaseHate(IGPO casterGPO, float value) {
            if (casterGPO.GetTeamID() == iGPO.GetTeamID() || value < 0) {
                return;
            }
            IncreaseTargetHateBaseValue(casterGPO, goldDashBehavior.OnHitHate);
        }

        private void TryFillHateToValue(IGPO casterGPO, float value) {
            if (casterGPO.GetTeamID() == iGPO.GetTeamID() || value < 0) {
                return;
            }
            IncreaseTargetHateBaseValue(casterGPO, value);
        }

        private void ClearAllHate() {
            for (int i = 0, count = hateTargetList.Count; i < count; i++) {
                IGPO hateGPO = hateTargetList[i].HateGPO;
                hateGPO.Unregister<SE_Behaviour.Event_ClearGPOSelfHate>(OnClearGPOSelfHate);
            }
            hateTargetList.Clear();
            maxHateGpo = baseHateGPO;
            Dispatcher(new SE_Behaviour.Event_MaxHateTarget {
                TargetGPO = maxHateGpo
            });
        }

        private void OnGetMaxHateTargetDataCallBack(ISystemMsg body, SE_Behaviour.Event_GetMaxHateTargetData ent) {
            float sumValue = 0;
            if (maxHateGpo != null) {
                GetHateForGPOID(maxHateGpo.GetGpoID(), out HateData data);
                sumValue = data.HateValue;
            }
            if (baseHateGPO != null && sumValue <= 0f) {
                sumValue = 999f;
            }
            ent.CallBack(maxHateGpo, sumValue);
        }

        private void OnGetMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_GetMaxHateTarget ent) {
            ent.CallBack(maxHateGpo);
        }

        private void IncreaseTargetHateBaseValue(IGPO gpo, float value) {
            HateData data;
            var index = GetHateForGPOID(gpo.GetGpoID(), out data);
            if (index < 0) {
                AddHateGPO(gpo, ref data, out index);
            }
            data.HateValue += value;
            HateCheckAndWrite(index, ref data);
        }

        private void SetTargetHateBaseValue(IGPO gpo, float value) {
            HateData data;
            var index = GetHateForGPOID(gpo.GetGpoID(), out data);
            if (index < 0) {
                AddHateGPO(gpo, ref data, out index);
            }
            data.HateValue = Mathf.Max(data.HateValue, value);
            HateCheckAndWrite(index, ref data);
        }

        private void HateCheckAndWrite(int index, ref HateData data) {
            float hateUpperLimit = minHateUpperLimit;
            if (goldDashBehavior.HateUpperLimit != 0) {
                hateUpperLimit = goldDashBehavior.HateUpperLimit;
            }
            float sum = data.HateValue;
            if (sum > hateUpperLimit) {
                data.HateValue = hateUpperLimit;
            } else if (sum < hateLowerLimit) {
                data.HateValue = hateLowerLimit;
            }
            hateTargetList[index] = data;
        }

        private void AddHateGPO(IGPO gpo, ref HateData data, out int index) {
            data.HateGPO = gpo;
            gpo.Register<SE_Behaviour.Event_ClearGPOSelfHate>(OnClearGPOSelfHate);
            hateTargetList.Add(data);
            index = hateTargetList.Count - 1;
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

        private void ReduceHateValue() {
            float hateReduceSpeed = 1;
            hateReduceSpeed = goldDashBehavior.HateReduceSpeed;
            for (int i = hateTargetList.Count - 1; i >= 0; i--) {
                var hateData = hateTargetList[i];
                float reduceValue = -hateReduceSpeed * Time.deltaTime;
                hateData.HateValue += reduceValue;
                if (hateData.HateGPO.IsDead() || hateData.HateGPO.IsClear() || hateData.HateGPO.IsGodMode() || hateData.HateValue <= hateLowerLimit) {
                    IGPO hateGPO = hateTargetList[i].HateGPO;
                    hateGPO.Unregister<SE_Behaviour.Event_ClearGPOSelfHate>(OnClearGPOSelfHate);
                    hateTargetList.RemoveAt(i);
                } else {
                    hateTargetList[i] = hateData;
                }
            }
        }

        private void PeriodicRefreshMaxHate(float delta) {
            if (refreshMaxHateTime > 0) {
                refreshMaxHateTime -= delta;
                return;
            }
            refreshMaxHateTime = hateRefreshInterval;
            RefreshMaxHate();
        }

        private void RefreshMaxHate() {
            HateTargetListSort();
            maxHateGpo = GetMaxHateTarget();
            Dispatcher(new SE_Behaviour.Event_MaxHateTarget {
                TargetGPO = maxHateGpo
            });
        }

        private IGPO GetMaxHateTarget() {
            return hateTargetList.Count > 0 ? hateTargetList[0].HateGPO : baseHateGPO;
        }

        public void HateTargetListSort() {
            
            for (int i = 1; i < hateTargetList.Count; i++) {
                HateData current = hateTargetList[i];
                int j = i - 1;
                while (j >= 0 && hateTargetList[j].HateValue < current.HateValue) {
                    hateTargetList[j + 1] = hateTargetList[j];
                    j--;
                }

                hateTargetList[j + 1] = current;
            }
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

        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            ClearAllHate();
        }

        // 获取距离当前位置最近的 GPO 距离
        private float GetMinDistanceGPO() {
            var point = iEntity.GetPoint();
            float minDistance = float.MaxValue;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
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
            var forward = (endPoint - startPoint).normalized;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance,
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