using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterAIAnim : ComponentBase {
        private C_AI_Base aiBase;
        private EntityBase entity = null;
        private string monsterSign = "";
        private EntityAnimConfig config;
        private SausagePlayable playable;
        private int playAnimId = 0;
        private bool isUseGun = false;
        private string useWeapon = "";
        private bool isReload = false;
        private bool isDrive = false;
        private string groupSign = "";

        protected override void OnAwake() {
            aiBase = (C_AI_Base)mySystem;
            mySystem.Register<CE_AI.Event_SetPlayAnimSign>(OnSetPlayAnimSignCallBack);
            mySystem.Register<CE_AI.Event_SetPlayClipId>(OnSetPlayClipIdCallBack);
            mySystem.Register<CE_Weapon.UseWeapon>(SetUseWeaponSign);
            mySystem.Register<CE_Weapon.OnReload>(OnReloadCallBack);
            mySystem.Register<CE_Weapon.Fire>(OnFireCallBack);
            mySystem.Unregister<CE_Weapon.EndFire>(OnFireEndCallBack);
            mySystem.Register<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            mySystem.Register<CE_Character.HoldOn>(OnSetHoldOnSign);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.TargetRpc_Anim.ID, OnTargetRpcPlayAnim);
            AddProtoCallBack(Proto_AI.Rpc_Anim.ID, OnRpcPlayAnim);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            playable?.Dispose();
            playable = null;
            RemoveProtoCallBack(Proto_AI.TargetRpc_Anim.ID, OnTargetRpcPlayAnim);
            RemoveProtoCallBack(Proto_AI.Rpc_Anim.ID, OnRpcPlayAnim);
            mySystem.Unregister<CE_AI.Event_SetPlayAnimSign>(OnSetPlayAnimSignCallBack);
            mySystem.Unregister<CE_AI.Event_SetPlayClipId>(OnSetPlayClipIdCallBack);
            mySystem.Unregister<CE_Weapon.UseWeapon>(SetUseWeaponSign);
            mySystem.Unregister<CE_Weapon.OnReload>(OnReloadCallBack);
            mySystem.Unregister<CE_Weapon.Fire>(OnFireCallBack);
            mySystem.Unregister<CE_Weapon.EndFire>(OnFireEndCallBack);
            mySystem.Unregister<CE_GPO.Event_PlayerDriveGPO>(OnDriveIngCallBack);
            mySystem.Unregister<CE_Character.HoldOn>(OnSetHoldOnSign);
        }

        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }

        private void OnDriveIngCallBack(ISystemMsg body, CE_GPO.Event_PlayerDriveGPO ent) {
            isDrive = ent.DriveGPO != null;
        }

        private void OnSetHoldOnSign(ISystemMsg body, CE_Character.HoldOn entData) {
            if (!string.IsNullOrEmpty(entData.HoldOnSign)) {
                playable?.PlayAnimId(AnimConfig_HTR.Anim_stand_pullout_grenade_high);
            } else {
                playable?.StopAnimId(AnimConfig_HTR.Anim_stand_pullout_grenade_high);
            }
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            aiBase = (C_AI_Base)mySystem;
            monsterSign = aiBase.AttributeData.Sign;
            config = AIAnimConfig.Get(AIAnimConfig.HTR);
            InitPlayableGraph();
        }

        private void OnTargetRpcPlayAnim(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.TargetRpc_Anim)docData;
            PlayAnim(rpcData.animId);
            ChangeRaiseGunState();
        }
        private void OnRpcPlayAnim(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.Rpc_Anim)docData;
            PlayAnim(rpcData.animId);
            ChangeRaiseGunState();
        }

        private void OnSetPlayAnimSignCallBack(ISystemMsg body, CE_AI.Event_SetPlayAnimSign ent) {
            playAnimId = playable.PlayAnimSign(ent.AnimSign);
        }

        private void OnSetPlayClipIdCallBack(ISystemMsg body, CE_AI.Event_SetPlayClipId ent) {
            PlayAnim(ent.ClipId);
        }

        private void SetUseWeaponSign(ISystemMsg body, CE_Weapon.UseWeapon ent) {
            if (ent.weapon == null || ent.weapon.GetWeaponType() != WeaponData.WeaponType.Gun) {
                if (isUseGun) {
                    CanceWeaponAnim();
                }
                isUseGun = false;
            } else {
                if (isUseGun && useWeapon != ent.weapon.GetWeaponSign()) {
                    CanceWeaponAnim();
                }
                isUseGun = true;
                useWeapon = ent.weapon.GetWeaponSign();
                SetGroupSign(useWeapon);
                if (playable != null) {
                    ChangeRaiseGunState();
                }
            }
            isReload = false;
        }

        private void CanceWeaponAnim() {
            if (playable == null) {
                return;
            }
            playable.StopSign(GetBlendTreeId());
            if (playable.GetAnimIdForPlaySign(AnimConfig_HTR.Play_Reload) != 0) {
                playable.StopSign(AnimConfig_HTR.Play_Reload);
            }
            if (playable.GetAnimIdForPlaySign(AnimConfig_HTR.Play_Fire) != 0) {
                playable.StopSign(AnimConfig_HTR.Play_Fire);
            }

            useWeapon = "";
        }


        private void OnReloadCallBack(ISystemMsg body, CE_Weapon.OnReload ent) {
            isReload = ent.IsReload;
            if (playable == null) {
                return;
            }
            if (isReload == false) {
                ChangeRaiseGunState();
            } else {
                playable?.SetPlaySignTime(AnimConfig_HTR.Play_Reload, ent.ReloadTime);
                PlayReloadAnim();
            }
        }
        private void PlayReloadAnim() {
            playable?.StopSign(GetBlendTreeId());
            playable?.PlayAnimSign(AnimConfig_HTR.Play_Reload);
        }

        private void OnFireCallBack(ISystemMsg body, CE_Weapon.Fire ent) {
            playable?.PlayAnimSign(AnimConfig_HTR.Play_Fire);
        }

        private void OnFireEndCallBack(ISystemMsg body, CE_Weapon.EndFire ent) {
            playable?.StopSign(AnimConfig_HTR.Play_Fire);
        }

        private void ChangeRaiseGunState() {
            if (isUseGun == false || isReload || playable == null) {
                return;
            }
            playable.PlayAnimSign(GetBlendTreeId());
            playable.SetParameterValue(playable.GetAnimIdForPlaySign(GetBlendTreeId()),0f, 0f);
        }
        private string GetBlendTreeId() {
            return AnimConfig_HTR.Play_StandRaiseGun;
        }

        private void InitPlayableGraph() {
            if (iEntity == null || iGPO == null) {
                return;
            }
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            if (animator == null) {
                return;
            }
            playable = new SausagePlayable();
            playable.Init(entity.transform, animator, config, $"C_GPOID:{iGPO.GetGpoID()}_{monsterSign}");
            SetGroupSign(groupSign);
            if (playAnimId == 0) {
                playAnimId = playable.PlayAnimSign(AIAnimConfig.Play_Idle);
            } else {
                PlayAnim(playAnimId);
            }
            if (isUseGun) {
                if (isReload) {
                    PlayReloadAnim();
                } else {
                    ChangeRaiseGunState();
                }
            }
        }

        private void SetGroupSign(string groupSign) {
            if (groupSign == "") {
                return;
            }
            this.groupSign = groupSign;
            playable?.SetGroupSign(this.groupSign);
        }

        private void PlayAnim(int anim) {
            playAnimId = anim;
            if (playable == null) {
                return;
            }
            playable?.PlayAnimId(anim);
            var clipData = config.GetAnimData(anim);
            if (clipData.PlaySign != "") {
                mySystem.Dispatcher(new CE_GPO.Event_PlayAnimSign {
                    AnimSign = clipData.PlaySign
                });
            }
        }
    }
}