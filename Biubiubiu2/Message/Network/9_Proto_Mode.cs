using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.NetworkMessage {
    public class Proto_Mode {
        public const byte ModID = 9;
        public struct Cmd_SendSaveBattleData : ICmd {
            public const string ID = "Proto_Mode.Cmd_SendSaveBattleData";
            public string GetID() => ID;
            public const byte FuncID = 1;
            public byte[] byteDatas;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(byteDatas.Length);
                for (int i = 0; i < byteDatas.Length; i++) {
                    buffer.Write(byteDatas[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var len = buffer.ReadInt();
                byteDatas = new byte[len];
                for (int i = 0; i < len; i++) {
                    byteDatas[i] = buffer.ReadByte();
                }
            }
        }
        
        public struct Rpc_GameState : IRpc {
            public const string ID = "Proto_Mode.Rpc_GameState";
            public string GetID() => ID;
            public const byte FuncID = 2;
            public byte gameState;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gameState);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gameState = buffer.ReadByte();
            }
        }
        
        public struct TargetRpc_GameState : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_GameState";
            public string GetID() => ID;
            public const byte FuncID = 3;
            public byte gameState;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gameState);
            }

            public void UnSerialize(ByteBuffer buffer) {
                gameState = buffer.ReadByte();
            }
        }
        
        public struct Rpc_GameDownTime : IRpc {
            public const string ID = "Proto_Mode.Rpc_GameDownTime";
            public string GetID() => ID;
            public const byte FuncID = 4;
            public ushort downTime; // 倒计时 秒
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(downTime);
            }
            public void UnSerialize(ByteBuffer buffer) {
                downTime = buffer.ReadUShort();
            }
        }
        
        public struct Rpc_Score : IRpc {
            public const string ID = "Proto_Mode.Rpc_Score";
            public string GetID() => ID;
            public const byte FuncID = 5;
            public long playerId;
            public int score; // 个人总得分
            public int continueScore; // 个人连续得分
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(score);
                buffer.Write(continueScore);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                score = buffer.ReadInt();
                continueScore = buffer.ReadInt();
            }
        }
        
        public struct Rpc_PlayCharacterData : IRpc {
            public const string ID = "Proto_Mode.Rpc_PlayCharacterData";
            public string GetID() => ID;
            public const byte FuncID = 6;
            public int gpoID;
            public int teamID;
            public string nickName;
            public long playerId;
            public int score;
            public int continueScore; // 个人连续得分
            public int hurtValue; // 伤害值
            public byte KillCount;
            public string avatarURL;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoID);
                buffer.Write(teamID);
                buffer.Write(nickName);
                buffer.Write(playerId);
                buffer.Write(score);
                buffer.Write(continueScore);
                buffer.Write(hurtValue);
                buffer.Write(KillCount);
                buffer.Write(avatarURL);
            }
            public void UnSerialize(ByteBuffer buffer) {
                gpoID = buffer.ReadInt();
                teamID = buffer.ReadInt();
                nickName = buffer.ReadString();
                playerId = buffer.ReadLong();
                score = buffer.ReadInt();
                continueScore = buffer.ReadInt();
                hurtValue = buffer.ReadInt();
                KillCount = buffer.ReadByte();
                avatarURL = buffer.ReadString();
            }
        }
        
        public struct TargetRpc_PlayCharacterData : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_PlayCharacterData";
            public string GetID() => ID;
            public const byte FuncID = 7;
            public int gpoID;
            public int teamID;
            public long playerId;
            public string NickName;
            public int score;
            public int continueScore; // 个人连续得分
            public int hurtValue; // 伤害值
            public byte KillCount;
            public string avatarURL;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoID);
                buffer.Write(teamID);
                buffer.Write(playerId);
                buffer.Write(score);
                buffer.Write(continueScore);
                buffer.Write(NickName);
                buffer.Write(hurtValue);
                buffer.Write(KillCount);
                buffer.Write(avatarURL);
            }
            public void UnSerialize(ByteBuffer buffer) {
                gpoID = buffer.ReadInt();
                teamID = buffer.ReadInt();
                playerId = buffer.ReadLong();
                score = buffer.ReadInt();
                continueScore = buffer.ReadInt();
                NickName = buffer.ReadString();
                hurtValue = buffer.ReadInt();
                KillCount = buffer.ReadByte();
                avatarURL = buffer.ReadString();
            }
        }   
        
        public struct Rpc_ModeMessage : IRpc {
            public const string ID = "Proto_Mode.Rpc_ModeMessage";
            public string GetID() => ID;
            public const byte FuncID = 8;
            public string mainText;
            public string subText;
            public byte messageState;
            public ushort itemId;
            public byte teamId;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(mainText);
                buffer.Write(subText);
                buffer.Write(messageState);
                buffer.Write(itemId);
                buffer.Write(teamId);
            }

            public void UnSerialize(ByteBuffer buffer) {
                mainText = buffer.ReadString();
                subText = buffer.ReadString();
                messageState = buffer.ReadByte();
                itemId = buffer.ReadUShort();
                teamId = buffer.ReadByte();
            }
        }
        
        public struct Rpc_PlayCharacterGPOData : IRpc {
            public const string ID = "Proto_Mode.TargetRpc_PlayCharacterData";
            public string GetID() => ID;
            public const byte FuncID = 9;
            public long playerId;
            public int gpoID;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(gpoID);
                buffer.Write(playerId);
            }
            public void UnSerialize(ByteBuffer buffer) {
                gpoID = buffer.ReadInt();
                playerId = buffer.ReadLong();
            }
        }   
        
        public struct BattlePlayerData {
            public long playerId;
            public int AllScore;  // 个人总得分
            public int killCount;  // 击杀数
            public int deadNum;  // 死亡次数
            public int hurtValue;  // 伤害值
        }
        public struct TargetRpc_VSModeWarEnd : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_VSModeWarEnd";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public const byte FuncID = 10;
            public List<int> dropitemList;
            public List<BattlePlayerData> battlePlayerDatas;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(dropitemList.Count);
                for (int i = 0; i < dropitemList.Count; i++) {
                    buffer.Write(dropitemList[i]);
                }
                buffer.Write(battlePlayerDatas.Count);
                for (int i = 0; i < battlePlayerDatas.Count; i++) {
                    var battlePlayerData = battlePlayerDatas[i];
                    buffer.Write(battlePlayerData.playerId);
                    buffer.Write(battlePlayerData.killCount);
                    buffer.Write(battlePlayerData.AllScore);
                    buffer.Write(battlePlayerData.deadNum);
                    buffer.Write(battlePlayerData.hurtValue);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var len = buffer.ReadInt();
                dropitemList = new List<int>();
                for (int i = 0; i < len; i++) {
                    dropitemList.Add(buffer.ReadInt());
                }
                len = buffer.ReadInt();
                battlePlayerDatas = new List<BattlePlayerData>();
                for (int i = 0; i < len; i++) {
                    var battlePlayerData = new BattlePlayerData();
                    battlePlayerData.playerId = buffer.ReadLong();
                    battlePlayerData.killCount = buffer.ReadInt();
                    battlePlayerData.AllScore = buffer.ReadInt();
                    battlePlayerData.deadNum = buffer.ReadInt();
                    battlePlayerData.hurtValue = buffer.ReadInt();
                    battlePlayerDatas.Add(battlePlayerData);
                }
            }
        }

        public struct TargetRpc_HurtValue : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_HurtValue";
            public string GetID() => ID;
            public const byte FuncID = 11;
            public long playerId;
            public int hurtValue; // 个人总得分
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(hurtValue);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                hurtValue = buffer.ReadInt();
            }
        }       
        
        public struct Rpc_KillNum : IRpc {
            public const string ID = "Proto_Mode.Rpc_KillNum";
            public string GetID() => ID;
            public const byte FuncID = 12;
            public long playerId;
            public byte killCount; 
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(killCount);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                killCount = buffer.ReadByte();
            }
        }
        
         public struct TargetRpc_VSModeWarEndWinTeamList : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_VSModeWarEndWinTeamList";
            public int GetChannel() => NetworkData.Channels.Reliable;
            public string GetID() => ID;
            public const byte FuncID = 13;
            public List<int> teamList;
            public long triggerPlayerId;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(triggerPlayerId);
                buffer.Write(teamList.Count);
                for (int i = 0; i < teamList.Count; i++) {
                    buffer.Write(teamList[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                triggerPlayerId = buffer.ReadLong();
                var len = buffer.ReadInt();
                teamList = new List<int>();
                for (int i = 0; i < len; i++) {
                    teamList.Add(buffer.ReadInt());
                }
            }
        }

        public struct Rpc_TeamWin : IRpc {
            public const string ID = "Proto_Mode.Rpc_TeamWin";
            public string GetID() => ID;
            public const byte FuncID = 14;
            public List<int> teamList;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(teamList.Count);
                for (int i = 0; i < teamList.Count; i++) {
                    buffer.Write(teamList[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                var len = buffer.ReadInt();
                teamList = new List<int>(len);
                for (int i = 0; i < len; i++) {
                    teamList.Add(buffer.ReadInt());
                }
            }
        }

        public struct TargetRpc_TeamsWinCount : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_TeamsWinCount";
            public string GetID() => ID;
            public const byte FuncID = 15;
            public int roundCount;
            public List<int> teamIDs;
            public List<byte> winCounts;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(roundCount);
                buffer.Write(teamIDs.Count);
                for (int i = 0; i < teamIDs.Count; i++) {
                    buffer.Write(teamIDs[i]);
                    buffer.Write(winCounts[i]);
                }
            }

            public void UnSerialize(ByteBuffer buffer) {
                roundCount = buffer.ReadInt();
                var count = buffer.ReadInt();
                teamIDs = new List<int>(count);
                winCounts = new List<byte>(count);
                for (int i = 0; i < count; i++) {
                    teamIDs.Add(buffer.ReadInt());
                    winCounts.Add(buffer.ReadByte());
                }
            }
        }

        public struct Rpc_OnlineChange : IRpc {
            public const string ID = "Proto_Mode.Rpc_OnlineChange";
            public string GetID() => ID;
            public const byte FuncID = 16;
            public long playerId;
            public bool isOnline;
            public float reconnectDuration;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(isOnline);
                buffer.Write(reconnectDuration);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                isOnline = buffer.ReadBoolean();
                reconnectDuration = buffer.ReadFloat();
            }
        }

        public struct TargetRpc_OnlineChange : ITargetRpc {
            public const string ID = "Proto_Mode.TargetRpc_OnlineChange";
            public string GetID() => ID;
            public const byte FuncID = 17;
            public long playerId;
            public bool isOnline;
            public float reconnectDuration;
            public int GetChannel() => NetworkData.Channels.Reliable;

            public void Serialize(ByteBuffer buffer) {
                buffer.Write(ModID);
                buffer.Write(FuncID);
                buffer.Write(playerId);
                buffer.Write(isOnline);
                buffer.Write(reconnectDuration);
            }

            public void UnSerialize(ByteBuffer buffer) {
                playerId = buffer.ReadLong();
                isOnline = buffer.ReadBoolean();
                reconnectDuration = buffer.ReadFloat();
            }
        }

        public static IProto_Doc ReadRpcBuffer(byte id) {
            IProto_Doc doc = null;
            switch (id) {
                case Rpc_GameState.FuncID:
                    doc = new Rpc_GameState();
                    break;
                case TargetRpc_GameState.FuncID:
                    doc = new TargetRpc_GameState();
                    break;
                case Rpc_GameDownTime.FuncID:
                    doc = new Rpc_GameDownTime();
                    break;
                case Rpc_Score.FuncID:
                    doc = new Rpc_Score();
                    break;
                case Rpc_PlayCharacterData.FuncID:
                    doc = new Rpc_PlayCharacterData();
                    break;
                case TargetRpc_PlayCharacterData.FuncID:
                    doc = new TargetRpc_PlayCharacterData();
                    break;
                case Rpc_ModeMessage.FuncID:
                    doc = new Rpc_ModeMessage();
                    break;
                case Rpc_PlayCharacterGPOData.FuncID:
                    doc = new Rpc_PlayCharacterGPOData();
                    break;
                case TargetRpc_VSModeWarEnd.FuncID:
                    doc = new TargetRpc_VSModeWarEnd();
                    break;
                case TargetRpc_HurtValue.FuncID:
                    doc = new TargetRpc_HurtValue();
                    break;
                case Rpc_KillNum.FuncID:
                    doc = new Rpc_KillNum();
                    break;
                case TargetRpc_VSModeWarEndWinTeamList.FuncID:
                    doc = new TargetRpc_VSModeWarEndWinTeamList();
                    break;
                case Rpc_TeamWin.FuncID:
                    doc = new Rpc_TeamWin();
                    break;
                case TargetRpc_TeamsWinCount.FuncID:
                    doc = new TargetRpc_TeamsWinCount();
                    break;
                case Rpc_OnlineChange.FuncID:
                    doc = new Rpc_OnlineChange();
                    break;
                case TargetRpc_OnlineChange.FuncID:
                    doc = new TargetRpc_OnlineChange();
                    break;
                default:
                    Debug.LogError("Proto_Network:ReadRpcBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
        
        public static ICmd ReadCmdBuffer(byte id) {
            ICmd doc = null;
            switch (id) {
                case Cmd_SendSaveBattleData.FuncID:
                    doc = new Cmd_SendSaveBattleData();
                    break;
                default:
                    Debug.LogError("Proto_GPO:ReadCmdBuffer 没有注册对应 ID:" + id);
                    return null;
            }
            return doc;
        }
    }
}