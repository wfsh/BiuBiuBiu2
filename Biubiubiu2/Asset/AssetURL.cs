using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Asset {
    public class AssetURL {
        public const string Main = "GoldDash/Sausage2Base/Sausage2Main";
        public const string UIRoot = "GoldDash/Sausage2Base/Sausage2UIRoot";
        public const string UILoadBaseAsset = "GoldDash/Sausage2Base/Sausage2UILoadBaseAsset";
        public const string GameWorldLayer = "GoldDash/Sausage2Base/Sausage2GameWorldLayer";
        public const string UI = "Assets/ToBundle/GoldDash/Sausage2/UI";
        public const string UIAtlas = "Assets/ToBundle/GoldDash/Sausage2/UI/Atlas";
        public const string UITexture = "Assets/Bundle/UI/Textures";
        public const string AIBehavior = "Assets/ToBundle/GoldDash/Sausage2/GamePlay/AIBehavior";
        public const string GamePlay = "Assets/ToBundle/GoldDash/Sausage2/GamePlay";
        public const string Effect = "Assets/ToBundle/GoldDash/Sausage2/GamePlay/Effects";
        public const string Anim = "Assets/ToBundle/GoldDash/Sausage2/GamePlay/Anim";
        public const string Scenes = "Assets/Scenes/GoldDash/Sausage2/Runtime";
        public const string ScenesConfigs = "Assets/ToBundle/GoldDash/Sausage2/GamePlay/Scene";
        public const string URPConfigs = "Assets/ToBundle/GoldDash/Sausage2/Graphic/RenderPipeline";
        public const string Audio = "Assets/Audio/Source";
        public const string UIEffect = "Assets/Bundle/GamePlay/Effects/UI";
        public const string AbilitySO = "Assets/Script/Biubiubiu2/Editor/AbilityEditor/";
        public const string AbilityCSV = "Assets/ToBundle/GoldDash/Sausage2/Configs/Ability";
        public static string AISO = "Assets/ToBundle/GoldDash/Sausage2/Configs/AI";

        public static string GetAudio1P(string name) {
            return string.Concat(Audio, "/1P/", name, ".prefab");
        }

        public static string GetAudio3P(string name) { 
            return string.Concat(Audio, "/3P/", name, ".prefab");
        }

        public static string GetAudioUI(string name) {
            return string.Concat(Audio, "/UI/", name, ".prefab");
        }

        public static string GetAudioMusic(string name) {
            return string.Concat(Audio, "/Music/", name, ".prefab");
        }

        public static string GetEffect(string name) {
            return string.Concat(Effect, "/", name, ".prefab");
        }

        public static string GetUIEffect(string name) {
            return string.Concat(UIEffect, "/", name, ".prefab");
        }
    }
}