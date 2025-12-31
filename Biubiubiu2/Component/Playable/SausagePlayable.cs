using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.Playable.Runtime;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Component {
    public partial class SausagePlayable {
        private static int index = 1;
        private PlayableController playable;
        private EntityAnimConfig config;
        public Action<EntityAnimConfig.EventData> OnAnimEvent;
        public Action<string, Action<string>> OnAssetUrlChange;
        private List<EntityAnimConfig.EventData> eventList = new List<EntityAnimConfig.EventData>();
        private bool isDebug = false;
        private string animSign;
        private Dictionary<int, EffectData> prefabDic = new Dictionary<int, EffectData>();
        private HitType[] partList;
        private Transform entity;
        private float prefabCheckTime = 0f;
        private bool isCloseLoadEffect = false;
        private bool isClear = false;
        private string entityName = "";
        private bool useAudio1P = false;
        
        public struct EffectData {
            public string AssetUrl;
            public int AnimId;
            public float LifeTime;
            public int Id;
            public float Time;
            public bool IsActionBreak;
            public GameObject Effect;
            public Transform EffectTran;
            public GPOData.PartEnum Part;
            public Vector3 DiffPoint;
            public float DiffScale;
        }
        private bool isInit = false;

        public void Init(Transform entity, Animator animator, EntityAnimConfig config, string animSign) {
            if (isInit) {
                return;
            }
            isInit = true;
            if (config == null) {
                Debug.LogError($"SausagePlayable Init Error config is null  {animSign}");
                return;
            }
            this.entity = entity;
            this.config = config;
            this.animSign = $"{index++}_{animSign}";
            GetBodyPart();
            playable = new PlayableController();
            playable.OnLoadAvateMask += OnLoadAvateMaskCallBack;
            playable.OnLoadAnimClip += OnLoadAnimClipCallBack;
            playable.OnLoadBlendTreeClip += OnLoadBlendTreeClipCallBack;
            playable.OnAnimEvent += OnAnimEventCallBack;
            playable.OnStartPlayAnim += OnStartPlayAnimCallBack;
            playable.Init(animator, config, this.animSign);
#if UNITY_EDITOR
            AddPlayableData(this.animSign, animator, this, playable);
#endif
        }
        
        public void Dispose() {
            isClear = true;
#if UNITY_EDITOR
            RemovePlayableData(animSign);
#endif
            ClearPrefab();
            OnAssetUrlChange = null;
            OnAnimEvent = null;
            playable?.Dispose();
            this.entity = null;
            playable = null;
            partList = null;
        }


        public void UserAudio1P() {
            useAudio1P = true;
        }

        public void CloseLoadEffect() {
            isCloseLoadEffect = true;
        }

        private void GetBodyPart() {
            partList = entity.GetComponentsInChildren<HitType>(true);
        }

        public void OnUpdate(float deltaTime) {
            if (isDebug) {
                Debug.Log("SausagePlayable OnUpdate:");
            }
            playable?.OnUpdate();
            UpdateAnimEvent();
            CheckPrefabTime();
        }

        public void StartDebug() {
            playable.IsDebug = true;
        }

        public void SetGroupSign(string sign) {
#if UNITY_EDITOR
            SetOperationType(this.animSign, sign, OperationType.SetGroupSign);
#endif
            playable?.SetGroupSign(sign);
        }

        public int PlayAnimSign(string playSign, Action<EntityAnimConfig.StateData> onEnd = null) {
            if (playable == null) {
                return 0;
            }
            return playable.PlayAnimSign(playSign, onEnd);
        }

        public void PlayAnimId(int animId, Action<EntityAnimConfig.StateData> onEnd = null) {
            if (!isInit) {
                return;
            }

            playable?.PlayAnimId(animId, onEnd);
        }

        public void StopSign(string playSign) {
            playable?.StopSign(playSign);
        }

        public void StopAnimId(int clipId) {
            playable?.StopAnimId(clipId);
        }

        public void SetParameterValue(int animId, float posX, float posY) {
            playable?.SetParameterValue(animId, posX, posY);
        }

        public void SetParameterValue(string animSign, float posX, float posY) {
            var animId = GetAnimIdForPlaySign(animSign);
            if (animId <= 0) {
                Debug.LogError($"SetParameterValue Error animId <= 0 {animSign}");
                return;
            }
            playable?.SetParameterValue(animId, posX, posY);
        }

        public void SetPlaySignTime(string playSign, float playTime) {
            var animId = GetAnimIdForPlaySign(playSign);
            if (animId <= 0) {
                Debug.LogError($"SetSpeed Error animId <= 0 {playSign}");
                return;
            }
            playable?.SetClipPlayTime(animId, playTime);
        }

        public int GetAnimIdForPlaySign(string playSign) {
            if (playable == null) {
                return 0;
            }
            return playable.GetAnimIdForPlaySign(playSign);
        }

        /// <summary>
        /// 动画片段当前播放时长
        /// </summary>
        /// <param name="animId"></param>
        public double GetPlayTime(int animId) {
            if (playable == null) {
                return 0f;
            }
            return playable.GetPlayTime(animId);
        }

        private void OnLoadAvateMaskCallBack(string url, Action<AvatarMask> callBack) {
            AssetManager.LoadAvatarMaskForFullUrl(url, clip => {
                if (isClear) {
                    return;
                }
                callBack(clip);
            });
        }

        private void OnLoadAnimClipCallBack(string url, Action<AnimationClip> callBack) {
#if UNITY_EDITOR
            var fileName = Path.GetFileNameWithoutExtension(url);
            SetOperationType(this.animSign, fileName, OperationType.PlayClip);
#endif
            AssetManager.LoadAnimationClipForFullUrl(url, clip => {
                if (isClear) {
                    return;
                }
                callBack(clip);
            });
        }

        private void OnLoadBlendTreeClipCallBack(string playSign, EntityAnimConfig.AnimBlendTreeData[] blendTreeDatas,
            Action<List<AnimationClip>> callBack) {
#if UNITY_EDITOR
            SetOperationType(this.animSign, playSign, OperationType.PlayBlendTree);
#endif
            var checkLen = blendTreeDatas.Length;
            var list = new List<AnimationClip>(checkLen);
            var loadNum = checkLen;
            for (int i = 0; i < checkLen; i++) {
                var treeData = blendTreeDatas[i];
                var url = config.GetAnimPath(treeData.AssetId);
                list.Add(null);
                var index = i;
                AssetManager.LoadAnimationClipForFullUrl(url, clip => {
                    if (isClear) {
                        return;
                    }
                    list[index] = clip;
                    loadNum--;
                    if (loadNum <= 0) {
                        callBack(list);
                    }
                });
            }
        }

        private void OnStartPlayAnimCallBack(int animId) {
            ActionBreakEvent(animId);
        }

        private void OnAnimEventCallBack(int animId, EntityAnimConfig.EventData eventData) {
            if (ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                return;
            }
            for (int i = 0; i < eventList.Count; i++) {
                var data = eventList[i];
                if (data.Id == eventData.Id) {
                    return;
                }
            }
            eventData.AnimId = animId;
            eventList.Add(eventData);
            OnAnimEvent?.Invoke(eventData);
        }
        
        private void UpdateAnimEvent() {
            var len = eventList.Count;
            for (int i = 0; i < len; i++) {
                var data = eventList[i];
                try {
                    switch (data.EventDataType) {
                        case EntityAnimConfig.EventDataType.Prefab:
                            PrefabEvent(data);
                            break;
                        case EntityAnimConfig.EventDataType.Audio:
                            AudioEvent(data);
                            break;
                        case EntityAnimConfig.EventDataType.WWise:
                            WWiseEvent(data);
                            break;
                    }
                } catch (Exception e) {
                    Debug.LogError($"AnimEvent Error {e}");
                }
            }
            eventList.RemoveRange(0, len);
        }

        private void AudioEvent(EntityAnimConfig.EventData eventData) {
            if (NetworkData.Config.IsStartClient == false || isCloseLoadEffect) {
                return;
            }
            var url = eventData.AssetUrl;
            if (useAudio1P == false) {
                url = eventData.AssetUrl2;
            }
            url = ChangeAssetUrl(url);
            AudioPoolManager.OnPlayAudio(url, entity.position);
        }
        private void WWiseEvent(EntityAnimConfig.EventData eventData) {
            if (NetworkData.Config.IsStartClient == false || isCloseLoadEffect) {
                return;
            }
            var url = eventData.AssetUrl;
            if (useAudio1P == false) {
                url = eventData.AssetUrl2;
            }
            url = ChangeAssetUrl(url);
            var lifeTime = eventData.LifeTime <= 0f ? 2f : eventData.LifeTime;
            AudioPoolManager.OnPlayWWise(url, lifeTime, entity.position, AudioPoolManager.AudioTypeEnum.Audio);
        }

        private string ChangeAssetUrl(string url) {
            if (OnAssetUrlChange != null) {
                OnAssetUrlChange.Invoke(url, newUrl => {
                    url = newUrl;
                });
            }
            return url;
        }

        private void PrefabEvent(EntityAnimConfig.EventData eventData) {
            if (NetworkData.Config.IsStartClient == false) {
                return;
            }
            if (isCloseLoadEffect) {
                return;
            }
            var parent = GetPerfabParent(eventData.Part);
            if (parent == null) {
                return;
            }
            if (eventData.IsShow) {
                EffectData data;
                if (prefabDic.TryGetValue(eventData.EventId, out data)) {
                    UpdateEffectData(data);
                } else {
                    var url = eventData.AssetUrl;
                    url = ChangeAssetUrl(url);
                    PrefabPoolManager.OnGetPrefab(url, parent, obj => {
                        if (isClear || prefabDic.ContainsKey(eventData.EventId)) {
                            PrefabPoolManager.OnReturnPrefab(url, obj);
                            return;
                        }
                        if (obj == null) {
                            Debug.Log($"PrefabEvent obj null {url}:");
                            return;
                        }
                        AddEffectData(obj, url, eventData);
                    });
                }
            } else {
                EffectData data;
                if (prefabDic.TryGetValue(eventData.EventId, out data)) {
                    RemoveEffectData(eventData.EventId, data);
                }
            }
        }

        private void AddEffectData(GameObject obj, string url, EntityAnimConfig.EventData eventData) {
            var effectData = new EffectData();
            effectData = new EffectData();
            effectData.Id = eventData.EventId;
            effectData.AssetUrl = url;
            effectData.Effect = obj;
            effectData.AnimId = eventData.AnimId;
            effectData.LifeTime = eventData.LifeTime == 0f ? 1f : eventData.LifeTime;
            effectData.EffectTran = obj.transform;
            effectData.IsActionBreak = eventData.IsActionBreak;
            effectData.DiffPoint = eventData.DiffPos;
            effectData.DiffScale = eventData.DiffScale == 0f ? 1f : eventData.DiffScale;
            effectData.Part = eventData.Part;
            prefabDic.Add(eventData.EventId, effectData);
            obj.name = animSign;
            UpdateEffectData(effectData);
        }

        private void UpdateEffectData(EffectData data) {
            var tran = data.EffectTran;
            if (data.Part == GPOData.PartEnum.Controller || data.Part == GPOData.PartEnum.None) {
                tran.position = entity.position + data.DiffPoint;
                tran.rotation = entity.rotation;
            } else {
                tran.localPosition = Vector3.zero + data.DiffPoint;
                tran.localRotation = Quaternion.identity;
            }
            tran.localScale = Vector3.one * data.DiffScale;
            if (Time.realtimeSinceStartup - data.Time > data.LifeTime) {
                data.Effect.SetActive(false);
                data.Effect.SetActive(true);
                data.Time = Time.realtimeSinceStartup;
            }
            prefabDic[data.Id] = data;
        }
        

        private void ActionBreakEvent(int playAnimId) {
            var keys = prefabDic.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++) {
                var key = keys[i];
                var value = prefabDic[key];
                if (value.IsActionBreak == false) {
                    continue;
                }
                if (value.AnimId != playAnimId) {
                    value.Time = 0f;
                    prefabDic[key] = value;
                }
            }
        }

        private Transform GetPerfabParent(GPOData.PartEnum part) {
            Transform parent = null;
            if (part == GPOData.PartEnum.Controller || part == GPOData.PartEnum.None) {
                parent = entity.parent;
            } else {
                parent = GetBodyTran(part);
            }
            return parent;
        }

        private void CheckPrefabTime() {
            if (prefabCheckTime > 0f) {
                prefabCheckTime -= Time.deltaTime;
                return;
            }
            prefabCheckTime = 1f;
            var time = Time.realtimeSinceStartup;
            var keys = prefabDic.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++) {
                var key = keys[i];
                var value = prefabDic[key];
                if (value.Effect == null) {
                    Debug.LogWarning($"CheckPrefabTime effect null   {value.AssetUrl} -  {this.animSign}");
                    prefabDic.Remove(key);
                    continue;
                }
                if (time - value.Time > value.LifeTime) {
                    RemoveEffectData(key, value);
                }
            }
        }

        private void RemoveEffectData(int key, EffectData effectData) {
            PrefabPoolManager.OnReturnPrefab(effectData.AssetUrl, effectData.Effect);
            prefabDic.Remove(key);
        }

        private void ClearPrefab() {
            foreach (var value in prefabDic.Values) {
                if (value.Effect != null) {
                    PrefabPoolManager.OnReturnPrefab(value.AssetUrl, value.Effect);
                } else {
                    Debug.LogWarning($"ClearPrefab effect null  {this.animSign} {value.AssetUrl}");
                }
            }
            prefabDic.Clear();
        }

        public void SetIsDebug(bool isDebug) {
            this.isDebug = isDebug;
            if (playable != null) {
                playable.IsDebug = isDebug;
            }
        }

        public bool GetIsDebug() {
            return this.isDebug;
        }

        public Transform GetBodyTran(GPOData.PartEnum partType) {
            for (int i = 0; i < partList.Length; i++) {
                var hitType = partList[i];
                if (hitType.Part == partType) {
                    return hitType.transform;
                }
            }
            return null;
        }
    }
}