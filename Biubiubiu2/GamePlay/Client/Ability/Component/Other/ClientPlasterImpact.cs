using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientPlasterImpact : ComponentBase {
        private static string url = AssetURL.GetEffect("PlasterImpact");
        private GameObject effectGameObj;
        private float lifeTime = 1f;

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entity = (EntityBase)iEntity;
            PrefabPoolManager.OnGetPrefab(url,
                entity.transform,
                gameObj => {
                    if (IsClear()) {
                        PrefabPoolManager.OnReturnPrefab(url, gameObj);
                        return;
                    }
                    effectGameObj = gameObj;
                });
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            ClearEffect();
        }

        private void OnUpdate(float deltaTime) {
            if (lifeTime > 0f) {
                lifeTime -= Time.deltaTime;
            } else {
                ClearEffect();
            }
        }

        private void ClearEffect() {
            if (effectGameObj != null) {
                PrefabPoolManager.OnReturnPrefab(url, effectGameObj);
                effectGameObj = null;
            }
            RemoveUpdate(OnUpdate);
        }
    }
}