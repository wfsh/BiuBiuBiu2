using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGunAnim : ComponentBase {
        private GunEntity entity = null;
        private C_Weapon_Base weaponBase;
        private SausagePlayable playable;
        private int base_layer = -1;
        private EntityAnimConfig animConfig;
        private string animSign = GunAnimConfig.Play_Idle;
        private bool isFire = false;
        private IGPO useGPO;
        private bool isUse1P = false;
        private bool isReload = false;
        
        protected override void OnAwake() {
            mySystem.Register<CE_Weapon.OnReload>(OnReloadCallBack);
            mySystem.Register<CE_Weapon.Fire>(OnFireCallBack);
            mySystem.Register<CE_Weapon.EndFire>(OnEndFireCallBack);
        }
        
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            weaponBase = (C_Weapon_Base)mySystem;
            animConfig = GunAnimConfig.Get(weaponBase.weaponSign);
            GetUseGPO1Por3P();
            InitPlayableGraph();
            AddUpdate(OnUpdate);
        }

        private void GetUseGPO1Por3P() {
            useGPO = weaponBase.UseGPO();
            isUse1P = useGPO.IsLocalGPO();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<CE_Weapon.Fire>(OnFireCallBack);
            mySystem.Unregister<CE_Weapon.OnReload>(OnReloadCallBack);
            mySystem.Unregister<CE_Weapon.EndFire>(OnEndFireCallBack);
            playable?.Dispose();
            playable = null;
            useGPO = null;
        }
        
        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }
        
        private void OnReloadCallBack(ISystemMsg body, CE_Weapon.OnReload ent) {
            isReload = ent.IsReload;
            if (playable == null) {
                return;
            }
            if (ent.IsReload) {
                PlayAnim(GunAnimConfig.Play_Reload);
                playable?.SetPlaySignTime(GunAnimConfig.Play_Reload, ent.ReloadTime);
            } else {
                PlayAnim(GunAnimConfig.Play_Idle);
            }
        }
        
        private void InitPlayableGraph() {
            if (isClear) {
                Debug.LogError("InitPlayableGraph IsClear");
                return;
            }
            entity = (GunEntity)iEntity;
            playable = new SausagePlayable();
            playable.Init(entity.transform, entity.WeaponAnim, animConfig, $"Weapon_{weaponBase.weaponSign}");
            if (isUse1P) {
                playable.UserAudio1P();
            }
            if (isReload) {
                PlayAnim(GunAnimConfig.Play_Reload);
            } else {
                PlayAnim(animSign);
            }
        }

        private void PlayAnim(string animSign) {
            this.animSign = animSign;
            if (playable == null) {
                return;
            }
            playable.PlayAnimSign(animSign);
        }

        private void OnFireCallBack(ISystemMsg body, CE_Weapon.Fire ent) {
            isFire = true;
            playable?.StopSign(GunAnimConfig.Play_Fire);
            playable?.PlayAnimSign(GunAnimConfig.Play_Fire, data => {
                if (isFire == false) {
                    playable?.PlayAnimSign(GunAnimConfig.Play_Idle);
                }
            });
        }

        private void OnEndFireCallBack(ISystemMsg body, CE_Weapon.EndFire ent) {
            isFire = false;
        }
    }
}
