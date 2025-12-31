using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class GameTestControl : ComponentBase {
        private Dictionary<GameTestEnum, List<GameObject>> testList = new Dictionary<GameTestEnum, List<GameObject>>();
        private bool isShowScene = true;
        private bool isShowTerrain = true;

        protected override void OnAwake() {
            MsgRegister.Register<M_Game.HideScene>(OnHideSceneCallBack);
            MsgRegister.Register<M_Game.HideTerrain>(OnHideTerrainCallBack);
            MsgRegister.Register<M_Game.AddGameTestObj>(OnAddGameTestObjCallBack);
        }
        
        protected override void OnClear() {
            base.OnClear();
            testList.Clear();
            MsgRegister.Unregister<M_Game.HideScene>(OnHideSceneCallBack);
            MsgRegister.Unregister<M_Game.HideTerrain>(OnHideTerrainCallBack);
            MsgRegister.Unregister<M_Game.AddGameTestObj>(OnAddGameTestObjCallBack);
        }
        private void OnHideTerrainCallBack(M_Game.HideTerrain ent) {
            List<GameObject> list;
            if (testList.TryGetValue(GameTestEnum.Scene, out list) == false) {
                return;
            }
            isShowTerrain = !isShowTerrain;
            for (int i = 0; i < list.Count; i++) {
                var gameObj = list[i];
                var terrainList = gameObj.GetComponentsInChildren<Terrain>();
                for (int j = 0; j < terrainList.Length; j++) {
                    var terrain = terrainList[j];
                    if (isShowTerrain) {
                        terrain.enabled = true;
                    } else {
                        terrain.enabled = false;
                    }
                }
            }
        }
        private void OnHideSceneCallBack(M_Game.HideScene ent) {
            List<GameObject> list;
            if (testList.TryGetValue(GameTestEnum.Scene, out list) == false) {
                return;
            }
            isShowScene = !isShowScene;
            for (int i = 0; i < list.Count; i++) {
                var gameObj = list[i];
                var renderList = gameObj.GetComponentsInChildren<Renderer>();
                for (int j = 0; j < renderList.Length; j++) {
                    var renderer = renderList[j];
                    if (isShowScene) {
                        renderer.enabled = true;
                    } else {
                        renderer.enabled = false;
                    }
                }
            }
        }

        private void OnAddGameTestObjCallBack(M_Game.AddGameTestObj ent) {
            var state = (GameTestEnum)ent.GameTestIndex;
            List<GameObject> list;
            if (testList.TryGetValue(state, out list) == false) {
                list = new List<GameObject>();
                testList.Add(state, list);
            }
            list.Add(ent.GameObj);
        }
    }
}