using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class AICreateAreaEntity : SceneElementEntity {
        public float Area = 10f;
        public int MaxMonsterNum = 3;
        // [SerializeField]
        // public MonsterData.LevelArea LevelArea;
        public string[] MonsterSign;
    }
}