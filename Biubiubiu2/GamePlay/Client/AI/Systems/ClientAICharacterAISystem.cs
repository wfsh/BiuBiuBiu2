using System;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAICharacterAISystem : C_AI_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            iEntity.SetPoint(startPoint);
            iEntity.SetRota(startRot);
            CreateEntityToPool(AISkinSign);
        }

        protected override void OnClear() {
            base.OnClear();
        }
        override protected void AddComponents() {
            base.AddComponents();
            AddComponent<ClientAIAttribute>();
            AddComponent<ClientCharacterAIAnim>();
            AddComponent<ClientGPOWeaponPack>();
            AddComponent<ClientAIWeapon>();
            AddComponent<ClientCharacterAISlide>();
            AddComponent<ClientCharacterDriveAnim>();
            AddComponent<CharacterGPODrive>();
            AddComponent<CliengGPOOtherTeamMaterial>();
        }
    }
}