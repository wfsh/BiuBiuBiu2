using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Asset;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    /// <summary>
    /// 4 月 25 日，完成了基础的特效显示。
    /// 但是后续需要针对开发 特效内存池，
    /// </summary>
    public class ABPlayEffect : ComponentBase {
        public float startTime;
        private float lifeTime = 0.0f;
        private string effectSign = "";
        private GameObject effectObj;
        private Vector3 startPoint = Vector3.zero;
        private Quaternion startRota = Quaternion.identity;


        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            ClearEffect();
            RemoveUpdate(OnUpdate);
            effectSign = "";
            effectObj = null;
        }

        private void OnUpdate(float delta) {
            HideEffect();
        }

        public void PlayEffect(string sign, float lifeTime, Vector3 point, Quaternion rota) {
            effectSign = sign;
            startPoint = point;
            startRota = rota;
            this.lifeTime = lifeTime;
            AssetManager.LoadGamePlayObjAsync("Effects/" + sign, LoadEffectsEndCallBack);
        }

        private void LoadEffectsEndCallBack(GameObject perfab) {
            if (perfab == null) {
                Debug.LogError("特效加载失败:" + effectSign);
                return;
            }
            if (mySystem == null) {
                return;
            }
            effectObj = GameObject.Instantiate(perfab);
            startTime = Time.realtimeSinceStartup;
            var tran = effectObj.transform;
            MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                layer = StageData.GameWorldLayerType.Item,
                transform = tran
            });
            effectObj.SetActive(true);
            tran.position = startPoint;
            tran.rotation = startRota;
            tran.localScale = Vector3.one;
        }

        private void ClearEffect() {
            if (effectObj) {
                GameObject.Destroy(effectObj);
                effectObj = null;
            }
        }

        private void HideEffect() {
            if (effectObj != null) {
                return;
            }
            if (Time.realtimeSinceStartup - startTime <= lifeTime) {
                return;
            }
            ClearEffect();
        }
    }
}