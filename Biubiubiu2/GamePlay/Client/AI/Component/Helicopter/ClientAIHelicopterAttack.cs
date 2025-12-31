using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIHelicopterAttack : ComponentBase {
        private bool enabledDriveMove = false;
        private ClientGPO driveGPO;
        private bool isDrive = false;
        private bool isFireDown = false;
        private float sendTargetPointTime = 0.3f;
        private Transform fireTran;
        private float audioTimer = 0;
        private GPOM_Helicopter useMData;
        private C_AI_Base aiSystem;

        protected override void OnAwake() {
            Register<CE_GPO.Event_DriveGPO>(OnDriveGPOCallBack);
            Register<CE_GPO.Event_OnInputDeviceFire>(OnDeviceStartFireCallBack);
            Register<CE_GPO.Event_GetDeviceSkillCD>(OnGetDeviceSkillCDCallBack);
            Register<CE_Weapon.Event_BulletSetFireGPO>(OnBulletSetFireGPOCallBack);
            aiSystem = (C_AI_Base)mySystem;
            useMData = (GPOM_Helicopter)aiSystem.MData;
        }

        protected override void OnStart() {
            base.OnStart();
            mySystem.Dispatcher(new CE_GPO.Event_DeviceSkillCD {
                NowCD = 0f, MaxCD = 0f,
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireTran = null;
            Unregister<CE_GPO.Event_DriveGPO>(OnDriveGPOCallBack);
            Unregister<CE_GPO.Event_OnInputDeviceFire>(OnDeviceStartFireCallBack);
            Unregister<CE_GPO.Event_GetDeviceSkillCD>(OnGetDeviceSkillCDCallBack);
            Unregister<CE_Weapon.Event_BulletSetFireGPO>(OnBulletSetFireGPOCallBack);
        }

        protected override void OnSetNetwork() {
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            fireTran = iEntity.GetBodyTran(GPOData.PartEnum.AttactPoint1);
        }

        private void OnGetDeviceSkillCDCallBack(ISystemMsg body, CE_GPO.Event_GetDeviceSkillCD ent) {
            ent.CallBack(0f, 0f);
        }

        private void OnBulletSetFireGPOCallBack(ISystemMsg body, CE_Weapon.Event_BulletSetFireGPO ent) {
            if (isSetEntityObj == false || audioTimer > 0) {
                return;
            }
            audioTimer = 0.05f;
            if (driveGPO != null && driveGPO.IsLocalGPO()) {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("TP_Helicopter_Fire_Loop"), fireTran.position);
            } else {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio3P("TP_Helicopter_Fire_Loop_3P"), fireTran.position);
            }
        }

        private void OnDriveGPOCallBack(ISystemMsg body, CE_GPO.Event_DriveGPO ent) {
            driveGPO = (ClientGPO)ent.PlayerDriveGPO;
            if (driveGPO == null) {
                enabledDriveMove = false;
            } else {
                enabledDriveMove = driveGPO.IsLocalGPO();
            }
        }

        private void OnUpdate(float delta) {
            audioTimer -= delta;
            audioTimer = Mathf.Max(audioTimer, 0);
            if (enabledDriveMove == false || isSetEntityObj == false) {
                return;
            }
            if (sendTargetPointTime > 0) {
                sendTargetPointTime -= delta;
                return;
            }
            if (isFireDown == false || ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }
            sendTargetPointTime = 0.3f;
            MsgRegister.Dispatcher(new CM_Camera.GetCameraCenterObjPoint {
                CallBack = GetCameraTargetPoint,
                FarDistance = useMData.MaxAttackDistance,
                IgnoreTeamId = iGPO.GetTeamID(),
                CheckForwardPoint = fireTran.position,
            });
        }

        private void GetCameraTargetPoint(Vector3 targetPoint, bool isHit) {
            var firePoints = new Vector3[1];
            firePoints[0] = targetPoint;
            mySystem.Dispatcher(new CE_GPO.Event_OnDeviceFire {
                Points = firePoints,
            });
        }

        private void OnDeviceStartFireCallBack(ISystemMsg body, CE_GPO.Event_OnInputDeviceFire ent) {
            if (enabledDriveMove == false || isSetEntityObj == false) {
                return;
            }
            isFireDown = ent.IsDown;
        }
    }
}