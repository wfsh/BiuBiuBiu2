using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIAttribute : ClientGPOAttribute {
        override protected GPOData.AttributeData CreateAttribute() {
            var monsterSystem = (C_AI_Base)mySystem;
            var useInData = (Proto_AI.TargetRpc_AddAIDefault)monsterSystem.InData;
            var monsterData = new GPOData.AttributeData();
            monsterData.Level = 1;
            monsterData.maxHp = useInData.maxHp;
            monsterData.nowHp = useInData.nowHp;
            return monsterData;
        }
    }
}