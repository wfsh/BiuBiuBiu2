using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeGoldDashDropItem : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<SM_Mode.Event_DropItem>(OnDeadDropItemCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<SM_Mode.Event_DropItem>(OnDeadDropItemCallBack);
        }
        
        public void OnDeadDropItemCallBack(SM_Mode.Event_DropItem ent) {
            var killCount = 0;
            long playerId = 0;
            ent.AttackGPO?.Dispatcher(new SE_Character.GetPlayerId() {
                CallBack = id => {
                    playerId = id;
                }
            });
            var deadGPO = ent.DropGpo;
            var mData = deadGPO.GetMData();
            var attackList = ent.AttackGPOList;
            if (attackList == null) {
                attackList = new List<IGPO>();
            }
            
            Vector3 dropBoxPosition = ent.OR_DropPoint == Vector3.zero? deadGPO.GetPoint() : ent.OR_DropPoint;
            MsgRegister.Dispatcher(new SM_AI.Event_AICreateDropBox() {
                PlayerId = playerId,
                AIGpoMId = deadGPO.GetGpoMID(),
                AIQuality = (AIData.AIQuality)mData.GetQuality(),
                BoxId = ent.DropItemId,
                Position = dropBoxPosition,
                hurtHistoryGPOSet = attackList,
            });
        }
    }
}