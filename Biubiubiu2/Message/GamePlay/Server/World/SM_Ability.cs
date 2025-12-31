using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerMessage {
    public partial class SM_Ability {
        public struct BeforeRemoveAbility : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<BeforeRemoveAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public IAbilitySystem abSystem;
        }

        public struct RemoveAbility : GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<RemoveAbility>();
            public int GetID() => _id;
            // 下面写你的参数
            public int AbilityId;
        }

        /// <summary>
        /// 这个不要在用了。都去用 PlayAbility
        /// </summary>
        public struct PlayAbilityOld: GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayAbilityOld>();
            public int GetID() => _id;
            // 常规标签
            public IGPO FireGPO;
            public IAbilityMData AbilityMData;
            public Action<IAbilitySystem> CallBack;
        }
        
        public struct PlayAbility: GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayAbility>();
            public int GetID() => _id;
            // 常规标签
            public IGPO FireGPO;
            // 删除无用的文件
            public IAbilityMData MData;
            public IAbilityInData InData;
            // 可选参数
            public Action<IAbilitySystem> OR_CallBack;
            public IAbilitySystem OR_ParentAB;
        }
        public struct PlayAbilityEffect: GamePlayEvent.IWorldEvent {
            private static readonly int _id = GamePlayEvent.ReadonlyWorldEventID<PlayAbilityEffect>();
            public int GetID() => _id;
            // 常规标签
            public IGPO FireGPO;
            public IGPO TargetGPO;
            public IAbilityEffectMData MData;
            public IAbilityEffectInData InData;
            // 可选参数
            public Action<IAbilitySystem> OR_CallBack;
            public IAbilitySystem OR_ParentAB; 
        }
    }
}