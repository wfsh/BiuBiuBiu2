using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterToSausageMan : ClientCharacterComponent {
        private C_Character_Base system;
        protected override void OnAwake() {
            base.OnAwake();
            system = (C_Character_Base)mySystem;
        }

        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new CM_Sausage.Event_AddPlayerGPOMapping() {
                PlayerId = system.PlayerId,
                gpo = iGPO,
            });
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_AI.TargetRpc_GPOHurtOutOfFightRange.ID, OnGPOHurtOutOfFightRangeCallBack);
            AddProtoCallBack(Proto_AI.TargetRpc_GPOChangeFightRangeStage.ID, OnGPOChangeFightRangeStage);
            AddProtoCallBack(Proto_AI.TargetRpc_PlayBubble.ID, OnPlayBubble);
            AddProtoCallBack(Proto_AI.TargetRpc_PlayAudio.ID, OnPlayAudio);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_AI.TargetRpc_GPOHurtOutOfFightRange.ID, OnGPOHurtOutOfFightRangeCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_GPOChangeFightRangeStage.ID, OnGPOChangeFightRangeStage);
            RemoveProtoCallBack(Proto_AI.TargetRpc_PlayBubble.ID, OnPlayBubble);
            RemoveProtoCallBack(Proto_AI.TargetRpc_PlayAudio.ID, OnPlayAudio);
            MsgRegister.Dispatcher(new CM_Sausage.Event_RemovePlayerGPOMapping() {
                PlayerId = system.PlayerId,
                gpo = iGPO,
            });
        }
        
        private void OnGPOHurtOutOfFightRangeCallBack(INetwork iBehaviour, IProto_Doc doc) {
            MsgRegister.Dispatcher(new CM_Sausage.Event_GPOHurtOutOfFightRange() {
                PlayerId = system.PlayerId,
            });
        }
        
        private void OnGPOChangeFightRangeStage(INetwork iBehaviour, IProto_Doc doc) {
            var rpcData = (Proto_AI.TargetRpc_GPOChangeFightRangeStage)doc;
            MsgRegister.Dispatcher(new CM_Sausage.LocalPlayerChangeBossFightBGM() {
                isInBossFight = rpcData.isInFightRange
            });
        }
        
        private void OnPlayBubble(INetwork iBehaviour, IProto_Doc doc) {
            var rpcData = (Proto_AI.TargetRpc_PlayBubble)doc;
            MsgRegister.Dispatcher(new CM_Sausage.PlayBubble() {
                EffectPos = rpcData.effectPos,
                EffectSign = rpcData.effectSign,
                LifeTime = rpcData.lifeTime,
            });
        }
        
        private void OnPlayAudio(INetwork iBehaviour, IProto_Doc doc) {
            var rpcData = (Proto_AI.TargetRpc_PlayAudio)doc;
            var audioData = WwiseAudioSet.GetWwiseAudioById(rpcData.WwiseId);
            MsgRegister.Dispatcher(new CM_Sausage.PlayWWise() {
                AudioType = AudioData.AudioTypeEnum.Audio,
                WwiseEventName = audioData.WwiseEvent,
            });
        }
    }
}