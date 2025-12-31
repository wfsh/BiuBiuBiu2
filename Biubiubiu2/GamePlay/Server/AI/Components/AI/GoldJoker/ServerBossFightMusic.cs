using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerBossFightMusic : ComponentBase {
        
        public struct InitData : SystemBase.IComponentInitData{
            public byte configId;
        }
        
        private S_AI_Base aiBase;
        private AbilityM_BossFightMusic mData;
        private byte bossType = 0;
        private byte configId = 0;
        private IGPO fireGPO;
        private bool isLoadConfig = false;
        private float lastPlayRandomMusicTime = 0f;
        private bool isStartRandomMusic = false;
        private float waitPlayFightRandomMusicTime = 10f;// 控制 随机战斗音乐 的播放间隔
        private float waitPlayBollInMusicTime = 0f;
        private int lastFightMusicIndex = -1;
        private float waitPlayBossDeadMusicTime = 0f;// 控制 boss 语音的播放间隔
        
        protected override void OnAwake() {
            Register<SE_AI.Event_PlayBossInMusic>(OnPlayBossInMusic);
            Register<SE_AI.Event_PlayFightRandomMusic>(OnPlayFightRandomMusic);
            Register<SE_AI.Event_PlayFightFailedMusic>(OnPlayBossOutMusic);
            Register<SE_AI.Event_PlayBossDeadMusic>(OnPlayBossDeadMusic);
        }

        protected override void OnStart() {
            base.OnStart();
            var system = (S_AI_Base)mySystem;
            configId = ((InitData)initDataBase).configId;
            mData = AbilityM_BossFightMusic.CreateForID(configId);
            mData.Select(() => {
                isLoadConfig = true;
                waitPlayFightRandomMusicTime = mData.M_BossFightSpaceTime;
            });
            fireGPO = system.GetGPO();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
           RemoveUpdate(OnUpdate);
           Unregister<SE_AI.Event_PlayBossInMusic>(OnPlayBossInMusic);
           Unregister<SE_AI.Event_PlayFightRandomMusic>(OnPlayFightRandomMusic);
           Unregister<SE_AI.Event_PlayFightFailedMusic>(OnPlayBossOutMusic);
           Unregister<SE_AI.Event_PlayBossDeadMusic>(OnPlayBossDeadMusic);
        }
        
        private void OnUpdate(float deltaTime) {
            waitPlayBossDeadMusicTime -= deltaTime;
            
            if (waitPlayBollInMusicTime > 0) {
                waitPlayBollInMusicTime -= deltaTime;
                if (waitPlayBollInMusicTime <= 0) {
                    if (waitPlayBossDeadMusicTime > 0) {
                        return;
                    }
                    PlayWWiseAudio(mData.M_BossIn, mData.M_BossInTime);
                    waitPlayBossDeadMusicTime = mData.M_BossFightSpaceTime;
                }
            }
            
            if (waitPlayFightRandomMusicTime > 0 || waitPlayBossDeadMusicTime > 0) {
                waitPlayFightRandomMusicTime -= deltaTime;
                return;
            }
            if (isLoadConfig && isStartRandomMusic) {
                isStartRandomMusic = false;
                int randomValue = UnityEngine.Random.Range(1, 11);
                if (randomValue < mData.M_BossFightRandom) {
                    PlayFightMusic();
                }
            }
        }

        private void OnPlayBossInMusic(ISystemMsg body, SE_AI.Event_PlayBossInMusic ent) {
            waitPlayBollInMusicTime = 0.5f;
        }
        
        private void OnPlayFightRandomMusic(ISystemMsg body, SE_AI.Event_PlayFightRandomMusic ent) {
            if (!isLoadConfig) {
                return;
            }
            var spaceTime = mData.M_BossFightSpaceTime;
            if (Time.time - lastPlayRandomMusicTime > spaceTime) {
                lastPlayRandomMusicTime = Time.time;
                isStartRandomMusic = true;
            }
        }

        private void PlayFightMusic() {
            waitPlayFightRandomMusicTime = mData.M_BossFightSpaceTime;
            var fightMusics = mData.M_BossFight;
            int idx = UnityEngine.Random.Range(0, fightMusics.Length);
            if (idx == lastFightMusicIndex) {
                idx++;
                idx = idx >= fightMusics.Length ? 0 : idx;
            }
            lastFightMusicIndex = idx;
            PlayWWiseAudio(mData.M_BossFight[idx], mData.M_BossFightTime[idx]);
        }
        
        private void OnPlayBossOutMusic(ISystemMsg body, SE_AI.Event_PlayFightFailedMusic ent) {
            if (waitPlayBossDeadMusicTime > 0) {
                return;
            }
            waitPlayBossDeadMusicTime = mData.M_BossOutTime;
            PlayWWiseAudio(mData.M_BossOut, mData.M_BossOutTime);
        }
        
        private void OnPlayBossDeadMusic(ISystemMsg body, SE_AI.Event_PlayBossDeadMusic ent) {
            //死亡优先级最高，需要立马播放，同时阻止其他音乐播放
            waitPlayBossDeadMusicTime = mData.M_BossDeadTime;
            PlayWWiseAudio(mData.M_BossDead, mData.M_BossDeadTime);
        }

        private void PlayWWiseAudio(byte audioKey, ushort lifeTime) {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = audioKey, In_StartPoint = fireGPO.GetPoint(), In_LifeTime = lifeTime, In_IsFollow = true
                }
            });
        }
    }
}