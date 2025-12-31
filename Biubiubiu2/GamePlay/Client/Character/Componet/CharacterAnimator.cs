using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterAnimator : ComponentBase {
        private CharacterEntity entity;
        private C_Character_Base characterSystem;
        private CharacterData.StandType standType = CharacterData.StandType.Stand;
        private CharacterData.FlyType flyType = CharacterData.FlyType.None;
        private CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        private CharacterData.WirebugType wirebugType = CharacterData.WirebugType.None;
        private SausagePlayable playable;
        private string groupSign = "";
        private string playAnimSign = "";
        private int playAnimId = 0;
        private bool isInitPlayable = false;
        private bool isLocalPlayer = false;
        private float px;
        private float py;
        private int moveDir = 0;
        private bool isDodga = false;
        private bool isSlide = false;
        private bool isWirebugMove = false;
        private bool isUseGun = false;
        private string holdOnSign = "";
        private string useWeapon = "";
        private bool isDrive = false;
        private string playRaiseGunSign = "";
        private int playBaseAnimId = 0;
        private bool isReload = false;
        private float cameraV = 0f;
        private int meleeAnimId = 0;
        private int holdAnimId = 0;
        private float vRota = 0f;
        private int weaponItemId = 0;
        private float fireTime = 0.0f;
        private EntityAnimConfig config;
        private IGPO lookGPO;

        protected override void OnAwake() {
            this.mySystem.Register<CE_GPO.Event_MoveDir>(OnMoveCallBack);
            this.mySystem.Register<CE_Character.CameraRotaV>(CameraVRotaCallBack);
            this.mySystem.Register<CE_Character.JumpTypeChange>(OnJumpTypeCallBack, 1);
            this.mySystem.Register<CE_Character.FlyTypeChange>(OnFlyTypeCallBack);
            this.mySystem.Register<CE_Character.StandTypeChange>(OnStandTypeCallBack);
            this.mySystem.Register<CE_Character.PlayAttackAnim>(OnPlayMeleeAnimCallBack);
            this.mySystem.Register<CE_Character.PlayAnim>(OnPlayAnimCallBack);
            this.mySystem.Register<CE_Character.StopAnim>(OnStopAnimCallBack);
            this.mySystem.Register<CE_Character.PlayAnimSign>(OnPlayAnimSignCallBack);
            this.mySystem.Register<CE_Character.StopAnimSign>(OnStopAnimSignCallBack);
            this.mySystem.Register<CE_Character.HoldOn>(SetHoldOnSign);
            this.mySystem.Register<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            this.mySystem.Register<CE_Character.Fall>(OnFallCallBack);
            this.mySystem.Register<CE_Character.Dodga>(OnDodgaCallBack);
            this.mySystem.Register<CE_Character.Event_SlideMove>(OnSlideMoveCallBack);
            this.mySystem.Register<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateCallBack);
            this.mySystem.Register<CE_Character.Throw>(OnThrowCallBack);
            this.mySystem.Register<CE_Character.StopThrow>(OnStopThrowCallBack);
            this.mySystem.Register<CE_Character.FallToGrounded>(OnFallToGroundedCallBack);
            this.mySystem.Register<CE_Character.GetAnimatorLoadEnd>(GetAnimatorLoadEndCallBack);
            this.mySystem.Register<CE_Character.SetAnimGroupSign>(SetAnimGroupSignCallBack);
            this.mySystem.Register<CE_Character.Event_StartSlideMove>(OnStartSlideMoveCallBack);
            this.mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            this.mySystem.Register<CE_Weapon.UseWeapon>(SetUseWeaponSign);
            this.mySystem.Register<CE_Weapon.OnReload>(OnReloadCallBack);
            this.mySystem.Register<CE_Weapon.Fire>(OnFireCallBack);
            this.mySystem.Register<CE_Weapon.EndFire>(OnFireEndCallBack);
            MsgRegister.Register<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
        }
        
        protected override void OnStart() {
            base.OnStart();
            MsgRegister.Dispatcher(new CM_GPO.GetLookGPO {
                CallBack = SetLookGPO
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            characterSystem = (C_Character_Base)mySystem;
            InitPlayableGraph();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<CM_GPO.AddLookGPO>(OnAddLookGPOCallBack);
            this.mySystem.Unregister<CE_GPO.Event_MoveDir>(OnMoveCallBack);
            this.mySystem.Unregister<CE_Character.CameraRotaV>(CameraVRotaCallBack);
            this.mySystem.Unregister<CE_Character.JumpTypeChange>(OnJumpTypeCallBack);
            this.mySystem.Unregister<CE_Character.FlyTypeChange>(OnFlyTypeCallBack);
            this.mySystem.Unregister<CE_Character.StandTypeChange>(OnStandTypeCallBack);
            this.mySystem.Unregister<CE_Character.PlayAttackAnim>(OnPlayMeleeAnimCallBack);
            this.mySystem.Unregister<CE_Character.PlayAnim>(OnPlayAnimCallBack);
            this.mySystem.Unregister<CE_Character.StopAnim>(OnStopAnimCallBack);
            this.mySystem.Unregister<CE_Character.HoldOn>(SetHoldOnSign);
            this.mySystem.Unregister<CE_Character.TakeOnMonster>(SetTakeOnMonsterSign);
            this.mySystem.Unregister<CE_Character.Fall>(OnFallCallBack);
            this.mySystem.Unregister<CE_Character.Dodga>(OnDodgaCallBack);
            this.mySystem.Unregister<CE_Character.Event_SlideMove>(OnSlideMoveCallBack);
            this.mySystem.Unregister<CE_Character.Throw>(OnThrowCallBack);
            this.mySystem.Unregister<CE_Character.StopThrow>(OnStopThrowCallBack);
            this.mySystem.Unregister<CE_Character.Event_WirebugMoveState>(OnWirebugMoveStateCallBack);
            this.mySystem.Unregister<CE_Character.FallToGrounded>(OnFallToGroundedCallBack);
            this.mySystem.Unregister<CE_Character.GetAnimatorLoadEnd>(GetAnimatorLoadEndCallBack);
            this.mySystem.Unregister<CE_Character.PlayAnimSign>(OnPlayAnimSignCallBack);
            this.mySystem.Unregister<CE_Character.SetAnimGroupSign>(SetAnimGroupSignCallBack);
            this.mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            this.mySystem.Unregister<CE_Character.Event_StartSlideMove>(OnStartSlideMoveCallBack);
            this.mySystem.Unregister<CE_Weapon.UseWeapon>(SetUseWeaponSign);
            this.mySystem.Unregister<CE_Weapon.OnReload>(OnReloadCallBack);
            this.mySystem.Unregister<CE_Weapon.Fire>(OnFireCallBack);
            this.mySystem.Unregister<CE_Weapon.EndFire>(OnFireEndCallBack);
            entity = null;
            playable?.Dispose();
            playable = null;
        }

        private void GetAnimatorLoadEndCallBack(ISystemMsg body, CE_Character.GetAnimatorLoadEnd ent) {
            ent.CallBack(playable != null);
        }

        private void CameraVRotaCallBack(ISystemMsg body, CE_Character.CameraRotaV entData) {
            SetCameraVRota(entData.vRota);
        }

        private void SetCameraVRota(float value) {
            this.vRota = value;
            var rotaX = vRota;
            var rotaValue = rotaX > 180 ? rotaX - 360 : rotaX;
            if (rotaValue < 0f) {
                rotaValue *= -1.5f;
            } else {
                rotaValue *= -1f;
            }
            cameraV = rotaValue;
        }

        private void OnUpdate(float delta) {
            if (playable == null) {
                return;
            }
            playable.OnUpdate(delta);
            UpdateMoveDir();
            UpdateRaiseGun();
        }

        private void InitPlayableGraph() {
            entity = (CharacterEntity)iEntity;
            config = AIAnimConfig.Get(AIAnimConfig.HTR);
            playable = new SausagePlayable();
            playable.Init(entity.transform, entity.animator, config, $"Client_{characterSystem.NickName}");
            if (isLocalPlayer) {
                playable.UserAudio1P();
            }
            mySystem.Dispatcher(new CE_Character.AnimatorLoadEnd());
            isInitPlayable = true;
            SetGroupSign(groupSign);
            if (playAnimId == 0 && playAnimSign == "") {
                PlayAnimId(AnimConfig_HTR.Anim_stand_idle_noweapon);
            } else {
                if (playAnimSign != "") {
                    PlayAnimSign(playAnimSign);
                } else {
                    PlayAnimId(playAnimId);
                }
            }
            if (isUseGun) {
                UpdateRaiseGun();
                if (isReload) {
                    PlayReloadAnim();
                }
            }
        }
        
        private void OnAddLookGPOCallBack(CM_GPO.AddLookGPO ent) {
            SetLookGPO(ent.LookGPO);
        }

        private void SetLookGPO(IGPO lookGpo) {
            this.lookGPO = lookGpo;
            if (lookGpo == null) {
                isLocalPlayer = false;
            } else {
                isLocalPlayer = lookGpo.GetGpoID() == GpoID;
                if (playable != null) {
                    if (isLocalPlayer) {
                        playable.UserAudio1P();
                    }
                }
            }
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO ent) {
            isDrive = ent.DriveGPO != null;
        }

        private void SetUseWeaponSign(ISystemMsg body, CE_Weapon.UseWeapon ent) {
            if (ent.weapon == null || ent.weapon.GetWeaponType() != WeaponData.WeaponType.Gun) {
                if (isUseGun) {
                    CanceWeaponAnim();
                }
                weaponItemId = 0;
                useWeapon = "";
                isUseGun = false;
            } else {
                if (isUseGun && useWeapon != ent.weapon.GetWeaponSign()) {
                    CanceWeaponAnim();
                }
                weaponItemId = ent.weapon.GetWeaponItemId();
                useWeapon = ent.weapon.GetWeaponSign();
                SetGroupSign(useWeapon);
                if (playable != null) {
                    UpdateRaiseGun();
                }
                isUseGun = true;
            }
        }


        private void CanceWeaponAnim() {
            if (playable == null) {
                return;
            }
            StopSign(GetRaiseGunBlendTreeSign());
            StopSign(AnimConfig_HTR.Play_Reload);
            StopSign(AnimConfig_HTR.Play_Fire);
            playRaiseGunSign = "";
        }

        private void SetAnimGroupSignCallBack(ISystemMsg body, CE_Character.SetAnimGroupSign ent) {
            SetGroupSign(ent.GroupSign);
        }

        private void SetGroupSign(string groupSign) {
            if (groupSign == "") {
                return;
            }
            this.groupSign = groupSign;
            playable?.SetGroupSign(this.groupSign);
        }

        private void OnReloadCallBack(ISystemMsg body, CE_Weapon.OnReload ent) {
            isReload = ent.IsReload;
            if (playable == null) {
                return;
            }
            if (isReload) {
                PlayReloadAnim();
                playable?.SetPlaySignTime(AnimConfig_HTR.Play_Reload, ent.ReloadTime);
            } else {
                StopSign(AnimConfig_HTR.Play_Reload);
                UpdateRaiseGun();
            }
        }

        private void PlayReloadAnim() {
            PlayAnimSign(AnimConfig_HTR.Play_Reload);
            StopSign(AnimConfig_HTR.Play_Fire);
        }

        private void OnFireCallBack(ISystemMsg body, CE_Weapon.Fire ent) {
            fireTime = Time.realtimeSinceStartup;
            PlayAnimSign(AnimConfig_HTR.Play_Fire);
        }

        private void OnFireEndCallBack(ISystemMsg body, CE_Weapon.EndFire ent) {
            StopSign(AnimConfig_HTR.Play_Fire);
        }

        private void OnPlayMeleeAnimCallBack(ISystemMsg body, CE_Character.PlayAttackAnim ent) {
            if (ent.PlayAnimId == 0) {
                if (meleeAnimId != 0) {
                    PlayAnimId(AnimConfig_HTR.Anim_stand_idle_noweapon);
                }
                meleeAnimId = ent.PlayAnimId;
                return;
            }
            meleeAnimId = ent.PlayAnimId;
            PlayAnimId(ent.PlayAnimId);
        }

        private void OnPlayAnimCallBack(ISystemMsg body, CE_Character.PlayAnim ent) {
            PlayAnimId(ent.AnimId, ent.PlayEndCallBack);
        }

        private void OnPlayAnimSignCallBack(ISystemMsg body, CE_Character.PlayAnimSign ent) {
            PlayAnimSign(ent.PlaySign, ent.PlayEndCallBack);
        }

        private void OnStopAnimCallBack(ISystemMsg body, CE_Character.StopAnim ent) {
            StopAnimId(ent.AnimId);
        }

        private void OnStopAnimSignCallBack(ISystemMsg body, CE_Character.StopAnimSign ent) {
            StopSign(ent.PlaySign);
        }

        public void SetHoldOnSign(ISystemMsg body, CE_Character.HoldOn entData) {
            holdOnSign = entData.HoldOnSign;
            if (holdOnSign == "") {
                StopHoldAnim();
            } else {
                PlayHoldAnim();
            }
        }

        public void SetTakeOnMonsterSign(ISystemMsg body, CE_Character.TakeOnMonster entData) {
            holdOnSign = entData.HoldOnSign;
            if (holdOnSign == "") {
                StopThrow();
                StopHoldAnim();
            } else {
                PlayHoldAnim();
            }
        }

        public void OnThrowCallBack(ISystemMsg body, CE_Character.Throw entData) {
            if (IsProne()) {
                PlayAnimId(AnimConfig_HTR.Anim_prone_throw_grenade_high);
            } else {
                PlayAnimId(AnimConfig_HTR.Anim_stand_throw_grenade_high);
            }
        }

        private void OnStopThrowCallBack(ISystemMsg body, CE_Character.StopThrow entData) {
            StopThrow();
        }

        private void StopThrow() {
            StopAnimId(AnimConfig_HTR.Anim_stand_throw_grenade_high);
            StopAnimId(AnimConfig_HTR.Anim_prone_throw_grenade_high);
        }

        private void PlayHoldAnim() {
            if (IsProne()) {
                PlayAnimId(AnimConfig_HTR.Anim_prone_pullout_grenade_high);
            } else {
                PlayAnimId(AnimConfig_HTR.Anim_stand_pullout_grenade_high);
            }
        }

        private void StopHoldAnim() {
            StopAnimId(AnimConfig_HTR.Anim_stand_pullout_grenade_high);
            StopAnimId(AnimConfig_HTR.Anim_prone_pullout_grenade_high);
        }

        private void OnMoveCallBack(ISystemMsg body, CE_GPO.Event_MoveDir entData) {
            this.px = entData.MoveV;
            this.py = entData.MoveH;
            UpdateMoveDir();
        }

        public void OnStandTypeCallBack(ISystemMsg body, CE_Character.StandTypeChange entData) {
            var standType = (CharacterData.StandType)entData.StandType;
            if (this.standType != standType) {
                this.standType = standType;
                PlayHoldAnim();
            }
        }

        public void OnDodgaCallBack(ISystemMsg body, CE_Character.Dodga entData) {
            isDodga = entData.IsDodge;
            if (entData.IsDodge) {
                if (isUseGun) {
                    PlayAnimId(AnimConfig_HTR.BlendTree_Jump);
                    playable?.SetParameterValue(AnimConfig_HTR.BlendTree_Jump, px, py);
                } else {
                    PlayAnimId(AnimConfig_HTR.Anim_guanyu_sprint);
                }
            } else {
                PlayAnimId(jumpType != CharacterData.JumpType.None
                    ? AnimConfig_HTR.Anim_jump_again
                    : AnimConfig_HTR.Anim_stand_idle_noweapon);
            }
        }

        public void OnStartSlideMoveCallBack(ISystemMsg body, CE_Character.Event_StartSlideMove entData) {
            StopAnimId(AnimConfig_HTR.Anim_soccer_Tackle);
        }

        public void OnSlideMoveCallBack(ISystemMsg body, CE_Character.Event_SlideMove entData) {
            if (isSlide != entData.IsSlide) {
                isSlide = entData.IsSlide;
                if (entData.IsSlide) {
                    PlayAnimId(AnimConfig_HTR.Anim_soccer_Tackle);
                } else {
                    UpdateMoveDir();
                }
            }
        }
        
        public void OnWirebugMoveStateCallBack(ISystemMsg body, CE_Character.Event_WirebugMoveState ent) {
            wirebugType = ent.State;
            PlayWirebugAnim();
        }

        private void PlayWirebugAnim() {
            switch (wirebugType) {
                case CharacterData.WirebugType.Start:
                    PlayAnimId(AnimConfig_HTR.Anim_stand_throw_ShipAnchor);
                    break;
                case CharacterData.WirebugType.Drop:
                case CharacterData.WirebugType.Glide:
                    PlayAnimId(AnimConfig_HTR.Anim_stand_throw_ShipAnchor_loop);
                    break;
                case CharacterData.WirebugType.None:
                    StopAnimId(AnimConfig_HTR.Anim_stand_throw_ShipAnchor);
                    break;
            }
        }

        private void UpdateMoveDir() {
            if (playable == null) {
                return;
            }
            var idleAnim = 0;
            if (isDrive == false &&
                meleeAnimId == 0 &&
                jumpType == CharacterData.JumpType.None &&
                isDodga == false &&
                isSlide == false &&
                wirebugType == CharacterData.WirebugType.None &&
                flyType != CharacterData.FlyType.OpenParachute) {
                idleAnim = GetBaseActionAnim();
                if (playBaseAnimId != idleAnim) {
                    PlayAnimId(idleAnim);
                }
                var (setX, setY) = NormalizeValues(px, py);
                playable.SetParameterValue(idleAnim, setX, setY);
            } else {
                if (playBaseAnimId != idleAnim) {
                    StopAnimId(playBaseAnimId);
                }
            }
            playBaseAnimId = idleAnim;
        }

        public (float, float) NormalizeValues(float px, float py) {
            var sum = Mathf.Abs(px) + Mathf.Abs(py);
            if (Math.Abs(sum) <= 0f) {
                return (0f, 0f);
            }
            // 归一化
            var scale = 1.0f / sum;
            return (px * scale, py * scale);
        }

        private bool isFireState() {
            var time = Time.realtimeSinceStartup;
            var isFire = (time - fireTime) < 0.5f;
            return isFire;
        }
        private void UpdateRaiseGun() {
            if (isReload == false && isUseGun && flyType == CharacterData.FlyType.None) {
                if (weaponItemId != ItemSet.Id_Gatling && isFireState() == false && this.px > 0.5f && weaponItemId != ItemSet.Id_FireGun) {
                    if (playRaiseGunSign != "") {
                        StopSign(GetRaiseGunBlendTreeSign());
                        playRaiseGunSign = "";
                    }
                } else {
                    var raiseGunSign = GetRaiseGunBlendTreeSign();
                    if (playRaiseGunSign != raiseGunSign) {
                        PlayAnimSign(GetRaiseGunBlendTreeSign());
                        playRaiseGunSign = raiseGunSign;
                    }
                    playable.SetParameterValue(playable.GetAnimIdForPlaySign(raiseGunSign), cameraV, 0f);
                }
            } else {
                if (playRaiseGunSign != "") {
                    StopSign(GetRaiseGunBlendTreeSign());
                    playRaiseGunSign = "";
                }
            }
        }

        private void StopSign(string sign) {
            if (playAnimSign == sign) {
                playAnimSign = "";
            }
            if (isInitPlayable == false) {
                return;
            }
            playable.StopSign(sign);
        }

        private void StopAnimId(int animId) {
            if (playAnimId == animId) {
                playAnimId = 0;
            }
            if (isInitPlayable == false) {
                return;
            }
            playable.StopAnimId(animId);
        }

        private void PlayAnimSign(string sign, Action<EntityAnimConfig.StateData> onEnd = null) {
            playAnimSign = sign;
            playAnimId = 0;
            if (isInitPlayable == false) {
                return;
            }
            playable.PlayAnimSign(sign, onEnd);
        }

        private void PlayAnimId(int anim, Action<EntityAnimConfig.StateData> onEnd = null) {
            playAnimId = anim;
            playAnimSign = "";
            if (isInitPlayable == false) {
                return;
            }
            playable.PlayAnimId(anim, onEnd);
        }

        private int GetBaseActionAnim() {
            var action = 0;
            var idle = px == 0f && py == 0f;
            if (weaponItemId != ItemSet.Id_Gatling && weaponItemId != ItemSet.Id_FireGun) {
                if (px > 0.5f && isFireState() == false && isReload == false) {
                    return AnimConfig_HTR.Anim_stand_sprint_forward_smg;
                }
            }
            if (idle) {
                if (IsFly()) {
                    action = AnimConfig_HTR.Anim_parachute_idle;
                } else if (IsProne()) {
                    action = AnimConfig_HTR.Anim_prone_idle_noweapon;
                } else if (IsCrouch()) {
                    action = AnimConfig_HTR.Anim_crouch_idle_noweapon;
                } else {
                    action = AnimConfig_HTR.Anim_stand_idle_noweapon;
                }
            } else {
                if (IsFly() || isUseGun) {
                    if (IsFly()) {
                        action = AnimConfig_HTR.BlendTree_Parachute;
                    } else if (IsProne()) {
                        action = AnimConfig_HTR.BlendTree_ProneWalk;
                    } else if (IsCrouch()) {
                        action = AnimConfig_HTR.BlendTree_CrouchRun;
                    } else {
                        action = AnimConfig_HTR.BlendTree_StandRun;
                    }
                } else {
                    if (IsProne()) {
                        action = AnimConfig_HTR.Anim_prone_walk_forward;
                    } else if (IsCrouch()) {
                        action = AnimConfig_HTR.Anim_crouch_run_forward;
                    } else {
                        action = AnimConfig_HTR.Anim_stand_run_forward;
                    }
                }
            }
            return action;
        }

        private string GetRaiseGunBlendTreeSign() {
            var blendTreeId = "";
            if (standType == CharacterData.StandType.Crouch) {
                blendTreeId = AnimConfig_HTR.Play_CrouchRaiseGun;
            } else if (standType == CharacterData.StandType.Prone) {
                blendTreeId = AnimConfig_HTR.Play_ProneRaiseGun;
            } else {
                blendTreeId = AnimConfig_HTR.Play_StandRaiseGun;
            }
            return blendTreeId;
        }

        private bool IsStand() {
            return standType == CharacterData.StandType.Stand;
        }

        private bool IsCrouch() {
            return standType == CharacterData.StandType.Crouch;
        }

        private bool IsProne() {
            return standType == CharacterData.StandType.Prone;
        }

        private bool IsFly() {
            return CharacterData.IsFly(flyType);
        }

        public void OnFallCallBack(ISystemMsg body, CE_Character.Fall entData) {
            if (entData.FallValue < -3f && flyType == CharacterData.FlyType.None) {
                PlayAnimId(AnimConfig_HTR.Anim_jump_loop);
            }
        }

        public void OnFallToGroundedCallBack(ISystemMsg body, CE_Character.FallToGrounded entData) {
        }

        public void OnJumpTypeCallBack(ISystemMsg body, CE_Character.JumpTypeChange entData) {
            this.jumpType = entData.JumpType;
            switch (jumpType) {
                case CharacterData.JumpType.Jump:
                case CharacterData.JumpType.Fall:
                    if (this.flyType == CharacterData.FlyType.None) {
                        PlayAnimId(AnimConfig_HTR.Anim_jump_loop);
                    }
                    break;
                case CharacterData.JumpType.AirJump:
                    PlayAnimId(GetJumAnim());
                    playable.SetParameterValue(AnimConfig_HTR.BlendTree_Jump, px, py);
                    break;
            }
        }

        private int GetJumAnim() {
            if (isUseGun) {
                return AnimConfig_HTR.BlendTree_Jump;
            }
            return AnimConfig_HTR.Anim_jump_again;
        }

        public void OnFlyTypeCallBack(ISystemMsg body, CE_Character.FlyTypeChange entData) {
            SetFlyType(entData.FlyType);
        }

        private void SetFlyType(CharacterData.FlyType flyType) {
            this.flyType = flyType;
            switch (flyType) {
                case CharacterData.FlyType.OpenParachute:
                    PlayAnimId(AnimConfig_HTR.Anim_parachute_open);
                    break;
            }
        }
    }
}