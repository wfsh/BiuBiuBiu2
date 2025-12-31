using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityMissileVisual : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int FireGpoId;
            public Proto_Ability.TargetRpc_Missile RpcData;
        }
        private float checkTime;
        private int fireGpoId;
        private IGPO fireGpo;
        private IGPO localGpo;
        private GameObject beaconObj;
        private GameObject areaObj;
        private bool checkPlayVFX;
        private Proto_Ability.TargetRpc_Missile rpcData;
        private const float CHECK_GPO_DURATION = 1f;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<CE_Ability.MissileMoveEnd>(OnMissileMoveEndCallBack);
            var initData = (InitData)initDataBase;
            Init(initData.FireGpoId, initData.RpcData);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (checkPlayVFX) {
                CheckPlayVFX(deltaTime);
            }
        }

        protected override void OnClear() {
            mySystem.Unregister<CE_Ability.MissileMoveEnd>(OnMissileMoveEndCallBack);
            DestroyVFX();
            RemoveUpdate(OnUpdate);
        }

        public void Init(int fireGpoId, Proto_Ability.TargetRpc_Missile rpcData) {
            this.fireGpoId = fireGpoId;
            this.rpcData = rpcData;
        }

        public void OnMissileMoveEndCallBack(ISystemMsg body, CE_Ability.MissileMoveEnd ent) {
            checkPlayVFX = true;
            CheckPlayVFX(0);
        }

        private void CheckPlayVFX(float deltaTime) {
            checkTime -= deltaTime;
            if (checkTime > 0) {
                return;
            }
            checkTime = CHECK_GPO_DURATION;
            if (fireGpo == null) {
                MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                    GpoId = fireGpoId,
                    CallBack = gpo => {
                        fireGpo = gpo;
                    }
                });
            }
            if (localGpo == null) {
                MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                    CallBack = gpo => {
                        localGpo = gpo;
                    }
                });
            }
            if (fireGpo == null || localGpo == null) {
                return;
            }
            checkPlayVFX = false;
            PlayVFX();
        }

        private void PlayVFX() {
            var finalPos = rpcData.points[^1] + new Vector3(0, 0.1f, 0);
            iEntity.SetRota(Quaternion.Euler(-90, 0, 0));
            iEntity.SetPoint(finalPos);
            var sameTeam = fireGpo.GetTeamID() == localGpo.GetTeamID();
            var beacon = sameTeam ? "fx_missile_beacon_blue" : "fx_missile_beacon_red";
            AssetManager.LoadGamePlayObjAsync($"Effects/{beacon}", obj => {
                if (IsClear()) {
                    return;
                }
                beaconObj = GameObject.Instantiate(obj);
                beaconObj.transform.localPosition = finalPos;
            });
            if (sameTeam) {
                AssetManager.LoadGamePlayObjAsync("Effects/fx_missile_hit_area_blue", obj => {
                    if (IsClear()) {
                        return;
                    }
                    areaObj = GameObject.Instantiate(obj);
                    areaObj.transform.localPosition = finalPos + new Vector3(0, 0.2f, 0);
                    areaObj.transform.localScale = Vector3.one * rpcData.areaRadius * 2;
                });
            }
        }

        private void DestroyVFX() {
            if (beaconObj != null) {
                GameObject.Destroy(beaconObj);
                beaconObj = null;
            }
            if (areaObj != null) {
                GameObject.Destroy(areaObj);
                areaObj = null;
            }
        }
    }
}