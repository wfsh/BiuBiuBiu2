using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonDragonCarFlame : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityIn_AuroraDragonDragonCarFlame Param;
        }
        private SAB_AuroraDragonDragonCarFlameSystem abSystem;
        private AbilityIn_AuroraDragonDragonCarFlame config;
        private ServerGPO fireGPO;
        
        private Vector3 startPos;
        private Vector3 endPos;
        private float speed;
        private float damageRadius;
        private float damageInterval;
        private float damageHeight;

        private float disMax;
        private Vector3 dir;
        private Vector3 dirNormal;
        private float disCur;
        private float checkDamageTime;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonDragonCarFlameSystem)mySystem;
            fireGPO = abSystem.FireGPO;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            SetDefaultValue();
        }
        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            disCur = 0f;
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
        }
        
        private void SetDefaultValue() {
            startPos = config.In_StartPos;
            endPos = config.In_EndPos;
            speed = config.In_Speed;
            damageRadius = config.In_DamageRadius;
            damageInterval = config.In_DamageInterval;
            damageHeight = config.In_DamageHeight;
            dir = endPos - startPos;
            disMax = dir.magnitude;
            dir.Normalize();
            dirNormal = Vector3.Cross(dir, Vector3.up).normalized;
            var myMonsterEntity = (AIEntity)fireGPO.GetEntity();
            var forward = myMonsterEntity.GetEulerAngles();
            forward.x = forward.z = 0f;
            iEntity.SetPoint(startPos);
            iEntity.SetRota(Quaternion.Euler(forward));
            iEntity.SetLocalScale(new Vector3(damageRadius / 2.5f, 1f, 0f));
        }

        private void OnUpdate(float deltaTime) {
            disCur += deltaTime * speed;
            if (disCur > disMax) {
                disCur = disMax;
            }
            var scale = new Vector3(damageRadius / 2.5f, 1f, disCur / 10f);
            iEntity.SetLocalScale(scale);
            checkDamageTime += deltaTime;
            if (checkDamageTime > damageInterval) {
                checkDamageTime = 0;

                var curPos = startPos + disCur * dir;
                Vector3[] corners = new Vector3[4];
                corners[0] = startPos - dirNormal * damageRadius;
                corners[1] = startPos + dirNormal * damageRadius;
                corners[2] = curPos + dirNormal * damageRadius;
                corners[3] = curPos - dirNormal * damageRadius;

                List<IGPO> targetGPOList = null;
                fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                    CallBack = (targetList) => { targetGPOList = targetList; },
                });
                if (null == targetGPOList || targetGPOList.Count <= 0) {
                    return;
                }

                for (int i = 0; i < targetGPOList.Count; ++i) {
                    var target = targetGPOList[i];
                    if (null != target &&
                        !target.IsDead()) {
                        // 高度差检测
                        if (Mathf.Abs(target.GetPoint().y - startPos.y) > damageHeight) {
                            continue;
                        }

                        MsgRegister.Dispatcher(new SM_Sausage.GetIsPointInRectangleIgnoreY {
                            Pos = target.GetPoint(),
                            Corners = corners,
                            CallBack = (isHit) => {
                                if (isHit) {
                                    Dispatcher(new SE_Ability.HitGPO {
                                        hitGPO = target,
                                        isHead = false,
                                        hitPoint = Vector3.zero,
                                        SourceAbilityType = abSystem.MData.GetTypeID(),
                                    });
                                }
                            }
                        });
                    }
                }
            }
        }
    }
}