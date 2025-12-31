using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class EventDirectorBase : IEventDirectorBase {
        protected static int Index = 0;
        protected bool IsClear = false;
        protected string dataValue = "";
        protected IEventDirectorData mData;
        protected int ID = 0;
        public void Awake() {
            Index++;
            ID = Index;
            OnAwakeBase();
            OnAwake();
            UpdateRegister.AddInvoke(Start, 0f);
        }
        virtual protected void OnAwakeBase() {
        }
        virtual protected void OnAwake() {
        }
        
        public int GetID() {
            return ID;
        }

        protected void Start() {
            if (IsClear) {
                return;
            }
            OnStartBase();
            OnStart();
        }
        virtual protected void OnStartBase() {
        }
        
        virtual protected void OnStart() {
        }
        
        public void Clear() {
            if (IsClear) {
                return;
            }
            IsClear = true;
            OnClearBase();
            OnClear();
        }
        virtual protected void OnClearBase() {
        }
        virtual protected void OnClear() {
        }
        
        public void SetData(string data) {
            this.dataValue = data;
        }
        
        protected T SerializeData<T>() where T : IEventDirectorData, new() {
            if (string.IsNullOrEmpty(dataValue)) {
                return default;
            }
            mData = new T();
            try {
                mData.Serialize(dataValue);
            } catch (Exception e) {
                Debug.LogError($"EventDirectorBase {typeof(T)} Value: {dataValue} Error: {e}");
            }
            return (T)mData;
        }
    }
}
