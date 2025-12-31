using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;
using AudioSource = UnityEngine.AudioSource;

namespace Sofunny.BiuBiuBiu2.Asset {
    public class AudioPoolManager {
        public enum AudioTypeEnum : byte {
            Audio = 1,
            BGM = 2,
        }
        public static event Action<string, int, bool, GameObject, Vector3, Action<uint>> OnPlayWWiseEvent;
        public static event Action<uint> OnStopWWiseEvent;
        private static bool useInstantiateAsync = true;
        public static int MaxAudioPerAsset = 10;
        public static AudioPoolManager Instance;
        private Dictionary<string, List<PlayAudioData>> audioPool = new Dictionary<string, List<PlayAudioData>>();
        private Dictionary<string, List<PlayAudioData>> activeAudioGroups = new Dictionary<string, List<PlayAudioData>>();
        private GameObject poolRoot;
        private Transform poolRootTransform;
        private float cleanupInterval = 1f;
        private float checkLifeTimeInterval = 1f;
        private float maxInactiveTime = 60f;
        private int InitIndex = 0;
        private int audioVolume = 100;
        private int bgmVolume = 100;
        public class PlayAudioData {
            public GameObject audioObj;
            public AudioTypeEnum audioType;
            public AudioSource audioSource;
            private Transform audioTransform;
            public bool loop;
            public float playTime;
            public float lifeTime;
            public Action OnPlayEnd;
            public string AssetSign;
            public uint wwisePlayId;
            private bool isPlayAudio = false;
            public bool IsClear {
                get;
                private set;
            }
            public bool IsPlay {
                get {
                    if (audioSource != null) {
                        return audioSource.isPlaying;
                    }
                    return isPlayAudio;
                }
            }
            public void Clear() {
                if (IsClear) {
                    return;
                }
                Stop();
                audioTransform = null;
                if (audioObj != null) {
                    GameObject.Destroy(audioObj);
                    audioObj = null;
                }
                audioSource = null;
                OnPlayEnd = null;
                IsClear = true;
            }

            public void Init() {
                IsClear = false;
                isPlayAudio = true;
                audioTransform = audioObj.transform;
            }

            public void Stop() {
                if (audioSource != null && audioSource.isPlaying) {
                    audioSource.Stop();
                }
                if (wwisePlayId != 0) {
                    OnStopWWiseEvent?.Invoke(wwisePlayId);
                    wwisePlayId = 0;
                }
                OnPlayEnd?.Invoke();
                isPlayAudio = false;
                playTime = Time.realtimeSinceStartup;
            }

            public void SetPoint(Vector3 point) {
                if (IsClear) {
                    return;
                }
                audioTransform.position = point;
            }

            public void SetVolume(int volume) {
                if (IsClear) {
                    return;
                }
                audioSource.volume = Mathf.Clamp01(volume * 0.01f);
            }

            public void SetPlayTime(float time) {
                if (IsClear) {
                    return;
                }
                if (time <= 0f) {
                    playTime = -Time.realtimeSinceStartup + 9999f;
                } else {
                    playTime = Time.realtimeSinceStartup + time;
                }
            }
        }

        public static void Init() {
            if (Instance != null) {
                Dispose();
            }
            Instance = new AudioPoolManager();
            Instance.OnInit();
        }

        public static void Update(float deltaTime) {
            if (Instance == null) {
                return;
            }
            Instance.OnUpdate(deltaTime);
        }
        public static void Dispose() {
            if (Instance != null) {
                Instance.OnDispose();
                Instance = null;
            }
        }
        public static void SetUseInstantiateAsync(bool use) {
            useInstantiateAsync = use;
        }
        public static void OnPlayBGM(string assetUrl, Action<PlayAudioData> callBack = null) {
            Instance?.PlayAudio(assetUrl, Vector3.zero, AudioTypeEnum.BGM, callBack);
        }
        public static void OnPlayAudio2D(string assetUrl, Action<PlayAudioData> callBack = null) {
            Instance?.PlayAudio(assetUrl, Vector3.zero, AudioTypeEnum.Audio, callBack);
        }

        public static void OnPlayAudio(string assetUrl, Vector3 position, Action<PlayAudioData> callBack = null) {
            return;
            try {
                Instance?.PlayAudio(assetUrl, position, AudioTypeEnum.Audio, callBack);
            } catch (Exception e) {
                Debug.LogError($"[AudioPoolManager] Error play audio: {assetUrl}, Exception: {e}");
            }
        }

        public static void OnPlayWWise(string wwiseSign, float playTime, Vector3 position, AudioTypeEnum audioType, Action<PlayAudioData> callBack = null) {
            Instance?.PlayWWise(wwiseSign, playTime, position, audioType, callBack);
        }

        public static void SetMaxAudioPerAsset(int value) {
            MaxAudioPerAsset = value;
        }
        private void OnInit() {
            InitIndex++;
            Instance = this;
            poolRoot = new GameObject("[AudioPoolManager]");
            poolRootTransform = poolRoot.transform;
            GameObject.DontDestroyOnLoad(poolRoot);
#if !BUILD_SERVER
            Debug.Log("OnInit AudioPoolManager");
#endif
        }

