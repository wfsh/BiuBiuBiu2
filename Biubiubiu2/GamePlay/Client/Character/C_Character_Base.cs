using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class C_Character_Base : C_GPO_Base {
        public long PlayerId { get; private set; }
        public INetworkCharacter characterNetwork {
            get { return (INetworkCharacter)this.network; }
        }
        public ICharacterSync characterSync { get; private set; }
        public string NickName { get; private set; }

        public void SetNickName(string name) {
            this.NickName = name;
        }

        public void SetPlayerId(long playerId) {
            this.PlayerId = playerId;
        }
        
        public void SetNetworkBehaviour(INetworkCharacter network, ICharacterSync netSync) {
            characterSync = netSync;
            SetNetwork(network);
            // network.SetParent(StageData.GameWorldLayerType.Character);
        }

        public void SetCharacterData(int gpoId, int teamId, IGPOM mData, IProto_Doc inData, bool isLocalGPO) {
            SetGPOData(gpoId, mData, GPOData.GPOType.Role, inData, teamId, isLocalGPO);
        }

        public override NetworkData.SpawnConnType GetSpawnConnType() {
            return NetworkData.SpawnConnType.Role;
        }
    }
}