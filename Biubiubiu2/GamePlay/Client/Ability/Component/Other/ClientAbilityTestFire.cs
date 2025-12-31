using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilityTestFire : ComponentBase {
        private int bombNum = 0;
        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_AbilityAB_Auto.Rpc_PlayEffect.ID, OnPlayTestFireCallBack);
        }
        

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            AddProtoCallBack(Proto_AbilityAB_Auto.Rpc_PlayEffect.ID, OnPlayTestFireCallBack);
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
        }
        private void OnPlayTestFireCallBack(INetwork network, IProto_Doc data) {
            var rpcData = (Proto_AbilityAB_Auto.Rpc_PlayEffect)data;
            var modData = AbilityM_PlayEffect.CreateForID(rpcData.rowId);
            modData.Select(() => {
                var url = AssetURL.GetEffect(modData.GetEffectSign());
                PrefabPoolManager.OnGetPrefab(url, null, (prefab) => {
                    if (prefab != null) {
                        prefab.transform.position = iEntity.GetPoint();
                        UpdateRegister.AddInvoke(() => {
                            PrefabPoolManager.OnReturnPrefab(url, prefab);
                        }, 1f);
                    }
                });
            });
        }
    }
}