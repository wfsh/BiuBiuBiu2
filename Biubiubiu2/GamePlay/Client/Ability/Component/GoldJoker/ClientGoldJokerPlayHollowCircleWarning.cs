using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGoldJokerPlayHollowCircleWarning : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float AttackOffset;
        }
        private float attackOffset = 0f;

        protected override void OnAwake() {
            base.OnAwake();
            var data = (InitData)initDataBase;
            attackOffset = data.AttackOffset;
        }

        protected override void OnClear() {
        }
        
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var scale = iEntity.GetLocalScale();
            var offsetY = (attackOffset / scale.x) - 0.2f;
            var list = iEntity.GetBodyTranList(GPOData.PartEnum.Object);
            for (int i = 0; i < list.Count; i++) {
                var tran = list[i];
                var renderer = tran.GetComponent<Renderer>();
                if (renderer != null) {
                    // 获取实例化材质球
                    var mat = renderer.material;
                    var offset = mat.GetTextureOffset("_MaskTex");
                    offset.y = offsetY;
                    mat.SetTextureOffset("_MaskTex", offset);
                }
            }
        }
    }
}