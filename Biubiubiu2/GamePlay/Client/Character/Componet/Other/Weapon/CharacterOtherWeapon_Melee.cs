using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherWeapon_Melee : ClientGPOWeapon_Melee {
        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Weapon.Rpc_AttackAnim.ID, OnRPCAttackAnimCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Weapon.Rpc_AttackAnim.ID, OnRPCAttackAnimCallBack);
        }

        public void OnRPCAttackAnimCallBack(INetwork network, IProto_Doc doc) {
            var data = (Proto_Weapon.Rpc_AttackAnim)doc;
            PlayAttackAnim(data.attackAnim);
        }
    }
}