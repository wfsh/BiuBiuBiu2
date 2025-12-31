using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerAIWorld {
        private S_AI_Base AddAIForGpoMTypeId(int gpoMTypeId, int gpoMId, Action<S_AI_Base> callBack) {
            S_AI_Base system = null;
            switch (gpoMTypeId) {
                case GpoTypeSet.Id_FeiYu:
                    system = manager.AddSystem<ServerAIFeiYuSystem>(callBack);
                    break;
                case GpoTypeSet.Id_WuGui:
                    system = manager.AddSystem<ServerAIWuGuiSystem>(callBack);
                    break;
                case GpoTypeSet.Id_RexKing:
                    system = manager.AddSystem<ServerAIRexKingSystem>(callBack);
                    break;
                case GpoTypeSet.Id_SwordTiger:
                    system = manager.AddSystem<ServerAISwordTigerSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Character:
                    system = AddCharacterAI(callBack);
                    break;
                case GpoTypeSet.Id_Tank:
                    system = manager.AddSystem<ServerAITankSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Helicopter:
                    system = manager.AddSystem<ServerAIHelicopterSystem>(callBack);
                    break;
                case GpoTypeSet.Id_AuroraDragon:
                    system = manager.AddSystem<ServerAIAuroraDragonSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Uav:
                    system = manager.AddSystem<ServerAIUavSystem>(callBack);
                    break;
                case GpoTypeSet.Id_MachineGun:
                    system = manager.AddSystem<ServerAIMachineGunSystem>(callBack);
                    break;
                case GpoTypeSet.Id_GiantDaDa:
                    system = manager.AddSystem<ServerAIGiantDaDaSystem>(callBack);
                    break;
                case GpoTypeSet.Id_AceJoker:
                    if (gpoMId == GPOM_AceJokerSet.Id_AceJoker) {
                        system = manager.AddSystem<ServerAIAceJokerSystem>(callBack);
                    } else {
                        system = manager.AddSystem<ServerAIGoldJokerSystem>(callBack);
                    }
                    break;
                case GpoTypeSet.Id_JokerUav:
                    system = manager.AddSystem<ServerAIJokerUavSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Gpospawner:
                    system = manager.AddSystem<ServerAIGPOSpawnerSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Sniper:
                    system = manager.AddSystem<ServerAISniperSystem>(callBack);
                    break;
                case GpoTypeSet.Id_BlindingShield:
                    system = manager.AddSystem<ServerAIBlindingShieldSystem>(callBack);
                    break;
                default:
                    Debug.LogError("[Error] ServerAIWorld gpoMTypeId" + gpoMTypeId + " gpoMId:" + gpoMId + " 没有对应的怪物系统");
                    break;
            }
            return system;
        }

        private S_AI_Base AddCharacterAI(Action<S_AI_Base> callBack) {
            S_AI_Base system = null;
            switch (ModeData.PlayMode) {
                case ModeData.ModeEnum.SausageGoldDash:
                    system = manager.AddSystem<ServerAIGoldDashAISystem>(callBack);
                    break;
                case ModeData.ModeEnum.SausageBeastCamp:
                    system = manager.AddSystem<ServerAIBeastCampSystem>(callBack);
                    break;
                default:
                    system = manager.AddSystem<ServerAIShootDuckAISystem>(callBack);
                    break;
            }
            return system;
        }
    }
}