using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;

public class SE_AI_AuroraDragon : MonoBehaviour {
        
    public struct Event_FireBallStartAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireBallStartAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }

    public struct Event_FireBallAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireBallAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_FireBallEndAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireBallEndAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool IsTrue;
    }
    
    public struct Event_FullScreenAOE : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FullScreenAOE>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_TreadStartAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TreadStartAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool isLeftFoot;
    }
    
    public struct Event_TreadAttackAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TreadAttackAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool isLeftFoot;
    }
    
    public struct Event_TreadFireAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TreadFireAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool IsTrue;
    }
    
    public struct Event_FireStartAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireStartAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_FireForwardAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FireForwardAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool IsTrue;
    }
    
    public struct Event_GetAttackTarget : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetAttackTarget>();

        public int GetID() => _id;
        // 下面写你的参数
        public Action<IGPO> CallBack;
    }
    
    public struct Event_TornadoAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TornadoAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public int animState;
    }
    
    public struct Event_DelayBlast : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DelayBlast>();

        public int GetID() => _id;
        // 下面写你的参数
        public int AttackNum;
        public float AttackCD;
    }
    
    public struct Event_DelayBlastStartAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DelayBlastStartAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_DelayBlastEndAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DelayBlastEndAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_DragonCarChargeAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DragonCarChargeAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_DragonCarAttackAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DragonCarAttackAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_DragonCarDownAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DragonCarDownAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_DragonCarEndAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_DragonCarEndAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool IsTrue;
    }
    
    public struct Event_AuroraDragonAppearAni : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AuroraDragonAppearAni>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool IsTrue;
}

    public struct Event_PlaySkillEnd : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_PlaySkillEnd>();

        public int GetID() => _id;
    }
    
    public struct Event_AuroraDragonOutAni : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AuroraDragonOutAni>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_AuroraDragonFireEffectScale : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AuroraDragonFireEffectScale>();

        public int GetID() => _id;
        // 下面写你的参数
        public Vector3 Scale;
    }
    
    public struct Event_FlyFlameStartAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FlyFlameStartAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_FlyFlameAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FlyFlameAnim>();

        public int GetID() => _id;
        // 下面写你的参数
    }
    
    public struct Event_FlyFlameEndAnim : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_FlyFlameEndAnim>();

        public int GetID() => _id;
        // 下面写你的参数
        public bool IsTrue;
    }
    
    public struct Event_GetBlockFireEndPoint : GamePlayEvent.ISystemEvent {
        private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetBlockFireEndPoint>();

        public int GetID() => _id;
        // 下面写你的参数
        public Vector3 FirePos;
        public Vector3 FireDir;
        public float AttackHeight;
        public float AttackLength;
        public Action<Vector3> CallBack;
    }
}
