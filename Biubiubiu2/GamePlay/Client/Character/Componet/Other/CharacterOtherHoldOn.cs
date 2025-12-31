using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherHoldOn : ClientCharacterComponent {
        private Transform lHandTran;
        private GameObject holdGameObj;
        private string sign = "";
        private bool isHoldOn = false;

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            sign = "";
            RemoveUpdate(OnUpdate);
            RemoveProtoCallBack(Proto_Weapon.Rpc_Throw.ID, OnRpcThrowCallBack);
            ClearMod();
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_Weapon.Rpc_Throw.ID, OnRpcThrowCallBack);
        }

        private void OnUpdate(float deltaTime) {
            if (characterNetwork == null || characterNetwork.IsDestroy()) {
                return;
            }
            // if (characterSync.UseHoldOn() != sign) {
            //     sign = characterSync.UseHoldOn();
            //     this.mySystem.Dispatcher(new CE_Character.HoldOn {
            //         HoldOnSign = sign
            //     });
            //     OnHoldOnCallBack();
            // }
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            lHandTran = iEntity.GetBodyTran(GPOData.PartEnum.LeftHand);
        }

        private void OnHoldOnCallBack() {
            if (sign != "") {
                HoleOn();
            } else {
                CanceHoleOn();
            }
        }

        private void HoleOn() {
            if (isHoldOn) {
                return;
            }
            isHoldOn = true;
            LoadMod();
        }

        private void CanceHoleOn() {
            if (isHoldOn == false) {
                return;
            }
            isHoldOn = false;
            ClearMod();
        }

        private void LoadMod() {
            AssetManager.LoadGamePlayObjAsync("Items/" + this.sign, LoadModEndCallBack);
        }

        private void ClearMod() {
            if (holdGameObj != null) {
                GameObject.Destroy(holdGameObj);
                holdGameObj = null;
            }
        }

        private void LoadModEndCallBack(GameObject perfab) {
            if (isHoldOn == false) {
                return;
            }
            if (perfab != null) {
                holdGameObj = GameObject.Instantiate(perfab);
                var tran = holdGameObj.transform;
                tran.SetParent(lHandTran);
                SetGameObj(tran);
            } else {
                Debug.LogError("加载失败:" + this.sign);
            }
        }

        private void SetGameObj(Transform tran) {
            tran.localPosition = Vector3.zero;
            tran.localRotation = Quaternion.identity;
            tran.localScale = Vector3.one;
        }

        private void OnRpcThrowCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            this.mySystem.Dispatcher(new CE_Character.Throw());
        }
    }
}