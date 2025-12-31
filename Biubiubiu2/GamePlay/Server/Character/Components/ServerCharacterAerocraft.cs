using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterAerocraft : ServerCharacterComponent {
        public string baseAerocraft = "";
        public string useAerocraft = "";
        public string monsterAerocraft = "";

        protected override void OnAwake() {
            this.mySystem.Register<SE_GPO.Event_SetMonsterAerocraftSign>(OnSetMonsterAerocraftSignCallBack);
            this.mySystem.Register<SE_GPO.Event_SetPackAerocraftSign>(OnSetPackAerocraftSignCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            baseAerocraft = "";
            useAerocraft = "";
            monsterAerocraft = "";
            RemoveProtoCallBack(Proto_Character.Cmd_TakeBackAerocraft.ID, OnTakeBackAerocraftCallBack);
            this.mySystem.Unregister<SE_GPO.Event_SetMonsterAerocraftSign>(OnSetMonsterAerocraftSignCallBack);
            this.mySystem.Unregister<SE_GPO.Event_SetPackAerocraftSign>(OnSetPackAerocraftSignCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.Cmd_TakeBackAerocraft.ID, OnTakeBackAerocraftCallBack);
            if (useAerocraft != "") {
                UseAerocraft();
            }
        }

        protected override void Sync(List<INetworkCharacter> networks) {
            TargetRpcList(networks, new Proto_Character.TargetRpc_UseAerocraft {
                aerocraftSign = useAerocraft,
            });
            TargetRpcList(networks, new Proto_Character.TargetRpc_UsePackAerocraft {
                aerocraftSign = baseAerocraft,
            });
        }

        private void OnTakeBackAerocraftCallBack(INetwork iNetwork, IProto_Doc protoDoc) {
            if (baseAerocraft == "") {
                return;
            }
            var sign = baseAerocraft;
            SetPackAerocraft("");
            //将飞高高重新转换成物品
            MsgRegister.Dispatcher(new SM_Item.Event_AddOwnItem {
                ItemSign = sign, ItemNum = 1, OwnGPO = iGPO,
            });
        }

        private void OnSetMonsterAerocraftSignCallBack(ISystemMsg body, SE_GPO.Event_SetMonsterAerocraftSign ent) {
            monsterAerocraft = ent.AerocraftSign;
            UseAerocraft();
        }

        private void OnSetPackAerocraftSignCallBack(ISystemMsg body, SE_GPO.Event_SetPackAerocraftSign ent) {
            SetPackAerocraft(ent.AerocraftSign);
        }

        private void SetPackAerocraft(string aerocraft) {
            baseAerocraft = aerocraft;
            TargetRpc(networkBase, new Proto_Character.TargetRpc_UsePackAerocraft {
                aerocraftSign = baseAerocraft,
            });
            UseAerocraft();
        }

        private void UseAerocraft() {
            var aerocraft = "";
            if (monsterAerocraft != "") {
                aerocraft = monsterAerocraft;
            } else {
                aerocraft = baseAerocraft;
            }
            if (aerocraft == useAerocraft) {
                return;
            }
            useAerocraft = aerocraft;
            Rpc(new Proto_Character.Rpc_UseAerocraft {
                aerocraftSign = useAerocraft,
            });
        }
    }
}