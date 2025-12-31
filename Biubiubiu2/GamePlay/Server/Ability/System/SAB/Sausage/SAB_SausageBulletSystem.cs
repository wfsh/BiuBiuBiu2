using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_SausageBulletSystem : S_Ability_Base {
        private AbilityData.PlayAbility_SausageBullet _abilityData;
        private float lifeTime = 0.0f;

        protected override void OnAwake() {
            base.OnAwake();
            _abilityData = (AbilityData.PlayAbility_SausageBullet)MData;
            iEntity.SetPoint(_abilityData.In_StartPoint);
            iEntity.SetRota(_abilityData.In_StartRota);
            AddComponents();
        }

        private void AddComponents() {
            lifeTime = _abilityData.In_LifeTime;
            AddLifeTime();
            AddMove();
            AddSelect();
            AddHit();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = lifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<SausageBulletMove>(new SausageBulletMove.InitData {
                speedAndGravity = _abilityData.In_SpeedAndGravity,
                startPoint = _abilityData.In_StartPoint,
                speedRatio = _abilityData.In_SpeedRatio,
            });
        }

        private void AddSelect() {
            AddComponent<BulletSelectGPO>(new BulletSelectGPO.InitData {
                IgnoreGpoID = FireGPO.GetGpoID(),
                IgnoreGPOType = _abilityData.In_IgnoreGPOType,
                HitCallBack = (o, hit) => {
                    LifeTimeEnd();
                }
            });
        }


        private void AddHit() {
            AddComponent<ServerAbilitySausageHurtGPO>(new ServerAbilitySausageHurtGPO.InitData {
                GunAutoItemId = _abilityData.In_GunAutoItemId,
                StartPoint = _abilityData.In_StartPoint,
                BuffDamage = _abilityData.In_BuffDamage,
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }
    }
}