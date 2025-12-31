using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerModeTime : ComponentBase {
        private const float UPDATE_INTERVAL = 1F;
        private float updateInterval = UPDATE_INTERVAL;
        private float roundTime = 0.0f;
        private float gameTime = 0.0f;
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;

        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_GameState>(OnSetGameStateCallBack);;
        }
         
        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Mode.Event_GameState>(OnSetGameStateCallBack);
        }

        private void OnUpdate(float deltaTime) {
            updateInterval -= deltaTime;
            if (updateInterval <= 0) {
                gameTime += UPDATE_INTERVAL;
                MsgRegister.Dispatcher(new SM_Mode.UpdateGameTime {
                    GameTime = (int)gameTime,
                });
                if (gameState == ModeData.GameStateEnum.RoundStart) {
                    roundTime += UPDATE_INTERVAL;
                    MsgRegister.Dispatcher(new SM_Mode.UpdateRoundTime {
                        RoundTime = (int)roundTime,
                    });
                }
                updateInterval = UPDATE_INTERVAL;
            }
        }

        private void OnSetGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState ent) {
            gameState = ent.GameState;
        }
    }
}