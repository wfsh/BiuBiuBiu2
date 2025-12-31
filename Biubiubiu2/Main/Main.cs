using System;
using System.Collections.Generic;
using Sofunny.PerfAnalyzer;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Stage;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.View;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Sofunny.BiuBiuBiu2.Main {
    public class Main : MonoBehaviour {
        private readonly Queue<Task> tasks = new Queue<Task>();
        private MsgRegister msgRegister;
        private UIViewManager uiViewManager;
        private StageManager stageManager;
        private LoadBaseAsset loadBaseAsset;

        void Awake() {
            DontDestroyOnLoad(this);
        }

        void Start() {
            InitDeviceLevel();
            msgRegister = MsgRegister.Init();
            InitTask();
        }

        private void OnDestroy() {
            FunnyDBSDKAgent.Instance.Dispose();
            GravitySDKAgent.Instance.Clear();
        }

        void InitDeviceLevel() {
            var level = PerfAnalyzerAgent.GetDeviceLevel();
            QualityData.Init(level);
        }

        void Update() {
            UpdateTask();
            UpdateLoadBaseAsset();
            UpdateMsgRegister();
            UpdateUIViewManager();
            UpdateStageManager();
            UpdatePoolManager();
        }

        private void UpdateMsgRegister() {
            PerfAnalyzerAgent.BeginSample("msgRegister:Update");
            msgRegister.OnUpdate(Time.deltaTime);
            PerfAnalyzerAgent.EndSample("msgRegister:Update");
        }

        private void UpdateStageManager() {
            PerfAnalyzerAgent.BeginSample("stageManager:Update");
            stageManager?.OnUpdate(Time.deltaTime);
            PerfAnalyzerAgent.EndSample("stageManager:Update");
        }

        private void UpdateUIViewManager() {
            PerfAnalyzerAgent.BeginSample("uiViewManager:Update");
            uiViewManager?.OnUpdate(Time.deltaTime);
            PerfAnalyzerAgent.EndSample("uiViewManager:Update");
        }

        private void UpdatePoolManager() {
            PerfAnalyzerAgent.BeginSample("PrefabPoolManager:Update");
            PrefabPoolManager.Update(Time.deltaTime);
            PerfAnalyzerAgent.EndSample("PrefabPoolManager:Update");
            PerfAnalyzerAgent.BeginSample("AudioPoolManager:Update");
            AudioPoolManager.Update(Time.deltaTime);
            PerfAnalyzerAgent.EndSample("AudioPoolManager:Update");
        }

        void InitTask() {
            if (NetworkData.IsServerPlatform() == false) {
                InitTencentAwsSDK();
            }
            tasks.Enqueue(new Task("InitSentry", InitSentry));
            tasks.Enqueue(new Task("InitFunnyDB", InitFunnyDB));
            tasks.Enqueue(new Task("InitData", InitData));
            tasks.Enqueue(new Task("StageManager", InitStageManager));
            if (NetworkData.IsServerPlatform() == false) {
                tasks.Enqueue(new Task("InitPerfanalyzer", InitPerfanalyzer));
                tasks.Enqueue(new Task("InitFunnySDK", InitFunnySDK));
                tasks.Enqueue(new Task("InitLoadBaseAsset", InitLoadBaseAsset));
            }
            tasks.Enqueue(new Task("UIViewManager", InitUIViewManager));
        }

        void UpdateTask() {
            if (tasks.Count > 0) {
                var task = tasks.Dequeue();
                task.OnStart();
            }
        }

        private void InitTencentAwsSDK() {
            TencentAWSSDKAgent.Instance.Init(GameData.IsRelease());
        }

        void InitLoadBaseAsset() {
            Debug.Log($"启动模式 IsServer = [{NetworkData.IsServerPlatform()}]");
            var uiObj = AssetManager.LoadUILoadBaseAsset();
            var gameObject = Instantiate(uiObj);
            var tran = gameObject.transform;
            tran.position = Vector3.zero;
            tran.localScale = Vector3.one;
            tran.localRotation = Quaternion.identity;
            loadBaseAsset = gameObject.GetComponent<LoadBaseAsset>();
            FunnyDBSDKAgent.Instance.ReportLoginLog("InitLoadBaseAsset", FunnyDBSDKAgent.LoginStep.InitLoadBaseAsset);
            loadBaseAsset.Init(() => {
                MsgRegister.Dispatcher(new M_Login.LoadBaseAssetFinish());
                loadBaseAsset.Clear();
                loadBaseAsset = null;
                Destroy(gameObject);
            });
        }

        private void UpdateLoadBaseAsset() {
            if (loadBaseAsset == null) {
                return;
            }
            PerfAnalyzerAgent.BeginSample("loadBaseAsset:Update");
            loadBaseAsset.OnUpdate(Time.deltaTime);
            PerfAnalyzerAgent.EndSample("loadBaseAsset:Update");
        }

        void InitUIViewManager() {
            MsgRegister.Dispatcher(new M_Login.LoginGameServerState {
                LoginState = (int)LoginGameServer.LoginState.UIViewStart,
            });
            uiViewManager = new UIViewManager();
            uiViewManager.Init();
            FunnyDBSDKAgent.Instance.ReportLoginLog("InitUIViewManager", FunnyDBSDKAgent.LoginStep.InitUIViewManager);
        }

        void InitStageManager() {
            stageManager = new StageManager();
            stageManager.Init();
            FunnyDBSDKAgent.Instance.ReportLoginLog("InitStageManager", FunnyDBSDKAgent.LoginStep.StageManager);
        }

        void InitData() {
            ModeData.Init();
            WeaponData.Init();
            GameSettingData.Init();
            FunnyDBSDKAgent.Instance.ReportLoginLog("InitData", FunnyDBSDKAgent.LoginStep.InitData);
        }

        void InitPerfanalyzer() {
            if (!PerfAnalyzerAgent.IsInit) {
                FunnyDBSDKAgent.Instance.ReportLoginLog("InitPerfanalyzer", FunnyDBSDKAgent.LoginStep.InitPerfanalyzer);
                var isShowUI = true;
                if (Application.isEditor == false) {
#if FIRST_TEST || RELEASE
                    isShowUI = false;
#endif
                }
                PerfAnalyzerAgent.Init("fkHTl5fiLuEBwVGCJYzBnAaH", PerfAnalyzerToken.UploadTypeEnum.Default, "登录游戏",
                    isShowUI);
                PerfAnalyzerAgent.SetGameVersion(GameData.ShowGameVersion);
                PerfAnalyzerAgent.SetBuildPackageName(GameData.GetBuildPackageName());
                PerfAnalyzerAgent.SetQuality(QualityData.GetQualitySign(QualityData.GetQualityType()));
                PerfAnalyzerAgent.SetOnStartFeatureCallBack(delegate {
                    PerfAnalyzerAgent.SetFpsCustomBtn("资源详细信息", delegate(int id) {
                        PerfAnalyzerAgent.StartResourceAssetsCheck();
                    });
                    PerfAnalyzerAgent.SetFpsCustomBtn("Show Ability", delegate(int id) {
                        MsgRegister.Dispatcher(new M_Game.HideAbility());
                    });
                    PerfAnalyzerAgent.SetFpsCustomBtn("跳过广告", delegate(int id) {
                        TestToolData.SetSkipAds(TestToolData.IsSkipAds);
                    });
                    PerfAnalyzerAgent.SetLog(
                        $"分辨率调整 {StageData.SceneWidth} x {StageData.SceneHeight} => {Screen.width} x {Screen.height}");
                });
                if (Application.isEditor) {
                    PerfAnalyzerAgent.EnabledOnlyConsoleSaveReport(true);
                }
            }
        }

        private void InitFunnyDB() {
            if (!Application.isEditor) {
                FunnyDBSDKAgent.Instance.enabled = true;
            }
            FunnyDBSDKAgent.Instance.Init(GameData.IsRelease());
        }

        private void InitFunnySDK() {
            // BuildMode = 3: 正式版本
            FunnyAccountSDKAgent.Instance.Init(GameData.IsRelease());
            FunnyDBSDKAgent.Instance.ReportLoginLog("InitFunnySDK", FunnyDBSDKAgent.LoginStep.InitFunnySDK);
        }

        private void InitSentry() {
            SentrySDKAgent.Instance.Init(GameData.GetServerName(), GameData.ShowGameVersion, GameData.NetworkVersion);
        }

        private class Task {
            protected readonly string taskName;
            protected readonly Action action;

            public Task(string taskName, Action action) {
                this.taskName = taskName;
                this.action = action;
            }

            public virtual void OnStart() {
                var taskSign = $"初始化：{taskName}";
                PerfAnalyzerAgent.BeginSample(taskSign);
                try {
#if UNITY_EDITOR
                    Debug.Log(taskSign);
#endif
                    PerfAnalyzerAgent.SetLog(taskSign);
                    action?.Invoke();
                } catch (Exception ex) {
                    Debug.LogErrorFormat("启动任务异常:{0} ,exception:{1}", taskName, ex);
                }
                PerfAnalyzerAgent.EndSample(taskSign);
            }
        }
    }
}