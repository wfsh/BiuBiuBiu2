using System;
using UnityEngine;
using UnityEngine.U2D;
using BehaviorDesigner.Runtime;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;

namespace Sofunny.BiuBiuBiu2.Asset {
    public class AssetManager {
        private static long assetLoadId = 0;
        public static event Action<string, long, Action<UnityEngine.Object>> OnAssetLoad;
        public static event Action<string, long> OnAssetUnloader;
        public static event Action<string, long, bool> OnAssetBundleUnloader;
        public static event Action<string, long, Action<bool>> OnAssetBundleLoader;
        private static string cdnUrl;
        private static string gameInfoUrl;

        public static string CdnUrl {
            get { return cdnUrl; }
        }

        public static string GameInfoUrl {
            get { return gameInfoUrl; }
        }

        public static void Reload() {
        }

        public static void ChangeSendGameInfo() {
            gameInfoUrl = gameInfoUrl.Replace("game_info", "game_info_shenhe");
        }
        private static long GetAssetLoadID() {
            return ++assetLoadId;
        }

        public static void LoadGamePlayObjAsync(string sign, Action<GameObject> callBack) {
            var url = $"{AssetURL.GamePlay}/{sign}.prefab";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as GameObject);
                });
        }
        

        public static void LoadUI(string sign, Action<GameObject> callBack) {
            var url = $"{AssetURL.UI}/{sign}/UI{sign}.prefab";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as GameObject);
                });
        }

        public static void LoadCharacterAnimAsync(string weaponSign, Action<ScriptableObject> callBack) {
            var url = $"{AssetURL.Anim}/{weaponSign}.asset";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as ScriptableObject);
                });
        }

        public static void LoadSpriteAtlas(string sign, Action<SpriteAtlas> callBack) {
            var url = $"{AssetURL.UIAtlas}/{sign}.spriteatlas";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as SpriteAtlas);
                });
        }
        
        public static void LoadTexture(string sign, Action<Texture2D> callBack) {
            var url = $"{AssetURL.UITexture}/{sign}.png";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as Texture2D);
                });
        }

        public static void LoadAIBehavior(string sign, Action<ExternalBehavior> callBack) {
            var url = $"{AssetURL.AIBehavior}/{sign}.asset";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as ExternalBehavior);
                });
        }

        public static void LoadAnimationClipForFullUrl(string url, Action<AnimationClip> callBack) {
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as AnimationClip);
                });
        }

        public static void LoadAvatarMaskForFullUrl(string url, Action<AvatarMask> callBack) {
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as AvatarMask);
                });
        }

        public static void LoadPerfabForFullUrl(string url, Action<GameObject> callBack) {
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as GameObject);
                });
        }

        public static void LoadAudioForFullUrl(string url, Action<GameObject> callBack) {
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as GameObject);
                });
        }

        public static void LoadAudioMixerForFullUrl(string url, Action<AudioMixer> callBack) {
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as AudioMixer);
                });
        }

        public static void LoadScenesConfigs(string sign, Action<ScriptableObject> callBack) {
            var url = $"{AssetURL.ScenesConfigs}/{sign}.asset";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as ScriptableObject);
                });
        }
        
        public static void LoadAISO(string sign, Action<ScriptableObject> callBack) {
            var url = $"{AssetURL.AISO}/{sign}.asset";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as ScriptableObject);
                });
        }
        
        public static void LoadURPConfigs(string sign, Action<UniversalRenderPipelineAsset> callBack) {
            var url = $"{AssetURL.URPConfigs}/{sign}";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as UniversalRenderPipelineAsset);
                });
        }

        public static void LoadSO(string url, Action<ScriptableObject> callBack) {
            url = $"{url}.asset";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as ScriptableObject);
                });
        }

        public static void LoadCSV(string url, Action<TextAsset> callBack) {
            url = $"{url}.csv";
            OnAssetLoad?.Invoke(
                url, 
                GetAssetLoadID(), 
                (o) => {
                    callBack?.Invoke(o as TextAsset);
                });
        }
        
        public static GameObject LoadUIRoot() {
            return Resources.Load<GameObject>(AssetURL.UIRoot);
        }

        public static GameObject LoadMain() {
            return Resources.Load<GameObject>(AssetURL.Main);
        }

        public static GameObject LoadGameWorldLayer() {
            return Resources.Load<GameObject>(AssetURL.GameWorldLayer);
        }

        public static GameObject LoadUILoadBaseAsset() {
            return Resources.Load<GameObject>(AssetURL.UILoadBaseAsset);
        }
        

        public static void LoadSceneAB(string sign, Action callBack) {
#if BUILD_SERVER
            callBack();
            return;
#endif
            var url = $"{AssetURL.Scenes}/{sign}.unity";
            OnAssetBundleLoader?.Invoke(
                url, 
                GetAssetLoadID(), 
                (success) => {
                    callBack?.Invoke();
                });
        }

        public static void UnloadAsset(string url, long loadId) {
            OnAssetUnloader?.Invoke(
                url, 
                loadId
            );
        }

        public static void UnloadAssetBundle(string bundleName, bool unloadAsset, long loadId) {
            OnAssetBundleUnloader?.Invoke(
                bundleName, 
                loadId,
                unloadAsset
            );
        }
    }
}