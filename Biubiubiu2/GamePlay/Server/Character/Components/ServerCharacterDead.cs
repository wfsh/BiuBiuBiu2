using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterDead : ServerGPODead {
        protected override void OnAddModeScore() {
            var lastAttackGPOID = attackGPO.GetGpoID();
            MsgRegister.Dispatcher(new SM_Mode.Event_ModeMessage {
                mainGpoId = lastAttackGPOID,
                subGpoId = iGPO.GetGpoID(),
                ItemId = attackWeaponId,
                MessageState = killRoleMessage,
            });

            MsgRegister.Dispatcher(new SM_Mode.AddScore {
                Channel = ModeData.GetScoreChannelEnum.KillRole, GpoId = lastAttackGPOID,AttackItemId = attackWeaponId
            });
        }
        protected override byte GetDeadRowId() {
            return AbilityM_PlayEffect.ID_Char_XCC_Fuhuo;
        }
    }
}