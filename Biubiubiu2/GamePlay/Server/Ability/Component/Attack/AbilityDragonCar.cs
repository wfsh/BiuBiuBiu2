using System;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityDragonCar : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public Vector3 StartPoint;
            public Vector3 EndPoint;
            public float Distance;
            public float MoveSpeed;
            public float Range;
            public Action OnMoveEndCallBack;
        }
        private S_Ability_Base abSystem;
        private ServerGPO fireGPO;
        private Action onMoveEndCallBack;
        private float range = 0f;
        private float delayTime = 0.0f;
        private List<Vector3> CheckPoints = new List<Vector3>();
        private Dictionary<int, bool> hitGPOData = new Dictionary<int, bool>();
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (S_Ability_Base)mySystem;
            fireGPO = abSystem.FireGPO;
            fireGPO.Register<SE_GPO.Event_MovePointEnd>(OnMovePointEnd);
            var initData = (InitData)initDataBase;
            SetTargetPoint(initData.StartPoint, initData.EndPoint, initData.Distance, initData.MoveSpeed, initData.Range,
                initData.OnMoveEndCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            AddCollider();
        }

        private void AddCollider() {
            var entity = (EntityBase)iEntity;
            var gameCollider = entity.GetComponentInChildren<Collider>();
            var collision = gameCollider.gameObject.AddComponent<CollisionEnterCheck>();
            collision.Init(OnHitCollision, null, null);
            var collider = gameCollider.gameObject.AddComponent<ColliderEnterCheck>();
            collider.Init(OnHitCollider, null, null);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO.Unregister<SE_GPO.Event_MovePointEnd>(OnMovePointEnd);
            fireGPO.Dispatcher(new SE_Ability.CanceMovePoint());
            onMoveEndCallBack = null;
            CheckPoints.Clear();
            fireGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            DrawLine();
            if (fireGPO == null || fireGPO.IsClear()) {
                MoveEnd();
                return;
            }
            iEntity.SetPoint(fireGPO.GetPoint());
            iEntity.SetRota(fireGPO.GetRota());
            if (delayTime > 0f) {
                delayTime -= Time.deltaTime;
                return;
            }
            delayTime = 0.5f;
            fireGPO.Dispatcher(new SE_GPO.GetAttackPoint {
                CallBack = GetAttackPoint,
            });
        }

        private void GetAttackPoint(Vector3 point) {
            CheckPoints.Add(point);
        }

        private void DrawLine() {
            if (CheckPoints.Count <= 0) {
                return;
            }
            var losePoint = Vector3.zero;
            for (int i = 0; i < CheckPoints.Count; i++) {
                var point = CheckPoints[i];
                if (losePoint != Vector3.zero) {
                    Debug.DrawLine(point, losePoint, i % 2 == 0 ? Color.green : Color.red);
                }
                losePoint = point;
            }
            Debug.DrawLine(iEntity.GetPoint(), losePoint, Color.yellow);
        }

        private void OnHitCollision(Collision collision) {
            HitGameObj(collision.gameObject);
        }

        private void OnHitCollider(Collider collision) {
            HitGameObj(collision.gameObject);
        }

        private void HitGameObj(GameObject gameObj) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                    return;
                }
                if (hitType.MyEntity != null && hitType.MyEntity.GetGPO() == null) {
                    return;
                }
                var hitGpo = hitType.MyEntity.GetGPO();
                var hitId = hitType.MyEntity.GetGPOID();
                if (hitId == fireGPO.GetGpoID() || fireGPO.GetTeamID() == hitGpo.GetTeamID()) {
                    return;
                }
                if (hitGPOData.ContainsKey(hitId)) {
                    return;
                }
                hitGPOData.Add(hitId, true);
                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = hitType.MyEntity.GetGPO(),
                    hitPoint = Vector3.zero
                });
            }
        }

        public void SetTargetPoint(Vector3 startPoint, Vector3 endPoint, float distance, float moveSpeed, float range,
            Action onMoveEndCallBack) {
            this.range = range;
            var targetPoint = Check(startPoint, endPoint, distance);
            this.onMoveEndCallBack = onMoveEndCallBack;
            fireGPO.Dispatcher(new SE_Ability.MovePoint {
                movePoint = targetPoint, isRun = true, moveSpeed = moveSpeed
            });
        }

        public void OnMovePointEnd(ISystemMsg body, SE_GPO.Event_MovePointEnd entData) {
            MoveEnd();
        }

        private void MoveEnd() {
            onMoveEndCallBack.Invoke();
        }

        // 计算从点A到点B直线上指定距离处的坐标  
        public Vector3 Check(Vector3 start, Vector3 end, float distance) {
            // 计算两点之间的向量  
            var direction = end - start;
            // 计算两点之间的距离  
            var distanceSquared = direction.sqrMagnitude;
            // 如果两点重合，返回起点并给出警告（或者你可以根据需求返回其他值）  
            if (distanceSquared <= float.Epsilon) {
                return start;
            }
            // 计算单位向量  
            var unitDirection = direction / Mathf.Sqrt(distanceSquared);
            // 计算目标点  
            var targetPoint = start + unitDirection * distance;
            return targetPoint;
        }
    }
}