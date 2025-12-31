using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    [CreateAssetMenu(fileName = "AIGpoSpawnerConfig", menuName = "AIEditor/AIGpoSpawnerConfig")]
    public class AIGpoSpawnerConfig : ScriptableObject {
        [Header("波次组信息")]
        public List<SpawnAIWaveGroup> WaveGroups;
        [Header("是否允许 GPO 在已生成过的点位上重新生成")]
        public bool AllowReSpawnOnPoints = true;
    }
    
    [Serializable]
    public class SpawnAIWaveGroup {
        [Header("波次组标识")]
        public string GroupSign;
        [Header("波次信息")]
        public List<SpawnAIWaveInfo> WaveInfos;
        [Header("GPO 等级")]
        public int Level;
        [Header("生成点最小范围过滤 0 = 全图")]
        public int MinRange; 
        [Header("生成点最大范围过滤 0 = 全图")]
        public int MaxRange; 
        [Header("生成点类型 空 = 全部")]
        public List<int> SpawnPointTypeIds;
        [Header("是否需要全 GPO 部死亡后才进入这一轮")]
        public bool NeedPrevSpawnGPOAllDead = true; // 是否需要全部死亡后才进入下一波 如果是者会在全部清理完后，才执行下一个波次的 DelaySpawnTime
        [Header("生成出来的 GPO 是否朝向生成点移动")]
        public bool IsSpawnAIMoveToPoint = true;
        [Header("生成点的最大使用率阈值，0 - 1之间(最少 1 个)")]
        public float MaxPointUsageRate = 1.0f; 
        [Header("波次生成延迟时间")]
        public float DelaySpawnTime;
    }
    
    [Serializable]
    public class SpawnAIWaveInfo {
        [Header("生成 GPO 的标识")]
        public string AISign;
        [Header("生成数量")]
        public int SpawnCount;
        [Header("生成延迟时间")]
        public float DelayTime;
    }
}
