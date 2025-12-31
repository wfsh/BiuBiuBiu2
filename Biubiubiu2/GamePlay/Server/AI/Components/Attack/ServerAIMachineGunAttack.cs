using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIMachineGunAttack : ServerNetworkComponentBase {
        private float fireIntervalTime = 0.5f;
        private float maxRange = 0f; // 扩散最大值
        private bool isFire = false;
        private WeaponData.GunData gunData;
        private List<Transform> fireTran;
        private int fireIndex = 0;
        private float limitAngle = 30f;
        private float targetAngle = 0f;
        private float syncTime = 0.1f;
        private IGPO targetGPO;
        private S_AI_Base aiSystem;
        private GPOM_MachineGun useMData;
        
        //过热
        private bool isFireDown = false;
        private float countFireOverHotTimer = 0f; // 持续射击时间
        private float fireOverHotTimer = 0f; // 持续射击时间
        private float fireOverHotCDTimer = 0f; // 冷却时间
        private bool isOverHotCoolDown = false;// 是否处于冷却
        private bool isCoolingDown = false;

        protected override void OnAwake() {
            mySystem.Register<SE_AI.Event_SetInsightTarget>(OnSetInsightTarget);
            aiSystem = (S_AI_Base)mySystem;
            useMData = (GPOM_MachineGun)aiSystem.MData;
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            gunData = (WeaponData.GunData)WeaponData.Get(ItemSet.Id_MachineGun);
            fireOverHotTimer = gunData.FireOverHotTime;
        }
        
        private void OnUpdate(float deltaTime) {
            if (ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            UpdateFireOverHotCD();
            CountFireOverHotTimer();
            FireInterval();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_AI.Event_SetInsightTarget>(OnSetInsightTarget);
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            fireTran = iEntity.GetBodyTranList(GPOData.PartEnum.AttactPoint1);
        }
        private void UpdateFireOverHotCD() {
            if (isCoolingDown == false) {
                return;
            }
            fireOverHotCDTimer -= Time.deltaTime;
            if (fireOverHotCDTimer <= 0f) {
                isCoolingDown = false;
            }
        }
        
        private void CountFireOverHotTimer() {
            if (fireOverHotTimer <= 0) {
                return;
            }
            if (isFireDown) {
                if (countFireOverHotTimer >= fireOverHotTimer) {
                    StartCoolingDown();
                } else {
                    countFireOverHotTimer += Time.deltaTime;
                }
            } else {
                if (countFireOverHotTimer > 0f) {
                    countFireOverHotTimer -= Time.deltaTime;
                }
            }
        }
        
        private void StartCoolingDown() {
            isCoolingDown = true;
            countFireOverHotTimer = 0f;
            fireOverHotCDTimer = gunData.FireOverHotCDTime;
        }
        private void OnSetInsightTarget(ISystemMsg body, SE_AI.Event_SetInsightTarget ent) {
            targetGPO = ent.TargetGPO;
            fireIntervalTime = 0.5f;
        }
        private void SetFireBoxRotation() {
            var targetBody = targetGPO.GetTargetTransform();
            var targetPoint = targetGPO.GetPoint();
            if (targetBody != null) {
                targetPoint = targetBody.position;
            }
            
            for (int i = 0; i < fireTran.Count; i++) {
                var fireBox = fireTran[i]; // 选取第一个炮管为基准
                fireBox.localRotation = Quaternion.identity;
                var targetDirection = (targetPoint - fireBox.position).normalized; // 目标朝向
                // 计算新的目标点
                var adjustedTargetPoint = fireBox.position + targetDirection * Vector3.Distance(fireBox.position, targetPoint);
                // 旋转到调整后的目标点
                fireBox.LookAt(adjustedTargetPoint);
            }
        }
        private void FireInterval() {
            if (isCoolingDown || targetGPO == null || targetGPO.IsClear()) {
                isFireDown = false;
                return;
            }
            isFireDown = true;
            if (fireIntervalTime > 0f) {
                fireIntervalTime -= Time.deltaTime;
            } else {
                SetFireBoxRotation();
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
                MData = AbilityM_Bullet.CreateForID(AbilityM_Bullet.ID_BulletMachineGun),
                InData = new AbilityIn_Bullet {
                    In_StartPoint = firePoint,
                    In_TargetPoint = entPoint,
                    In_Speed = gunData.BulletSpeed,
                    In_MoveDistance = gunData.FireDistance,
                    In_BulletAttnMap = gunData.BulletAttnMaps,
                    In_WeaponItemId = gunData.ItemId
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