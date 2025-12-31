using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SAB_AIBallSystem : S_Ability_Base {
        private AbilityData.PlayAbility_AIBall inData;
        protected override void OnAwake() {
            base.OnAwake();
            inData = (AbilityData.PlayAbility_AIBall)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(inData.M_EffectSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }

        override protected void AddComponents() {
            base.AddComponents();
            AddLifeTime();
            AddMove();
            AddSelect();
            RPCAbility(new Proto_Ability.Rpc_ThreadMonsterBall() {
                EffectSign = inData.M_EffectSign, 
                points = inData.In_Points,
                LifeTime = inData.M_LifeTime,
                MonsterPID = inData.In_MonsterPID,
                Speed = inData.M_Speed,
            });
        }

        private void AddLifeTime() {
            AddComponent<TimeReduce>(new TimeReduce.InitData {
                LifeTime = inData.M_LifeTime,
                CallBack = LifeTimeEnd
            });
        }

        private void AddMove() {
            AddComponent<MoveGrenade>(new MoveGrenade.InitData {
                Points = inData.In_Points,
                Speed = inData.M_Speed
            });
        }

        private void AddSelect() {
             AddComponent<ServerAIBallHit>(new ServerAIBallHit.InitData {
                 MonsterPid = inData.In_MonsterPID,
                 EndCallBack = LifeTimeEnd
            });
        }

        private void LifeTimeEnd() {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = AbilityId
            });
        }

        private void PlayAE() {
            // MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
            //     CallBack = null,
            //     FireGPO = iGPO,
            //     AbilityData = new AbilityData.PlayAbility_PlayEffect {
            //         ModSign = AbilityModData.PlayEffect_Bomb,
            //         In_StartPoint = iEntity.GetPoint(),
            //         In_StartRota = Quaternion.identity
            //     }
            // });
        }
    }
}