using Sofunny.BiuBiuBiu2.CoreGamePlay;
namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterAttribute : ServerGPOAttribute {
        protected ICharacterSync characterSync { get; private set; }

        protected override void OnSetNetwork() {
            base.OnSetNetwork();
            characterSync = (ICharacterSync)networkBase.GetNetworkSync();
            characterSync.SetATK(attributeData.ATK);
            characterSync.SetMaxHp(attributeData.maxHp);
            characterSync.SetHP(attributeData.nowHp);
        }

        protected override void ChangeATK() {
            base.ChangeATK();
            characterSync?.SetATK(ATK());
        }

        protected override void ChangeMaxHp() {
            base.ChangeMaxHp();
            characterSync?.SetMaxHp(attributeData.maxHp);
        }

        protected override void ChangeHP() {
            base.ChangeHP();
            characterSync?.SetHP(attributeData.nowHp);
        }
    }
}