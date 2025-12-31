using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class CharacterData {
        public static string NickName = "";
        public const int MaxPackWeight = 1000;
        public enum StandType : int {
            Stand = 0,
            Crouch = 1,
            Prone = 2
        }

        public enum JumpType : int {
            None = 0,
            Jump = 1,
            AirJump = 2,
            Fall = 3,
        }

        public enum FlyType : int {
            None = 0,
            OpenParachute = 1,
            Fly = 2,
        }

        public enum WirebugType : int {
            None = 0,
            Start = 1,
            Drop = 2,
            Glide = 3,
        }

        public static bool IsStand(StandType standType) {
            return standType == StandType.Stand;
        }

        public static bool IsCrouch(StandType standType) {
            return standType == StandType.Crouch;
        }

        public static bool IsProne(StandType standType) {
            return standType == StandType.Prone;
        }

        public static bool IsFly(FlyType flyType) {
            return flyType != FlyType.None;
        }

        public static bool IsWirebugMove(WirebugType wirebugType) {
            return wirebugType != WirebugType.None;
        }

        public static bool IsJump(JumpType jumpType) {
            return jumpType == JumpType.Jump || jumpType == JumpType.AirJump;
        }
    }
}