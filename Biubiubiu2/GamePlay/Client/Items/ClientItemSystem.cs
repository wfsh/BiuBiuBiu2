using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientItemSystem : SystemBase {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }
        protected override void OnClear() {
            base.OnClear();
        }

        private void AddComponents() {
            AddComponent<ClientItemWorld>();
        }
    }
}