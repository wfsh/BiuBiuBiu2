using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 本地操控直升机
    /// </summary>
    public class ClientAIHelicopterMoveForDrive : ComponentBase {
        private CharacterController controller;
        private EntityBase entity;
        private Vector3 moveDir = Vector3.zero;
        private bool isDriveIng = false;
        private bool isEnabledDriveMoveIng = false;
        private Vector3 cameraForward = Vector3.zero;
        private C_AI_Base aiSystem;
        private GPOM_Helicopter useMData;
        private float moveSpeed = 0f; // 跑步速度
        private float rotaSpeed = 0f;

        [Header("Movement Settings")]
        public float ascendSpeed = 5f; // 垂直升降速度
        private float groundHeight = 0f; // 地面高度，初始化时设定
        private bool isMoveUp = false;
        private bool isMoveDown = false;
        private RaycastHit[] hitBuffer = new RaycastHit[10]; // 预分配 10 个碰撞体存储
        private float deltaGetHeight = 0f;

        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_MoveUp>(OnMoveUpCallBack);
            mySystem.Register<CE_AI.Event_MoveDown>(OnMoveDownCallBack);
            mySystem.Register<CE_AI.Event_MoveDir>(OnMoveCallBack);
            mySystem.Register<CE_GPO.Event_DriveGPO>(OnDriveIngCallBack);
            mySystem.Register<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (C_AI_Base)mySystem;
            useMData = (GPOM_Helicopter)aiSystem.MData;
            this.moveSpeed = useMData.MoveSpeed;
            this.rotaSpeed = useMData.RotaSpeed;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            GetCharacterController();
            AddUpdate(OnUpdate);
            
            
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            StopMove();
            mySystem.Unregister<CE_AI.Event_MoveUp>(OnMoveUpCallBack);
            mySystem.Unregister<CE_AI.Event_MoveDown>(OnMoveDownCallBack);
            mySystem.Unregister<CE_AI.Event_MoveDir>(OnMoveCallBack);
            mySystem.Unregister<CE_GPO.Event_DriveGPO>(OnDriveIngCallBack);
            mySystem.Unregister<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
            controller = null;
            aiSystem = null;
        }

        private void OnUpdate(float delta) {
            if (!isDriveIng) {
                return;
            }
            GetCameraForward();
            if (isEnabledDriveMoveIng) {
                HandleMovement(delta);
                UpdateHelicopterRotation(delta);
            }
        }

        private void GetCharacterController() {
            controller = entity.GetComponent<CharacterController>();
            controller.enabled = isEnabledDriveMoveIng;
        }

        public void OnMoveCallBack(ISystemMsg body, CE_AI.Event_MoveDir ent) {
            if (!isEnabledDriveMoveIng) {
                return;
            }
            moveDir.x = ent.MoveX;
            moveDir.z = ent.MoveZ;
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_DriveGPO ent) {
            isDriveIng = ent.PlayerDriveGPO?.GetGPOType() == GPOData.GPOType.Role;
        }

        private void OnMoveUpCallBack(ISystemMsg body, CE_AI.Event_MoveUp ent) {
            isMoveUp = ent.IsTrue;
        }

        private void OnMoveDownCallBack(ISystemMsg body, CE_AI.Event_MoveDown ent) {
            isMoveDown = ent.IsTrue;
        }

        private void OnEnabledDriveMoveCallBack(ISystemMsg body, CE_AI.Event_EnabledDriveMove ent) {
            isEnabledDriveMoveIng = ent.IsTrue;
            if (controller != null) {
                controller.enabled = isEnabledDriveMoveIng;
            }
            if (!isEnabledDriveMoveIng) {
                StopMove();
            } else {
                // MsgRegister.Register<CM_Camera.CameraHVRota>(OnCameraHVRotaCallBack);
            }
        }
        
        private void OnCameraHVRotaCallBack(CM_Camera.CameraHVRota ent) {
            // GetCameraForward();
            // UpdateHelicopterRotation(Time.deltaTime);
        }

        private void StopMove() {
            moveDir = Vector3.zero;
            // MsgRegister.Unregister<CM_Camera.CameraHVRota>(OnCameraHVRotaCallBack);
        }

        private void GetCameraForward() {
            MsgRegister.Dispatcher(new CM_Camera.GetCameraForward {
                CallBack = (forward) => {
                    cameraForward = forward;
                }
            });
        }

        private void HandleMovement(float delta) {
            if (deltaGetHeight > 0.5f) {
                deltaGetHeight = 0f;
                groundHeight = GetGroundHeight();
            } else {
                deltaGetHeight += delta;
            }
            // 计算相对于摄像机的移动方向
            var right = Vector3.Cross(cameraForward, Vector3.up); // 摄像机右方向
            var direction = (cameraForward * moveDir.z + right * -moveDir.x).normalized;
            var velocity = direction * moveSpeed;

            // 垂直升降控制
            var currentHeight = iEntity.GetPoint().y;
            var limitHeight = groundHeight + useMData.MaxFlyHeight;
            if (isMoveUp && currentHeight < limitHeight) {
                velocity.y += ascendSpeed;
            } else if (isMoveDown || currentHeight > (limitHeight + 1f)) {
                velocity.y -= ascendSpeed;
            } else {
                velocity.y = 0;
            }
            // 限制飞行高度
            if (currentHeight < groundHeight) {
                velocity.y = Mathf.Max(0, velocity.y);
            }
            controller.Move(velocity * delta);
        }
        
        private float GetGroundHeight() {
            var startPoint = iEntity.GetPoint();
            var targetPy = 0f;
            var distance = useMData.MaxFlyHeight + 3f;
#if UNITY_EDITOR
            Debug.DrawLine(startPoint, startPoint + Vector3.up * distance, Color.red);
#endif
            var count = Physics.RaycastNonAlloc(startPoint, Vector3.down, hitBuffer, distance, ~LayerData.ServerLayerMask);
            var groundDis = -1F;
            var groundHeight = 0f;
            for (int i = 0; i < count; i++) {
                var ray = hitBuffer[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                if (groundHeight < ray.point.y) {
                    groundHeight = ray.point.y;
                }
            }
            return groundHeight;
        }

        private void UpdateHelicopterRotation(float delta) {
            // 计算目标方向
            var targetRotation = Quaternion.LookRotation(cameraForward, Vector3.up);
            iEntity.SetRota(Quaternion.Slerp(iEntity.GetRota(), targetRotation, delta * 10f));
        }
    }
}