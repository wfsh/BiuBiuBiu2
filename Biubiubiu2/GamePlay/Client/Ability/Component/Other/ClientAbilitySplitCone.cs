using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAbilitySplitCone : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int FireGpoId;
        }
        private IGPO localGpo;
        private int fireGpoId;
        private float checkTime;
        private bool checkPlayAudio;
        private const float CHECK_GPO_DURATION = 1f;

        protected override void OnAwake() {
            checkPlayAudio = true;
            var initData = (InitData)initDataBase;
            SetFireGpo(initData.FireGpoId);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float deltaTime) {
            if (!checkPlayAudio) {
                return;
            }
            CheckPlayAudio(deltaTime);
        }

        public void SetFireGpo(int fireGpoId) {
            this.fireGpoId = fireGpoId;
        }

        private void CheckPlayAudio(float deltaTime) {
            checkTime -= deltaTime;
            if (checkTime > 0) {
                return;
            }
            checkTime = CHECK_GPO_DURATION;
            if (localGpo == null) {
                MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                    CallBack = gpo => {
                        localGpo = gpo;
                    }
                });
            }
            if (localGpo == null) {
                return;
            }
            checkPlayAudio = false;
            var config = (AbilityData.PlayAbility_SplitCone)AbilityConfig.GetAbilityModData(AbilityConfig.BulletSplitCone);
            AudioPoolManager.OnPlayAudio(fireGpoId == localGpo.GetGpoID() ? AssetURL.GetAudio1P(config.M_AudioSign) : AssetURL.GetAudio3P($"{config.M_AudioSign}_3P"), iEntity.GetPoint());
        }
    }
}