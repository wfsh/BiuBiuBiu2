using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterWeapon_Melee : ServerGPOWeapon_Melee {
        protected override void OnAwake() {
        }
        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Weapon.Cmd_BatLoopAttack.ID, OnBatLoopAttackCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_BatEndAttack.ID, OnBatEndAttackCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_PlungerAttack.ID, OnPlungerAttackCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_PlungerEnd.ID, OnPlungerEndCallBack);
            AddProtoCallBack(Proto_Weapon.Cmd_AttackAnim.ID, OnAttackAnimCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            this.weapon.Dispatcher(new SE_Weapon.Event_PutOn {
                IsTrue = false,
            });
            this.weapon = null;
            RemoveProtoCallBack(Proto_Weapon.Cmd_BatLoopAttack.ID, OnBatLoopAttackCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_BatEndAttack.ID, OnBatEndAttackCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_PlungerAttack.ID, OnPlungerAttackCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_PlungerEnd.ID, OnPlungerEndCallBack);
            RemoveProtoCallBack(Proto_Weapon.Cmd_AttackAnim.ID, OnAttackAnimCallBack);
        }

        private void OnBatLoopAttackCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_BatLoopAttack)cmdData;
            this.weapon.Dispatcher(new SE_Weapon.Event_BatLoopAttack {
                IsAttack = data.IsAttack,
            });
        }

        private void OnBatEndAttackCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_BatEndAttack)cmdData;
            this.weapon.Dispatcher(new SE_Weapon.Event_BatEndAttack {
                StartRota = data.startRota, StartPoint = data.startPoint,
            });
        }

        private void OnPlungerAttackCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_PlungerAttack)cmdData;
            this.weapon.Dispatcher(new SE_Weapon.Event_PlungerAttack {
                StartRota = data.startRota,
            });
        }

        private void OnPlungerEndCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_PlungerEnd)cmdData;
            this.weapon.Dispatcher(new SE_Weapon.Event_PlungerEnd {
                StartRota = data.startRota, StartPoint = data.startPoint,
            });
        }

        private void OnAttackAnimCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Weapon.Cmd_AttackAnim)cmdData;
            Rpc(new Proto_Weapon.Rpc_AttackAnim {
                attackAnim = data.attackAnimId,
            });
        }
    }
}