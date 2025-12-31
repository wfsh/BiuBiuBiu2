using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAITouch : ComponentBase {
        private const float UpdateInterval = 0.5f;
        private List<IGPO> gpoList;
        private S_AI_Base aiBase;
        private List<IGPO> gpoInTouchList;
        private bool hasBehaviorInit;
        private bool isEnabled;
        private bool isSausageSwitchAllBehavior = false;
        private MonsterBehavior goldDashBehavior;
        private float checkTimer;
        private RaycastHit[] raycastHit;

        protected override void OnAwake() {
            base.OnAwake();
            aiBase = (S_AI_Base)mySystem;
            gpoInTouchList = new List<IGPO>();
            raycastHit = new RaycastHit[10];
            mySystem.Register<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            mySystem.Register<SE_Behaviour.Event_EnabledBehavior>(OnEnabledBehaviorCallBack);
            goldDashBehavior = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
        }

        protected override void OnStart() {
            base.OnStart();
            gpoList = aiBase.GPOList;
            isSausageSwitchAllBehavior = true;
            isEnabled = true;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            mySystem.Unregister<SE_Behaviour.Event_EnabledBehavior>(OnEnabledBehaviorCallBack);
            gpoInTouchList.Clear();
            gpoInTouchList = null;
            raycastHit = null;
        }
        
        private void OnEnabledBehaviorCallBack(ISystemMsg body, SE_Behaviour.Event_EnabledBehavior ent) {
            isEnabled = ent.IsEnabled;
        }
        
        private void OnUpdate(float deltaTime) {
            if (!isEnabled || !isSausageSwitchAllBehavior || !hasBehaviorInit) {
                return;
            }
            
            if (checkTimer <= 0) {
                checkTimer = UpdateInterval;
                UpdateRangeTouchGpoList();
                IncreaseHateValue();
                
#if UNITY_EDITOR
                DrawDebugView();
#endif
            }
            
            checkTimer -= deltaTime;
        }
        
        private void UpdateRangeTouchGpoList() {
            if (iEntity == null) {
                return;
            }
            
            gpoInTouchList.Clear();
            for (int i = 0, count = gpoList.Count; i < count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }

                var pos = iGPO.GetPoint();
                var targetPos = gpo.GetPoint();
                var yOffset = targetPos.y - pos.y;
                if (Mathf.Abs(yOffset) > goldDashBehavior.TouchHeight) {
                    continue;
                }
                
                var dir = targetPos - pos;
                var distanceSqr = dir.sqrMagnitude;
                
                if (distanceSqr > goldDashBehavior.TouchRadius * goldDashBehavior.TouchRadius) {
                    continue;
                }

                var horizontalDir = new Vector3(dir.x, 0, dir.z);
                var angle = Vector3.Angle(iGPO.GetForward(), horizontalDir);
                if (angle > goldDashBehavior.TouchAngle) {
                    continue;
                }

                bool isObstacle = CheckObstacle(gpo);
                if (!isObstacle) {
                    gpoInTouchList.Add(gpo);
                }
            }
        }

        private void IncreaseHateValue() {
            int count = gpoInTouchList.Count;
            for (int i = 0; i < count; i++) {
                var gpo = gpoInTouchList[i];
                mySystem.Dispatcher(new SE_Behaviour.Event_FillHateToValue() {
                    CasterGPO = gpo, Value = goldDashBehavior.TouchHateSpeed,
                });
            }
        }

        private bool CheckObstacle(IGPO gpo) {
            var headTrans = iGPO.GetTargetTransform();
            var targetHeadTrans = gpo.GetTargetTransform();
            
            var headDir = targetHeadTrans.position - headTrans.position;
            var distance = headDir.magnitude;
            var count = Physics.RaycastNonAlloc(headTrans.position, headDir.normalized, raycastHit, distance,
                ~(LayerData.ClientLayerMask | LayerData.RoleLayerMask | LayerData.AirWallLayerMask));
            
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

#if UNITY_EDITOR
        private void DrawDebugView() {
            Vector3 origin = iGPO.GetPoint();
            Vector3 dir = iGPO.GetForward();

            // 绘制扇形边缘
            float half = goldDashBehavior.TouchAngle;
            Quaternion leftRot = Quaternion.AngleAxis(-half, Vector3.up);
            Vector3 leftDir = leftRot * dir;
            Vector3 lastPoint = origin + leftDir * goldDashBehavior.TouchRadius;

            for (int i = 1; i <= 10; i++)
            {
                float t = (float)i / 10;
                float currentAngle = Mathf.Lerp(-half, half, t);
                Quaternion rot = Quaternion.AngleAxis(currentAngle, Vector3.up);
                Vector3 nextDir = rot * dir;
                Vector3 nextPoint = origin + nextDir * goldDashBehavior.TouchRadius;
                
                Debug.DrawLine(lastPoint, nextPoint, Color.magenta, UpdateInterval);
                lastPoint = nextPoint;
            }
            
            Debug.DrawLine(origin, origin + Quaternion.AngleAxis(-half, Vector3.up) * dir * goldDashBehavior.TouchRadius, Color.magenta, UpdateInterval);
            Debug.DrawLine(origin, origin + Quaternion.AngleAxis(half, Vector3.up) * dir * goldDashBehavior.TouchRadius, Color.magenta, UpdateInterval);
        }
#endif
        
        private void AfterBehaviorConfigInitCallBack(ISystemMsg body, SE_Behaviour.Event_AfterBehaviorConfigInit ent) {
            hasBehaviorInit = true;
        }
        
        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            isSausageSwitchAllBehavior = ent.isEnabled;
        }
    }
}