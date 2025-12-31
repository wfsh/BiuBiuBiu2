using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_DragonFullScreenAOESpawner : AbilityMData {
        [Header("M_模板数据（服务端）")]
        public string M_SceneType = "";
        public byte M_BossType = 0;
        public float M_NextTime;
        public float M_SpawnTime;
        public float M_AttackCheckTime;
        public float M_MaxDistance;
        public float[] M_Radius;
        public float M_MoveSpeed;
        public float M_LifeTime;
        public ushort M_ATK;
        public byte M_SpawnerCount;
        public Vector3[][] M_SpawnPoints;
    }
}