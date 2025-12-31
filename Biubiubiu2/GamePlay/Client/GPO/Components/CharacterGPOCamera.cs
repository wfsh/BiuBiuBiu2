using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterGPOCamera : ComponentBase {
        protected bool isDriveIng = false;
        private IGPO lookGpo;
        private IGPO driveGPO;
        protected override void OnAwake() {
            mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveGPOCallBack);
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveGPOCallBack);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            lookGpo = null;
        }
        
        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            lookGpo = ent.LookGPO;
            if (driveGPO != null) {
                UpdateDriveState(driveGPO.GetGpoID());
            }
        }
        
        private void OnDriveGPOCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO ent) {
            isDriveIng = ent.DriveGPO != null;
            var driveGpoId = driveGPO != null ? driveGPO.GetGpoID() : 0;
            driveGPO = ent.DriveGPO;
            if (lookGpo == null) {
                return;
            }
            UpdateDriveState(driveGpoId);
        }

        private void UpdateDriveState(int driveGpoId) {
            if (lookGpo.GetGpoID() == GpoID || lookGpo.GetGpoID() == driveGpoId) {
                if (isDriveIng) {
                    if (lookGpo.GetGpoID() != driveGpoId) {
                        MsgRegister.Dispatcher(new CM_GPO.AddLookGPO {
                            LookGPO = driveGPO,
                            IsDrive = true,
                        });
                    }
                } else {
                    if (lookGpo.GetGpoID() != GpoID) {
                        MsgRegister.Dispatcher(new CM_GPO.AddLookGPO {
                            LookGPO = iGPO,
                            IsDrive = false,
                        });
                        var rota = iEntity.GetEulerAngles();
                        rota.x = 0f;
                        rota.z = 0f;
                        iEntity.SetEulerAngles(rota);
                    }
                }
            }
        }
    }
}