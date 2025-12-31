using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonTreadSpawner : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonTreadSpawner Param;
        }
        private SAB_AuroraDragonTreadSpawnerSystem abSystem;
        private AbilityM_AuroraDragonTreadSpawner useMData;
        private ServerGPO fireGPO;
        private List<IGPO> attackTargetList;
        private IGPO attackTarget;
        
        private int treadAttackNum; // 踩踏次数
        private float treadAttackTime; // 踩踏攻击时长
        private float treadFireAttacKTime; // 踩踏喷火攻击时长

        private int curAttackNum; // 当前踩踏次数
        private float curAnimTime; // 当前攻击动画计时
        private bool isLeftFoot; // 当前是否迈出左脚
        private string sceneTypeSign;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonTreadSpawnerSystem)mySystem;
            var initData = (InitData)initDataBase;
            useMData = initData.Param;
            fireGPO = abSystem.FireGPO;
            
            treadAttackNum = useMData.M_TreadAttackNum;
            treadAttackTime = useMData.M_TreadAttackTime;
            treadFireAttacKTime = useMData.M_TreadFireAttacKTime;
        }

        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = useMData.GetTypeID(),
            });
            #endregion
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_GetAttackTarget() {
                CallBack = (targetGPO) => { attackTarget = targetGPO; },
            });
            LookForTarget();
            if (fireGPO != null && attackTarget != null) {
                var cross = Vector3.Cross(fireGPO.GetForward(), attackTarget.GetPoint() - fireGPO.GetPoint());
                isLeftFoot = cross.y <= 0f;
            }
            sceneTypeSign = ExtractPart(SceneData.Get(ModeData.SceneId).ElementConfig);
            OnTreadAttack();
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
            useMData = null;
        }

        private void OnUpdate(float deltaTime) {
            if (curAnimTime > 0f) {
                curAnimTime -= deltaTime;
                if (curAnimTime <= 0f) {
                    if (curAttackNum < treadAttackNum) {
                        OnTreadAttack();
                    } else {
                        OnFireAttack();
                    }
                }
            }
        }
        
        private void LookForTarget() {
            if (attackTarget == null || attackTarget.IsClear() ||
                fireGPO == null || fireGPO.IsClear()) {
                return;
            }
            var targetPoint = this.attackTarget.GetPoint();
            var point = Vector3.zero;
            point.x = targetPoint.x;
            point.y = fireGPO.GetPoint().y;
            point.z = targetPoint.z;
            fireGPO.GetEntity().LookAT(point);
        }

        private void OnTreadAttack() {
            ++curAttackNum;
            curAnimTime = treadAttackTime;
            LookForTarget();
            
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = fireGPO,
                MData = AbilityM_AuroraDragonTread.CreateForKey(useMData.K_BossType, sceneTypeSign),
                InData = new AbilityIn_AuroraDragonTread() {
                    In_IsLeftFoot = isLeftFoot,
                },
            });
            isLeftFoot = !isLeftFoot; // 换脚
        }
        
        private string ExtractPart(string input) {
            var parts = input.Split('_');
            if (parts.Length >= 2) {
                // 取最后两个部分组合
                return $"{parts[parts.Length - 2]}_{parts[parts.Length - 1]}";
            }
            return input; // 格式不符合时返回原始字符串
        }  

        private void OnFireAttack() {
            LookForTarget();
             MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                 FireGPO = fireGPO,
                 MData = AbilityM_AuroraDragonTreadFire.CreateForKey(useMData.K_BossType, sceneTypeSign),
             });
        }
    }
}