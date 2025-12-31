using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientElementBox {
        public SceneData.ElementEnum Element { get; private set; }
        public int CreateId { get; private set; }
        public Vector3 Point { get; private set; }
        private GameObject gameObj;
        private Transform gameTran;
        private bool isInit = false;

        public void Init(ushort resourceId, int createId, Vector3 point) {
            isInit = true;
            Element = (SceneData.ElementEnum)resourceId;
            CreateId = createId;
            Point = point;
            AddItemMod();
        }

        public void Clear() {
            isInit = false;
            RemoveItem();
        }

        private void AddItemMod() {
            AssetManager.LoadGamePlayObjAsync($"SceneElement/{Element}", LoadGameObjectCallBack);
        }

        private void LoadGameObjectCallBack(GameObject perfab) {
            if (isInit == false) {
                return;
            }
            if (perfab != null) {
                gameObj = GameObject.Instantiate(perfab);
                gameTran = gameObj.transform;
                MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                    layer = StageData.GameWorldLayerType.Item, transform = gameTran
                });
                SetGameObj(gameTran);
                gameTran.position = Point;
            } else {
                Debug.LogError("Resource 加载失败:" + Element);
            }
        }

        private void SetGameObj(Transform tran) {
            tran.localPosition = Vector3.zero;
            tran.localRotation = Quaternion.identity;
            tran.localScale = Vector3.one;
        }

        private void RemoveItem() {
            if (gameObj == null) {
                return;
            }
            GameObject.Destroy(gameObj);
            gameObj = null;
            gameTran = null;
        }
    }
}