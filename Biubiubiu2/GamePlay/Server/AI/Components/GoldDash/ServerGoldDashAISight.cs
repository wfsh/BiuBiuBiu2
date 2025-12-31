using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    //用于控制怪物在服务端的视野逻辑（视野检测 + 障碍检测 + 仇恨系统）。
    //它大致实现了“怪物能看到哪些敌人”的判定，并用于后续 AI 行为控制，比如攻击、追击等。
    public class ServerGoldDashAISight : ComponentBase {
        private const float gpoInSightUpdateInterval = 0.5f;
        private const float forwardObstacleCheckDis = 1;
        private float lastUpdateTime;
        private List<IGPO> gpoList;
        private S_AI_Base aiBase;
        private List<IGPO> gpoInSightList;
        private RaycastHit[] raycastHit;
        private bool hasBehaviorInit;
        private bool isEnabled = false;
        private bool isSausageSwitchAllBehavior = false;
        private long tempPlayerId;
        private bool tempIsFriendBubbleTarget;
        private Dictionary<long, float> playerAddSights = new();
        private Dictionary<long, PlayerBubbleState> playerBubbleStates = new();
        private int sceneId;
        private MonsterBehavior goldDashBehavior;
        
        private struct PlayerBubbleState {
            public bool isInFriendRange;
            public float lastPlayBubbleTime;
        }

        protected override void OnAwake() {
            base.OnAwake();
            hasBehaviorInit = false;
            Register<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            Register<SE_Behaviour.Event_GetGPOListInSight>(OnGetGPOListInSightCallBack);
            MsgRegister.Register<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            MsgRegister.Register<SM_Sausage.SausageSwitchAISight>(OnSwitchAISight);
            Register<SE_AI.Event_IsSectorAreaHasGPO>(OnGetSectorAreaMinDistanceGPOCallBack);
            goldDashBehavior = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(iGPO.GetSign());
            mySystem.Register<SE_Behaviour.Event_EnabledBehavior>(OnEnabledBehaviorCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
            aiBase = (S_AI_Base)mySystem;
            gpoList = aiBase.GPOList;
            gpoInSightList = new List<IGPO>();
            raycastHit = new RaycastHit[10];
            isSausageSwitchAllBehavior = true;
            isEnabled = true;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            Unregister<SE_Behaviour.Event_AfterBehaviorConfigInit>(AfterBehaviorConfigInitCallBack);
            Unregister<SE_Behaviour.Event_GetGPOListInSight>(OnGetGPOListInSightCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAllBehavior>(OnSwitchAllBehaviorCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSwitchAISight>(OnSwitchAISight);
            Unregister<SE_AI.Event_IsSectorAreaHasGPO>(OnGetSectorAreaMinDistanceGPOCallBack);
            mySystem.Unregister<SE_Behaviour.Event_EnabledBehavior>(OnEnabledBehaviorCallBack);
            gpoInSightList.Clear();
            gpoInSightList = null;
            raycastHit = null;
            RemoveUpdate(OnUpdate);
        }
        
        private void OnSwitchAISight(SM_Sausage.SausageSwitchAISight ent) {
            if (!playerAddSights.ContainsKey(ent.PlayerId)) {
                playerAddSights.Add(ent.PlayerId, 0);
            }
            playerAddSights[ent.PlayerId] = ent.ReduceValue;
        }

        private void AfterBehaviorConfigInitCallBack(ISystemMsg body, SE_Behaviour.Event_AfterBehaviorConfigInit ent) {
            hasBehaviorInit = true;
        }

        private void OnGetGPOListInSightCallBack(ISystemMsg body, SE_Behaviour.Event_GetGPOListInSight ent) {
            ent.CallBack?.Invoke(gpoInSightList);
        }
        /// <summary>
        /// 接收 AI 是否启用的消息
        /// </summary>
        private void OnSwitchAllBehaviorCallBack(SM_Sausage.SausageSwitchAllBehavior ent) {
            isSausageSwitchAllBehavior = ent.isEnabled;
        }
        
        private void OnEnabledBehaviorCallBack(ISystemMsg body, SE_Behaviour.Event_EnabledBehavior ent) {
            isEnabled = ent.IsEnabled;
        }

        private void OnUpdate(float deltaTime) {
            if (!isEnabled || !isSausageSwitchAllBehavior || !hasBehaviorInit) {
                return;
            }
            if (Time.realtimeSinceStartup - lastUpdateTime <= gpoInSightUpdateInterval) {
                return;
            }
            lastUpdateTime = Time.realtimeSinceStartup;
            // 如果怪物已死亡，不再处理视野检测
            bool isDead = false;
            iGPO.Dispatcher(new SE_GPO.Event_GetIsDead() {
                CallBack = result => {
                    isDead = result;
                }
            });
            if (isDead) {
                return;
            }
            CheckObstacleForward();  // 检查前方是否有障碍物（用于跳跃）
            UpdateGPOInSight(); // 更新视野内的玩家列表

#if UNITY_EDITOR
            if (iEntity == null) {
                return;
            }
            // Scene 视图绘制 视野范围
            var point = iEntity.GetPoint();
            var forward = iGPO.GetForward();

            // 绘制视野半径圆圈（用多边形近似）
            int circleSegments = 18;
            for (int i = 0; i < circleSegments; i++) {
                float angle1 = (float)i / circleSegments * 2 * Mathf.PI;
                float angle2 = (float)(i + 1) / circleSegments * 2 * Mathf.PI;
                Vector3 point1 = point + new Vector3(Mathf.Cos(angle1), 0, Mathf.Sin(angle1)) * goldDashBehavior.SightRadius;
                Vector3 point2 = point + new Vector3(Mathf.Cos(angle2), 0, Mathf.Sin(angle2)) * goldDashBehavior.SightRadius;
                Debug.DrawLine(point1, point2, Color.yellow, gpoInSightUpdateInterval);
            }

            // 绘制视野角度扇形区域
            var leftDir = Quaternion.Euler(0, -goldDashBehavior.SightAngle, 0) * forward;
            var rightDir = Quaternion.Euler(0, goldDashBehavior.SightAngle, 0) * forward;
            Debug.DrawLine(point, point + leftDir * goldDashBehavior.SightRadius, Color.cyan, gpoInSightUpdateInterval);
            Debug.DrawLine(point, point + rightDir * goldDashBehavior.SightRadius, Color.cyan, gpoInSightUpdateInterval);

            // 绘制视野内检测到的目标连线
            foreach (var gpo in gpoInSightList) {
                if (!gpo.IsClear()) {
                    Debug.DrawLine(point, gpo.GetPoint(), Color.red, gpoInSightUpdateInterval);
                }
            }
#endif
        }

        /// <summary>
        /// 更新 GPO 视野检测逻辑（半径 + 角度 + 射线遮挡）
        /// </summary>
        private void UpdateGPOInSight() {
            if (iEntity == null) {
                return;
            }
            var point = iEntity.GetPoint();
            for (int i = 0, count = gpoList.Count; i < count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }

                bool isBeforeInSight = IsTargetInSight(iEntity, iGPO, gpo, goldDashBehavior.SightRadius, goldDashBehavior.SightAngle);
                var reduceSightValue = GetAddMonsterSight(gpo, out var playerId);
                bool isAfterInSight = IsTargetInSight(iEntity, iGPO, gpo, goldDashBehavior.SightRadius * (1 - reduceSightValue), goldDashBehavior.SightAngle);
                
                if (playerId != 0) {
                    bool isNowFriendRange = isBeforeInSight && !isAfterInSight;
                    if (playerBubbleStates.TryGetValue(playerId, out var data) && 
                        data.isInFriendRange != isNowFriendRange) {
                        OnFriendRangeChanged(playerId, gpo, isNowFriendRange);
                    }else if (!playerBubbleStates.ContainsKey(playerId) && isNowFriendRange) {
                        playerBubbleStates.Add(playerId, new PlayerBubbleState() {
                            isInFriendRange = true,
                        });
                        OnFriendRangeChanged(playerId, gpo, true);
                    }
                }

                if (isAfterInSight) {
                    gpoInSightList.Add(gpo);
                    FillGPOHateInSight(gpo);
                }
            }
        }

        private void OnFriendRangeChanged(long playerId, IGPO player, bool isEnter) {
            var copy = playerBubbleStates[playerId];
            if (isEnter) {
                if (Time.time - copy.lastPlayBubbleTime < 5) {
                    return;
                }
                
                var headTrans = iEntity.GetBodyTran(GPOData.PartEnum.Head);
                if (headTrans == null) {
                    return;
                }

                if (CheckPlayBubble(playerId)) {
                    player.Dispatcher(new SE_Character.PlayBubble() {
                        PlayerId = playerId,
                        EffectSign = "Expression_500",
                        EffectPos = headTrans.position + Vector3.up * 1,
                        LifeTime = 2.5f,
                    });
                }
                
                copy.lastPlayBubbleTime = Time.time;
            }
            copy.isInFriendRange = isEnter;
            playerBubbleStates[playerId] = copy;
        }

        private bool CheckPlayBubble(long playerId) {
            OnGetCurSceneData(SceneData.Get(ModeData.SceneId));

            bool isInBubbleMap = sceneId == SceneData.GoldDash_SOGoldDash_03_JokerBase_03 ||
                                 sceneId == SceneData.GoldDash_SOGoldDash_03_JokerBase_03A ||
                                 sceneId == SceneData.GoldDash_SOGoldDash_03_JokerBase_03B;
            if (!isInBubbleMap) {
                return false;
            }

            tempIsFriendBubbleTarget = false;
            MsgRegister.Dispatcher(new SM_Sausage.GetSausageRoleIsFriendBubbleTarget() {
                PlayerId = playerId,
                Callback = OnGetPlayerIsBubbleTarget
            });
            
            return tempIsFriendBubbleTarget;
        }

        private void OnGetPlayerIsBubbleTarget(bool isTarget) {
            tempIsFriendBubbleTarget = isTarget;
        }

        private void OnGetCurSceneData(SceneData.Data data) {
            sceneId = data.ID;
        }

        private float GetAddMonsterSight(IGPO checkTarget, out long findPlayerId) {
            findPlayerId = 0;
            if (checkTarget.GetGPOType() != GPOData.GPOType.Role) {
                return 0;
            }

            tempPlayerId = 0;
            checkTarget.Dispatcher(new SE_Character.GetPlayerId() {
                CallBack = OnSetPlayerId
            });
            if (tempPlayerId == 0) {
                return 0;
            }

            findPlayerId = tempPlayerId;
            return playerAddSights.GetValueOrDefault(tempPlayerId, 0);
        }
        
        private void OnSetPlayerId(long playerId) {
            tempPlayerId = playerId;
        }

        private bool IsTargetInSight(IEntity myEntity, IGPO myGpo, IGPO targetGpo, float checkDis, float checkAngle) {
            var nowDisSqr = Vector3.SqrMagnitude(myEntity.GetPoint() - targetGpo.GetPoint());
            if (nowDisSqr < checkDis * checkDis) {
                Vector3 dir = targetGpo.GetPoint() - iGPO.GetPoint();
                float angle = Vector3.Angle(myGpo.GetForward(), dir);
                if (angle <= checkAngle) {
                    bool isObstacle = CheckObstacleGPO(targetGpo);
                    if (!isObstacle) {
                        return true;
                    }
                }
            }

            return false;
        }
        
        /// <summary>
        /// 向行为系统上报仇恨填充值
        /// </summary>
        private void FillGPOHateInSight(IGPO gpo) {
            mySystem.Dispatcher(new SE_Behaviour.Event_FillHateToValue() {
                CasterGPO = gpo, Value = goldDashBehavior.GpoInSightHateFillLevel,
            });
        }

        /// <summary>
        /// 检查怪物正前方是否有障碍物（用于跳跃逻辑）
        /// </summary>
        private bool CheckObstacleForward() {
            var forward = iGPO.GetForward();
            var startPoint = iGPO.GetPoint() + 0.5f * Vector3.up;
            var endPoint = startPoint + forwardObstacleCheckDis * forward;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance,
                ~(LayerData.ClientLayerMask | LayerData.RoleLayerMask));
            var isHit = false;
            if (count > 0) {
                isHit = CheckObstacleRaycastHit(count, false);
            }
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, endPoint, isHit ? Color.red : Color.green);
#endif
            if (isHit) {
                Dispatcher(new SE_AI.Event_JumpOverObstacle());
            }
            return isHit;
        }

        /// <summary>
        /// 检查与 GPO 间是否有障碍（用于视野判断）
        /// </summary>
        private bool CheckObstacleGPO(IGPO targetGpo) {
            var myTran = CheckGPOTran(iGPO);
            var targetTran = CheckGPOTran(targetGpo);
            if (myTran == null || targetTran == null) {
                return false;
            }
            var startPoint = myTran.position;
            var endPoint = targetTran.position;
            var forward = (endPoint - startPoint).normalized;
            var distance = Vector3.Distance(startPoint, endPoint);
            var count = Physics.RaycastNonAlloc(startPoint, forward, raycastHit, distance,
                ~(LayerData.ClientLayerMask | LayerData.RoleLayerMask | LayerData.AirWallLayerMask));
            var isHit = false;
            if (count > 0) {
                isHit = CheckObstacleRaycastHit(count, true);
            }
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, endPoint, isHit ? Color.red : Color.green);
#endif
            return isHit;
        }
        /// <summary>
        /// 通用射线检测结果判定（是否命中障碍）
        /// </summary>
        public bool CheckObstacleRaycastHit(int count, bool checkGPO) {
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                if (checkGPO) {
                    var gameObj = ray.collider.gameObject;
                    // 注意，是二代的 HitType
                    var hitType = gameObj.GetComponent<HitType>();
                    if (hitType != null) {
                        continue;
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取 GPO 的头或身体 Transform（用于确定视野起点）
        /// </summary>
        private Transform CheckGPOTran(IGPO iGpo) {
            var tran = iGpo.GetBodyTran(GPOData.PartEnum.Head);
            if (tran == null) {
                tran = iGpo.GetBodyTran(GPOData.PartEnum.Body);
            }
            return tran;
        }

        private void OnGetSectorAreaMinDistanceGPOCallBack(ISystemMsg arg1, SE_AI.Event_IsSectorAreaHasGPO ent) {
            var isHas = IsSectorAreaHasGPO(ent.SweepAngle);
            ent.CallBack(isHas);
        }

        private bool IsSectorAreaHasGPO(float sweepAngle) {
            var maxAngle = goldDashBehavior.SightAngle + sweepAngle;
            for (int i = 0, count = gpoList.Count; i < count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }

                var reduceSightValue = GetAddMonsterSight(gpo, out var playerId);
                bool isAfterInSight = IsTargetInSight(iEntity, iGPO, gpo, goldDashBehavior.SightRadius * (1 - reduceSightValue), goldDashBehavior.SightAngle / 2);
                if (isAfterInSight) {
                    return true;
                }
            }

            return false;
        }
    }
}