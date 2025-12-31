using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityAuroraDragonTread : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public AbilityM_AuroraDragonTread Param;
        }
        private SAB_AuroraDragonTreadSystem abSystem;
        private AbilityM_AuroraDragonTread useMData;
        private AbilityIn_AuroraDragonTread useInData;
        private ServerGPO fireGPO;
        private bool isLeftFoot;
        private float startAnimTime = 1.5f; // 抬腿动作时长
        private float attackAnimTime = 2.2f; // 攻击动作时长
        private float hitTime;
        private Vector3 monsterScale = Vector3.one; // 怪物缩放，万一要修改怪物体型
        
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_AuroraDragonTreadSystem)mySystem;
            useInData = (AbilityIn_AuroraDragonTread)abSystem.InData;
            var initData = (InitData)initDataBase;
            useMData = initData.Param;
            fireGPO = abSystem.FireGPO;
            hitTime = useMData.M_CheckDamageTime;
            isLeftFoot = useInData.In_IsLeftFoot;
        }
        
        protected override void OnStart() {
            base.OnStart();
            PlayWarringEffect();
            AddUpdate(OnUpdate);
            PlayAttackStartAnim();
        }
        
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
            useMData = null;
        }

        private void OnUpdate(float deltaTime) {
            if (startAnimTime > 0f) {
                startAnimTime -= deltaTime;
                if (startAnimTime <= 0f) {
                    PlayAttackAnim();
                    PlayAttackEffect();
                }
            }
            if (hitTime > 0f) {
                hitTime -= deltaTime;
                if (hitTime <= 0f) {
                    CheckDamage();
                }
            }
        }
        
        private void PlayWarringEffect() {
            var effectRotaion = Quaternion.LookRotation(fireGPO.GetForward());
            var footPoint = (isLeftFoot ? useMData.M_FootLeftLocalPos : useMData.M_FootRightLocalPos) * monsterScale.x;
            var effectPos = fireGPO.GetPoint() + effectRotaion * footPoint + new Vector3(0, 0.1f, 0);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_FanWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = effectPos,
                    In_StartRota = effectRotaion,
                    In_StartScale = new Vector3(useMData.M_AttackRadius * 2f, 2f, useMData.M_AttackRadius * 2f),
                    In_LifeTime = hitTime,
                },
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(AbilityM_PlayEffectWithFullDimensionScale.ID_RoundWarring),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = effectPos,
                    In_StartScale = new Vector3(useMData.M_BoomRadius * 2f, 1.5f, useMData.M_BoomRadius * 2f),
                    In_LifeTime = hitTime,
                },
            });
        }

        private void PlayAttackStartAnim() {
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_TreadStartAnim {
                isLeftFoot = isLeftFoot
            });
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonSkill1Attack,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 3f,
                }
            });
        }
        
        private void PlayAttackAnim() {
            fireGPO.Dispatcher(new SE_AI_AuroraDragon.Event_TreadAttackAnim {
                isLeftFoot = isLeftFoot
            });
        }

        private void PlayAttackEffect() {
            var effectRotaion = Quaternion.LookRotation(fireGPO.GetForward());
            var footPoint = (isLeftFoot ? useMData.M_FootLeftLocalPos : useMData.M_FootRightLocalPos) * monsterScale.x;
            var effectPos = fireGPO.GetPoint() + effectRotaion * footPoint;
            var rowId = useMData.K_BossType == 1
                ? AbilityM_PlayEffectWithFullDimensionScale.ID_AuroraDragonTreadAttack
                : AbilityM_PlayEffectWithFullDimensionScale.ID_AncientDragonTreadAttack;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayEffectWithFullDimensionScale.CreateForID(rowId),
                InData = new AbilityIn_PlayEffectWithFullDimensionScale() {
                    In_StartPoint = effectPos,
                    In_StartRota = effectRotaion,
                    In_StartScale = useMData.M_AttackEffectScale,
                    In_LifeTime = attackAnimTime,
                },
            });
        }

        private void CheckDamage() {
            if (fireGPO == null || fireGPO.IsDead() || fireGPO.IsClear()) {
                return;
            }
            List<IGPO> targetGPOList = null;
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetAllTargetInFightRange() {
                CallBack = (targetList) => { targetGPOList = targetList; },
            });
            if (null == targetGPOList || targetGPOList.Count <= 0) {
                return;
            }

            for (int i = 0; i < targetGPOList.Count; ++i) {
                var target = targetGPOList[i];
                if (target == null || target.IsDead() || target.IsClear()) {
                    continue;
                }
                
                var footPoint = (isLeftFoot ? useMData.M_FootLeftLocalPos:useMData.M_FootRightLocalPos) * monsterScale.x;
                var offsetPos = Quaternion.LookRotation(fireGPO.GetForward()) * footPoint;
                var checkPos = fireGPO.GetPoint() + offsetPos;
                var dest = target.GetPoint() - checkPos;
                // to do 可能需要定位地面坐标通过离地面高度 + 高度差双重检测
                if (Mathf.Abs(dest.y) > useMData.M_AttackHeight) {
                    continue;
                }

                dest.y = 0;
                float dist = dest.magnitude;
                if (dist < useMData.M_BoomRadius) {
                    // 爆炸检测
                    mySystem.Dispatcher(new SE_Ability.HitGPO {
                        hitGPO = target,
                        isHead = false,
                        hitPoint = Vector3.zero,
                        SourceAbilityType = useMData.GetTypeID(),
                    });
                    // 不重复检测
                    break;
                }

                if (dist > useMData.M_AttackRadius) {
                    continue;
                }

                var dir = fireGPO.GetForward();
                dest.Normalize();
                if (dest.x * dir.x + dest.z * dir.z < Mathf.Cos(useMData.M_AttackAngle * Mathf.PI / 360)) {
                    continue;
                }

                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = target,
                    isHead = false,
                    hitPoint = Vector3.zero,
                    SourceAbilityType = useMData.GetTypeID(),
                });
            }
        }
    }
}