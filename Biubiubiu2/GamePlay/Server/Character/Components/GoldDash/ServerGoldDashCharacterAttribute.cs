using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashCharacterAttribute : ServerGPOAttribute {
        private long playerId;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Character.GetPlayerId>(OnGetPlayerIdCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            var characterSystem = (S_Character_Base)mySystem;
            playerId = characterSystem.PlayerId;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Character.GetPlayerId>(OnGetPlayerIdCallBack);
        }

        protected override void OnUpHp(int upHp) {
            base.OnUpHp(upHp);
            MsgRegister.Dispatcher(new SM_Sausage.SausageUpHp {
                PlayerId = playerId,
                UpHp = upHp,
            });
        }

        protected override GPOData.AttributeData CreateAttribute() {
            var data = new GPOData.AttributeData();
            data.Level = 1;
            if (WarData.TestMaxHpInput > 0) {
                data.maxHp = WarData.TestMaxHpInput;
            } else {
                data.maxHp = 1000;
            }
            data.nowHp = data.maxHp;
            data.ATK = 0;
            data.AttackRange = 1f;
            return data;
        }

        private void OnGetPlayerIdCallBack(ISystemMsg body, SE_Character.GetPlayerId ent) {
            var serverCharacter = (S_Character_Base)mySystem;
            ent.CallBack?.Invoke(serverCharacter.PlayerId);
        }
    }
}