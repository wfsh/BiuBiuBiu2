using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSausageAICharacterSystem : S_SausageAI_Base {
        protected override void OnAwake() {
            AddComponents();
        }

        private void AddComponents() {
            AddComponent<ServerSausageAICharacterAttribute>();
            AddComponent<ServerSausageAICharacterHurt>();
        }

        protected override void OnStart() {
            CreateEntityObj($"Character/Server/ServerCharacter", StageData.GameWorldLayerType.Character);
        }
        
        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            var entity = (EntityBase)iEntity;
            entity.SetName(NickName + "_S");
        }
    }
}