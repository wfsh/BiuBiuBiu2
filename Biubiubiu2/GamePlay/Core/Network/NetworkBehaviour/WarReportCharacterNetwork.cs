using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class WarReportCharacterNetwork : WarReportNetworkBase, INetworkCharacter {
        override protected void OnStart() {
            MsgRegister.Dispatcher(new M_Character.SetNetwork {
                iNetwork = this,
            });
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Dispatcher(new M_Character.DestoryNetwork {
                NetworkId = GetNetworkId(),
            });
        }

        public void SetPoint(Vector3 point) {
            if (isDestroy) {
                return;
            }
            transform.position = point;
        }

        public void SetRota(Quaternion rota) {
            if (isDestroy) {
                return;
            }
            transform.rotation = rota;
        }

        public Vector3 GetPoint() {
            return transform.position;
        }

        public Quaternion GetRota() {
            return transform.rotation;
        }

        public bool IsLocalPlayer() {
            return false;
        }

        public void Cmd(ICmd proto) {
        }
        public void SetInterpolatePositionState(bool state) {
        }
        
        public void SetCharacterReady(bool ready) {
        }

        public bool IsCharacterReady() {
            return true;
        }
    }
}