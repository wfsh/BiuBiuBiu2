using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraFar : ComponentBase {
        private const float DEFAULT_LERP_RATE = 3;

        private Transform farTransform;
        private Transform cameraTransform;
        private Transform lookTransform;
        private IGPO lookGPO;
        private bool isDriveIng = false;
        private bool isHoldOn = false;
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private bool isTakeOnMonster = false;
        private float farPointY = 2f;
        private float farLerpRate = DEFAULT_LERP_RATE;
        private Vector3 targetOffset = Vector3.zero;
        private int monsterTypeId = 0;

        protected override void OnAwake() {
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            var cameraEntity = (CameraEntity)iEntity;
            farTransform = cameraEntity.Far;
            cameraTransform = cameraEntity.UseCamera.transform;
            SyncCameraFarPoint();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            ClearLookGPO();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            farTransform = null;
            lookTransform = null;
            cameraTransform = null;
        }

        //更新当前的怪物类型
        private void UpdateMonsterType() {
            if (lookGPO.GetGPOType() == GPOData.GPOType.MasterAI) {
                monsterTypeId = lookGPO.GetGpoMTypeID();
                Debug.LogError("aiTypeId:" + monsterTypeId);
            } else {
                monsterTypeId = 0;
            }
        }

        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            ClearLookGPO();
            lookGPO = ent.LookGPO;
            isDriveIng = ent.IsDrive;

            UpdateMonsterType();
            moveType = AIData.MoveType.Stand;
            lookGPO.Register<CE_Character.HoldOn>(SetHoldOnSign);
            lookGPO.Register<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            lookGPO.Register<CE_AI.Event_MoveDir>(OnMoveCallBack);
            lookGPO.Register<Event_SystemBase.SetEntityObj>(OnEntityObjCallBack);
            lookGPO.Register<CE_Camera.SetCameraOffset>(OnSetCameraDistance);
            GetLookGPOBody();
        }

        private void OnEntityObjCallBack(ISystemMsg body, Event_SystemBase.SetEntityObj ent) {
            GetLookGPOBody();
        }

        private void OnSetCameraDistance(ISystemMsg body, CE_Camera.SetCameraOffset ent) {
            targetOffset = -ent.Offset;
            farLerpRate = ent.LerpRate == 0 ? DEFAULT_LERP_RATE : ent.LerpRate;
        }

        private void GetLookGPOBody() {
            if (lookGPO.IsClear()) {
                return;
            }

            lookTransform = lookGPO.GetBodyTran(GPOData.PartEnum.Head);
            if (lookTransform == null) {
                lookTransform = lookGPO.GetBodyTran(GPOData.PartEnum.Body);
            }

            SyncCameraFarPoint();
        }

        private void ClearLookGPO() {
            if (lookGPO == null) {
                return;
            }

            mySystem.Unregister<CE_AI.Event_MoveDir>(OnMoveCallBack);
            lookGPO.Unregister<CE_Character.HoldOn>(SetHoldOnSign);
            lookGPO.Unregister<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            lookGPO.Unregister<Event_SystemBase.SetEntityObj>(OnEntityObjCallBack);
            lookGPO.Unregister<CE_Camera.SetCameraOffset>(OnSetCameraDistance);
            lookGPO = null;
        }

        public void OnMoveCallBack(ISystemMsg body, CE_AI.Event_MoveDir ent) {
            if (isDriveIng == false) {
                return;
            }

            if (ent.MoveZ < 0) {
                moveType = AIData.MoveType.BackMove;
            } else if (ent.MoveZ > 0) {
                moveType = ent.MoveZ > 0.5f ? AIData.MoveType.Run : AIData.MoveType.Walk;
            } else {
                moveType = AIData.MoveType.Stand;
            }
        }

        public void SetHoldOnSign(ISystemMsg body, CE_Character.HoldOn envData) {
            isHoldOn = envData.HoldOnSign != "";
            SyncCameraFarPoint();
        }

        public void SetTakeOnMonsterSign(ISystemMsg body, CE_Character.TakeOnMonster envData) {
            isTakeOnMonster = envData.HoldOnSign != "";
            SyncCameraFarPoint();
        }

        private void SyncCameraFarPoint() {
            if (farTransform == null) {
                return;
            }

            var point = GetCameraFarPoint();
            SetFarPoint(point);
        }

        private Vector3 GetCameraFarPoint() {
            var point = new Vector3();
            if (isDriveIng) {
                switch (monsterTypeId) {
                    case GpoSet.Id_MachineGun:
                        point.x = 0f;
                        break;
                    default:
                        point.x = 1f;
                        break;
                }

                point.y = 0f;
                switch (moveType) {
                    case AIData.MoveType.Run:
                        point.z = -5f;
                        break;
                    case AIData.MoveType.Walk:
                        point.z = -4.5f;
                        break;
                    case AIData.MoveType.BackMove:
                        point.z = -3f;
                        break;
                    default:
                        point.z = -4f;
                        break;
                }
            } else if (isTakeOnMonster) {
                point.x = 1f;
                point.y = 0f;
                point.z = -1.8f;
            } else {
                point.x = 0.7f;
                point.y = 0.6f;
                point.z = -5.25f;
                point += targetOffset;
            }

            var ratio = CheckAddFarPointY();
            var addY = farPointY * (1f - ratio);
            point.y += addY;
            return point;
        }

        private void OnUpdate(float delta) {
            if (lookTransform == null || farTransform == null) {
                return;
            }

            var targetPoint = GetCameraFarPoint();
            SetFarPoint(Vector3.Slerp(farTransform.localPosition, targetPoint, farLerpRate * Time.deltaTime));
        }

        private void SetFarPoint(Vector3 point) {
            farTransform.localPosition = point;
            lookGPO?.Dispatcher(new CE_Camera.EndFarPoint {
                FarPoint = point,
            });
        }

        private float CheckAddFarPointY() {
            if (lookTransform == null || farTransform == null) {
                return 1f;
            }

            var lookTranPoint = lookTransform.position;
            var farDistance = Vector3.Distance(farTransform.position, lookTranPoint);
            var cameraDistance = Vector3.Distance(cameraTransform.position, lookTranPoint);
            var ratio = Mathf.Min(cameraDistance / farDistance, 1f);
            return ratio;
        }
    }
}