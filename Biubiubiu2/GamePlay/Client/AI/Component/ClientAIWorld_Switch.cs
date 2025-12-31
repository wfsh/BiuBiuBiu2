using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Config;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public partial class ClientAIWorld : ComponentBase {
        private C_AI_Base AddSystem(int gpoMTypeId, int gpoMId, Action<C_AI_Base> callBack) {
            C_AI_Base system = null;
            switch (gpoMTypeId) {
                case GpoTypeSet.Id_FeiYu:
                    system = manager.AddSystem<ClientAIFeiYuSystem>(callBack);
                    break;
                case GpoTypeSet.Id_WuGui:
                    system = manager.AddSystem<ClientAIWuGuiSystem>(callBack);
                    break;
                case GpoTypeSet.Id_RexKing:
                    system = manager.AddSystem<ClientAIRexKingSystem>(callBack);
                    break;
                case GpoTypeSet.Id_SwordTiger:
                    system = manager.AddSystem<ClientAISwordTigerSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Sniper:
                case GpoTypeSet.Id_Character:
                    system = manager.AddSystem<ClientAICharacterAISystem>(callBack);
                    break;
                case GpoTypeSet.Id_Tank:
                    system = manager.AddSystem<ClientAITankSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Helicopter:
                    system = manager.AddSystem<ClientAIHelicopterSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Uav:
                    system = manager.AddSystem<ClientAIUavSystem>(callBack);
                    break;
                case GpoTypeSet.Id_MachineGun:
                    system = manager.AddSystem<ClientAIMachineGunSystem>(callBack);
                    break;
                case GpoTypeSet.Id_GiantDaDa:
                    system = manager.AddSystem<ClientAIGiantDaDaSystem>(callBack);
                    break;
                case GpoTypeSet.Id_AuroraDragon:
                    if (gpoMId == GPOM_AuroraDragonSet.Id_AuroraDragon) {
                        system = manager.AddSystem<ClientAIAuroraDragonSystem>(callBack);
                    } else {
                        system = manager.AddSystem<ClientAIAncientDragonSystem>(callBack);
                    }
                    break;
                case GpoTypeSet.Id_AceJoker:
                    system = manager.AddSystem<ClientAIAceJokerSystem>(callBack);
                    break;
                case GpoTypeSet.Id_JokerUav:
                    system = manager.AddSystem<ClientAIJokerUavSystem>(callBack);
                    break;
                case GpoTypeSet.Id_Gpospawner:
                    system = manager.AddSystem<ClientAIGPOSpawnerSystem>(callBack);
                    break;
                case GpoTypeSet.Id_BlindingShield:
                    system = manager.AddSystem<ClientAIBlindingShieldSystem>(callBack);
                    break;
                default:
                    Debug.LogError("[Error] ClientAIWorld gpoMTypeId" + gpoMTypeId + " gpoMId:" + gpoMId + " 没有对应的怪物系统");
                    break;
            }
            return system;
        }
    }
}