        private void OnDispose() {
            UpdateRegister.RemoveUpdate(OnUpdate);
            Instance = null;
            ClearActiveAudio();
            ClearAudioPool();
            poolRoot = null;
        }

        private void ClearAudioPool() {
            foreach (var pair in audioPool) {
                foreach (var audioData in pair.Value) {
                    audioData.Clear();
                }
            }
            audioPool.Clear();
        }

        private void ClearActiveAudio() {
            foreach (var pair in activeAudioGroups) {
                foreach (var audioData in pair.Value) {
                    audioData.Clear();
                }
            }
            activeAudioGroups.Clear();
        }

        private void OnUpdate(float deltaTime) {
            UpdateAudio(deltaTime);
            UpdatePoolAudioLife(deltaTime);
        }

        // 播放音效
        public void PlayWWise(string wwiseSign, float playTime, Vector3 position, AudioTypeEnum audioType, Action<PlayAudioData> callBack) {
            if (string.IsNullOrEmpty(wwiseSign)) {
                return;
            }
            List<PlayAudioData> audioQueue;
            if (!activeAudioGroups.TryGetValue(wwiseSign, out audioQueue)) {
                audioQueue = new List<PlayAudioData>();
                activeAudioGroups[wwiseSign] = audioQueue;
            }
            PlayAudioData playAudioData;
            // 超过最大限制时，复用最旧的 AudioSource
            if (audioQueue.Count >= MaxAudioPerAsset) {
                playAudioData = audioQueue[0];
                playAudioData.Stop();
                audioQueue.RemoveAt(0);
                SetPlayWWiseData(audioQueue, playTime, playAudioData,  position, audioType, wwiseSign);
                callBack?.Invoke(playAudioData);
                return;
            }
            GetWWiseSource(wwiseSign, audioData => {
                SetPlayWWiseData(audioQueue, playTime, audioData, position,audioType,  wwiseSign);
                callBack?.Invoke(audioData);
            });

        }

        // 播放音效
        public void PlayAudio(string assetUrl, Vector3 position, AudioTypeEnum audioType,  Action<PlayAudioData> callBack) {
            if (string.IsNullOrEmpty(assetUrl)) {
                return;
            }
            List<PlayAudioData> audioQueue;
            if (!activeAudioGroups.TryGetValue(assetUrl, out audioQueue)) {
                audioQueue = new List<PlayAudioData>();
                activeAudioGroups[assetUrl] = audioQueue;
            }
            PlayAudioData playAudioData;
            // 超过最大限制时，复用最旧的 AudioSource
            if (audioQueue.Count >= MaxAudioPerAsset) {
                playAudioData = audioQueue[0];
                audioQueue.RemoveAt(0);
                playAudioData.audioSource.Stop(); // 停止播放当前音频
                SetPlayAudioData(audioQueue, playAudioData, position,audioType,  assetUrl);
                callBack?.Invoke(playAudioData);
                return;
            }
            GetAudioSource(assetUrl, audioData => {
                SetPlayAudioData(audioQueue, audioData, position,audioType,  assetUrl);
                callBack?.Invoke(audioData);
            });
        }

        private void SetPlayAudioData(List<PlayAudioData> audioQueue, PlayAudioData playAudioData, Vector3 position, AudioTypeEnum audioType, string assetUrl) {
            var audioSource = playAudioData.audioSource;
            audioSource.transform.position = position;
            audioSource.enabled = true;
            audioSource.Play();
            playAudioData.audioType = audioType;
            playAudioData.audioObj = audioSource.gameObject;
            playAudioData.loop = audioSource.loop;
            playAudioData.SetPlayTime(audioSource.clip.length);
            playAudioData.lifeTime = Time.realtimeSinceStartup;
            playAudioData.AssetSign = assetUrl;
            // 添加到活动队列
            audioQueue.Add(playAudioData);
        }
        private void SetPlayWWiseData(List<PlayAudioData> audioQueue, float playTime, PlayAudioData playAudioData, Vector3 position,  AudioTypeEnum audioType, string wwiseSign) {
            playAudioData.SetPoint(position);
            playAudioData.SetPlayTime(playTime);
            playAudioData.lifeTime = Time.realtimeSinceStartup;
            playAudioData.AssetSign = wwiseSign;
            playAudioData.audioType = audioType;
            // 添加到活动队列
            audioQueue.Add(playAudioData);
            OnPlayWWiseEvent?.Invoke(wwiseSign, (int)audioType, true, playAudioData.audioObj, position, wwisePlayID => {
                playAudioData.wwisePlayId = wwisePlayID;
            });
        }

