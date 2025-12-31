using System.Collections;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CharacterOtherAerocraft : ClientCharacterComponent {
        private Transform lHandTran;
        private GameObject aerocraftObj;
        private Animator animator;
        private EntityAnimConfig animConfig;
        private AerocraftBaseInfoConfig baseInfoConfig;
        private SausagePlayable playable;
        private string aerocraftSign = "";

        protected override void OnAwake() {
            this.mySystem.Register<CE_Character.FlyTypeChange>(OnFlyTypeCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            ClearMod();
            RemoveUpdate(OnUpdate);
            this.mySystem.Unregister<CE_Character.FlyTypeChange>(OnFlyTypeCallBack);
            RemoveProtoCallBack(Proto_Character.Rpc_UseAerocraft.ID, OnRpcUseAerocraftCallBack);
            RemoveProtoCallBack(Proto_Character.TargetRpc_UseAerocraft.ID, OnTargetRpcUseAerocraftCallBack);
        }

        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.Rpc_UseAerocraft.ID, OnRpcUseAerocraftCallBack);
            AddProtoCallBack(Proto_Character.TargetRpc_UseAerocraft.ID, OnTargetRpcUseAerocraftCallBack);
        }

        private void OnTargetRpcUseAerocraftCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Character.TargetRpc_UseAerocraft)rpcData;
            SetAerocraftSign(data.aerocraftSign);
        }

        private void OnRpcUseAerocraftCallBack(INetwork iBehaviour, IProto_Doc rpcData) {
            var data = (Proto_Character.Rpc_UseAerocraft)rpcData;
            SetAerocraftSign(data.aerocraftSign);
        }

        protected override void OnSetEntityObj(IEntity ientity) {
            lHandTran = iEntity.GetBodyTran(GPOData.PartEnum.LeftHand);
            CheckUseAerocraft();
        }

        public void SetAerocraftSign(string sign) {
            if (this.aerocraftSign == sign) {
                return;
            }
            this.aerocraftSign = sign;
            if (aerocraftSign == "") {
                ClearMod();
            } else {
                CheckUseAerocraft();
            }
        }

        private void CheckUseAerocraft() {
            if (aerocraftSign == "" || lHandTran == null) {
                return;
            }
            animConfig = AerocraftAnimConfig.Get(aerocraftSign);
            baseInfoConfig = AerocraftBaseInfoConfig.Get(aerocraftSign);
            this.mySystem.Dispatcher(new CE_Character.FlyMoveData {
                FlySpeed = baseInfoConfig.FlySpeed, FlyWeight = baseInfoConfig.Weight
            });
            LoadMod();
        }

        public void OnFlyTypeCallBack(ISystemMsg body, CE_Character.FlyTypeChange entData) {
            if (aerocraftObj == null) {
                return;
            }
            var flyType = entData.FlyType;
            switch (flyType) {
                case CharacterData.FlyType.OpenParachute:
                    SetActive(true);
                    playable?.PlayAnimSign(AerocraftAnimConfig.Play_Idle);
                    break;
                case CharacterData.FlyType.Fly:
                    SetActive(true);
                    playable?.PlayAnimSign(AerocraftAnimConfig.Play_Open);
                    break;
                default:
                    SetActive(false);
                    playable?.PlayAnimSign(AerocraftAnimConfig.Play_Idle);
                    break;
            }
        }

        private void LoadMod() {
            AssetManager.LoadGamePlayObjAsync("Aerocraft/" + aerocraftSign, LoadModEndCallBack);
        }

        private void ClearMod() {
            if (aerocraftObj != null) {
                GameObject.Destroy(aerocraftObj);
                aerocraftObj = null;
            }
            if (playable != null) {
                playable.Dispose();
                playable = null;
            }
            animator = null;
        }

        private void LoadModEndCallBack(GameObject perfab) {
            ClearMod();
            if (perfab != null) {
                var gameObj = GameObject.Instantiate(perfab);
                SetGameObj(gameObj);
            } else {
                Debug.LogError("加载失败:" + aerocraftSign);
            }
        }

        private void SetGameObj(GameObject gameObj) {
            aerocraftObj = gameObj;
            var tran = gameObj.transform;
            tran.SetParent(lHandTran);
            tran.localPosition = Vector3.zero;
            tran.localRotation = Quaternion.identity;
            tran.localScale = Vector3.one;
            animator = aerocraftObj.GetComponentInChildren<Animator>();
            InitPlayableGraph();
            SetActive(false);
        }

        private void SetActive(bool isTrue) {
            if (isTrue != aerocraftObj.activeSelf) {
                aerocraftObj.SetActive(isTrue);
            }
        }

        private void InitPlayableGraph() {
            playable = new SausagePlayable();
            playable.Init(aerocraftObj.transform,  animator, animConfig, $"Aerocraft_{aerocraftSign}");
        }
    }
}