using System;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAceJokerPreAnimLoadEffect : ComponentBase {

        protected override void OnAwake() {
        }

        protected override void OnStart() {
            base.OnStart();
            PreLoadEffect("AceJoker/Sfx_GoldDash_BossAceJoker_RocketBomb_Muzzle_Option");
            PreLoadEffect("GoldJoker/Sfx_GoldDash_BossSupremeJoker_RocketBomb_Muzzle_Option");
        }

        protected override void OnSetNetwork() {
        }

        protected override void OnClear() {
            base.OnClear();
        }

        private void PreLoadEffect(string effectUrl) {
            var poolUrl = $"{AssetURL.GamePlay}/Ability/{effectUrl}.prefab";
            PrefabPoolManager.OnGetPrefab(poolUrl, null, o => { PrefabPoolManager.OnReturnPrefab(poolUrl, o); });
            PrefabPoolManager.OnGetPrefab(poolUrl, null, o => { PrefabPoolManager.OnReturnPrefab(poolUrl, o); });
        }
    }
}