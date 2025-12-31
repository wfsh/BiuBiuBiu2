using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterAIAnim : ServerNetworkComponentBase {
        private const float runThresholdAngle = 20f; // 转向多少角度内开始跑步
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private Vector3 moveDir = Vector3.zero;
        private S_AI_Base aiBase;
        private EntityAnimConfig config;
        private SausagePlayable playable;
        private EntityBase entity = null;
        private string monsterSign = "";
        private string weaponSign = "";
        private bool isUseGun = false;
        private bool isMovePoint = false;
        private int playAnim = 0;
        private int attackAnimId = 0;
        private bool isSlide = false;
        private bool isDrive = false;
        private string groupSign = "";
        private bool isInAlertStatus;
        private bool isInFightStatus;
        private bool isPlayWarReportAnim = false;

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Register<SE_GPO.Event_MovePointEnd>(OnMovePoineEndCallBack);
            mySystem.Register<SE_GPO.UseWeapon>(SetUseWeapon);
            mySystem.Register<SE_GPO.Event_JumpTypeChange>(OnJumpTypeCallBack, 1);
            mySystem.Register<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            mySystem.Register<SE_GPO.Event_SlideMove>(OnSlideMoveCallBack);
            mySystem.Register<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            mySystem.Register<SE_GPO.Event_PlayAnimId>(OnPlayAnimIdStartCallBack);
            mySystem.Register<SE_GPO.Event_PlayWarReportAnimIdStart>(OnPlayWarReportAnimIdStartCallBack);
            mySystem.Register<SE_Character.Event_SetHoldOnSign>(OnSetHoldOnSign);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            aiBase = (S_AI_Base)mySystem;
            monsterSign = aiBase.AttributeData.Sign;
            config = AIAnimConfig.Get(AIAnimConfig.HTR);
            InitPlayableGraph();
            PlayIdle();
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_GPO.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Unregister<SE_GPO.Event_MovePointEnd>(OnMovePoineEndCallBack);
            mySystem.Unregister<SE_GPO.UseWeapon>(SetUseWeapon);
            mySystem.Unregister<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            mySystem.Unregister<SE_GPO.Event_JumpTypeChange>(OnJumpTypeCallBack);
            mySystem.Unregister<SE_GPO.Event_SlideMove>(OnSlideMoveCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayerDrive>(OnOnDriveGPOCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayAnimId>(OnPlayAnimIdStartCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayWarReportAnimIdStart>(OnPlayWarReportAnimIdStartCallBack);
            mySystem.Unregister<SE_Character.Event_SetHoldOnSign>(OnSetHoldOnSign);
            playable?.Dispose();
            playable = null;
        }

        protected override ITargetRpc SyncData() {
            return new Proto_AI.TargetRpc_Anim {
                animId = (ushort)playAnim
            };
        }

        private void SetUseWeapon(ISystemMsg body, SE_GPO.UseWeapon ent) {
            var useWeapon = (S_Weapon_Base)ent.Weapon;
            if (useWeapon == null || useWeapon.GetWeaponType() != WeaponData.WeaponType.Gun) {
                if (isUseGun) {
                    StopSign(GetBlendTreeId());
                    weaponSign = "";
                }
                isUseGun = false;
            } else {
                if (isUseGun && weaponSign != useWeapon.GetWeaponSign()) {
                    StopSign(GetBlendTreeId());
                }
                isUseGun = true;
                weaponSign = useWeapon.GetWeaponSign();
                SetGroupSign(weaponSign);
                if (playable != null) {
                    ChangeRaiseGunState();
                }
            }
        }

        private void StopSign(string sign) {
            playable?.StopSign(sign);
        }

        private void SetGroupSign(string sign) {
            if (sign == "") {
                return;
            }
            groupSign = sign;
            playable?.SetGroupSign(groupSign);
        }

        public void OnSlideMoveCallBack(ISystemMsg body, SE_GPO.Event_SlideMove entData) {
            if (isSlide != entData.IsSlide) {
                isSlide = entData.IsSlide;
                if (entData.IsSlide) {
                    PlayAnim(AnimConfig_HTR.Anim_soccer_Tackle);
                } else {
                    MoveDirAction();
                }
            }
        }

        public void OnOnDriveGPOCallBack(ISystemMsg body, SE_GPO.Event_PlayerDrive ent) {
            isDrive = ent.IsDrive;
        }

        private void OnSetHoldOnSign(ISystemMsg body, SE_Character.Event_SetHoldOnSign entData) {
            if (!string.IsNullOrEmpty(entData.HoldOnSign)) {
                playable?.PlayAnimId(AnimConfig_HTR.Anim_stand_pullout_grenade_high);
            } else {
                playable?.StopAnimId(AnimConfig_HTR.Anim_stand_pullout_grenade_high);
            }
        }

        private string GetBlendTreeId() {
            return AnimConfig_HTR.Play_StandRaiseGun;
        }

        private void ChangeRaiseGunState() {
            if (isUseGun == false) {
                return;
            }
            playable?.PlayAnimSign(GetBlendTreeId());
            playable?.SetParameterValue(GetBlendTreeId(), 0f, 0f);
        }

        private void InitPlayableGraph() {
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            if (animator != null && animator.enabled) {
                playable = new SausagePlayable();
                playable.Init(entity.transform, animator, config, $"S_GPOID:{iGPO.GetGpoID()}_{monsterSign}");
                playable.CloseLoadEffect();
            }
            SetGroupSign(groupSign);
            PlayAnim(playAnim);
            if (isUseGun) {
                ChangeRaiseGunState();
            }
        }

        private void OnMovePoineEndCallBack(ISystemMsg body, SE_GPO.Event_MovePointEnd ent) {
            if (attackAnimId != 0) {
                return;
            }
            PlayIdle();
        }

        private void PlayIdle() {
            PlayAnim(AnimConfig_HTR.Anim_stand_idle_noweapon);
        }

        private void SetMoveDirCallBack(ISystemMsg body, SE_GPO.Event_MoveDir ent) {
            moveDir = ent.MoveDir;

            if (attackAnimId != 0 || isMovePoint == false) {
                return;
            }
            MoveDirAction();
        }

        public void OnIsMovePointCallBack(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            this.isMovePoint = ent.IsTrue;
            if (this.isMovePoint == false && attackAnimId == 0) {
                PlayIdle();
                return;
            }
            this.moveType = ent.MoveType;
        }

        public void OnPlayAnimIdStartCallBack(ISystemMsg body, SE_GPO.Event_PlayAnimId ent) {
            PlayAnim(ent.AnimId);
        }

        public void OnPlayWarReportAnimIdStartCallBack(ISystemMsg body, SE_GPO.Event_PlayWarReportAnimIdStart ent) {
            isPlayWarReportAnim = true;
            var animId = ent.AnimId;
            playAnim = animId;
            playable?.PlayAnimId(animId);
            PlayAnimEnd();
        }

        private void PlayAnim(int animId) {
            if (playAnim == animId || isPlayWarReportAnim) {
                return;
            }
            playAnim = animId;
            playable?.PlayAnimId(animId);
            PlayAnimEnd();
        }

        private void PlayAnimEnd() {
            mySystem.Dispatcher(new SE_GPO.Event_PlayAnimIdEnd {
                AnimId = playAnim,
            });
            Rpc(new Proto_AI.Rpc_Anim {
                animId = (ushort)playAnim,
            });
        }

        public void OnJumpTypeCallBack(ISystemMsg body, SE_GPO.Event_JumpTypeChange ent) {
            this.jumpType = ent.JumpType;
            switch (jumpType) {
                case CharacterData.JumpType.Jump:
                case CharacterData.JumpType.Fall:
                    PlayAnim(AnimConfig_HTR.Anim_jump_loop);
                    break;
                case CharacterData.JumpType.AirJump:
                    MoveDirJumpAction();
                    break;
                case CharacterData.JumpType.None:
                    MoveDirAction();
                    break;
            }
        }

        private void MoveDirJumpAction() {
            if (this.moveType == AIData.MoveType.BackMove) {
                PlayAnim(AnimConfig_HTR.Anim_jump_again_backward);
            } else {
                var angle = Vector3.Angle(iEntity.GetForward(), moveDir);
                if (angle > runThresholdAngle && angle < 160f) {
                    var crossProduct = Vector3.Cross(iEntity.GetForward(), moveDir);
                    if (crossProduct.y > 0) {
                        PlayAnim(AnimConfig_HTR.Anim_jump_again_right);
                    } else {
                        PlayAnim(AnimConfig_HTR.Anim_jump_again_left);
                    }
                } else {
                    PlayAnim(AnimConfig_HTR.Anim_jump_again);
                }
            }
        }

         private void MoveDirAction() {
             if (this.jumpType != CharacterData.JumpType.None || isSlide || isDrive) {
                 return;
             }

             if (moveDir.sqrMagnitude <= float.Epsilon) {
                PlayIdle();
                return;
             }

             Dispatcher(new SE_AI.Event_GetActivateStatus() {
                 CallBack = SetActivateStatus,
             });

             if (moveType == AIData.MoveType.BackMove) {
                 if (isInFightStatus) {
                     PlayAnim(AnimConfig_HTR.Anim_stand_run_backward);
                 } else if (isInAlertStatus) {
                     PlayAnim(AnimConfig_HTR.Anim_stand_walk_Back_Engaged);
                 } else {
                     PlayAnim(AnimConfig_HTR.Anim_stand_walk_Back_NoEngaged);
                 }
             } else {
                 var angle = Vector3.Angle(iEntity.GetForward(), moveDir);
                 if (angle > runThresholdAngle && angle < 160f) {
                     var crossProduct = Vector3.Cross(iEntity.GetForward(), moveDir);
                     if (crossProduct.y > 0) {
                         if (this.moveType == AIData.MoveType.Run) {
                             PlayAnim(AnimConfig_HTR.Anim_stand_run_right);
                         } else {
                             if (isInFightStatus) {
                                 PlayAnim(AnimConfig_HTR.Anim_stand_run_right);
                             } else if (isInAlertStatus) {
                                 PlayAnim(AnimConfig_HTR.Anim_stand_walk_Right_Engaged);
                             } else {
                                 PlayAnim(AnimConfig_HTR.Anim_stand_walk_Right_NoEngaged);
                             }
                         }
                     } else {
                         if (this.moveType == AIData.MoveType.Run) {
                             PlayAnim(AnimConfig_HTR.Anim_stand_run_left);
                         } else {
                             if (isInFightStatus) {
                                 PlayAnim(AnimConfig_HTR.Anim_stand_run_left);
                             } else if (isInAlertStatus) {
                                 PlayAnim(AnimConfig_HTR.Anim_stand_walk_Left_Engaged);
                             } else {
                                 PlayAnim(AnimConfig_HTR.Anim_stand_walk_Left_NoEngaged);
                             }
                         }
                     }
                 } else {
                     if (this.moveType == AIData.MoveType.Run) {
                         PlayAnim(AnimConfig_HTR.Anim_stand_run_forward);
                     } else {
                         if (isInFightStatus) {
                             PlayAnim(AnimConfig_HTR.Anim_stand_run_forward);
                         } else if (isInAlertStatus) {
                             PlayAnim(AnimConfig_HTR.Anim_stand_walk_forward_Engaged);
                         } else {
                             PlayAnim(AnimConfig_HTR.Anim_stand_walk_forward_NoEngaged);
                         }
                     }
                 }
             }
         }

        private void SetActivateStatus(bool isInAlertStatus, bool isInFightStatus) {
            this.isInAlertStatus = isInAlertStatus;
            this.isInFightStatus = isInFightStatus;
        }
    }
}