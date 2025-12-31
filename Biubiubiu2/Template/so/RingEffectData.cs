using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Template {
    [CreateAssetMenu(fileName = "RingEffectData", menuName = "MonsterEditor/EffectConfig/RingEffectData")]
    public class RingEffectData : ScriptableObject {
        public List<RingEffectFrame> posData;
        public Mesh _mesh;
        public Material _mat;

        [System.Serializable]
        public class RingEffectFrame {
            public List<Vector3> posFrame = new List<Vector3>();
            public List<float> randomHeight = new List<float>();
            public float[] randomSpeed;
        }
    }

}