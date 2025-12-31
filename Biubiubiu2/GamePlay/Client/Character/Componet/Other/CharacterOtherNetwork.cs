using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherNetwork : ClientCharacterComponent {
        private C_Character_Base system;
        public CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        public CharacterData.FlyType flyType = CharacterData.FlyType.None;
        public CharacterData.StandType standType = CharacterData.StandType.Stand;
        public bool isDodge = false;
        private bool isOnline = true;
        private float deltaCheckOnline = 0;

        protected override void OnAwake() {
            base.OnAwake();
            system = (C_Character_Base)mySystem;
            mySystem.Register<CE_Network.GetIsOnline>(OnGetIsOnlineCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }


        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_Network.GetIsOnline>(OnGetIsOnlineCallBack);
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            SetSync();
            SetNetworkName();
        }

        protected override void OnSetEntityObj(IEntity entity) {
            base.OnSetEntityObj(entity);
            SetNetworkName();
        }

        private void SetNetworkName() {
            if (isClear) {
                return;
            }
            if (iEntity == null || iEntity is EntityBase == false) {
                return;
            }
            var entityBase = (EntityBase)iEntity;
            var localInfo = iGPO.IsLocalGPO() ? "L" : "O";
            entityBase.SetName(characterSync.SyncNickName() + $"_{localInfo}");
        }

        private void OnUpdate(float deltaTime) {
            UpdatreCheckIsOnline();
            if (characterNetwork == null || characterNetwork.IsDestroy()) {
                return;
            }
            SetSync();
        }

        private void OnGetIsOnlineCallBack(ISystemMsg body, CE_Network.GetIsOnline ent) {
            ent.CallBack?.Invoke(isOnline);
        }
        
        // 根据时间间隔判断 network 是否 = null 判断 isOnline 是否 =  true
        private void UpdatreCheckIsOnline() {
            if (deltaCheckOnline < 0.5f) {
                deltaCheckOnline += Time.deltaTime;
                return;
            }
            deltaCheckOnline = 0;
            var isOnline = true;
            if (networkBase == null || networkBase.IsOnline() == false) {
                isOnline = false;
            }
            if (isOnline == this.isOnline) {
                return;
            }
            this.isOnline = isOnline;
            this.mySystem.Dispatcher(new CE_Network.IsOnline {
                IsTrue = isOnline
            });
        }

        private void SetSync() {
            if (characterSync.JumpType() != jumpType) {
                jumpType = characterSync.JumpType();
                this.mySystem.Dispatcher(new CE_Character.JumpTypeChange {
                    JumpType = jumpType
                });
            }
            if (characterSync.FlyType() != flyType) {
                flyType = characterSync.FlyType();
                this.mySystem.Dispatcher(new CE_Character.FlyTypeChange {
                    FlyType = flyType
                });
            }
            if (characterSync.StandType() != standType) {
                standType = characterSync.StandType();
                this.mySystem.Dispatcher(new CE_Character.StandTypeChange {
                    StandType = standType
                });
            }
            if (characterSync.IsDodge() != isDodge) {
                isDodge = characterSync.IsDodge();
                this.mySystem.Dispatcher(new CE_Character.Dodga {
                    IsDodge = isDodge
                });
            }
        }
    }
}