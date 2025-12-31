using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientBulletDecal : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Ability.AddBulletDecal>(OnAddBulletDecalCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Ability.AddBulletDecal>(OnAddBulletDecalCallBack);
        }

        private void OnAddBulletDecalCallBack(ISystemMsg body, CE_Ability.AddBulletDecal ent) {
            AddBulletDecal(ent.TargetPoint, ent.TargetNormal, ent.BulletDecal, ent.IsShowDecal);
        }

        private void AddBulletDecal(Vector3 targetPoint, Vector3 targetNormal, string bulletDecal, bool isShowDecal) {
            if (isShowDecal == false || string.IsNullOrEmpty(bulletDecal) || IsClear()) {
                return;
            }
            IGPO localGPO = null;
            MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                CallBack = gpo => {
                    localGPO = gpo;
                }
            });
            if (localGPO == null) {
                return;
            }
            var distance = Vector3.Distance(localGPO.GetPoint(), targetPoint);
            if (distance > 20f) {
                return;
            }
            var url = AssetURL.GetEffect(bulletDecal);
            PrefabPoolManager.OnGetPrefab(url, null, bulletDecal => {
                if (ModeData.IsIntoMode == false) {
                    PrefabPoolManager.OnReturnPrefab(url, bulletDecal);
                } else {
                    bulletDecal.transform.position = targetPoint;
                    bulletDecal.transform.rotation = Quaternion.LookRotation(targetNormal);
                    UpdateRegister.AddInvoke(() => {
                        PrefabPoolManager.OnReturnPrefab(url, bulletDecal);
                    }, 0.5f);
                }
            });
        }
    }
}