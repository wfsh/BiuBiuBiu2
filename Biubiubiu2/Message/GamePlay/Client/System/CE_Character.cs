using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientMessage {
    public class CE_Character {        
        public struct JumpTypeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<JumpTypeChange>();
            public int GetID() => _id;

            // 下面写你的参数
            public CharacterData.JumpType JumpType;
        }

        public struct FlyTypeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<FlyTypeChange>();
            public int GetID() => _id;

            // 下面写你的参数
            public CharacterData.FlyType FlyType;
        }

        public struct UseAerocraft : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UseAerocraft>();
            public int GetID() => _id;

            // 下面写你的参数
            public string useAerocraft;
        }

        public struct UsePackAerocraft : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UsePackAerocraft>();
            public int GetID() => _id;

            // 下面写你的参数
            public string useAerocraft;
        }

        public struct GetUseAerocraft : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetUseAerocraft>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<string> CallBack;
        }

        public struct GetUsePackAerocraft : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetUsePackAerocraft>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<string> CallBack;
        }

        public struct Dodga : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Dodga>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsDodge;
        }

        public struct Fall : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Fall>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FallValue;
        }

        public struct IsGround : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<IsGround>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
        }

        public struct GroundDistance : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GroundDistance>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
            public float GroundDis;
        }

        public struct FallToGrounded : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<FallToGrounded>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct GetWeaponList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetWeaponList>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<IWeapon>> CallBack;
        }

        public struct UpdateWeaponList : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<UpdateWeaponList>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<IWeapon> Weapons;
        }

        public struct HoldOn : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<HoldOn>();
            public int GetID() => _id;

            // 下面写你的参数
            public string HoldOnSign;
        }

        public struct TakeOnMonster : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<TakeOnMonster>();
            public int GetID() => _id;

            // 下面写你的参数
            public string HoldOnSign;
        }

        public struct AutoFire : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AutoFire>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
        }

        public struct Throw : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Throw>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct StopThrow : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StopThrow>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct StandTypeChange : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StandTypeChange>();
            public int GetID() => _id;

            // 下面写你的参数
            public CharacterData.StandType StandType;
        }

        public struct CameraRotaV : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<CameraRotaV>();
            public int GetID() => _id;

            // 下面写你的参数
            public float vRota;
        }

        public struct IsLookForward : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<IsLookForward>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
        }

        public struct PlatformMovement : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlatformMovement>();
            public int GetID() => _id;

            // 下面写你的参数
            public int elementId;
            public Vector3 point;
        }

        public struct AnimatorLoadEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<AnimatorLoadEnd>();

            public int GetID() => _id;
            // 下面写你的参数
        }

        public struct GetAnimatorLoadEnd : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<GetAnimatorLoadEnd>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<bool> CallBack;
        }

        public struct PlayAttackAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayAttackAnim>();
            public int GetID() => _id;

            // 下面写你的参数
            public int PlayAnimId;
        }

        public struct PlayAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayAnim>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AnimId;
            public Action<EntityAnimConfig.StateData> PlayEndCallBack;
        }

        public struct PlayAnimSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<PlayAnimSign>();
            public int GetID() => _id;

            // 下面写你的参数
            public string PlaySign;
            public Action<EntityAnimConfig.StateData> PlayEndCallBack;
        }

        public struct SetAnimGroupSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<SetAnimGroupSign>();
            public int GetID() => _id;

            // 下面写你的参数
            public string GroupSign;
        }

        public struct StopAnim : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StopAnim>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AnimId;
        }

        public struct StopAnimSign : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<StopAnimSign>();
            public int GetID() => _id;

            // 下面写你的参数
            public string PlaySign;
        }
        public struct FlyMoveData : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<FlyMoveData>();
            public int GetID() => _id;

            // 下面写你的参数
            public float FlySpeed;
            public float FlyWeight;
        }

        // GPO 击退移动
        public struct Event_KnockbackMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_KnockbackMovePoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 KnockbackMove;
        }


        // GPO 击飞
        public struct Event_AttackMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_AttackMovePoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 AttackMove;
        }

        // GPO 击飞
        public struct Event_StrikeFlyMovePoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StrikeFlyMovePoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 StrikeFlyMove;
        }

        // GPO 击飞
        public struct Event_StopDefaultMoveForAttack : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StopDefaultMoveForAttack>();
            public int GetID() => _id;

            // 下面写你的参数
            public bool IsTrue;
        }

        /// 获取所有物品
        public struct Event_GetItems : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetItems>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<ItemData.PickItemData>> CallBack;
        }

        /// 获取快捷使用物品
        public struct Event_GetQuickItems : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetQuickItems>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<List<ItemData.PickItemData>> CallBack;
        }

        /// 装备物品
        public struct Event_EquipItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_EquipItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        /// 使用物品
        public struct Event_UseAbilityItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UseAbilityItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        /// 更新物品
        public struct Event_UpDateItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpDateItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ItemData.PickItemData> PickItemList;
        }

        /// 更新物品
        public struct Event_UpDateQuickItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpDateQuickItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public List<ItemData.PickItemData> PickItemList;
        }

        public struct Event_TakeBackWeapon : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TakeBackWeapon>();
            public int GetID() => _id;

            // 下面写你的参数
            public IWeapon iWeapon;
        }

        public struct Event_TakeBackQuickItem : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TakeBackQuickItem>();
            public int GetID() => _id;

            // 下面写你的参数
            public int AutoItemId;
        }

        public struct Event_TakeBackAerocraft : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_TakeBackAerocraft>();
            public int GetID() => _id;

            // 下面写你的参数
        }

        public struct Event_UpdateWeight : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_UpdateWeight>();
            public int GetID() => _id;

            // 下面写你的参数
            public float Weight;
            public float MaxWeight;
        }

        /// 获取所有物品
        public struct Event_GetWeight : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetWeight>();
            public int GetID() => _id;

            // 下面写你的参数
            public Action<float, float> CallBack;
        }
        // 翔虫位移
        public struct Event_WirebugMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_WirebugMove>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 WirebugMove;
        }    
        // 开始滑产
        public struct Event_StartSlideMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_StartSlideMove>();
            public int GetID() => _id;

            // 下面写你的参数
        }   
        // 滑产
        public struct Event_SlideMove : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SlideMove>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 SlideVelocity;
            public bool IsSlide;
        }   
        
        // 翔虫位移
        public struct Event_WirebugMoveTargetPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_WirebugMoveTargetPoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public Vector3 MoveTarget;
        }     
        
        // 翔虫位移类型
        public struct Event_WirebugMoveState : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_WirebugMoveState>();
            public int GetID() => _id;

            // 下面写你的参数
            public CharacterData.WirebugType State;
        }

        /// 获取物品数据
        public struct Event_GetItemDataForItemId : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_GetItemDataForItemId>();
            public int GetID() => _id;

            // 下面写你的参数
            public int ItemId;
            public Action<ItemData.PickItemData> CallBack;
        }
        
        public struct Event_SetLineRenderStartEndPoint : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetLineRenderStartEndPoint>();
            public int GetID() => _id;

            // 下面写你的参数
            public string LineRenderName;
            public Vector3 StartPoint;
            public Vector3 EndPoint;
            public float AddHeight;
            public int Count;
            public Action<Vector3[]> CallBack;
        }

        public struct Event_SetLineRenderPoints : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_SetLineRenderPoints>();
            public int GetID() => _id;

            // 下面写你的参数
            public string LineRenderName;
            public Vector3[] Points;
            public int Count;
        }
        public struct Event_HideLineRender : GamePlayEvent.ISystemEvent {
            private static readonly int _id = GamePlayEvent.ReadonlySystemEventID<Event_HideLineRender>();
            public int GetID() => _id;

            // 下面写你的参数
            public string LineRenderName;
        }
    }
}