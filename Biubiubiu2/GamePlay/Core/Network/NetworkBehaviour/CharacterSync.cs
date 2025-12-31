using System.Collections;
using System.Collections.Generic;
using Mirror;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public interface ICharacterSync : INetworkSync {
        long SyncPlayerId();
        void SyncPlayerId(long value);
        string SyncNickName();
        void SyncNickName(string value);
        int SyncGpoId();
        void SyncGpoId(int value);
        int SyncTeamId();
        void SyncTeamId(int value);
        CharacterData.JumpType JumpType();
        void JumpType(CharacterData.JumpType value);
        CharacterData.FlyType FlyType();
        void FlyType(CharacterData.FlyType value);
        CharacterData.StandType StandType();
        void StandType(CharacterData.StandType value);
        bool IsDodge();
        void IsDodge(bool value);
        string UseHoldOn();
        void UseHoldOn(string value);
        int GetHP();
        void SetHP(int value);
        int GetATK();
        void SetATK(int value);
        int GetLevel();
        void SetLevel(int value);
        int GetMaxHp();
        void SetMaxHp(int value);
    }

    public class CharacterSync : SyncBase, ICharacterSync {
        [SyncVar]
        public long syncPlayerId = 0;
        [SyncVar]
        public string syncNickName = "";
        [SyncVar]
        public int syncGpoId = 0;
        [SyncVar]
        public int syncTeamId = 0;
        [SyncVar]
        public CharacterData.JumpType jumpType = CharacterData.JumpType.None;
        [SyncVar]
        public CharacterData.FlyType flyType = CharacterData.FlyType.None;
        [SyncVar]
        public CharacterData.StandType standType = CharacterData.StandType.Stand;
        [SyncVar]
        public bool isDodge = false;
        [SyncVar]
        public string useHoldOn = "";
        [SyncVar]
        public int Level = 0;
        [SyncVar]
        public int Hp = 0;
        [SyncVar]
        public int MaxHp = 0;
        [SyncVar]
        public int Atk = 0;
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
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.PlayerId,
                Value = value.ToString()
            });
        }

        public string SyncNickName() {
            return syncNickName;
        }

        public void SyncNickName(string value) {
            syncNickName = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.NickName,
                Value = value
            });
        }

        public int SyncGpoId() {
            return syncGpoId;
        }

        public void SyncGpoId(int value) {
            syncGpoId = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.GpoId,
                Value = value.ToString()
            });
        }

        public CharacterData.JumpType JumpType() {
            return jumpType;
        }

        public void JumpType(CharacterData.JumpType value) {
            jumpType = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.JumpType,
                Value = ((int)value).ToString()
            });
        }

        public CharacterData.FlyType FlyType() {
            return flyType;
        }

        public void FlyType(CharacterData.FlyType value) {
            flyType = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.FlyType,
                Value = ((int)value).ToString()
            });
        }

        public CharacterData.StandType StandType() {
            return standType;
        }

        public void StandType(CharacterData.StandType value) {
            standType = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.StandType,
                Value = ((int)value).ToString()
            });
        }

        public bool IsDodge() {
            return isDodge;
        }

        public void IsDodge(bool value) {
            isDodge = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.IsDodge,
                Value = value.ToString()
            });
        }

        public string UseHoldOn() {
            return useHoldOn;
        }

        public void UseHoldOn(string value) {
            useHoldOn = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.UseHoldOn,
                Value = value
            });
        }
        
        public int GetHP() {
            return Hp;
        }

        public void SetHP(int value) {
            Hp = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.Hp,
                Value = value.ToString()
            });
        }

        public int GetATK() {
            return Atk;
        }

        public void SetATK(int value) {
            Atk = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.Atk,
                Value = value.ToString()
            });
        }

        public int GetLevel() {
            return Level;
        }

        public void SetLevel(int value) {
            Level = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.Level,
                Value = value.ToString()
            });
        }

        public int GetMaxHp() {
            return MaxHp;
        }

        public void SetMaxHp(int value) {
            MaxHp = value;
            MsgRegister.Dispatcher(new M_WarReport.SetCharacterSyncData {
                NetworkId = network.GetNetworkId(),
                SyncType = WarReportData.CharacterSyncType.MaxHp,
                Value = value.ToString()
            });
        }
    }
}