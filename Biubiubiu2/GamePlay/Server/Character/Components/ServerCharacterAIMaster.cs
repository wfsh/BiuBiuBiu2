using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterAIMaster : ServerGPOAIMaster {
    //     protected override void OnAwake() {
    //         base.OnAwake();
    //         Register<SE_Monster.Event_UpdateMonsterAttributeData>(UpdateMonsterAttributeDataMonsterData);
    //     }
    //
    //     protected override void OnClear() {
    //         base.OnClear();
    //         RemoveProtoCallBack(Proto_Monster.Cmd_TakeOutMonster.ID, OnTakeOutMonsterCallBack);
    //         Unregister<SE_Monster.Event_UpdateMonsterAttributeData>(UpdateMonsterAttributeDataMonsterData);
    //     }
    //
    //     protected override void OnSetNetwork() {
    //         base.OnSetNetwork();
    //         AddProtoCallBack(Proto_Monster.Cmd_TakeOutMonster.ID, OnTakeOutMonsterCallBack);
    //         RPCALLMonster();
    //         RpcFollowMonster();
    //     }
    //
    //     private void OnTakeOutMonsterCallBack(INetwork iBehaviour, IProto_Doc cmdData) {
    //         var data = (Proto_Monster.Cmd_TakeOutMonster)cmdData;
    //         var outData = GetMonsterData(Data.PlayerId);
    //         if (outData == null) {
    //             return;
    //         }
    //         SummerMonster(outData, iEntity.GetPoint());
    //     }
    //
    //     private void RPCALLMonster() {
    //         for (int i = 0; i < followMonsterList.Count; i++) {
    //             var data = followMonsterList[i];
    //             TargetRPCCatchMonsterData(data);
    //         }
    //     }
    //
    //     protected override void OnCatchMonster(GPOData.AttributeData data) {
    //         base.OnCatchMonster(data);
    //         TargetRPCCatchMonsterData(data);
    //     }
    //
    //     protected override void OnDiscardMonster(GPOData.AttributeData data) {
    //         base.OnDiscardMonster(data);
    //         TargetRpc(networkBase, new Proto_Character.TargetRpc_DiscardMonster() {
    //             monsterPID = Data.PlayerId,
    //         });
    //     }
    //
    //     private void TargetRPCCatchMonsterData(GPOData.AttributeData data) {
    //         TargetRpc(networkBase, new Proto_Monster.TargetRpc_CatchMonster {
    //             monsterLevel = data.Level,
    //             monsterPID = Data.PlayerId,
    //             monsterSign = data.monsterSign,
    //             maxHp = data.maxHp,
    //             nowHp = data.nowHp,
    //             nowSkillPoint = data.nowSkillPoint,
    //             isSkillType = data.isSkillType,
    //             def = data.DEF,
    //             atk = data.ATK,
    //             nextExp = data.nextExp,
    //         });
    //     }
    //
    //     private void RpcFollowMonster() {
    //         if (followMonster == null) {
    //             return;
    //         }
    //         OnSetFollowMonster(followMonster);
    //     }
    //
    //     protected override void OnSetFollowMonster(IMonster monster) {
    //         base.OnSetFollowMonster(monster);
    //         TargetRpc(networkBase, new Proto_Monster.TargetRpc_FollowMonster() {
    //             monsterPID = followMonster.GetAttributeData().monsterPID,
    //         });
    //     }
    //
    //     private void UpdateMonsterAttributeDataMonsterData(ISystemMsg body, SE_Monster.Event_UpdateMonsterAttributeData ent) {
    //         var data = ent.MonsterData;
    //         TargetRPCCatchMonsterData(data);
    //     }
    //
    //     protected override void OnHPChange(int hp) {
    //         base.OnHPChange(hp);
    //         TargetRpc(networkBase, new Proto_Monster.TargetRpc_MasterMonsterHPChange {
    //             monsterPID = followMonster.GetAttributeData().monsterPID, nowHp = hp,
    //         });
    //     }
    }
}