        // 获取一个 WWiseSource
        private void GetWWiseSource(string wwiseSign, Action<PlayAudioData> callBack) {
            PlayAudioData playAudioData;
            List<PlayAudioData> pools;
            if (audioPool.TryGetValue(wwiseSign, out pools) == false) {
                pools = new List<PlayAudioData>();
                audioPool.Add(wwiseSign, pools);
            }
            while (pools.Count > 0) {
                playAudioData = pools[0];
                pools.RemoveAt(0);
                if (!playAudioData.IsClear) {
                    callBack?.Invoke(playAudioData);
                    return;
                }
            } 
            var audioObj = new GameObject("wwiseObj");
            audioObj.transform.SetParent(poolRootTransform);
            playAudioData = new PlayAudioData();
            playAudioData.audioObj = audioObj;
            playAudioData.Init();
            callBack?.Invoke(playAudioData);
        }

        // 获取一个 AudioSource
        private void GetAudioSource(string assetUrl, Action<PlayAudioData> callBack) {
            PlayAudioData playAudioData;
            List<PlayAudioData> pools;
            if (audioPool.TryGetValue(assetUrl, out pools) == false) {
                pools = new List<PlayAudioData>();
                audioPool.Add(assetUrl, pools);
            }
            while (pools.Count > 0) {
                playAudioData = pools[0];
                pools.RemoveAt(0);
                if (!playAudioData.IsClear) {
                    callBack?.Invoke(playAudioData);
                    return;
                }
            } 
            var loadIndex = InitIndex;
            AssetManager.LoadAudioForFullUrl(assetUrl, async sourceObj => {
                if (loadIndex != InitIndex) {
                    return;
                }
                if (sourceObj != null) {
                    var audioObj = await InstantiateAsyncWithTask(sourceObj, poolRootTransform);
                    var source = audioObj.GetComponent<AudioSource>();
                    playAudioData = new PlayAudioData();
                    playAudioData.audioObj = audioObj;
                    playAudioData.audioSource = source;
                    playAudioData.Init();
                    callBack?.Invoke(playAudioData);
                } else {
                    Debug.LogError($"Failed to load audio: {assetUrl}");
                }
            });
        }
  
        public async Task<GameObject> InstantiateAsyncWithTask(GameObject prefab, Transform parent) {
            if (useInstantiateAsync == false) {
                var go = GameObject.Instantiate(prefab);
                if (go != null && parent != null) {
                    go.transform.SetParent(parent, false);
                }
                return go;
            } else {
                var request = GameObject.InstantiateAsync(prefab); 
                while (!request.isDone) {
                    await Task.Yield();
                }
                var go = request.Result[0];
                if (go != null && parent != null) {
                    go.transform.SetParent(parent, false);
                }
                return go;
            }
        }

        // 更新活动音频
        private void UpdateAudio(float deltaTime) {
            if (cleanupInterval > 0) {
                cleanupInterval -= deltaTime;
                return;
            }
            cleanupInterval = 0.5f;
            var time = Time.realtimeSinceStartup;
            foreach (var pair in activeAudioGroups) {
                var audioQueue = pair.Value;
                var len = audioQueue.Count - 1;
                for (int i = len; i >= 0; i--) {
                    var audioData = audioQueue[i];
                    if (audioData.IsClear || time > audioData.playTime || !audioData.IsPlay) {
                        audioData.Stop();
                        audioQueue.RemoveAt(i);
                        ReturnToPool(pair.Key, audioData);
                    } else {
                        audioQueue[i] = audioData;
                    }
                }
            }
        }

        private void UpdatePoolAudioLife(float deltaTime) {
            if (checkLifeTimeInterval > 0) {
                checkLifeTimeInterval -= deltaTime;
                return;
            }
            checkLifeTimeInterval = 2f;
            var time = Time.realtimeSinceStartup;
            foreach (var pair in audioPool) {
                var pools = pair.Value;
                var len = pools.Count - 1;
                for (int i = len; i >= 0; i--) {
                    var audioData = pools[i];
                    if (audioData.IsClear || audioData.lifeTime > 0f && time - audioData.lifeTime > maxInactiveTime) {
                        audioData.Clear();
                        pools.RemoveAt(i);
                    }
                }
            }
        }

        private void ReturnToPool(string assetUrl, PlayAudioData playAudioData) {
            if (playAudioData.IsClear) {
                return;
            }
            List<PlayAudioData> pools;
            if (audioPool.TryGetValue(assetUrl, out pools) == false) {
                playAudioData.Clear();
                return;
            }
            playAudioData.lifeTime = Time.realtimeSinceStartup;
            playAudioData.OnPlayEnd = null;
            var audioSource = playAudioData.audioSource;
            if (audioSource) {
                audioSource.Stop();
                audioSource.enabled = false;
            }
            pools.Add(playAudioData);
        }

        public void SetAudioVolume(int volume) {
            audioVolume = volume;
            foreach (var pair in activeAudioGroups) {
                foreach (var audioData in pair.Value) {
                    if (audioData.audioType == AudioTypeEnum.Audio && !audioData.IsClear) {
                        audioData.SetVolume(volume);
                    }
                }
            }
        }

        public void SetBGMVolume(int volume) {
            bgmVolume = volume;
            foreach (var pair in activeAudioGroups) {
                foreach (var audioData in pair.Value) {
                    if (audioData.audioType == AudioTypeEnum.BGM && !audioData.IsClear) {
                        audioData.SetVolume(volume);
                    }
                }
            }
        }
    }
}