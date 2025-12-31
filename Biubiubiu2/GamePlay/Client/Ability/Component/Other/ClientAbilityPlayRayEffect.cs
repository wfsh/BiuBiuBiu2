using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityPlayRayEffect : ComponentBase {
        private CAB_PlayRaySystem abSystem;
        private Proto_AbilityAB_Auto.Rpc_PlayRay useInData;
        private AbilityM_PlayRay useMData;
        private IGPO fireGPO;
        private RaycastHit[] raycastHit;
        private Transform hitEffect;
        private string hitEffectUrl;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (CAB_PlayRaySystem)mySystem;
            useInData = abSystem.useInData;
            useMData = (AbilityM_PlayRay)abSystem.MData;
            raycastHit = new RaycastHit[10];
        }

        protected override void OnStart() {
            base.OnStart();
            hitEffectUrl = $"{AssetURL.GamePlay}/Ability/{useMData.M_HitEffect}.prefab";
            if (!string.IsNullOrEmpty(useMData.M_HitEffect)) {
                PrefabPoolManager.OnGetPrefab(hitEffectUrl,
                    null,
                    gameObj => {
                        if (IsClear()) {
                            PrefabPoolManager.OnReturnPrefab(hitEffectUrl, gameObj);
                            return;
                        }
                        hitEffect = gameObj.transform;
                    });
            }
            GetFireGPO();
            AddUpdate(OnUpdate);
        }

        private void GetFireGPO() {
            MsgRegister.Dispatcher(new CM_GPO.GetGPO() {
                GpoId = abSystem.FireGpoId,
                CallBack = gpo => {
                    fireGPO = gpo;
                }
            });
        }

        protected override void OnClear() {
            base.OnClear();
            if (hitEffect != null) {
                PrefabPoolManager.OnReturnPrefab(hitEffectUrl, hitEffect.gameObject);
            }
            hitEffectUrl = "";
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            if (fireGPO == null || fireGPO.IsClear() || fireGPO.IsDead() || iEntity.IsClear()) {
                iEntity.SetLocalScale(Vector3.zero);
                return;
            }

            if (iEntity.GetRota() == Quaternion.identity) {
                iEntity.SetLocalScale(Vector3.zero);
                return;
            }
            
            var startPoint = iEntity.GetPoint();
            Vector3 endPoint = startPoint + iEntity.GetForward() * useInData.maxDistance;
            if (Raycast(out var hit)) {
                endPoint = hit.point;
            }

            iEntity.SetLocalScale(new Vector3(1, 1, Vector3.Distance(startPoint, endPoint)));
            if (hitEffect != null) {
                hitEffect.position = endPoint;
            }
        }
        
        private bool Raycast(out RaycastHit hit) {
            var distance = (float)useInData.maxDistance;
            var count = Physics.RaycastNonAlloc(iEntity.GetPoint(), iEntity.GetForward(), raycastHit, distance, LayerData.DefaultLayerMask | LayerData.BeHitPartMask | LayerData.TerrainLayerMask);
#if UNITY_EDITOR
            Debug.DrawRay(iEntity.GetPoint(), iEntity.GetForward() * distance, Color.red);
#endif
            var isHit = false;
            var hitRay = new RaycastHit();
            for (int i = 0; i < count; i++) {
                var ray = raycastHit[i];
                if (ray.collider == null) {
                    continue;
                }

                var gameObj = ray.collider.gameObject;
                var hitType = gameObj.GetComponent<HitType>();
                if (hitType != null) {
                    if (hitType.Layer == GPOData.LayerEnum.Ignore) {
                        continue;
                    }

                    var hitEntity = hitType.MyEntity;
                    if (hitEntity != null) {
                        if (hitEntity.GetGPOID() == iEntity.GetGPOID()) {
                            continue;
                        }

                        if (hitEntity.GetTeamID() == iEntity.GetTeamID()) {
                            continue;
                        }
                    }
                }

                if (distance > ray.distance) {
                    hitRay = ray;
                    distance = ray.distance;
                    isHit = true;
                }
            }

            if (!isHit) {
                hit = default;
                return false;
            }

            hit = hitRay;
            return true;
        }
    }
}
