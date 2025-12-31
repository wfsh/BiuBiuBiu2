using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientChangeFlashRange : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int Length;
            public Proto_Ability.Rpc_RangeFlash AbilityData;
            public IAbilityMData ModMData;
        }
        private List<GameObject> mEffects = new List<GameObject>();
        private Proto_Ability.Rpc_RangeFlash abilityData;
        private float[] originTilingX = new[] { 0.58f, 0.8f };
        private string flashEffectUrl;
        private IGPO fireGPO;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.Length, initData.AbilityData, initData.ModMData);
        }

        protected override void OnStart() {
            base.OnStart();
            CheckLoadAllEffect();
        }

        protected override void OnClear() {
            base.OnClear();
            for (int i = 0; i < mEffects.Count; i++) {
                GameObject effect = mEffects[i];
                if (effect != null) {
                    PrefabPoolManager.OnReturnPrefab(flashEffectUrl, effect);
                }
            }

            mEffects.Clear();
            fireGPO = null;
        }

        public void SetData(int length, Proto_Ability.Rpc_RangeFlash abilityData, IAbilityMData modMData) {
            this.abilityData = abilityData;
            flashEffectUrl = AssetURL.GetEffect(modMData.GetEffectSign());
        }

        private void CheckLoadAllEffect() {
            MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                GpoId = abilityData.GPOId[abilityData.GPOId.Length - 1],
                CallBack = gpo => {
                    if (gpo != null) {
                        fireGPO = gpo;
                    }
                }
            });
            if (fireGPO != null) {
                LoadAllEffects();
            }
        }

        private void LoadAllEffects() {
            for (int i = 0; i < abilityData.GPOId.Length - 1; i++) {
                IGPO hitGPO = null;
                MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                    GpoId = abilityData.GPOId[i],
                    CallBack = gpo => {
                        if (gpo != null) {
                            hitGPO = gpo;
                        }
                    }
                });
                if (hitGPO != null) {
                    LoadFlashEffect(hitGPO);
                }
            }
        }

        private void LoadFlashEffect(IGPO hitGPO) {
            var loadFlashEffectUrl = flashEffectUrl;
            PrefabPoolManager.OnGetPrefab(loadFlashEffectUrl, null, (obj) => {
                var tempGPO = hitGPO;
                if (isClear || hitGPO == null || hitGPO.IsClear() || fireGPO == null || fireGPO.IsClear()) {
                    PrefabPoolManager.OnReturnPrefab(loadFlashEffectUrl, obj);
                    return;
                }

                if (obj != null) {
                    mEffects.Add(obj);
                    PlayFlashAndAudio(tempGPO, obj);
                }
            });
        }

        private void PlayFlashAndAudio(IGPO hitGPO, GameObject obj) {
            var flash = obj;
            var flashTransform = flash.transform;
            try {
                MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                    layer = StageData.GameWorldLayerType.Ability, transform = flashTransform
                });
                var fireGPOBody = fireGPO.GetBodyTran(GPOData.PartEnum.Body);
                var hitGPOBody = hitGPO.GetBodyTran(GPOData.PartEnum.Body);
                var fireGPOPoint = fireGPOBody == null ? fireGPO.GetPoint() + Vector3.up : fireGPOBody.position;
                var hitGPOPoint = hitGPOBody == null ? hitGPO.GetPoint() + Vector3.up : hitGPOBody.position;
                flashTransform.position = (fireGPOPoint + hitGPOPoint) / 2;
                if (hitGPO.IsLocalGPO()) {
                    AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("WP_NG_SP_Flashgun_Link"), hitGPOPoint);
                } else {
                    AudioPoolManager.OnPlayAudio(AssetURL.GetAudio3P("WP_NG_SP_Flashgun_Link_3P"), hitGPOPoint);
                }

                var distance = Vector3.Distance(fireGPOPoint, hitGPOPoint);
                flashTransform.localScale = new Vector3(1, 1, distance / 4f);
                flashTransform.LookAt(hitGPOPoint);
                SetTiling(flash, distance / 4f);
            } catch (Exception e) {
                Debug.LogError($"[PlayFlashAndAudio] Error, Exception: {e}");
            }
        }

        private void SetTiling(GameObject gameoObject, float scale) {
            try {
                var effectRenderer = gameoObject.GetComponent<EffectRenderer>();
                Renderer[] renderers = effectRenderer.GetRenderers();
                for (int i = 0; i < renderers.Length; i++) {
                    renderers[i].material.SetTextureScale("_MainTex", new Vector2(originTilingX[i] * scale, 1));
                }
            } catch (Exception e) {
                Debug.LogError($"[FlashSetTiling] Error, Exception: {e}");
            }
        }
    }
}