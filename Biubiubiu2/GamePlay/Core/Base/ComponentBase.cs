using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class ComponentBase: SystemBase.IComponent {
        public SystemBase mySystem;
        private bool isInit = false;
        protected bool isClear = false;
        protected bool isSetEntityObj = false;

        public IEntity iEntity {
            get { return this.isClear ? null : mySystem.GetEntity(); }
        }

        public INetwork networkBase {
            get { return this.isClear ? null : mySystem.network; }
        }

        public ManagerBase manager {
            get { return this.isClear ? null : mySystem.manager; }
        }

        public INetwork tempSaveNetwokBase = null;
        public IEntity tempSaveIEntity = null;

        public IGPO iGPO {
            get { return this.isClear ? null : mySystem.GetGPO(); }
        }

        public int GpoID {
            get { return iGPO == null ? 0 : iGPO.GetGpoID(); }
        }

        public int ConnID {
            get { return this.isClear ? -1 : mySystem.ConnID; }
        }

        public bool IsDebug = false;
        protected string sign = "";
        protected SystemBase.IComponentInitData initDataBase;
        
        /// <summary>
        /// 业务层不要直接重写 Awake 很危险，请直接用 OnAwake
        /// </summary>
        /// <param name="system"></param>
        public void Awake(SystemBase system, SystemBase.IComponentInitData data) {
            if (system == null) {
                Debug.LogError("Awake system is null");
            }
            this.isInit = true;
            this.initDataBase = data;
            this.mySystem = system;
            this.mySystem.Register<Event_SystemBase.SetNetwork>(EventSetNetwork);
            this.mySystem.Register<Event_SystemBase.SetEntityObj>(EventSetEntityObj);
            OnAwakeBase();
            OnAwake();
        }

        /// <summary>
        /// 这个里面只适合放协议的注册和监听 (基类用)
        /// </summary>
        virtual protected void OnAwakeBase() {
        }

        /// <summary>
        /// 这个里面只适合放协议的注册和监听
        /// </summary>
        virtual protected void OnAwake() {
        }

        /// <summary>
        /// 业务层不要直接重写 Start 很危险，请直接用 OnStart
        /// </summary>
        /// <param name="system"></param>
        public void Start() {
            if (this.isClear == false) {
                if (iEntity as EntityBase) {
                    SetEntityObj();
                }
                OnStartBase();
                OnStart();
            }
        }

        /// <summary>
        /// 这个里面只适合放协议的注册和监听 (基类用)
        /// </summary>
        virtual protected void OnStartBase() {
        }

        /// <summary>
        /// Start 里面可以放对事件的触发
        /// </summary>
        virtual protected void OnStart() {
        }

        /// <summary>
        /// 业务层不要直接重写 Clear 很危险，请直接用 OnClear
        /// </summary>
        /// <param name="system"></param>
        public void Clear() {
            if (this.isClear) {
                return;
            }
            try {
                OnClear();
                OnClearBase();
            } catch (Exception e) {
                Debug.LogError($"[{GetType().Name}] Clear() 异常: {e}");
            } finally {
                mySystem.Unregister<Event_SystemBase.SetNetwork>(EventSetNetwork);
                mySystem.Unregister<Event_SystemBase.SetEntityObj>(EventSetEntityObj);
                tempSaveNetwokBase = null;
                tempSaveIEntity = null;
                this.mySystem = null;
                this.initDataBase = null;
                sign = "";
                this.isClear = true;
            }
        }
        
        virtual protected void OnClearBase() {
        }

        virtual protected void OnClear() {
        }

        protected void AddUpdate(Action<float> onUpdate) {
            this.mySystem.AddUpdate(onUpdate);
        }

        protected void RemoveUpdate(Action<float> onUpdate) {
            this.mySystem?.RemoveUpdate(onUpdate);
        }

        private void EventSetNetwork(ISystemMsg body, Event_SystemBase.SetNetwork ent) {
            SetNetwork();
        }

        /// <summary>
        /// 业务层不要直接重写 SetNetwork 很危险，请直接用 OnSetNetwork
        /// </summary>
        /// <param name="system"></param>
        public void SetNetwork() {
            if (this.isClear) {
                return;
            }
            if (networkBase == null || networkBase == tempSaveNetwokBase) {
                return;
            }
            tempSaveNetwokBase = networkBase;
            OnSetNetworkBase();
            OnSetNetwork();
        }

        virtual protected void OnSetNetworkBase() {
        }

        virtual protected void OnSetNetwork() {
        }

        private void EventSetEntityObj(ISystemMsg body, Event_SystemBase.SetEntityObj ent) {
            SetEntityObj();
        }

        private void SetEntityObj() {
            if (this.isClear) {
                return;
            }
            if (iEntity == null || (iEntity is EntityBase) == false) {
                return;
            }
            if (iEntity == tempSaveIEntity) {
                return;
            }
            tempSaveIEntity = iEntity;
            isSetEntityObj = true;
            OnSetEntityObj(iEntity);
        }

        virtual protected void OnSetEntityObj(IEntity iEntity) {
        }

        /// <summary>
        /// 注册接收该网络对象放送的所有消息
        /// </summary>
        virtual protected void AddProtoCallBack(string id, Action<INetwork, IProto_Doc> action) {
            if (null == networkBase) {
                Debug.LogError($"AddProtoCallBack 缺少网络组件:{id}");
                return;
            }
            if (networkBase is INetworkCharacter) {
                networkBase.AddProtoCallBack(id, action);
            } else {
                if (ConnID <= 0) {
                    networkBase.AddProtoCallBack(id, action);
                } else {
                    networkBase.AddProtoCallBack(ConnID, id, action);
                }
            }
        }

        /// <summary>
        /// 清除接收该网络对象放送的所有消息
        /// </summary>
        virtual protected void RemoveProtoCallBack(string id, Action<INetwork, IProto_Doc> action) {
            if (null == networkBase) {
                return;
            }
            if (networkBase is INetworkCharacter) {
                networkBase.RemoveProtoCallBack(id, action);
            } else {
                if (ConnID <= 0) {
                    networkBase.RemoveProtoCallBack(id, action);
                } else {
                    networkBase.RemoveProtoCallBack(ConnID, id, action);
                }
            }
        }

        /// <summary>
        /// 发送给指定 Network 下的当前网络对象
        /// </summary>
        virtual protected void TargetRpc(INetwork network, ITargetRpc proto) {
            if (null == networkBase || networkBase.IsDestroy()) {
                return;
            }
            this.networkBase.TargetRpc(network, proto);
        }

        virtual protected void TargetRpcList(List<INetworkCharacter> list, ITargetRpc proto) {
            if (null == networkBase || networkBase.IsDestroy()) {
                return;
            }
            this.networkBase.TargetRpcList(list, proto);
        }
        /// <summary>
        /// 发送给所有 Network 下的当前网络对象
        /// </summary>
        virtual protected void Rpc(IRpc proto) {
            if (null == networkBase || networkBase.IsDestroy()) {
                return;
            }
            this.networkBase.Rpc(proto);
        }

        public void Register<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent {
            this.mySystem?.Register(handler);
        }

        public void Register<T>(Action<ISystemMsg, T> handler, int priority) where T : GamePlayEvent.ISystemEvent {
            this.mySystem?.Register(handler, priority);
        }

        public void Unregister<T>(Action<ISystemMsg, T> handler) where T : GamePlayEvent.ISystemEvent {
            this.mySystem?.Unregister(handler);
        }

        public void Dispatcher<T>(T ent) where T : GamePlayEvent.ISystemEvent {
            this.mySystem?.Dispatcher(ent);
        }

        public bool IsClear() {
            return isClear;
        }

        public int GetConnID() {
            return ConnID;
        }

        public string GetName() {
            return iEntity != null ? iEntity.GetName() : "NULL";
        }

        public string GetSign() {
            return sign;
        }

        public void SetSign(string sign) {
            this.sign = sign;
        }

        public IEntity GetEntity() {
            return iEntity;
        }

        public INetwork GetNetwork() {
            return networkBase;
        }

        public void StartDebug() {
            IsDebug = true;
        }
    }
}