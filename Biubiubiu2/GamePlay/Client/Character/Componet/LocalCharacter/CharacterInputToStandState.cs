using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterInputToStandState : ClientCharacterComponent {
        private CharacterData.StandType standType = CharacterData.StandType.Stand;
        protected override void OnAwake() {
            MsgRegister.Register<CM_InputPlayer.Prone>(OnProneCallBack);
            MsgRegister.Register<CM_InputPlayer.Crouch>(OnCrouchCallBack);
            MsgRegister.Register<CM_InputPlayer.Jump>(OnJumpTypeCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_InputPlayer.Prone>(OnProneCallBack);
            MsgRegister.Unregister<CM_InputPlayer.Crouch>(OnCrouchCallBack);
            MsgRegister.Unregister<CM_InputPlayer.Jump>(OnJumpTypeCallBack);
        }

        private void OnJumpTypeCallBack(CM_InputPlayer.Jump ent) {
            if (standType == CharacterData.StandType.Stand) {
                return;
            }
            standType = CharacterData.StandType.Stand;
            this.mySystem.Dispatcher(new CE_Character.StandTypeChange() {
                StandType = standType
            });
            Cmd(new Proto_Character.Cmd_StandType {
                standType = (CharacterData.StandType)standType,
            });
        }

        private void OnProneCallBack(CM_InputPlayer.Prone ent) {
            if (standType != CharacterData.StandType.Prone) {
                standType = CharacterData.StandType.Prone;
            } else {
                standType = CharacterData.StandType.Stand;
            }
            this.mySystem.Dispatcher(new CE_Character.StandTypeChange() {
                StandType = standType
            });
        }

        private void OnCrouchCallBack(CM_InputPlayer.Crouch ent) {
            if (standType != CharacterData.StandType.Crouch) {
                standType = CharacterData.StandType.Crouch;
            } else {
                standType = CharacterData.StandType.Stand;
            }
            this.mySystem.Dispatcher(new CE_Character.StandTypeChange() {
                StandType = standType
            });
        }
    }
}