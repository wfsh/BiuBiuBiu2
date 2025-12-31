using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class C_Ability_Base : SystemBase {
        public IProto_Doc InData { get; private set; }
        public IAbilityMData MData { get; private set; }
        public int AbilityId { get; private set; }
        public ushort ConfigID { get; private set; }
        public byte RowId { get; private set; }
        public int FireGpoId { get; private set; }
        private bool isUsePoolEntity = false;
        private float invokeClearPoolEntityTime = 0.0f;
        private string poolUrl = "";

        public void SetAbilityData(int abilityId, int fireGpoId, IProto_Doc data, IAbilityMData mData) {
            this.AbilityId = abilityId;
            this.InData = data;
            if (mData != null) {
                this.RowId = mData.GetRowID();
                this.ConfigID = mData.GetConfigId();
            }
            this.MData = mData;
            this.FireGpoId = fireGpoId;
        }

        public void CreateEntity(string sign) {
            if (string.IsNullOrEmpty(sign)) {
                Debug.LogError(" Ability CreateEntity sign is null ConfigId = " + ConfigID + " RowId = " + RowId);
                return;
            }
            CreateEntityObj(string.Concat("Ability/", sign), StageData.GameWorldLayerType.Ability);
        }

        /// <summary>
        /// 使用缓冲池的方式创建实体
        /// </summary>
        /// <param name="sign"></param>
        public void CreateEntityToPool(string sign) {
            if (string.IsNullOrEmpty(sign)) {
                Debug.LogError($"Ability CreateEntityToPool sign is null : {this.GetType()}");
                return;
            }
            poolUrl = $"{AssetURL.GamePlay}/Ability/{sign}.prefab";
            PrefabPoolManager.OnGetPrefab(poolUrl,
                null,
                gameObj => {
                    if (IsClear()) {
                        PrefabPoolManager.OnReturnPrefab(poolUrl, gameObj);
                        return;
                    }
                    isUsePoolEntity = true;
                    SetEntity(gameObj, StageData.GameWorldLayerType.Ability);
                });
        }

        protected override void OnRemoveEntity() {
            base.OnRemoveEntity();
            if (isUsePoolEntity) {
                var entityBase = (EntityBase)iEntity;
                if (invokeClearPoolEntityTime <= 0f || ModeData.IsIntoMode == false) {
                    PrefabPoolManager.OnReturnPrefab(poolUrl, entityBase.gameObject);
                } else {
                    UpdateRegister.AddInvoke(() => {
                        if (entityBase == null) return;
                        PrefabPoolManager.OnReturnPrefab(poolUrl, entityBase.gameObject);
                    }, invokeClearPoolEntityTime);
                }
            }
        }

        /// <summary>
        /// 部分特效需要在 AB 被销毁后仍然存在一段时间，需要设置销毁时间
        /// </summary>
        /// <param name="time"></param>
        protected void SetInvokeClearPoolEntityTime(float time) {
            invokeClearPoolEntityTime = time;
        }

        // public void GetSceneEntity(int elementId) {
        //     MsgRegister.Dispatcher(new CM_SceneElement.GetSceneElement {
        //         CallBack = SetSceneEntity, ElementId = elementId
        //     });
        // }

        public void SetSceneEntity(IEntity iEntity) {
            SetIEntity(iEntity);
        }

        public int GetAbilityId() {
            return this.AbilityId;
        }

        public Vector3 GetPoint() {
            return iEntity.GetPoint();
        }
    }
}