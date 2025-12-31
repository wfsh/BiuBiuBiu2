using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientParticleCannonAnim : ComponentBase {
        private ParticleCannonAxisPart[] axisList;
        private bool isAddUpdate = false;

        protected override void OnAwake() {
            mySystem.Register<CE_Weapon.Fire>(OnFireCallBack);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entityBase = (EntityBase)iEntity;
            axisList = entityBase.GetComponentsInChildren<ParticleCannonAxisPart>();
            for (int i = 0; i < axisList.Length; i++) {
                var axis = axisList[i];
                axis.Init();
            }
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            isAddUpdate = false;
            mySystem.Unregister<CE_Weapon.Fire>(OnFireCallBack);
        }

        private void OnUpdate(float deltaTime) {
            var isPlayAnim = false;
            for (int i = 0; i < axisList.Length; i++) {
                var axis = axisList[i];
                var isTrue = axis.UpdatePosition(deltaTime);
                if (isTrue) {
                    isPlayAnim = true;
                }
            }
            if (isPlayAnim == false) {
                RemoveUpdate(OnUpdate);
                isAddUpdate = false;
            }
        }

        private void OnFireCallBack(ISystemMsg body, CE_Weapon.Fire ent) {
            for (int i = 0; i < axisList.Length; i++) {
                var axis = axisList[i];
                axis.TriggerExpansion();
            }
            if (isAddUpdate == false) {
                AddUpdate(OnUpdate);
                isAddUpdate = true;
            }
        }
    }
}