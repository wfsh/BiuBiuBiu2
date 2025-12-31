using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGunVfx : ComponentBase {
        private int skinItemId;
        private GameObject vfx;

        protected override void OnAwake() {
            skinItemId = ((C_Weapon_Base)mySystem).weaponSkinItemId;
        }

        protected override void OnClear() {
            ClearVfx();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            CreateVfx();
        }

        private void ClearVfx() {
            if (vfx == null) {
                return;
            }
            var vfxUrl = WeaponSkinData.GetSkinSfx(skinItemId);
            PrefabPoolManager.OnReturnPrefab(vfxUrl, vfx);
            vfx = null;
        }

        private void CreateVfx() {
            if (iEntity is not EntityBase entity || skinItemId == 0) {
                return;
            }
            Transform vfxParent = entity.transform;
            var vfxUrl = WeaponSkinData.GetSkinSfx(skinItemId);
            if (string.IsNullOrEmpty(vfxUrl)) {
                return;
            }
            PrefabPoolManager.OnGetPrefab(vfxUrl,
                vfxParent,
                gameObj => {
                    if (IsClear()) {
                        PrefabPoolManager.OnReturnPrefab(vfxUrl, gameObj);
                        return;
                    }
                    vfx = gameObj;
                }
            );
        }
    }
}