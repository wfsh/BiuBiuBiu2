using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterAttribute : ClientGPOAttribute {
        protected ICharacterSync characterSync {
            get;
            private set;
        }

        
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            characterSync = (ICharacterSync)networkBase.GetNetworkSync();
        }

        private void OnUpdate(float deltaTime) {
            if (characterSync == null || networkBase == null || networkBase.IsDestroy()) {
                return;
            }
            if (characterSync.GetMaxHp() != attributeData.maxHp) {
                attributeData.maxHp = characterSync.GetMaxHp();
                ChangeMaxHp();
            }
            if (characterSync.GetATK() != attributeData.ATK) {
                attributeData.ATK = characterSync.GetATK();
            }
            if (characterSync.GetHP() != attributeData.nowHp) {
                attributeData.nowHp = characterSync.GetHP();
                ChangeHP();
            }
            if (characterSync.GetLevel() != attributeData.Level) {
                attributeData.Level = characterSync.GetLevel();
                ChangeLevel();
            }
        }
    }
}