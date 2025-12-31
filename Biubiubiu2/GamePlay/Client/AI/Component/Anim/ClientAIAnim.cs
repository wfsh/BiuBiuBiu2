using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIAnim : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public string ConfigSign;
            public string ChangeAssetUrl;
            public string ToAssetUrl;
        }
        private Dictionary<string, string> assetUrlChangeDic = new Dictionary<string, string>();
        protected C_AI_Base AIBase;
        private EntityBase entity = null;
        private int base_layer = -1;
        protected string monsterSign = "";
        protected EntityAnimConfig config;
        protected SausagePlayable playable;
        private int playAnimId = 0;
        private string playAnimSign = "";
        private string configSign = "";
        private string changeAssetUrl = "";
        private string toAssetUrl = "";
        private bool isDrive = false;

        protected override void OnAwake() {
            AIBase = (C_AI_Base)mySystem;
            mySystem.Register<CE_GPO.Event_DriveGPO>(OnDriveGpoCallBack);
            mySystem.Register<CE_AI.Event_SetPlayAnimSign>(OnSetPlayAnimSignCallBack);
            mySystem.Register<CE_AI.Event_SetPlayClipId>(OnSetPlayClipIdCallBack);
            var initData = (InitData)initDataBase;
            if (initData != null) {
                configSign = initData.ConfigSign;
                changeAssetUrl = initData.ChangeAssetUrl;
                toAssetUrl = initData.ToAssetUrl;
            }
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.TargetRpc_Anim.ID, OnTargetRpcPlayAnim);
            AddProtoCallBack(Proto_AI.Rpc_Anim.ID, OnRpcPlayAnim);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            assetUrlChangeDic.Clear();
            mySystem.Unregister<CE_GPO.Event_DriveGPO>(OnDriveGpoCallBack);
            mySystem.Unregister<CE_AI.Event_SetPlayAnimSign>(OnSetPlayAnimSignCallBack);
            mySystem.Unregister<CE_AI.Event_SetPlayClipId>(OnSetPlayClipIdCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_Anim.ID, OnTargetRpcPlayAnim);
            RemoveProtoCallBack(Proto_AI.Rpc_Anim.ID, OnRpcPlayAnim);
            ClearPlayableGraph();
        }
        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            AIBase = (C_AI_Base)mySystem;
            monsterSign = AIBase.AttributeData.Sign;
            if (string.IsNullOrEmpty(configSign) == false) {
                config = AIAnimConfig.Get(configSign);
            } else {
                config = AIAnimConfig.Get(AIBase.AttributeData.Sign);
            }
            if (config == null) {
                Debug.LogWarning($"ClientMonsterAnim config is null, animId: {monsterSign}");
                return;
            }
            InitPlayableGraph();
        }

        private void OnDriveGpoCallBack(ISystemMsg body, CE_GPO.Event_DriveGPO ent) {
            var isTrue = false;
            if (ent.PlayerDriveGPO != null) {
                isTrue = ent.PlayerDriveGPO.GetGPOType() == GPOData.GPOType.Role;
            }
            if (!isTrue && isDrive) {
                PlayAnimSign(AIAnimConfig.Play_Idle);
            }
            isDrive = isTrue;
        }

        private void OnSetPlayAnimSignCallBack(ISystemMsg body, CE_AI.Event_SetPlayAnimSign ent) {
            if (isDrive == false) {
                return;
            }
            PlayAnimSign(ent.AnimSign);
        }


        private void OnSetPlayClipIdCallBack(ISystemMsg body, CE_AI.Event_SetPlayClipId ent) {
            if (isDrive == false) {
                return;
            }
            PlayAnim(ent.ClipId);
        }

        private void OnTargetRpcPlayAnim(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.TargetRpc_Anim)docData;
            if (isDrive) {
                return;
            }
            PlayAnim(rpcData.animId);
        }


        private void OnRpcPlayAnim(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_AI.Rpc_Anim)docData;
            if (isDrive) {
                return;
            }
            PlayAnim(rpcData.animId);
        }

        virtual protected void InitPlayableGraph() {
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            playable = new SausagePlayable();
            SetAssetUrlChange();
            playable.Init(entity.transform, animator, config, $"Client_{monsterSign}");
            if (!string.IsNullOrEmpty(playAnimSign)) {
                PlayAnimSign(playAnimSign);
            } else if (playAnimId != 0) {
                PlayAnim(playAnimId);
            } else {
                playAnimId = playable.PlayAnimSign(AIAnimConfig.Play_Idle);
            }
        }

        void ClearPlayableGraph() {
            if (playable != null) {
                playable.Dispose();
                playable = null;
            }
            config = null;
        }

        private void SetAssetUrlChange() {
            if (string.IsNullOrEmpty(changeAssetUrl) || string.IsNullOrEmpty(toAssetUrl)) {
                return;
            }
            playable.OnAssetUrlChange = OnAssetUrlChange;
        }

        private void PlayAnimSign(string animSign) {
            playAnimSign = animSign;
            playAnimId = 0;
            if (playable == null) {
                return;
            }
            playAnimId = playable.PlayAnimSign(playAnimSign);
        }

        private void PlayAnim(int anim) {
            playAnimId = anim;
            playAnimSign = "";
            if (playable == null) {
                return;
            }
            playAnimId = anim;
            playable.PlayAnimId(anim);
            var clipData = config.GetAnimData(anim);
            if (clipData.PlaySign != "") {
                mySystem.Dispatcher(new CE_GPO.Event_PlayAnimSign {
                    AnimSign = clipData.PlaySign
                });
            }
            OnPlayAnim(anim);
        }

        virtual protected void OnPlayAnim(int anim) {
        }

        private void OnAssetUrlChange(string url, Action<string> callBack) {
            if (!assetUrlChangeDic.TryGetValue(url, out var newUrl)) {
                newUrl = url.Contains(changeAssetUrl) 
                    ? url.Replace(changeAssetUrl, toAssetUrl) 
                    : url;
                assetUrlChangeDic[url] = newUrl;
            }
            callBack(newUrl);
        }
    }
}