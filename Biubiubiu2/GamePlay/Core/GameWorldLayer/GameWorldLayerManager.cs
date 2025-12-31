using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.CoreGamePlay {
    public class GameWorldLayerManager : ManagerBase {
        private GameWorldLayerEntity entity;

        protected override void OnStart() {
            base.OnStart();
            CreateEntity();
            MsgRegister.Register<M_Stage.SetGamePlayWorldLayer>(OnSetGamePlayWorldLayerCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<M_Stage.SetGamePlayWorldLayer>(OnSetGamePlayWorldLayerCallBack);
            if (entity != null) {
                GameObject.Destroy(entity.gameObject);
                entity = null;
            }
        }

        public void CreateEntity() {
            var prefab = AssetManager.LoadGameWorldLayer();
            if (prefab != null) {
                var roleGameObj = GameObject.Instantiate(prefab);
                entity = roleGameObj.GetComponent<GameWorldLayerEntity>();
            } else {
                Debug.LogError("GamePlayRole 加载失败:");
            }
        }

        public void OnSetGamePlayWorldLayerCallBack(M_Stage.SetGamePlayWorldLayer ent) {
            if (ent.transform == null) {
                return;
            }
            var layerTran = GetLayer((StageData.GameWorldLayerType)ent.layer);
            ent.transform.SetParent(layerTran);
        }

        private Transform GetLayer(StageData.GameWorldLayerType layerState) {
            Transform layer = null;
            switch (layerState) {
                case StageData.GameWorldLayerType.Base:
                    layer = entity.BaseLayer;
                    break;
                case StageData.GameWorldLayerType.AI:
                    layer = entity.MonsterLayer;
                    break;
                case StageData.GameWorldLayerType.Character:
                    layer = entity.CharacterLayer;
                    break;
                case StageData.GameWorldLayerType.Item:
                    layer = entity.ItemLayer;
                    break;
                case StageData.GameWorldLayerType.Ability:
                    layer = entity.AbilityLayer;
                    break;
                default:
                    Debug.LogError("层级设置错误:" + layerState);
                    break;
            }
            return layer;
        }
    }
}