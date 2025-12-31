using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using UnityEngine.AI;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerDropCondition_PosUnreachable : ServerDropCondition_Base {
        private int dropItemId = 0;
        private RaycastHit[] cachedHits;
        
        protected override void OnAwake() {
            cachedHits = new RaycastHit[10];
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
        }

        protected override void OnSetDropData() {
            base.OnSetDropData();
            if (dropIdList.Count == 0) {
                Debug.LogError($"[Error] ServerDropCondition_DeadDrop 没有配置掉落物品 ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                return;
            }
            dropItemId = dropIdList[0];
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            cachedHits = null;
        }
        
        private void OnDeadCallBack( ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            PlayDropItem(iGPO,dropItemId);
        }
        
        virtual protected void PlayDropItem(IGPO dropGpo, int dropItemId) {
            mySystem.Dispatcher(new SE_GPO.Event_GetLastAttackGpo {
                CallBack = gpo => {
                    if (gpo != null) {
                        var pos = GetDropBoxPosition(gpo);
                        mySystem.Dispatcher(new SE_Item.Event_DropItem {
                            DropTypeId = dropTypeId,
                            DropGpo = dropGpo,
                            DropItemId = dropItemId,
                            OR_DropPoint = pos
                        });
                    }
                }
            });
        }
        
        private Vector3 GetDropBoxPosition(IGPO lastAttackGPO) {
            var result = lastAttackGPO.GetPoint();
            var attackPos = lastAttackGPO.GetPoint();
            var curPos = iGPO.GetPoint();
            var dir = new Vector3(curPos.x, attackPos.y, curPos.z) - attackPos;
            var dirNorm = dir.normalized;
            var distance = dir.magnitude;
            var len = Physics.RaycastNonAlloc(attackPos + Vector3.up, dirNorm, cachedHits, distance, ~LayerData.ClientLayerMask);

            Vector3 groundCheckPos;
            if (GetObstaclePos(len, out var obstaclePos)) {
                groundCheckPos = obstaclePos;
            } else {
                groundCheckPos = attackPos + dirNorm * distance;
            }

            len = Physics.RaycastNonAlloc(groundCheckPos + Vector3.up, Vector3.down, cachedHits, 100, ~LayerData.ClientLayerMask);
            if (GetObstaclePos(len, out obstaclePos)) {
                result = obstaclePos;
            }

            return result;
        }

        public bool GetObstaclePos(int count, out Vector3 pos) {
            if (count <= 0) {
                pos = Vector3.zero;
                return false;
            }

            for (var i = 0; i < count; i++) {
                var ray = cachedHits[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }

                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType == null) {
                    pos = ray.point + ray.normal * 0.5f;
                    return true;
                }
            }

            pos = Vector3.zero;
            return false;
        }
    }
}