using System;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 使用该组件优先考虑。音频是否一定需要从网络下发。是否可以走动作编辑器
    /// </summary>
    public class ClientWWiseAudio : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public ushort WWiseID;
            public bool IsFollow;
            public float LifeTime;
        }
        private ushort wwiseID;
        private bool isFollow;
        private float playTime;
        private AudioPoolManager.PlayAudioData playAudioData;
        private C_Ability_Base ability;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            this.isFollow = initData.IsFollow;
            this.wwiseID = initData.WWiseID;
            if (initData.LifeTime <= 0) {
                this.playTime = -1f;
            }
        }
        protected override void OnStart() {
            base.OnStart();
            PlayWWise();
            if (isFollow && this.wwiseID > 0) {
                AddUpdate(OnUpdate);
            }
        }
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            if (playAudioData != null) {
                playAudioData.Stop();
                playAudioData = null;
            }
        }
        private void OnUpdate(float deltaTime) {
            playAudioData?.SetPoint(iEntity.GetPoint());
        }

        private void PlayWWise() {
            if (this.wwiseID <= 0) {
                return;
            }
            if (WwiseAudioSet.HasWwiseAudioById(wwiseID) == false) {
                Debug.LogError("[Error] ClientWWiseAudio 播放音频失败，没有找到音频配置，ID:" + wwiseID);
                return;
            }
            var data = WwiseAudioSet.GetWwiseAudioById(wwiseID);
            AudioPoolManager.OnPlayWWise(data.WwiseEvent, playTime,iEntity.GetPoint(), (AudioPoolManager.AudioTypeEnum)data.AudioType, playData => {
                if (isClear) {
                    playData.Stop();
                    return;
                }
                playData.Init();
                playAudioData = playData;
            });
        }
    }
}