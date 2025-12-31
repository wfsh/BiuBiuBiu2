using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    // 直线匀速移动
    public class ClientAIMoveForDrive : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public float MoveSpeed;
            public float MoveRotaSpeed;
        }
        private const float gravitey = 1f; // 影响下落速度和跳跃高度 默认 1 倍重力
        private float moveSpeed = 4f; // 步行速度
        private CharacterController controller;
        private EntityBase entity;
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 moveDir = Vector3.zero;
        private RaycastHit[] raycastHit;
        private float groundDis = 0f;
        private bool isGround = false;
        private bool isDriveIng = false;
        private bool isEnabledDriveMoveIng = false;
        private float abiityMoveSpeed = 0f;
        private float checkGroundDeltaTime = 0.0f;
        private C_AI_Base aiSystem;
        private Vector3 moveDirection;
        private float moveAngle;
        private Vector3 cameraForward = Vector3.zero;
        private bool isMove = false;
        private AudioPoolManager.PlayAudioData playAudioData;
        private bool isLoadAudio = false;
        private float checkTime = 0f;

        protected override void OnAwake() {
            mySystem.Register<CE_AI.Event_MoveDir>(OnMoveCallBack);
            mySystem.Register<CE_GPO.Event_DriveGPO>(OnDriveIngCallBack);
            mySystem.Register<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
            var initData = (InitData)initDataBase;
            this.moveSpeed = initData.MoveSpeed;
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (C_AI_Base)mySystem;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            GetCharacterController();
            AddUpdate(OnUpdate);
            raycastHit = new RaycastHit[10];
            PlayMoveAudio();
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_AI.Event_MoveDir>(OnMoveCallBack);
            mySystem.Unregister<CE_GPO.Event_DriveGPO>(OnDriveIngCallBack);
            mySystem.Unregister<CE_AI.Event_EnabledDriveMove>(OnEnabledDriveMoveCallBack);
            controller = null;
            raycastHit = null;
            aiSystem = null;
            StopMoveAudio();
        }

        private void OnUpdate(float delta) {
            if (isDriveIng == false) {
                return;
            }
            GetCameraForward();
            MoveRota(delta);
            if (isEnabledDriveMoveIng) {
                MovePoint(delta);
                CheckGrounded();
            }
        }

        private void GetCharacterController() {
            controller = entity.GetComponent<CharacterController>();
            controller.enabled = isEnabledDriveMoveIng;
            if (controller == null) {
                Debug.LogError("缺少移动组件 CharacterController");
            }
        }

        public void OnMoveCallBack(ISystemMsg body, CE_AI.Event_MoveDir ent) {
            if (isEnabledDriveMoveIng == false) {
                return;
            }
            moveDir.x = ent.MoveX;
            moveDir.z = ent.MoveZ;
            if (moveDir.z < 0) {
                moveType = AIData.MoveType.BackMove;
            } else {
                moveType = Mathf.Abs(moveDir.z) + Mathf.Abs(moveDir.x) > 0.6f ? AIData.MoveType.Run : AIData.MoveType.Walk;
            }
            moveDirection = new Vector3(ent.MoveX, 0, ent.MoveZ).normalized;
            CheckIsMove();
        }

        private void CheckIsMove() {
            if (moveDir.x != 0f || moveDir.z != 0f) {
                isMove = true;
                PlayMoveAudio();
            } else {
                isMove = false;
                StopMoveAudio();
            }
        }

        private void PlayMoveAudio() {
            if (isMove == false || playAudioData != null || entity == null || isLoadAudio || ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver || ModeData.PlayGameState == ModeData.GameStateEnum.QuitApp) {
                return;
            }
            isLoadAudio = true;
            AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("TP_Tank_Mov"), iGPO.GetPoint(), data => {
                playAudioData = data;
                if (isMove == false) {
                    StopMoveAudio();
                }
            });
        }

        private void StopMoveAudio() {
            if (playAudioData == null) {
                return;
            }
            isLoadAudio = false;
            playAudioData.Stop();
            playAudioData = null;
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_DriveGPO ent) {
            if (ent.PlayerDriveGPO == null) {
                isDriveIng = false;
            } else {
                isDriveIng = ent.PlayerDriveGPO.GetGPOType() == GPOData.GPOType.Role;
            }
        }

        private void OnEnabledDriveMoveCallBack(ISystemMsg body, CE_AI.Event_EnabledDriveMove ent) {
            isEnabledDriveMoveIng = ent.IsTrue;
            if (this.controller != null) {
                this.controller.enabled = isEnabledDriveMoveIng;
            }
            if (isEnabledDriveMoveIng == false) {
                StopMove();
            }
        }

        private void StopMove() {
            moveDir.x = 0;
            moveDir.z = 0;
        }

        private void GetCameraForward() {
            MsgRegister.Dispatcher(new CM_Camera.GetCameraForward {
                CallBack = (forward) => {
                    cameraForward = forward;
                }
            });
        }

        private void MovePoint(float deltaTime) {
            if (moveDirection.magnitude <= 0.1f) {
                UpdateFallByIdle(deltaTime);
                return;
            }
            FallMove(deltaTime, 15f);
            var speed = GetSpeed();
            var forwardMove = iEntity.GetForward() * speed;
            if (moveType == AIData.MoveType.BackMove) {
                forwardMove = -forwardMove;
            }
            forwardMove.y = moveDir.y;
            this.controller.Move(forwardMove * deltaTime);
        }

        /// <summary>
        /// 根据目标朝向进行旋转
        /// </summary>
        private void MoveRota(float deltaTime) {
            if (moveDirection.magnitude <= 0.1f) {
                return;
            }
            var dir = moveDir;
            if (moveType == AIData.MoveType.BackMove) {
                dir = -dir;
            }
            moveAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            var rotaSpeed = 2f;
            var targetRotation = Quaternion.identity;
            targetRotation = Quaternion.Euler(0, moveAngle, 0) * cameraForwardRotation();
            iEntity.SetRota(Quaternion.Slerp(iEntity.GetRota(), targetRotation, rotaSpeed * deltaTime));
        }
        
        private Quaternion cameraForwardRotation() {
            cameraForward.y = 0f;
            cameraForward.Normalize();
            return Quaternion.LookRotation(cameraForward, Vector3.up);
        }
        
        private float GetSpeed() {
            if (moveType == AIData.MoveType.BackMove) {
                return moveSpeed * 0.5f;
            }
            return moveSpeed;
        }

        private void FallMove(float deltaTime, float weight) {
            moveDir.y -= gravitey * 15 * deltaTime;
            var maxDownGravitey = -weight * gravitey;
            if (groundDis >= 0f && groundDis <= 1f) {
                maxDownGravitey *= groundDis;
            }
            if (moveDir.y <= maxDownGravitey) {
                moveDir.y = maxDownGravitey;
            }
        }

        private void CheckGrounded() {
            if (this.controller.isGrounded) {
                IsGround(true, 0f);
                return;
            }
            if (this.groundDis < 0f || Time.realtimeSinceStartup - checkGroundDeltaTime >= 0.2f) {
                checkGroundDeltaTime = Time.realtimeSinceStartup;
                var point = this.iEntity.GetPoint() + Vector3.up * 0.2f;
                var count = Physics.RaycastNonAlloc(point, Vector3.down, raycastHit, 1f);
                this.groundDis = -1f;
                GetGoundData(count, point, raycastHit);
            }
            IsGround(this.groundDis >= 0f && this.groundDis <= 0.5f, this.groundDis);
        }

        private void GetGoundData(int count, Vector3 point, RaycastHit[] list) {
            for (int i = 0; i < count; i++) {
                var ray = list[i];
                if (ray.collider == null || ray.collider.isTrigger) {
                    continue;
                }
                var dis = Vector3.Distance(point, ray.point);
                if (groundDis < 0f || groundDis > dis) {
                    groundDis = dis;
                }
            }
        }

        public void IsGround(bool isTrue, float groundDis) {
            this.groundDis = groundDis;
            if (isGround != isTrue) {
                isGround = isTrue;
            }
        }

        private void UpdateFallByIdle(float deltaTime) {
            if (controller.isGrounded) {
                checkTime += deltaTime;
                if (checkTime < 0.3f) {
                    return;
                }
                checkTime = 0;
                controller.Move(Vector3.zero);
            } else {
                FallMove(deltaTime, 15f);
                controller.Move(new Vector3(0, moveDir.y, 0) * deltaTime);
            }
        }
    }
}