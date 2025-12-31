using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterAnimator : ServerCharacterComponent {
        private CharacterData.StandType standType = CharacterData.StandType.Stand;
        private S_Character_Base characterSystem;
        private SausagePlayable playable;
        private string weaponSign = "";
        private bool isUseGun = false;
        
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.UseWeapon>(SetUseWeapon);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_GPO.UseWeapon>(SetUseWeapon);
            if (playable != null) {
                playable.Dispose();
                playable = null;
            }
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            characterSystem = (S_Character_Base)mySystem;
            InitPlayableGraph();
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
            if (standType != characterSync.StandType()) {
                standType = characterSync.StandType();
                UpdateStandByAnim();
            }
        }
        
        protected override void OnSetNetwork() {
            AddCmdCallBack();
        }

        private void AddCmdCallBack() {
            AddProtoCallBack(Proto_Character.Cmd_StandType.ID, OnStandTypeCallBack);
        }
        
        private void SetUseWeapon(ISystemMsg body, SE_GPO.UseWeapon ent) {
            var useWeapon = (S_Weapon_Base)ent.Weapon;
            if (useWeapon == null || useWeapon.GetWeaponType() != WeaponData.WeaponType.Gun) {
                if (isUseGun) {
                    playable.StopSign(GetRaiseGunBlendTree());
                }
                isUseGun = false;
            } else {
                isUseGun = true;
                weaponSign = useWeapon.GetWeaponSign();
                if (playable != null) {
                    playable.SetGroupSign(useWeapon.GetWeaponSign());
                    ChangeRaiseGunState();
                }
            }
        }
        
        private string GetRaiseGunBlendTree() {
            return AnimConfig_HTR.Play_StandRaiseGun;
        }
        
        private void ChangeRaiseGunState() {
            if (isUseGun == false) {
                return;
            }
            playable.PlayAnimSign(GetRaiseGunBlendTree());
            playable.SetParameterValue(GetRaiseGunBlendTree(), 0f, 0f);
        }

        private void InitPlayableGraph() {
            var entity = (CharacterEntity)iEntity;
            playable = new SausagePlayable();
            var config = AIAnimConfig.Get(AIAnimConfig.HTR);
            playable.Init(entity.transform, entity.animator, config, $"Server_Anim_{characterSystem.NickName}");
            playable.CloseLoadEffect();
            if (isUseGun) {
                playable.SetGroupSign(weaponSign);
                ChangeRaiseGunState();
            }
            UpdateStandByAnim();
        }

        private void OnStandTypeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.Cmd_StandType)cmdData;
            standType = data.standType;
            UpdateStandByAnim();
        }

        private void UpdateStandByAnim() {
            var anim = AnimConfig_HTR.Anim_stand_idle_noweapon;
            switch (standType) {
                case CharacterData.StandType.Crouch:
                    anim = AnimConfig_HTR.Anim_crouch_idle_noweapon;
                    break;
                case CharacterData.StandType.Prone:
                    anim = AnimConfig_HTR.Anim_prone_idle_noweapon;
                    break;
            }
            playable?.PlayAnimId(anim);
        }
    }
}