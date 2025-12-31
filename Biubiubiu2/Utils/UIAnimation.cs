using UnityEngine;
#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.Util {
    public class UIAnimation : MonoBehaviour {
        [System.Serializable]
        public class AnimationStep {
            public enum AnimationType {
                Move,
                Scale,
                Alpha,
                Shake,
                Rotate
            }

            public List<AnimationTypeData> stepDatas; // 多种类型数据
            public float duration = 1f; // 持续时间
            public float delay = 0; // 延迟时间
        }

        [System.Serializable]
        public class AnimationTypeData {
            public AnimationStep.AnimationType type;
            public Vector2 startPosition; // 起始位置（仅适用于 Move 类型）
            public Vector2 endPosition; // 目标位置（仅适用于 Move 类型）
            public Vector3 startScale = Vector3.one; // 起始缩放（仅适用于 Scale 类型）
            public Vector3 endScale = Vector3.one; // 目标缩放（仅适用于 Scale 类型）
            public float startAlpha = 1; // 起始透明度（仅适用于 Alpha 类型）
            public float endAlpha = 1; // 目标透明度（仅适用于 Alpha 类型）
            public Vector3 startRotation = Vector3.zero; // 起始旋转角度（仅适用于 Rotate 类型）
            public Vector3 endRotation = Vector3.zero; // 目标旋转角度（仅适用于 Rotate 类型）
            public float shakeMagnitude = 10f; // 抖动幅度（仅适用于 Shake 类型）
            public int shakeFrequency = 20; // 抖动频率（仅适用于 Shake 类型）
            public AnimationCurve curve = AnimationCurve.Linear(0, 0, 1, 1);
        }

        [Header("Animation Settings")]
        public List<AnimationStep> steps = new List<AnimationStep>();

        public Vector2 startPosition;
        public Vector3 startScale = Vector3.one;
        public float startAlpha = 1;
        public float startDelay = 0f;
        public Vector3 startRotation = Vector3.zero;
        public RectTransform rectTransform;
        public CanvasGroup canvasGroup;
        private int currentStepIndex = 0;
        private float elapsedTime = 0;
        private bool isPlaying = false;
        private bool isInit = false;
        private bool isInitUI = false;

        public void InitUI() {
            isInitUI = true;
            HidObj();
            Init();
        }
        public void InitEditor() {
            Init();
        }

        public void SetStartDelay(float delay) {
            startDelay = delay;
        }

        public void SetStartPoint(Vector3 point) {
            startPosition = point;
        }

        private void Init() {
            if (isInit == true) {
                return;
            }
            isInit = true;
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        public void OnUpdate(float deltaTime) {
            if (isPlaying) {
                if (startDelay > 0f) {
                    startDelay -= deltaTime;
                    return;
                } else {
                    ShowObj();
                }
            }
            if (isPlaying && currentStepIndex < steps.Count) {
                var step = steps[currentStepIndex];
                if (step.duration < 0) {
                    if (step.stepDatas.Count > 0) {
                        Debug.LogError("动画持续时间不能小于0:" + gameObject.name);
                    }
                    return;
                }
                elapsedTime += deltaTime;
                if (elapsedTime >= step.delay) {
                    var t = Mathf.Clamp01((elapsedTime - step.delay) / step.duration);
                    // 更新每个动画步骤的起始值和结束值
                    for (int i = 0; i < step.stepDatas.Count; i++) {
                        var stepData = step.stepDatas[i];
                        var easedT = stepData.curve.Evaluate(t);
                        // 根据类型执行不同的动画
                        switch (stepData.type) {
                            case AnimationStep.AnimationType.Move:
                                rectTransform.anchoredPosition = Vector2.Lerp(stepData.startPosition,
                                    stepData.endPosition, easedT);
                                break;
                            case AnimationStep.AnimationType.Scale:
                                rectTransform.localScale = Vector3.Lerp(stepData.startScale, stepData.endScale, easedT);
                                break;
                            case AnimationStep.AnimationType.Alpha:
                                canvasGroup.alpha = Mathf.Lerp(stepData.startAlpha, stepData.endAlpha, easedT);
                                break;
                            case AnimationStep.AnimationType.Shake:
                                if (t < 1f) {
                                    float offset = Mathf.Sin(t * stepData.shakeFrequency * Mathf.PI * 2) *
                                                   stepData.shakeMagnitude *
                                                   (1 - t);
                                    rectTransform.anchoredPosition = stepData.startPosition +
                                                                     new Vector2(Random.Range(-offset, offset),
                                                                         Random.Range(-offset, offset));
                                } else {
                                    rectTransform.anchoredPosition = stepData.startPosition;
                                }
                                break;
                            case AnimationStep.AnimationType.Rotate:
                                if (stepData.startRotation.z > 180) {
                                    stepData.startRotation.z -= 360;
                                }
                                if (stepData.endRotation.z > 180) {
                                    stepData.endRotation.z -= 360;
                                }
                                rectTransform.localEulerAngles = Vector3.Lerp(stepData.startRotation,
                                    stepData.endRotation, easedT);
                                ;
                                break;
                        }
                    }

                    // 完成当前步骤
                    if (t >= 1f) {
                        elapsedTime = 0;
                        currentStepIndex++;
                        if (currentStepIndex < steps.Count) {
                            InitNextStepData();
                        } else {
                            StopAnimation();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 开始播放动画
        /// </summary>
        public void PlayAnimation() {
            isPlaying = true;
            currentStepIndex = 0;
            elapsedTime = 0;
            rectTransform.anchoredPosition = startPosition;
            rectTransform.localScale = startScale;
            canvasGroup.alpha = startAlpha;
            rectTransform.localRotation = Quaternion.Euler(startRotation);
            InitNextStepData();
        }

        public void ShowObj() {
            if (gameObject.activeSelf == true) {
                return;
            }
            gameObject?.SetActive(true);
        }

        public void HidObj() {
            if (gameObject.activeSelf == false) {
                return;
            }
            gameObject?.SetActive(false);
        }

        /// <summary>
        /// 停止动画
        /// </summary>
        public void StopAnimation() {
            isPlaying = false;
            HidObj();
#if UNITY_EDITOR
            StopAnimctionEditor();
#endif
        }

#if UNITY_EDITOR
        private bool isSetEditorUpdate = false;
        private float lastTime = 0f;

        void Awake() {
            var anims = GetComponents<UIAnimation>();
            if (anims.Length > 1) {
                Debug.LogError("UIAnimation 只能添加一个 :" + gameObject.name);
            }
        }

        /// <summary>
        /// 开始播放动画
        /// </summary>
        public void PlayAnimationEditor() {
            StopAnimctionEditor();
            if (isSetEditorUpdate == false) {
                isSetEditorUpdate = true;
                EditorApplication.update -= EditorUpdate;
                EditorApplication.update += EditorUpdate;
            }
            lastTime = 0f;
            // 注册更新回调函数
            PlayAnimation();
        }

        void StopAnimctionEditor() {
            // 注销更新回调函数
            isSetEditorUpdate = false;
            EditorApplication.update -= EditorUpdate;
        }

        // 编辑器模式下的更新方法
        private void EditorUpdate() {
            if (isInitUI) {
                return;
            }
            if (lastTime == 0) {
                lastTime = (float)EditorApplication.timeSinceStartup;
            }
            var deltaTime = (float)EditorApplication.timeSinceStartup - lastTime; // 计算时间差
            lastTime = (float)EditorApplication.timeSinceStartup;
            OnUpdate(deltaTime);
        }

#endif

        private void InitNextStepData() {
            var stepData = steps[currentStepIndex];
            for (int i = 0; i < stepData.stepDatas.Count; i++) {
                var data = stepData.stepDatas[i];
                data.startPosition = rectTransform.anchoredPosition;
                data.startScale = rectTransform.localScale;
                data.startAlpha = canvasGroup.alpha;
                data.startRotation = rectTransform.localRotation.eulerAngles;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(UIAnimation))]
    public class UIAnimationEditor : Editor {
        private int insertIndex = 0;

        public override void OnInspectorGUI() {
            var uiAnimation = (UIAnimation)target;
            uiAnimation.InitEditor();

            // 用 Undo.RecordObject 来记录更改，使撤销生效
            Undo.RecordObject(uiAnimation, "UIAnimation Changes");

            // 初始 Position
            DrawVector2FieldWithButton("初始 Point", ref uiAnimation.startPosition,
                uiAnimation.rectTransform.anchoredPosition, (pos) => {
                    uiAnimation.rectTransform.anchoredPosition = pos;
                });

            // 初始 Scale
            DrawVector3FieldWithButton("初始 Scale", ref uiAnimation.startScale, uiAnimation.rectTransform.localScale,
                vector3 => {
                    uiAnimation.rectTransform.localScale = vector3;
                });

            // 初始 Rotation
            DrawVector3FieldWithButton("初始 Rota", ref uiAnimation.startRotation,
                uiAnimation.rectTransform.localEulerAngles, vector3 => {
                    uiAnimation.rectTransform.localEulerAngles = vector3;
                });
            
            // 初始 延迟
            DrawFloatFieldWithButton("初始 延迟", ref uiAnimation.startDelay, uiAnimation.startDelay,
                alpha => {
                    uiAnimation.startDelay = alpha;
                });
            // 初始 Alpha
            DrawFloatFieldWithButton("初始 Alpha", ref uiAnimation.startAlpha, uiAnimation.canvasGroup.alpha,
                alpha => {
                    uiAnimation.canvasGroup.alpha = alpha;
                });
            if (uiAnimation.steps == null) uiAnimation.steps = new List<UIAnimation.AnimationStep>();

            // 插入步骤的 UI 输入框
            EditorGUILayout.BeginHorizontal();
            insertIndex = EditorGUILayout.IntField("插入 Step 前的索引", insertIndex);
            if (insertIndex < 0) insertIndex = 0;
            if (insertIndex > uiAnimation.steps.Count) insertIndex = uiAnimation.steps.Count;
            if (GUILayout.Button("在指定位置插入 Step")) {
                Undo.RecordObject(uiAnimation, "Insert Animation Step");
                uiAnimation.steps.Insert(insertIndex, new UIAnimation.AnimationStep());
            }
            EditorGUILayout.EndHorizontal();

            // 步骤展示
            for (int i = 0; i < uiAnimation.steps.Count; i++) {
                EditorGUILayout.Space();
                var step = uiAnimation.steps[i];
                EditorGUILayout.LabelField($" ------------------------- Step {i + 1} ----------------------------",
                    EditorStyles.boldLabel);

                // 删除当前步骤
                if (GUILayout.Button("Remove Step", GUILayout.Width(100))) {
                    Undo.RecordObject(uiAnimation, "Remove Animation Step");
                    uiAnimation.steps.RemoveAt(i);
                    break;
                }

                // 记录 Undo 并修改持续时间和延迟时间
                step.duration = EditorGUILayout.FloatField(" - 持续时间", step.duration);
                step.delay = EditorGUILayout.FloatField(" - 延迟时间", step.delay);
                if (step.stepDatas == null) step.stepDatas = new List<UIAnimation.AnimationTypeData>();
                for (int j = 0; j < step.stepDatas.Count; j++) {
                    EditorGUILayout.Space();
                    var stepData = step.stepDatas[j];
                    stepData.type =
                        (UIAnimation.AnimationStep.AnimationType)EditorGUILayout.EnumPopup(" -- 动画类型", stepData.type);
                    switch (stepData.type) {
                        case UIAnimation.AnimationStep.AnimationType.Move:
                            DrawVector2FieldWithButton(" -- 目标 Point", ref stepData.endPosition,
                                uiAnimation.rectTransform.anchoredPosition, (pos) => {
                                    uiAnimation.rectTransform.anchoredPosition = pos;
                                });
                            break;
                        case UIAnimation.AnimationStep.AnimationType.Scale:
                            DrawVector3FieldWithButton(" -- 目标 Scale", ref stepData.endScale,
                                uiAnimation.rectTransform.localScale,
                                vector3 => {
                                    uiAnimation.rectTransform.localScale = vector3;
                                });
                            break;
                        case UIAnimation.AnimationStep.AnimationType.Alpha:
                            DrawFloatFieldWithButton(" -- 目标 Alpha", ref stepData.endAlpha,
                                uiAnimation.canvasGroup.alpha, alpha => {
                                    uiAnimation.canvasGroup.alpha = alpha;
                                });
                            break;
                        case UIAnimation.AnimationStep.AnimationType.Rotate:
                            Vector3 eulerAngles = uiAnimation.rectTransform.localEulerAngles;
                            if (eulerAngles.z > 180) {
                                eulerAngles.z -= 360;
                            }
                            DrawVector3FieldWithButton(" -- 目标 Rotation", ref stepData.endRotation,
                                eulerAngles, vector3 => {
                                    uiAnimation.rectTransform.localEulerAngles = vector3;
                                });
                            break;
                        case UIAnimation.AnimationStep.AnimationType.Shake:
                            stepData.shakeMagnitude =
                                EditorGUILayout.FloatField(" -- 抖动幅度", stepData.shakeMagnitude);
                            stepData.shakeFrequency =
                                EditorGUILayout.IntField(" -- 抖动频率", stepData.shakeFrequency);
                            break;
                    }
                    stepData.curve = EditorGUILayout.CurveField(" -- 动画曲线", stepData.curve);
                    EditorGUILayout.Space();

                    // 删除动画类型
                    if (GUILayout.Button($"删除动画 {stepData.type}", GUILayout.Width(200))) {
                        Undo.RecordObject(uiAnimation, "Remove Animation Type");
                        step.stepDatas.RemoveAt(j);
                        break;
                    }
                }

                // 添加动画到当前步骤
                if (GUILayout.Button($"添加动画到 Step {i + 1}", GUILayout.Width(400))) {
                    Undo.RecordObject(uiAnimation, "Add Animation Type");
                    step.stepDatas.Add(new UIAnimation.AnimationTypeData());
                }
                EditorGUILayout.Space();
            }

            // 添加步骤到末尾
            if (GUILayout.Button("添加 Step 到最后")) {
                Undo.RecordObject(uiAnimation, "Add Animation Step");
                uiAnimation.steps.Add(new UIAnimation.AnimationStep());
            }

            // 播放动画按钮
            if (GUILayout.Button("Play Animation")) {
                uiAnimation.PlayAnimationEditor();
            }

            // 标记 UIAnimation 对象为脏，以便保存更改
            EditorUtility.SetDirty(uiAnimation);
        }

        // Vector2, Vector3 和 float 输入框的绘制方法
        private void DrawVector2FieldWithButton(string label, ref Vector2 value, Vector2 currentValue,
            Action<Vector2> OnSetCallBack) {
            EditorGUILayout.BeginHorizontal();
            value = EditorGUILayout.Vector2Field(label, value);
            if (GUILayout.Button("Set", GUILayout.Width(50))) {
                value = currentValue;
            }
            if (GUILayout.Button("Get", GUILayout.Width(50))) {
                OnSetCallBack?.Invoke(value);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawVector3FieldWithButton(string label, ref Vector3 value, Vector3 currentValue,
            Action<Vector3> OnSetCallBack) {
            EditorGUILayout.BeginHorizontal();
            value = EditorGUILayout.Vector3Field(label, value);
            if (GUILayout.Button("Set", GUILayout.Width(50))) {
                value = currentValue;
            }
            if (GUILayout.Button("Get", GUILayout.Width(50))) {
                OnSetCallBack?.Invoke(value);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawFloatFieldWithButton(string label, ref float value, float currentValue,
            Action<float> OnSetCallBack) {
            EditorGUILayout.BeginHorizontal();
            value = EditorGUILayout.FloatField(label, value);
            if (GUILayout.Button("Set", GUILayout.Width(50))) {
                value = currentValue;
            }
            if (GUILayout.Button("Get", GUILayout.Width(50))) {
                OnSetCallBack?.Invoke(value);
            }
            EditorGUILayout.EndHorizontal();
        }
    }
#endif
}