using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CameraShake : ComponentBase {
        [Header("基础参数")] 
        private float defaultDuration = 0.5f;
        private float defaultMagnitude = 0.1f;
        private float dampingSharpness = 2f; // 衰减锐度（值越大衰减越快）

        [Header("高级选项")] 
        private bool useSmoothNoise = true;
        private Vector3 noiseFrequency = new Vector3(2, 2, 0);
        private bool affectRotation = false;
        private float maxRotationAngle = 10f;

        private Vector3 originalPos;
        private Quaternion originalRot;
        private float remainingDuration;
        private float currentMagnitude;
        private bool isShaking;

        private Transform shakeTransform;
        
        protected override void OnAwake() {
            MsgRegister.Register<CM_Camera.ShakeCamera>(TriggerShake);
            originalPos = Vector3.zero;
            originalRot = Quaternion.identity;
        }
        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
            var cameraEntity = (CameraEntity)iEntity;
            shakeTransform = cameraEntity.Shake;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_Camera.ShakeCamera>(TriggerShake);
            RemoveUpdate(OnUpdate);
        }

        void OnUpdate(float deltaTime) {
            if (!isShaking) return;

            // 更新震动逻辑
            remainingDuration -= deltaTime;

            if (remainingDuration <= 0) {
                StopShake();
                return;
            }

            ApplyShakeEffect();
        }

        public void TriggerShake(CM_Camera.ShakeCamera ent) {
            //初始化震动参数
            defaultDuration = ent.Duration;
            defaultMagnitude = ent.Magnitude;
            dampingSharpness = ent.DampingSharpness;
            affectRotation = ent.AffectRotation;
            maxRotationAngle = ent.MaxRotationAngle;
            noiseFrequency = ent.NoiseFrequency;
            
            currentMagnitude = defaultMagnitude;
            remainingDuration = defaultDuration;
            isShaking = true;
        }

        private void ApplyShakeEffect() {
            // 计算衰减比例（使用指数衰减）
            float damping = Mathf.Clamp01(remainingDuration * dampingSharpness);
            float intensity = currentMagnitude * damping;

            // 生成震动偏移
            Vector3 offset = useSmoothNoise ? CalculateNoiseOffset(intensity) : UnityEngine.Random.insideUnitSphere * intensity;

            // 应用位置震动
            shakeTransform.localPosition = originalPos + offset;

            // 应用旋转震动
            if (affectRotation) {
                Vector3 rotOffset = new Vector3(
                    offset.y * maxRotationAngle,
                    offset.x * maxRotationAngle,
                    offset.z * maxRotationAngle
                );
                shakeTransform.localRotation = Quaternion.Euler(rotOffset) * originalRot;
            }
        }

        private Vector3 CalculateNoiseOffset(float intensity) {
            float time = Time.time;
            return new Vector3(
                (Mathf.PerlinNoise(time * noiseFrequency.x, 0) * 2 - 1),
                (Mathf.PerlinNoise(0, time * noiseFrequency.y) * 2 - 1),
                (Mathf.PerlinNoise(time * noiseFrequency.z, time) * 2 - 1)
            ) * intensity;
        }

        private void StopShake() {
            isShaking = false;
            shakeTransform.localPosition = originalPos;
            shakeTransform.localRotation = originalRot;
        }
    }
}