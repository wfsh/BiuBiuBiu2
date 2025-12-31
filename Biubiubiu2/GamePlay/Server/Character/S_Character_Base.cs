using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class S_Character_Base : S_GPO_Base {
        public INetworkCharacter characterNetwork {
            get { return (INetworkCharacter)network; }
        }
        public ICharacterSync characterNetSync { get; private set; }
        public long PlayerId { get; private set; }
        public string NickName { get; private set; }

        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            AddNetwork();
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            var entity = (EntityBase)iEntity;
            entity.SetName(NickName + "_S");
        }
        
        public void SetCharacterData(long playerId, string name, int teamId, IGPOM mData, IGPOInData inData) {
            this.PlayerId = playerId;
            this.NickName = name;
            SetGPOData(0, GPOData.GPOType.Role, mData, inData, teamId);
        }

        protected override void OnSetNetwork() {
            characterNetSync = (ICharacterSync)network.GetNetworkSync();
            characterNetSync.SyncPlayerId(this.PlayerId);
            characterNetSync.SyncTeamId(this.TeamId);
            characterNetSync.SyncNickName(this.NickName);
            characterNetwork.SetPoint(iEntity.GetPoint());
            characterNetwork.SetRota(iEntity.GetRota());
            network.SetName("TeamID:" + this.TeamId + "_" + characterNetSync.SyncNickName() + "_S_NET");
            // network.SetParent(StageData.GameWorldLayerType.Character);
        }

        private void AddNetwork() {
            // 网络同步组件
            AddComponent<ServerCharacterNetwork>( new ServerCharacterNetwork.InitData {
                CallBack = OnSyncSpawnProto,
            });
        }

        void OnSyncSpawnProto(ServerCharacterNetwork sync) {
            sync.SetSpawnRPC(RpcCharacter());
        }

        virtual protected ITargetRpc RpcCharacter() {
            return new Proto_Login.TargetRpc_CharacterInfo() {
                playerId = PlayerId,
            };
        }
        public override NetworkData.SpawnConnType GetSpawnConnType() {
            return NetworkData.SpawnConnType.Role;
        }
    }
}