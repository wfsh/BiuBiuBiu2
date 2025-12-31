using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class WarReportCharacterSync : MonoBehaviour, ICharacterSync {
        public long syncPlayerId = 0;
        public string syncNickName = "";
        public int syncGpoId = 0;
        public int syncTeamId = 0;
        public CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        public CharacterData.FlyType flyType = CharacterData.FlyType.None;
        public CharacterData.StandType standType = CharacterData.StandType.Stand;
        public bool isDodge = false;
        public string useHoldOn = "";
        public int Level = 0;
        public int Hp = 0;
        public int MaxHp = 0;
        public int Atk = 0;
        public int Def = 0;
        private INetwork network;
        public void SetNetwork(INetwork network) {
            this.network = network;
        }

        public int SyncTeamId() {
            return syncTeamId;
        }
        
        public void SyncTeamId(int value) {
            syncTeamId = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.TeamId,
                Value = value.ToString()
            });
        }

        public long SyncPlayerId() {
            return syncPlayerId;
        }

        public void SyncPlayerId(long value) {
            syncPlayerId = value;
        }

        public string SyncNickName() {
            return syncNickName;
        }

        public void SyncNickName(string value) {
            syncNickName = value;
        }

        public int SyncGpoId() {
            return syncGpoId;
        }

        public void SyncGpoId(int value) {
            syncGpoId = value;
        }

        public CharacterData.JumpType JumpType() {
            return jumpType;
        }

        public void JumpType(CharacterData.JumpType value) {
            jumpType = value;
        }

        public CharacterData.FlyType FlyType() {
            return flyType;
        }

        public void FlyType(CharacterData.FlyType value) {
            flyType = value;
        }

        public CharacterData.StandType StandType() {
            return standType;
        }

        public void StandType(CharacterData.StandType value) {
            standType = value;
        }

        public bool IsDodge() {
            return isDodge;
        }

        public void IsDodge(bool value) {
            isDodge = value;
        }

        public string UseHoldOn() {
            return useHoldOn;
        }

        public void UseHoldOn(string value) {
            useHoldOn = value;
        }
        
        public int GetHP() {
            return Hp;
        }

        public void SetHP(int value) {
            Hp = value;
        }

        public int GetATK() {
            return Atk;
        }

        public void SetATK(int value) {
            Atk = value;
        }

        public int GetDEF() {
            return Def;
        }

        public void SetDEF(int value) {
            Def = value;
        }

        public int GetLevel() {
            return Level;
        }

        public void SetLevel(int value) {
            Level = value;
        }

        public int GetMaxHp() {
            return MaxHp;
        }

        public void SetMaxHp(int value) {
            MaxHp = value;
        }
    }
}