using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using System.Threading.Tasks;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public partial class SystemBase : ManagerBase.ISystem, ISystemMsg {
        public interface IComponentInitData  {}
        public interface IComponent {
            void Awake(SystemBase system, IComponentInitData initData);
            void Start();
            void SetNetwork();
            void Clear();
            void StartDebug();
        }
        public INetwork network {
            get;
            private set;
        }
        public ManagerBase manager {
            get;
            private set;
        }
        public int ConnID {
            get {
                return connId;
            }
        }
        protected List<IComponent> components = new List<IComponent>();
        private List<BodyUpdate> updateList = new List<BodyUpdate>();
        private List<IComponent> invokeStartComponents = new List<IComponent>();
        protected IEntity iEntity;
        private int connId = -1;
        private GameObject entityObj;
        private bool init = false;
        private bool isClear = false;
        private bool isClearIng = false;
        private bool isGameObjDestory = false;
        private bool isStart = false;
        private bool isAwake = false;
        protected bool isDebug = false;
        protected IGPO Gpo;
        protected int GpoID;
        public void Init(ManagerBase manager) {
            if (init) {
                Debug.LogError("重复初始化");
                return;
            }
            init = true;
            this.manager = manager;
            this.iEntity = new EntityData();
        }
        

        /// <summary>
        /// 业务层不要直接重写 Awake 很危险，请直接用 OnAwake
        /// 需要在 AddSystem 后手动在需要的地方调用
        /// </summary>
        /// <param name="system"></param>
        public void Awake() {
            OnAwakeBase();
            OnAwake();
            SendNetwork();
            isAwake = true;
        }
        virtual protected void OnAwakeBase() {
        }

        virtual protected void OnAwake() {
        }

        /// <summary>
        /// 业务层不要直接重写 Start 很危险，请直接用 OnStart
        /// </summary>
        public void Start() {
            if (IsClear()) {
                return;
            }
            SendEntityObj();
            isStart = true;
            OnStartBase();
            OnStart();
        }
        virtual protected void OnStartBase() {
        }

        virtual protected void OnStart() {
        }

        /// <summary>
        /// 业务层不要直接重写 Clear 很危险，请直接用 OnClear
        /// </summary>
        /// <param name="system"></param>
        public void Clear() {
            if (isClearIng) {
                return;
            }
            isClearIng = true;
            updateList.Clear();
            OnStartClear();
        }
        virtual protected void OnStartClear() {
            
        }

        public void ClearImmediate() {
            if (isClear == true) {
                return;
            }
            try {
                RemoveAllUpdate();
                RemoveALLComponent();
                OnClear();
                OnClearBase();
                RemoveEntity();
            } catch (Exception e) {
                Debug.LogError($"[{GetType().Name}] Clear() 异常: {e}");
            } finally {
                isClear = true;
                updateList.Clear();
                handlers.Clear();
                this.iEntity = null;
                this.manager = null;
                this.network = null;
            }
        }

        protected virtual void OnClear() {
        }

        virtual protected void OnClearBase() {
        }

        public void AddUpdate(Action<float> callBack) {
            if (IsClear()) {
                return;
            }
#if UNITY_EDITOR
            if (HasUpdateBody(callBack)) {
                Debug.LogError("重复添加 Update");
            }
#endif
            updateList.Add(new BodyUpdate(callBack));
        }
        public void RemoveUpdate(Action<float> callBack) {
            for (int i = 0; i < updateList.Count; i++) {
                var body = updateList[i];
                if (body.Handler == callBack) {
                    updateList.RemoveAt(i);
                    break;
                }
            }
        }

        private void RemoveAllUpdate() {
            updateList.Clear();
        }

        public void Update(float deltaTime) {
            InvokeComponentStart();
            for (int i = 0; i < updateList.Count; i++) {
                var body = updateList[i];
                if (body == null) {
                    continue;
                }
                try {
                    body.Invoke(deltaTime);
                } catch (Exception e) {
                    Debug.LogError($"[UpdateSystem] Error in callback: {body.GetTypeName()} \nException: {e}");
                } 
            }
        }
        public void AddComponent<T>()  where T : IComponent, new() {
            AddComponentChild<T>();
        }
        
        /// <summary>
        /// 这个不要再直接用了。之前个也都会陆续改掉
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T AddComponentChild<T>() where T : IComponent, new() {
#if UNITY_EDITOR
            var isTrue = HasComponent<T>();
            if (isTrue) {
                Debug.LogError($"Component 重复注册 {typeof(T)}");
            }
#endif
            IComponent feature = new T();
            components.Add(feature);
            try {
                feature.Awake(this, null);
                if (isAwake) {
                    feature.SetNetwork();
                }
                invokeStartComponents.Add(feature);
            } catch (Exception e) {
                Debug.LogError($"AddComponent {feature} Error:" + e);
            }
            return (T)feature;
        }
        
        public void AddComponent<T>(IComponentInitData data) 
            where T : IComponent, new() {
            #if UNITY_EDITOR
            var isTrue = HasComponent<T>();
            if (isTrue) {
                Debug.LogError($"Component 重复注册 {typeof(T)}");
            }
            #endif
            IComponent feature = new T();
            components.Add(feature);
            try {
                feature.Awake(this, data);
                if (isAwake) {
                    feature.SetNetwork();
                }
                invokeStartComponents.Add(feature);
            } catch (Exception e) {
                Debug.LogError($"AddComponent {feature} Error:" + e);
            }
        }
        
        private void InvokeComponentStart() {
            if (invokeStartComponents.Count == 0) {
                return;
            }
            for (int i = 0; i < invokeStartComponents.Count; i++) {
                var feature = invokeStartComponents[i];
                try {
                    feature.Start();
                } catch (Exception e) {
                    Debug.LogError($"StartComponent {feature} Error:" + e);
                }
            }
            invokeStartComponents.Clear();
        }
        // private T GetComponent<T>() where T : IComponent, new() {
        //     var t = typeof(T);
        //     int length = components.Count;
        //     for (int i = 0; i < length; ++i) {
        //         var c = components[i];
        //         if (t == c.GetType()) {
        //             return (T)c;
        //         }
        //     }
        //     Debug.LogError("没有找到对应的 Compoenent:" + t);
        //     return new T();
        // }
        protected bool HasComponent<T>() where T : IComponent {
            var t = typeof(T);
            int length = components.Count;
            for (int i = 0; i < length; ++i) {
                var c = components[i];
                if (t == c.GetType()) {
                    return true;
                }
            }
            return false;
        }
        protected bool RemoveComponent<T>() where T : IComponent {
            var t = typeof(T);
            int length = components.Count;
            for (int i = 0; i < length; ++i) {
                var c = components[i];
                if (t == c.GetType()) {
                    c.Clear();
                    components.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool RemoveComponent(IComponent component) {
            int length = components.Count;
            for (int i = 0; i < length; ++i) {
                var c = components[i];
                if (component == c) {
                    c.Clear();
                    components.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        protected void RemoveALLComponent() {
            var count = components.Count - 1;
            for (int i = count; i >= 0; i--) {
                var component = components[i];
                if (component == null) {
                    continue;
                }
                try {
                    component.Clear();
                } catch (Exception e) {
                    Debug.LogError($"[{component.GetType().Name}] Clear() 异常: {e}");
                }
            }
            components.Clear();
        }

        protected void CreateEntityObj(string url, StageData.GameWorldLayerType layer) {
            RemoveEntity();
            AssetManager.LoadGamePlayObjAsync(url, async prefab => {
                if (IsClear()) {
                    return;
                }
                isGameObjDestory = true;
                if (prefab != null) {
                    entityObj = await InstantiateAsyncWithTask(prefab);
                    if (isClear) {
                        GameObject.Destroy(entityObj);
                        entityObj = null;
                        return;
                    }
                    SetEntity(entityObj, layer);
                } else {
                    entityObj = null;
                    Debug.LogError("[Error]加载失败:" + url);
                    OnLoadEntityEnd(null);
                }
            });
        }
        
        public async Task<GameObject> InstantiateAsyncWithTask(GameObject prefab) {
            if (WarData.UseInstantiateAsync == false) {
                // 服务端直接同步实例化
                var go = GameObject.Instantiate(prefab);
                return go;
            } else {
                var request = GameObject.InstantiateAsync(prefab); 
                while (!request.isDone) {
                    await Task.Yield(); // 等待下一帧
                }
                return request.Result[0];
            }
        }

        
        protected void SetEntity(GameObject gameObject, StageData.GameWorldLayerType layer) {
            entityObj = gameObject;
            var iEntity = entityObj.GetComponent<IEntity>();
            if (iEntity == null) {
                iEntity = entityObj.AddComponent<EntityBase>();
            }
            if (layer != StageData.GameWorldLayerType.None) {
                MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                    layer = layer, transform = entityObj.transform
                });
            }
            SetIEntity(iEntity);
        }

        protected void SetIEntity(IEntity iEntity) {
            if (iEntity != null) {
                var entityData = (EntityData)this.iEntity;
                if (entityData.IsPointChange) {
                    iEntity.SetPoint(entityData.GetPoint());
                }
                if (entityData.IsRotaChange) {
                    iEntity.SetRota(entityData.GetRota());
                }
                if (entityData.IsScaleChange) {
                    iEntity.SetLocalScale(entityData.GetLocalScale());
                }
                iEntity.SetGpo(Gpo);
                iEntity.SetConnID(connId);
                var parent = entityData.GetParent();
                if (parent != null) {
                    iEntity.SetParent(parent);
                }
                entityData.Clear();
            }
            this.iEntity = iEntity;
            if (isStart) {
                SendEntityObj();
            }
        }
        
        private void SendEntityObj() {
            if (this.iEntity is EntityBase == false) {
                return;
            }
            try {
                OnLoadEntityEnd(iEntity);
            } catch (Exception e) {
                Debug.LogError($"OnLoadEntityEnd {iEntity} Error:" + e);
            }
            Dispatcher(new Event_SystemBase.SetEntityObj());
        }


        virtual protected void OnLoadEntityEnd(IEntity iEnter) {
        }

        protected void RemoveEntity() {
            if (entityObj == null) {
                return;
            }
            try {
                OnRemoveEntity();
                if (isGameObjDestory) {
                    GameObject.Destroy(entityObj);
                    if (iEntity != null) {
                        iEntity.Clear();
                    }
                }
            } catch (Exception e) {
                Debug.LogError($"[Error] RemoveEntity: {entityObj.name} e:{e}");
            } finally {
                entityObj = null;
                iEntity = null;
            }
        }

        virtual protected void OnRemoveEntity() {
            
        }

        public IEntity GetEntity() {
            return iEntity;
        }

        public bool IsClearIng() {
            return isClearIng;
        }
        
        public bool IsClear() {
            return isClear || isClearIng;
        }
        
        public void SetIGPO(IGPO iGPO) {
            this.Gpo = iGPO;
            this.GpoID = iGPO != null ? iGPO.GetGpoID() : 0;
            this.iEntity?.SetGpo(iGPO);
        }

        public IGPO GetGPO() {
            return Gpo;
        }

        public int GetGPOId() {
            return this.GpoID;
        }

        public void SetNetwork(INetwork network) {
            if (network == null) {
                Debug.LogError("network 不能进行 null 赋值");
                return;
            }
            this.network = network;
            if (network is CharacterNetwork) {
                SetConnID(network.ConnId());
            }
            if (isAwake) {
                SendNetwork();
            }
        } 

        private void SendNetwork() {
            if (this.network == null) {
                return;
            }
            OnSetNetwork();
            Dispatcher(new Event_SystemBase.SetNetwork {
                network = this.network,
            });
        }

        virtual protected void OnSetNetwork() {
        }

        virtual public void SetConnID(int connId) {
            if (network is CharacterNetwork == false) {
                if (ConnID > 0 && connId != ConnID) {
                    Debug.LogError($"每个对象绑定的 ConnID 都是唯一的，不能重复创建 {connId} !=> {ConnID}");
                    return;
                } 
            }
            this.connId = connId;
            this.iEntity.SetConnID(connId);
        }

        virtual public NetworkData.SpawnConnType GetSpawnConnType() {
            return NetworkData.SpawnConnType.None;
        }
        protected void StartDebug() {
            isDebug = true;
        }
        
        public bool HasUpdateBody(Action<float> callBack) {
            for (int i = 0; i < updateList.Count; i++) {
                if (updateList[i].Handler == callBack) {
                    return true;
                }
            }
            return false;
        }
        
        private class BodyUpdate {
            public Action<float> Handler;
            public  string TypeName;
            private bool isStartPerf = false;

            public BodyUpdate(Action<float> handler) {
                Handler = handler;
                var random = UnityEngine.Random.Range(0f, 1f);
                if (random < GameData.PerfRate) {
                    GetTypeName();
                    isStartPerf = true;
                } else {
                    TypeName = "";
                }
            }
            
            public string GetTypeName() {
                var tName = Handler.Target.GetType().ToString().Replace("Sofunny.BiuBiuBiu2.", "");
                TypeName = $"{tName}:{Handler.Method.Name}";
                return TypeName;
            }

            public void Invoke(float delta) {
                if (Handler == null) {
                    return;
                }
                if (isStartPerf) {
                    PerfAnalyzerAgent.BeginSample(TypeName);
                    Handler.Invoke(delta);
                    PerfAnalyzerAgent.EndSample(TypeName);
                } else {
                    Handler.Invoke(delta);
                }
            }
        }
    }
}