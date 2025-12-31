using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashCharacterSystem : S_Character_Base {
        public GPOM_Character useMData;

        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityObj($"Character/Server/ServerCharacter", StageData.GameWorldLayerType.Character);
        }

        private void AddComponents() {
            AddComponent<ServerGoldDashCharacterNetwork>();
            AddComponent<ServerCharacterDead>();
            AddComponent<ServerGPOFollowPoint>();
            AddComponent<ServerCharacterAnimator>();
            AddComponent<ServerGoldDashCharacterAttribute>();
            AddComponent<ServerCharacterToSausageMan>();
            AddComponent<ServerGoldDashCharacterHurt>();
        }
    }
}