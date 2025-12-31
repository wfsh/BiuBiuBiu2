using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterAIHoldOn : ComponentBase {
        private Transform lHandTran;
        private GameObject holdGameObj;
        private string sign;
        private bool isHoldOn;

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Skill.Rpc_HoldOnSign.ID, OnRpcHoldOnSign);
            AddProtoCallBack(Proto_Skill.TargetRpc_HoldOnSign.ID, OnTargetRpcHoldOnSign);
        }

        protected override void OnClear() {
            ClearMod();
            RemoveProtoCallBack(Proto_Skill.Rpc_HoldOnSign.ID, OnRpcHoldOnSign);
            RemoveProtoCallBack(Proto_Skill.TargetRpc_HoldOnSign.ID, OnTargetRpcHoldOnSign);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            lHandTran = iEntity.GetBodyTran(GPOData.PartEnum.LeftHand);
        }

        private void HoldOnChange() {
            if (!string.IsNullOrEmpty(sign)) {
                HoleOn();
            } else {
                CancelHoleOn();
            }
        }

        private void HoleOn() {
            if (isHoldOn) {
                return;
            }
            isHoldOn = true;
            LoadMod();
        }

        private void CancelHoleOn() {
            if (!isHoldOn) {
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

        private void OnRpcHoldOnSign(INetwork iBehaviour, IProto_Doc rpcData) {
            var rpc =  (Proto_Skill.Rpc_HoldOnSign)rpcData;
            sign = rpc.holdOnSign;
            Dispatcher(new CE_Character.HoldOn {
                HoldOnSign = sign
            });
            HoldOnChange();
        }

        private void OnTargetRpcHoldOnSign(INetwork iBehaviour, IProto_Doc rpcData) {
            var rpc =  (Proto_Skill.TargetRpc_HoldOnSign)rpcData;
            sign = rpc.holdOnSign;
            Dispatcher(new CE_Character.HoldOn {
                HoldOnSign = sign
            });
            HoldOnChange();
        }
    }
}