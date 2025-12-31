using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityMissileLifeCycle : ComponentBase {
        private bool moving;
        private bool waitBombing;
        private int abilityId;
        private float timer;
        private float delayBombingDuration;

        protected override void OnAwake() {
            Register<SE_Ability.Ability_EndMissileBombing>(OnEndMissileBombing);
            Register<SE_Ability.Ability_MoveGrenadeEnd>(OnMoveGrenadeEnd);
            Register<SE_Ability.Ability_GetMissileMoveing>(OnGetMissileMoveing);
            
            var abilitySystem = (S_Ability_Base)mySystem;
            abilityId = abilitySystem.AbilityId;
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            Unregister<SE_Ability.Ability_EndMissileBombing>(OnEndMissileBombing);
            Unregister<SE_Ability.Ability_MoveGrenadeEnd>(OnMoveGrenadeEnd);
            Unregister<SE_Ability.Ability_GetMissileMoveing>(OnGetMissileMoveing);
        }

        protected override void OnStart() {
            AddUpdate(OnUpdate);
            moving = true;
            waitBombing = false;
        }

        private void OnUpdate(float deltaTime) {
            if (!waitBombing) {
                return;
            }
            timer += deltaTime;
            if (timer >= delayBombingDuration) {
                timer = 0;
                waitBombing = false;
                Dispatcher(new SE_Ability.Ability_StartMissileBombing());
            }
        }

        public void OnMoveGrenadeEnd(ISystemMsg body, SE_Ability.Ability_MoveGrenadeEnd ent) {
            moving = false;
            waitBombing = true;
        }
        
        public void OnGetMissileMoveing(ISystemMsg body, SE_Ability.Ability_GetMissileMoveing ent) {
            ent.CallBack(moving);
        }

        private void OnEndMissileBombing(ISystemMsg body, SE_Ability.Ability_EndMissileBombing ent) {
            MsgRegister.Dispatcher(new SM_Ability.RemoveAbility {
                AbilityId = abilityId
            });
        }
    }
}