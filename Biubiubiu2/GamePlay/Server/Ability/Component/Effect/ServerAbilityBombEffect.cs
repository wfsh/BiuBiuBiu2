using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityBombEffect : ComponentBase{
        public class InitData : SystemBase.IComponentInitData {
            public IAbilityEffectMData MData;
            public IAbilityEffectInData InData;
        }

        private S_Ability_Base abSystem;
        private IGPO fireGPO;
        private InitData initData;
        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (S_Ability_Base)mySystem;
            fireGPO = abSystem.FireGPO;
            mySystem.Register<SE_Ability.HitGPO>(HitCallBack);
            initData = (InitData)initDataBase;
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.HitGPO>(HitCallBack);
        }

        private void HitCallBack(ISystemMsg arg1, SE_Ability.HitGPO entData) {
            if (entData.hitGPO.IsClear()) {
                return;
            }
            
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityEffect {
                FireGPO = fireGPO,
                TargetGPO = entData.hitGPO,
                MData = initData.MData,
                InData = initData.InData,
            });
        }
    }
}