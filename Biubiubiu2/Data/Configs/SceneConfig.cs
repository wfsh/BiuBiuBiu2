using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    [Serializable]
    public class PlayerSpawnPoint {
        public bool IsEnable = true; // 是否启用
        public Vector3 Point;
        public Quaternion Rota;
        public int Team; // 1 = 队伍 1, 2 = 队伍 2 
    }
    
    [Serializable]
    public class SlimeSpawnPoint {
        public bool IsEnable = true; // 是否启用
        public Vector3 Point;
        public Quaternion Rota;
    }

    [Serializable]
    public class AIWeight {
        [FormerlySerializedAs("AISign")]
        public string AISign; // 怪物标识
        public int Weight; // 权重
    }

    [Serializable]
    public class PatrolPointData {
        public SceneConfig.PatrolType PatrolType = SceneConfig.PatrolType.Range; // 巡逻类型
        public List<Vector3> PointList = new List<Vector3>(); // 巡逻点列表
        public float Radius; // 半径

#if  UNITY_EDITOR
        public class PointObjData {
            public GameObject PointObj;
            public GameObject RangeObj;
        }
        
        [NonSerialized]
        public List<PointObjData> PointObjList = new List<PointObjData>(); // 巡逻点列表
#endif
    }

    [Serializable]
    public class AISpawnPoint {
        public bool IsEnable = true; // 是否启用
        public string GroupName = ""; // 名称
        public float SpawnRadius; // 生成半径
        public int SpawnCount; // 生成数量
        public float SpawnDelayTime; // 生成延迟时间
        public Vector3 Point;
        public Quaternion Rota;
        public PatrolPointData PatrolPoint = new PatrolPointData(); // 巡逻点
        public List<AIWeight> AIList = new List<AIWeight>(); // 怪物标识
    }

    [Serializable]
    public class ScenePoint {
        public int ID;
        public string PointTypes = "0"; // 生成点类型
        public Vector3 Point;
        public Quaternion Rota;
    }
    
    [Serializable]
    public class ElementPointData {
        public Vector3 Point;
        public Quaternion Rota;
#if  UNITY_EDITOR
        public GameObject PointObj;
#endif
    }
    
    [Serializable]
    public class HarvestElementData {
        public SceneData.ElementEnum Element; // 物品类型
        public int Weight; // 权重
    }

    [Serializable]
    public class ElementSpawnPoint {
        public bool IsEnable = true; // 是否启用
        public string GroupName = ""; // 名称
        public float SpawnDelayTime; // 生成延迟时间
        public float RespawnCooldown; // 生成 CD 时间
        // 一次性可采集的次数
        public int GatheringLimit;
        // 一次采集可获得的数量
        public int HarvestNum;
        // 每次采集需要的时间
        public float HarvestTime;
        // 采集获得的物品
        public List<HarvestElementData> HarvestElements;
        public List<ElementPointData> PointList = new List<ElementPointData>();
    }

    [Serializable]
    public class RoomArea {
        public OBB obb;
    }

    [CreateAssetMenu(fileName = "SceneConfig", menuName = "SceneEditor/SceneConfig")]
    public class SceneConfig : ScriptableObject {
        public enum PatrolType {
            Range = 1, // 范围巡逻
            Point = 2, // 点巡逻
        }
        public string TargetScenePath = ""; // 目标场景地址
        public ModeData.ModeEnum Mode = ModeData.ModeEnum.None;
        public int Weapon1Id = ItemSet.Id_Ump9; // 武器1
        public int Weapon2Id = ItemSet.Id_ParticleCannon; // 武器2
        public int SuperWeaponId = ItemSet.Id_Helicopter; // 超级武器
        
        public List<PlayerSpawnPoint> PlayerSpawnPoints = new List<PlayerSpawnPoint>();
        public List<AISpawnPoint> AISpawnPoints = new List<AISpawnPoint>();
        public List<SlimeSpawnPoint> SlimeSpawnPoints = new List<SlimeSpawnPoint>();
        public List<ElementSpawnPoint> ElementSpawnPoints = new List<ElementSpawnPoint>();
        public List<RoomArea> RoomAreas = new List<RoomArea>();
        public List<ScenePoint> ScenePoints = new List<ScenePoint>();
    }
}