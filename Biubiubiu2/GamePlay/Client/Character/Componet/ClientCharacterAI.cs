using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientCharacterAI : ClientCharacterComponent {
        private const int FOLLOW_COUNT = 6;
        private List<GPOData.AttributeData> followList = new List<GPOData.AttributeData>(FOLLOW_COUNT);
        private GPOData.AttributeData followMonster;

        protected override void OnAwake() {
            mySystem.Register<CE_GPO.Event_GetFollowAIList>(OnGetFollowMonsterListCallBack);
            mySystem.Register<CE_GPO.Event_GetFollowAI>(OnGetFollowMonsterCallBack);
            mySystem.Register<CE_GPO.Event_TakeOutAI>(OnTakeOutMonsterCallBack);
            mySystem.Register<CE_GPO.Event_TakeBackAI>(OnTakeBackMonsterCallBack);
            mySystem.Register<CE_Game.Event_GetSaveMonsterData>(OnGetSaveItemDataCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<CE_GPO.Event_GetFollowAIList>(OnGetFollowMonsterListCallBack);
            mySystem.Unregister<CE_GPO.Event_GetFollowAI>(OnGetFollowMonsterCallBack);
            mySystem.Unregister<CE_GPO.Event_TakeOutAI>(OnTakeOutMonsterCallBack);
            mySystem.Unregister<CE_GPO.Event_TakeBackAI>(OnTakeBackMonsterCallBack);
            mySystem.Unregister<CE_Game.Event_GetSaveMonsterData>(OnGetSaveItemDataCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_CatchAI.ID, OnCatchMonsterCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_FollowAI.ID, OnFollowMonsterCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_UpNextExp.ID, OnUpNextExpCallBack);
            RemoveProtoCallBack(Proto_AI.TargetRpc_MasterAIHPChange.ID, OnMasterMonsterHPChangeCallBack);
            RemoveProtoCallBack(Proto_Character.TargetRpc_DiscardMonster.ID, OnDiscardMonsterCallBack);
            followList.Clear();
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_AI.TargetRpc_CatchAI.ID, OnCatchMonsterCallBack);
            AddProtoCallBack(Proto_AI.TargetRpc_FollowAI.ID, OnFollowMonsterCallBack);
            AddProtoCallBack(Proto_AI.TargetRpc_UpNextExp.ID, OnUpNextExpCallBack);
            AddProtoCallBack(Proto_AI.TargetRpc_MasterAIHPChange.ID, OnMasterMonsterHPChangeCallBack);
            AddProtoCallBack(Proto_Character.TargetRpc_DiscardMonster.ID, OnDiscardMonsterCallBack);
        }

        private void OnGetSaveItemDataCallBack(ISystemMsg body, CE_Game.Event_GetSaveMonsterData entData) {
            var list = new List<CE_Game.SaveMonsterData>();
            for (int i = 0; i < followList.Count; i++) {
                var followMonster = followList[i];
                var data = new CE_Game.SaveMonsterData();
                data.MonsterLevel = followMonster.Level;
                data.MonsterSign = followMonster.Sign;
                list.Add(data);
            }
            entData.CallBack(list);
        }

        private void OnGetFollowMonsterListCallBack(ISystemMsg body, CE_GPO.Event_GetFollowAIList entData) {
            entData.CallBack.Invoke(followList);
        }

        private void OnGetFollowMonsterCallBack(ISystemMsg body, CE_GPO.Event_GetFollowAI entData) {
            entData.CallBack.Invoke(followMonster);
        }

        private void OnTakeOutMonsterCallBack(ISystemMsg body, CE_GPO.Event_TakeOutAI entData) {
            var outData = GetMonsterData(entData.GpoId);
            if (outData == null) {
                Debug.LogError($"没有这只宠物 {entData.GpoId}");
                return;
            }
            // if (followMonster != null) {
            //     characterNetwork.Cmd(new Proto_Character.Cmd_TakeBackMonster {
            //         monsterPid = followMonster.monsterPID
            //     });
            // }
        }

        private void OnTakeBackMonsterCallBack(ISystemMsg body, CE_GPO.Event_TakeBackAI entData) {
            var monsterPID = entData.GpoId;
            var outData = GetMonsterData(monsterPID);
            if (outData == null) {
                Debug.LogError($"没有这只宠物 {monsterPID}");
                return;
            }
            // Cmd(new Proto_Character.Cmd_TakeBackMonster {
            //     monsterPid = monsterPID
            // });
        }

        private void OnCatchMonsterCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            if (followList.Count >= FOLLOW_COUNT) {
                Debug.LogError($"超过 {FOLLOW_COUNT} 只宠物");
                return;
            }
            var data = (Proto_AI.TargetRpc_CatchAI)cmdData;
            var monsterData = GetMonsterData(data.aiGpoId);
            if (monsterData == null) {
                monsterData = new GPOData.AttributeData();
                followList.Add(monsterData);
            }
            SetMonsterDataForProto(data, monsterData);
            mySystem.Dispatcher(new CE_GPO.Event_ADDFollowAIData {
                AIData = monsterData
            });
        }

        private void SetMonsterDataForProto(Proto_AI.TargetRpc_CatchAI data, GPOData.AttributeData monsterData) {
            monsterData.GpoId = data.aiGpoId;
            monsterData.Level = data.aiLevel;
            monsterData.Sign = data.aiSign;
            monsterData.SkinSign = data.aiSign;
            monsterData.ATK = data.atk;
            monsterData.maxHp = data.maxHp;
            monsterData.nowHp = data.nowHp;
        }

        private void OnFollowMonsterCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_AI.TargetRpc_FollowAI)cmdData;
            if (data.gpoId == 0) {
                followMonster = null;
            } else {
                var outData = GetMonsterData(data.gpoId);
                if (outData == null) {
                    return;
                }
                followMonster = outData;
            }
            mySystem.Dispatcher(new CE_GPO.Event_ChangeFollowAI {
                AIData = followMonster
            });
        }

        private void OnMasterMonsterHPChangeCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_AI.TargetRpc_MasterAIHPChange)cmdData;
            var outData = GetMonsterData(data.gpoId);
            if (outData == null) {
                return;
            }
            outData.nowHp = data.nowHp;
        }

        private void OnUpNextExpCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_AI.TargetRpc_UpNextExp)cmdData;
        }

        private void OnDiscardMonsterCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
            var data = (Proto_Character.TargetRpc_DiscardMonster)cmdData;
            var count = followList.Count - 1;
            for (int i = count; i >= 0; i--) {
                var monster = followList[i];
                if (data.monsterPID == monster.GpoId) {
                    followList.RemoveAt(i);
                }
            }
            mySystem.Dispatcher(new CE_GPO.Event_UpdateFollowList {
                FollowList = followList
            });
        }

        private GPOData.AttributeData GetMonsterData(int mpid) {
            for (int i = 0; i < followList.Count; i++) {
                var data = followList[i];
                if (data.GpoId == mpid) {
                    return data;
                }
            }
            return null;
        }
    }
}