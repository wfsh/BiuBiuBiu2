using Sofunny.BiuBiuBiu2.ClientMessage;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherSystem : C_Character_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityObj("Character/OtherCharacter", StageData.GameWorldLayerType.Character);
        }

        private void AddComponents() {
            AddComponent<ClientCharacterAttribute>();
            AddComponent<CharacterOtherNetwork>();
            AddComponent<CharacterOtherMove>();
            AddComponent<ClientGPODead>();
            AddComponent<ClientGPOWeaponPack>();
            AddComponent<CharacterOtherWeapon>();
            AddComponent<CharacterOtherAerocraft>();
            AddComponent<CharacterLookForward>();
            AddComponent<CharacterBodyRota>();
            AddComponent<CharacterOtherWirebug>();
            AddComponent<ClientGPOAttribute>();
            AddComponent<ClientCharacterPlatformMovement>();
            AddComponent<ClientCharacterDriveAnim>();
            AddComponent<CharacterOtherDrive>();
            AddComponent<CharacterOtherSlideState>();
            AddComponent<ClientGPOGodMode>();
            AddComponent<ClientGPOShowEntity>();
            AddComponent<CliengGPOOtherTeamMaterial>();
            AddComponent<ClientGPOAbilityEffect>();
            AddComponent<CharacterGPOHoldOn>();
            AddComponent<CharacterGPOSkill>();
            AddComponent<CharacterGPOPickItem>();
            AddComponent<CharacterGPOCamera>();
            if (ModeData.PlayMode != ModeData.ModeEnum.SausageGoldDash) {
                AddComponent<CharacterAnimator>();
            }
            // 香肠对接
            AddComponent<ClientCharacterToSausageMan>();
        }
    }
}