using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterDriveAnim : ComponentBase {
        private IGPO driveGPO = null;
        private bool isAnimatorLoadEnd = false;
        private bool isLoadAnim = false;

        protected override void OnAwake() {
            base.OnAwake();
            this.mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            ClearDirveGPO();
            this.mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveGPOCallBack);
            base.OnClear();
        }
        private void OnDriveGPOCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO ent) {
            SetDriveGPO(ent.DriveGPO);
        }

        private void SetDriveGPO(IGPO gpo) {
            if (gpo == null) {
                ClearDirveGPO();
                return;
            }
            driveGPO = gpo;
            driveGPO.Register<CE_GPO.Event_PlayAnimSign>(OnDriveGPOPlayAnimCallBack);
            mySystem.Dispatcher(new CE_Character.SetAnimGroupSign {
                GroupSign = gpo.GetSign()
            });
        }


        private void ClearDirveGPO() {
            if (driveGPO == null) {
                return;
            }
            isLoadAnim = false;
            driveGPO.Unregister<CE_GPO.Event_PlayAnimSign>(OnDriveGPOPlayAnimCallBack);
            driveGPO = null;
        }

        private void OnDriveGPOPlayAnimCallBack(ISystemMsg body, CE_GPO.Event_PlayAnimSign ent) {
            mySystem.Dispatcher(new CE_Character.PlayAnimSign {
                PlaySign = ent.AnimSign,
            });
        }
    }
}