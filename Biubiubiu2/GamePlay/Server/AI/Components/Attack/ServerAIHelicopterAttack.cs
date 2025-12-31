using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Template;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIHelicopterAttack : ServerNetworkComponentBase {
        private float fireIntervalTime = 0.1f;
        private float maxRange = 0f; // 扩散最大值
        private bool isFire = false;
        private WeaponData.GunData gunData;
        private List<Transform> fireTran;
        private float startFireTime = 0f;
        private int fireIndex = 0;
        private float limitAngle = 30f;
        private float targetAngle = 0f;
        private IGPO target; // AI模式下的目标
        private Transform rootTran; 
        private float syncTime = 0.1f;
        private S_AI_Base aiSystem;
        private GPOM_Helicopter useMData;

        protected override void OnAwake() {
            Register<SE_AI.Event_PlayerDriveFireForPoints>(OnPlayerDriveFireForPointsCallBack);
            Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            Register<SE_Behaviour.Event_PlayAttack>(OnPlayAttackCallBack);
            aiSystem = (S_AI_Base)mySystem;
            useMData = (GPOM_Helicopter)aiSystem.MData;
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            gunData = (WeaponData.GunData)WeaponData.Get(ItemSet.Id_Helicopter);
        }

        protected override void OnClear() {
            base.OnClear();
            Unregister<SE_AI.Event_PlayerDriveFireForPoints>(OnPlayerDriveFireForPointsCallBack);
            Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            Unregister<SE_Behaviour.Event_PlayAttack>(OnPlayAttackCallBack);
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            fireTran = iEntity.GetBodyTranList(GPOData.PartEnum.AttactPoint1);
            rootTran = iEntity.GetBodyTran(GPOData.PartEnum.RootBody);
        }

        public void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            target = ent.TargetGPO;
        }

        public void OnPlayAttackCallBack(ISystemMsg body, SE_Behaviour.Event_PlayAttack ent) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            // 触发射击逻辑
            if (targetAngle < 90f) {
                this.startFireTime = Time.realtimeSinceStartup;
            }
        }

        public void OnPlayerDriveFireForPointsCallBack(ISystemMsg body, SE_AI.Event_PlayerDriveFireForPoints ent) {
            if (ent.Points.Length <= 0) {
                return;
            }
            var targetPoint = ent.Points[0];
            for (int i = 0; i < fireTran.Count; i++) {
                var fireBox = fireTran[i]; // 选取第一个炮管为基准
                fireBox.localRotation = Quaternion.identity;
                var fireDirection = fireBox.forward; // 当前炮管朝向
                var targetDirection = (targetPoint - fireBox.position).normalized; // 目标朝向
                // 计算当前朝向与目标朝向的夹角
                // var angle = Vector3.Angle(fireDirection, targetDirection);
                // // 限制旋转角度
                // if (angle > limitAngle) {
                //     // 计算旋转轴
                //     var rotationAxis = Vector3.Cross(fireDirection, targetDirection).normalized;
                //     // 限制旋转角度到 limitAngle 度
                //     var limitedRotation = Quaternion.AngleAxis(limitAngle, rotationAxis);
                //     targetDirection = limitedRotation * fireDirection;
                // }
                // 计算新的目标点
                var adjustedTargetPoint = fireBox.position + targetDirection * Vector3.Distance(fireBox.position, targetPoint);
                // 旋转到调整后的目标点
                fireBox.LookAt(adjustedTargetPoint);
            }
            this.startFireTime = Time.realtimeSinceStartup;
        }

        private void OnUpdate(float deltaTime) {
            TargetRota();
            FireInterval();
        }

        private void TargetRota() {
            if (target == null || rootTran == null) {
                return;
            }
            var lockPoint = target.GetPoint();
            var lockRootBody = target.GetBodyTran(GPOData.PartEnum.RootBody);
            if (lockRootBody) {
                lockPoint = lockRootBody.position;
            }
            // 计算 rootTran 到目标的方向
            var directionToTarget = (lockPoint - rootTran.position).normalized;
            // 计算应当旋转的 x 轴角度
            var targetXRotation = Mathf.Asin(directionToTarget.y) * Mathf.Rad2Deg;
            // 平滑过渡 x 旋转
            var localEulerAngles = rootTran.localEulerAngles;
            localEulerAngles.x = Mathf.LerpAngle(localEulerAngles.x, -targetXRotation, Time.deltaTime * 5f);
            rootTran.localEulerAngles = localEulerAngles;
            // 计算 rootTran 当前朝向与目标之间的夹角
            targetAngle = Vector3.Angle(rootTran.forward, directionToTarget);
            SyncUpperBodyRota();
        }
        
        private void SyncUpperBodyRota() {
            if (Time.realtimeSinceStartup - syncTime < 0.1f) {
                return;
            }
            syncTime = Time.realtimeSinceStartup;
            Rpc(new Proto_AI.Rpc_SyncHelicopterRootRotaX() {
                rx = rootTran.localEulerAngles.x,
            });
        }

        private void FireInterval() {
            if (Time.realtimeSinceStartup - startFireTime > 0.5f) {
                return;
            }
            if (fireIntervalTime > 0f) {
                fireIntervalTime -= Time.deltaTime;
            } else {
                fireIntervalTime = useMData.AttackIntervalTime;
                fireIndex++;
                if (fireIndex >= fireTran.Count) {
                    fireIndex = 0;
                }
                var fireBox = fireTran[fireIndex];
                var firePointPosition = fireBox.position;
                var entPoint = firePointPosition + fireBox.forward * useMData.MaxAttackDistance;
                FireBullet(firePointPosition, entPoint);
            }
        }

        /// <summary>
        /// entPoint 为 firePoint 正前方 100 米的位置
        /// </summary>
        private void FireBullet(Vector3 firePoint, Vector3 entPoint) {
            entPoint = GetRandomPoint(firePoint, entPoint);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_Bullet.CreateForID(AbilityM_Bullet.ID_BulletGatling),
                InData = new AbilityIn_Bullet {
                    In_StartPoint = firePoint,
                    In_TargetPoint = entPoint,
                    In_Speed = gunData.BulletSpeed,
                    In_MoveDistance = gunData.FireDistance,
                    In_BulletAttnMap = gunData.BulletAttnMaps,
                    In_WeaponItemId = gunData.ItemId,
                }
            });
        }

        /// <summary>
        /// 扩散计算
        /// </summary>
        private Vector3 GetRandomPoint(Vector3 firePoint, Vector3 endPoint) {
            var shootDirection = (endPoint - firePoint).normalized;
            var distance = Vector3.Distance(firePoint, endPoint);
            var forwardPoint = firePoint + shootDirection * Mathf.Max(100, distance); // 作为散布中心
            var randomDistance = Random.Range(0f, gunData.FireRange * 0.1f);
            var angle = Random.Range(0f, 360f);
            var x = randomDistance * Mathf.Cos(angle * Mathf.Deg2Rad);
            var y = randomDistance * Mathf.Sin(angle * Mathf.Deg2Rad);
            return forwardPoint + new Vector3(x, y, x);
        }
    }
}