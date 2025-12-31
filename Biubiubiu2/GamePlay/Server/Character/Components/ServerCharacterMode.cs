using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterMode : ServerCharacterComponent {
        private bool isAddBattleData = false;

        protected override void OnAwake() {
            mySystem.Register<SE_Entity.SyncPointAndRota>(OnSetMovePointCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            LoginMode();
        }

        protected override void OnClear() {
            base.OnClear();
            isAddBattleData = false;
            mySystem.Unregister<SE_Entity.SyncPointAndRota>(OnSetMovePointCallBack);
            RemoveProtoCallBack(Proto_Mode.Cmd_SendSaveBattleData.ID, OnSendSaveBattleDataCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Mode.Cmd_SendSaveBattleData.ID, OnSendSaveBattleDataCallBack);
        }

        private void OnSendSaveBattleDataCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            if (isAddBattleData) {
                return;
            }
            isAddBattleData = true;
            var data = (Proto_Mode.Cmd_SendSaveBattleData)cmdData;
            MsgRegister.Dispatcher(new SM_Mode.Event_GetSaveBattleData {
                ByteDatas = data.byteDatas,
                CharacterGPO = iGPO
            });
        }

        public void LoginMode() {
            MsgRegister.Dispatcher(new SM_Mode.AddCharacter {
                PlayerId = characterSystem.PlayerId,
                NickName = characterSystem.NickName,
                CharacterGPO = iGPO
            });
            MsgRegister.Dispatcher(new SM_Mode.GetStartPoint {
                CallBack = SetStartPoints,
                CharacterGPO = iGPO,
            });
        }
        
        private void OnSetMovePointCallBack(ISystemMsg body, SE_Entity.SyncPointAndRota ent) {
            SetStartPoints(ent.Point, ent.Rota);
        }

        public void SetStartPoints(Vector3 startPoint, Quaternion startRota) {
            iEntity.SetPoint(startPoint);
            iEntity.SetRota(startRota);
            characterNetwork?.SetPoint(startPoint);;
            characterNetwork?.SetRota(startRota);
            TargetRpc(networkBase, new Proto_Character.TargetRpc_MovePoint() {
                Point = startPoint,
                Rotation = startRota,
            });
        }
    }
}