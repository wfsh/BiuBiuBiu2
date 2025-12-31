using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIEffect : ComponentBase {
        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.Rpc_Effect.ID, OnRpcPlayEffect);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_AI.Rpc_Effect.ID, OnRpcPlayEffect);
        }

        private void OnRpcPlayEffect(INetwork network, IProto_Doc docData) {
            if (isSetEntityObj == false) {
                return;
            }
            var rpcData = (Proto_AI.Rpc_Effect)docData;
            if (iEntity is EntityBase == false) {
                Debug.LogError("ClientAIEffect OnRpcPlayEffect iEntity is not EntityBase System:"+ mySystem.GetType() + " effectSign:" + rpcData.effectSign);
                return;
            }
            var assetUrl = AssetURL.GetEffect(rpcData.effectSign);
            var entity = (EntityBase)iEntity;
            PrefabPoolManager.OnGetPrefab(assetUrl,
                entity.transform,
                gameObj => {
                    if (IsClear() || entity == null) {
                        PrefabPoolManager.OnReturnPrefab(assetUrl, gameObj);
                        return;
                    }
                    var part = entity.GetBodyTran(rpcData.offsetParentPart);
                    var offsetParentPos = part?.position ?? Vector3.zero;
                    gameObj.transform.position = offsetParentPos + rpcData.offset;
                    UpdateRegister.AddInvoke(() => {
                        PrefabPoolManager.OnReturnPrefab(assetUrl, gameObj);
                    }, rpcData.lifeTime);
                });
        }
    }
}