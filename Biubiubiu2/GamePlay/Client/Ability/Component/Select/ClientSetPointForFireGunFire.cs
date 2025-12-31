using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSetPointForFireGunFire : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public int FireGPOId;
        }
        private Transform fireBox;
        private float deltaTime = 0f;
        private bool isSetFireBox = false;
        private AudioPoolManager.PlayAudioData playAudioData;
        private bool isLoadAudio = false;
        private int GPOId;
        private float delayCheckTime;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.FireGPOId);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        public void SetData(int GPOId) {
            this.GPOId = GPOId;
        }

        private void PlayAudio() {
            var sys = (C_Ability_Base)mySystem;
            MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                GpoId = sys.FireGpoId,
                CallBack = gpo => {
                    //播放开火音效
                    if (gpo.IsLocalGPO()) {
                        AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("WP_NG_SP_Firegun_Fire"), fireBox.position);
                    }
                    else {
                        AudioPoolManager.OnPlayAudio(AssetURL.GetAudio3P("WP_NG_SP_Firegun_Fire_3P"), fireBox.position);
                    }
                    //播放喷火音效
                    if (gpo.IsLocalGPO()) {
                        AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("WP_NG_SP_Firegun_Fire_Loop"), fireBox.position, data => {
                            playAudioData = data;
                            isLoadAudio = true;
                        });
                    } else {
                        AudioPoolManager.OnPlayAudio(AssetURL.GetAudio3P("WP_NG_SP_Firegun_Fire_Loop_3P"), fireBox.position, data => {
                            playAudioData = data;
                            isLoadAudio = true;
                        });
                    }
                }
            });
            
        }

        private void StopAudio() {
            if (playAudioData != null) {
                isLoadAudio = false;
                playAudioData.Stop();
                playAudioData = null;
            }
        }

        private void SetAudioPosition() {
            if (isLoadAudio) {
                playAudioData.SetPoint(fireBox.position);
            }
        }
        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireBox = null;
            StopAudio();
        }

        private void OnUpdate(float delayTime) {
            if (isSetEntityObj == false) {
                return;
            }

            if (isSetFireBox == false) {
                if (delayCheckTime <= 0) {
                    delayCheckTime = 1f;
                    MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                        GpoId = GPOId, CallBack = SetFireGPO
                    });
                }
                return;
            }
            if (fireBox != null) {
                iEntity.SetPoint(fireBox.position);
                iEntity.SetRota(fireBox.rotation);
                SetAudioPosition();
            }
        }
        
        
        private void SetFireGPO(IGPO gpo) {
            if (gpo != null) {
                var isSetFireBox = false;
                gpo.Dispatcher(new CE_Weapon.GetFireBox {
                    CallBack = fireBox => {
                        SetFireBox(fireBox);
                        isSetFireBox = true;
                    }
                });
                gpo.Dispatcher(new CE_Weapon.Event_BulletSetFireGPO());
                if (isSetFireBox) {
                    return;
                }

                var fireTranList = gpo.GetBodyTranList(GPOData.PartEnum.AttactPoint1);
                if (fireTranList.Count > 0) {
                    var fireIndex = UnityEngine.Random.Range(0, fireTranList.Count);
                    var fireTran = fireTranList[fireIndex];
                    SetFireBox(fireTran);
                }
            }
        }

        private void SetFireBox(Transform fireBox) {
            if (fireBox == null) {
                return;
            }
            var mSystem = (C_Ability_Base)mySystem;
            iEntity.SetPoint(fireBox.position);
            iEntity.SetRota(fireBox.rotation);
            isSetFireBox = true;
            this.fireBox = fireBox;
            PlayAudio();
        }
        
    }
}