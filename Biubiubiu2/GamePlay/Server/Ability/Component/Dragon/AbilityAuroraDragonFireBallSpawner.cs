using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonFireBallSpawner : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonFireBallSpawner Param;
        }
        private SAB_AuroraDragonFireBallSpawnerSystem abSystem;
        private AbilityM_AuroraDragonFireBallSpawner config;
        private ServerGPO fireGPO;
        private List<IGPO> attackTargetList;
        
        private float fireBallStartAnimTime; // 升空动画时长
        private int fireBallAttackNum; // 发射次数
        private float fireBallAnimTime; // 发射动画时长
        private float fireBallEndAnimTime; // 结束动画时长

        private Transform attackBoxTran;
        private int curAttackNum; // 当前发射次数
        private float curAnimTime; // 当前攻击动画计时
        private float waitAddForceTime = -1; // 添加击退力的延迟时间
        private string sceneTypeSign;
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonFireBallSpawnerSystem)mySystem;
            var initData = (InitData)initDataBase;
            config = initData.Param;
            SetDefaultValue();
        }

        protected override void OnStart() {
            base.OnStart();
            #region 埋点
            MsgRegister.Dispatcher(new SM_Sausage.BossReleaseAbility() {
                SourceAbilityType = config.GetTypeID(),
            });
            #endregion
            var entity = (AIEntity)fireGPO.GetEntity();
            attackBoxTran = entity.AttackTran;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { attackTargetList = targetList; },
            });
            sceneTypeSign = ExtractPart(SceneData.Get(ModeData.SceneId).ElementConfig);
            StartFlyUp();
            AddUpdate(OnUpdate);
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
            config = null;
        }

        private void OnUpdate(float deltaTime) {
            if (fireBallStartAnimTime > 0f) {
                fireBallStartAnimTime -= deltaTime;
                if (fireBallStartAnimTime <= 0f) {
                    // 进入攻击阶段
                    StartAttack();
                }
            } else if (curAnimTime > 0f) {
                curAnimTime -= deltaTime;
                if (curAnimTime <= 0f) {
                    if (curAttackNum < fireBallAttackNum) {
                        OnAttack();
                    } else {
                        fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FireBallEndAnim {IsTrue = true});
                        waitAddForceTime = 0.5f;
                    }
                }
            }
            if (waitAddForceTime > 0) {
                waitAddForceTime -= deltaTime;
                if (waitAddForceTime <= 0) {
                    AddEndForce();
                }
            }
        }
        
        private void SetDefaultValue() {
            fireGPO = abSystem.FireGPO;
            fireBallStartAnimTime = config.M_FireBallStartAnimTime;
            fireBallAttackNum = config.M_FireBallAttackNum;
            fireBallAnimTime = config.M_FireBallAnimTime;
            fireBallEndAnimTime = config.M_FireBallEndAnimTime;
        }
        
        private string ExtractPart(string input) {
            var parts = input.Split('_');
            if (parts.Length >= 2) {
                // 取最后两个部分组合
                return $"{parts[parts.Length - 2]}_{parts[parts.Length - 1]}";
            }
            return input; // 格式不符合时返回原始字符串
        }  

        private void StartFlyUp() {
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FireBallStartAnim {});
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(AbilityM_PlayEffect.ID_AuroraDragonFireBallStart),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = fireGPO.GetPoint(),
                    In_StartRota = fireGPO.GetRota(),
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill3Fly,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = config.M_FireBallStartAnimTime
                }
            });
        }

        private void StartAttack() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill3Swing,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = config.M_FireBallAttackNum * config.M_FireBallAnimTime
                }
            });
            OnAttack();
        }
        
        private void OnAttack() {
            ++curAttackNum;
            curAnimTime = fireBallAnimTime;
            
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_FireBallAnim {});
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill3Cast,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = fireBallAnimTime
                }
            });
            var startPoint = attackBoxTran != null ? attackBoxTran.position : fireGPO.GetPoint();
            if (attackTargetList != null && attackTargetList.Count > 0) {
                for (int i = 0; i < attackTargetList.Count; ++i) {
                    var targetGPO = attackTargetList[i];
                    if (targetGPO == null || targetGPO.IsDead()) {
                        continue;
                    }

                    var endPos = targetGPO.GetPoint();
                    MsgRegister.Dispatcher(new SM_Sausage.GetGroundExceptWater {
                        NowPoint = endPos + Vector3.up,
                        CallBack = (pos) => { endPos = pos; }
                    });
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                        FireGPO = fireGPO,
                        MData = AbilityM_AuroraDragonFireBall.CreateForKey(config.M_BossType, sceneTypeSign),
                        InData = new AbilityIn_AuroraDragonFireBall() {
                            In_StartPos = startPoint,
                            In_EndPos = endPos + new Vector3(0f, 0.1f, 0f), // 略高于地面，不然会闪
                        },
                    });
                }
            }
        }
        
        private void AddEndForce() {
            List<IGPO> playerGpo = new List<IGPO>();
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { playerGpo = targetList; },
            });
            foreach (var roleGpo in playerGpo) {
                if (!roleGpo.IsDead()) {
                    var rolePoint = roleGpo.GetPoint();
                    rolePoint.y = 0;
                    var centerPoint = fireGPO.GetPoint();
                    centerPoint.y = 0;
                    float sqrDistance = (rolePoint - centerPoint).sqrMagnitude;
                    float sqrThreshold = 6f * 6f;
                    if (sqrDistance < sqrThreshold) {
                        roleGpo.Dispatcher(new SE_Character.AddSausageRoleMoveForce() {
                            CenterPoint = centerPoint,
                        });
                    }
                }
            }
        }
    }
}