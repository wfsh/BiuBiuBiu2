using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ParticleCannonAxisPart : MonoBehaviour {
        public const float MoveDistance = 0.12f; // 伸展的距离
        public const float RetractSpeed = 5f; // 收回的速度
        public const float ExpandSpeed = 80f; // 伸展的速度
        
        [SerializeField]
        private float angle = 0.0f;

        private Vector3 startPosition;
        private Vector3 targetPosition;
        private Vector3 direction;
        private float moveProgress = 0;
        private bool isExpanding = false;
        private bool isReturning = false;
        private bool isPlayAnim = false;

        public void Init() {
            this.startPosition = transform.localPosition;
            this.direction = Quaternion.Euler(0, 0, angle) * Vector3.right;
            this.targetPosition = startPosition + direction.normalized * MoveDistance;
        }

        public void TriggerExpansion() {
            if (isReturning) {
                isReturning = false;
                moveProgress = 0;
            }
            isExpanding = true;
            isPlayAnim = true;
        }

        public bool UpdatePosition(float deltaTime) {
            if (isExpanding) {
                moveProgress += deltaTime * RetractSpeed;
                transform.localPosition = Vector3.Lerp(startPosition, targetPosition, moveProgress);
                if (moveProgress >= 1f) {
                    isExpanding = false;
                    isReturning = true;
                    moveProgress = 0;
                }
            } else if (isReturning) {
                moveProgress += deltaTime * ExpandSpeed;
                transform.localPosition = Vector3.Lerp(targetPosition, startPosition, moveProgress);
                if (moveProgress >= 1f) {
                    isReturning = false;
                    moveProgress = 0;
                    isPlayAnim = false;
                }
            }
            return isPlayAnim;
        }
    }
}