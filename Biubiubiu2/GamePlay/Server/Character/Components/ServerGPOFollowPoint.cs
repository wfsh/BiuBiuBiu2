using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOFollowPoint : ServerNetworkComponentBase {
        public struct FollowPointStruct {
            public IGPO gpo;
            public GameObject gameObject;
        }

        private EntityBase mEntity;
        private Transform mEntityTransform;

        private Dictionary<GPOData.FollowPointType, List<FollowPointStruct>> followPoints =
            new Dictionary<GPOData.FollowPointType, List<FollowPointStruct>>();

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_OnGetFollowPoint>(OnGetFollowPointCallBack);
            mySystem.Register<SE_GPO.Event_OnBackFollowPoint>(OnBackFollowPointCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            followPoints.Add(GPOData.FollowPointType.UAV, new List<FollowPointStruct>());
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            mEntity = (EntityBase)iEntity;
            mEntityTransform = mEntity.transform;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_OnGetFollowPoint>(OnGetFollowPointCallBack);
            mySystem.Unregister<SE_GPO.Event_OnBackFollowPoint>(OnBackFollowPointCallBack);
            foreach (var points in followPoints) {
                foreach (var point in points.Value) {
                    GameObject.Destroy(point.gameObject);
                }
            }
            followPoints.Clear();
        }

        private void OnGetFollowPointCallBack(ISystemMsg body, SE_GPO.Event_OnGetFollowPoint ent) {
            switch (ent.pointType) {
                case GPOData.FollowPointType.UAV:
                    GameObject obj = GetFollowPoint(ent);
                    ent.CallBack?.Invoke(obj);
                    break;
            }
        }

        private void OnBackFollowPointCallBack(ISystemMsg body, SE_GPO.Event_OnBackFollowPoint ent) {
            List<FollowPointStruct> points = followPoints[ent.pointType];
            for (var i = 0; i < points.Count; i++) {
                var point = points[i];
                if (point.gpo == null) continue;
                if (point.gpo.IsClear() || point.gpo.GetGpoID() == ent.useGPO.GetGpoID()) {
                    point.gpo = null;
                    points[i] = point;
                }
            }
            ReLayoutFollowPoints(ent.pointType);
        }

        private GameObject GetFollowPoint( SE_GPO.Event_OnGetFollowPoint ent) {
            var points = followPoints[ent.pointType];

            // 复用空位
            for (int i = 0; i < points.Count; i++) {
                var point = points[i];
                if (point.gpo == null || point.gpo.IsClear() || point.gpo.IsDead()) {
                    point.gpo = ent.useGPO;
                    points[i] = point;
                    ReLayoutFollowPoints(ent.pointType);
                    return point.gameObject;
                }
            }

            // 创建新挂点
            var followTarget = new GameObject("DynamicFollowPoint");
            followTarget.transform.SetParent(mEntityTransform);
            followTarget.transform.localEulerAngles = Vector3.zero;
            points.Add(new FollowPointStruct {
                gpo = ent.useGPO, gameObject = followTarget
            });
            ReLayoutFollowPoints(ent.pointType);
            return followTarget;
        }

        /// <summary>
        /// 对指定类型的跟随点进行重新布局，将有效跟随点按弧形规则排列
        /// </summary>
        /// <param name="type">需要布局的跟随点类型</param>
        private void ReLayoutFollowPoints(GPOData.FollowPointType type) {
            // 1. 从字典中获取该类型对应的所有跟随点，若不存在则直接返回
            if (!followPoints.TryGetValue(type, out var points)) return;

            // 2. 筛选有效跟随点（关联对象存在、未被清除、未死亡）
            var activePoints = new List<FollowPointStruct>();
            foreach (var p in points) {
                if (p.gpo != null && !p.gpo.IsClear() && !p.gpo.IsDead()) {
                    activePoints.Add(p);
                }
            }

            // 3. 若没有有效跟随点，直接结束布局
            var total = activePoints.Count;
            if (total == 0) return;

            // 4. 弧形布局参数设置
            var radius = 1.5f;               // 弧形半径（水平方向距离中心点的距离）
            var verticalOffset = 1.8f;       // 所有点的固定垂直高度（Y轴偏移）
            var angleOffset = 30f;           // 弧形总角度范围（左右各30度，共60度）
            var angleStep = (2 * angleOffset) / Mathf.Max(total - 1, 1);  // 每个点的角度间隔（确保均匀分布）

            // 5. 计算并设置每个有效点的位置（按弧形排列）
            for (int i = 0; i < total; i++) {
                var angle = -angleOffset + i * angleStep;  // 当前点的角度（从左到右分布）
                var rad = angle * Mathf.Deg2Rad;           // 角度转弧度（用于三角函数计算）
                // 计算弧形坐标：X（左右）、Y（固定高度）、Z（前后）
                var offset = new Vector3(Mathf.Sin(rad) * radius, verticalOffset, -Mathf.Cos(rad) * radius);
                activePoints[i].gameObject.transform.localPosition = offset;  // 应用计算出的位置
            }
        }
    }
}