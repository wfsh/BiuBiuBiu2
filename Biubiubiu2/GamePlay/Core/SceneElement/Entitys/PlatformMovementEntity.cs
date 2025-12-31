using System;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class PlatformMovementEntity : SceneElementEntity {
        public Vector3 EndPoint;
        public Transform TriggerTransform;
        public float MoveSpeed = 1f;
    }
}