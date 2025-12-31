using System;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public enum GameTestEnum {
        Scene,
    }
    public class GameTestComponent : MonoBehaviour {
        [SerializeField]
        private GameTestEnum gameTestEnum;
        void Start() {
            MsgRegister.Dispatcher(new M_Game.AddGameTestObj {
                GameTestIndex = (int)gameTestEnum,
                GameObj = gameObject,
            });
        }
    }
}