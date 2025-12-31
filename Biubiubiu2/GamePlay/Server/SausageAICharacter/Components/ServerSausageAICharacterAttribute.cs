using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Message;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSausageAICharacterAttribute : ServerGPOAttribute {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_AICharacter.GetAiId>(OnGetAiIdCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_AICharacter.GetAiId>(OnGetAiIdCallBack);
        }
        
        private void OnGetAiIdCallBack(ISystemMsg body, SE_AICharacter.GetAiId ent) {
            ent.CallBack?.Invoke(((ServerSausageAICharacterSystem)mySystem).AIId);
        }
    }
}