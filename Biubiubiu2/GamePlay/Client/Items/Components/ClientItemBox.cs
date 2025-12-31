using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientItemBox {
        public int ItemId { get; private set; }
        public int AutoItemId { get; private set; }
        public ushort ItemNum { get; private set; }
        public Vector3 Point { get; private set; }
        public Item TemplateData;
        private bool isInit = false;
        private GameObject gameObj;
        private Transform gameTran;

        public void Init(int itemId, int autoItemId, ushort itemNum, Vector3 point) {
            isInit = true;
            ItemId = itemId;
            AutoItemId = autoItemId;
            ItemNum = itemNum;
            Point = point;
            TemplateData = ItemData.GetData(ItemId);
            AddItemMod();
        }

        public void Clear() {
            isInit = false;
            RemoveItem();
        }

        private void AddItemMod() {
            AssetManager.LoadGamePlayObjAsync($"Items/{TemplateData.Sign}", LoadGameObjectCallBack);
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
                Debug.LogError("Item 加载失败:" + TemplateData.Sign);
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