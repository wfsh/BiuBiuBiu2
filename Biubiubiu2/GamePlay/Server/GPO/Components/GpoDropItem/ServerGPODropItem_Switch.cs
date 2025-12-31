using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerGPODropItem {
        private void AddDropConditionComponent() {
            if (dropCondition == GpoDropCondition.Empty) {
                return;
            }
            switch (dropCondition) {
                case GpoDropCondition.DeadDrop:
                case GpoDropCondition.DeadDropNoSetItem:
                    mySystem.AddComponent<ServerDropCondition_DeadDrop>();
                    break;
                case GpoDropCondition.HpFixedRatioDrop:
                case GpoDropCondition.HpFixedRatioScatterDrop:
                    mySystem.AddComponent<ServerDropCondition_HpFixedRatioDrop>();
                    break;
                case GpoDropCondition.HpRatioDrop:
                    mySystem.AddComponent<ServerDropCondition_HpRatioDrop>();
                    break;
                case GpoDropCondition.GpoSpawnerWaveEnd:
                    mySystem.AddComponent<ServerDropCondition_GpoSpawnerWaveEnd>();
                    break;
                case GpoDropCondition.DeadDropPosUnreachable:
                    mySystem.AddComponent<ServerDropCondition_PosUnreachable>();
                    break;
                default:
                    Debug.LogError("ServerGPODropItem AddDropTypeComponent dropCondition error:" + dropCondition);
                    break;
            }
        }
    }
}