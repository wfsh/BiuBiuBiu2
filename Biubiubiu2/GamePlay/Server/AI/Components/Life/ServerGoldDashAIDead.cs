using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashAIDead : ServerAIDead {
        protected override byte GetDeadRowId() {
            return AbilityM_PlayEffect.ID_Char_XCC_Fuhuo;
        }
    }
}