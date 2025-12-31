using System.Collections.Generic;
using System.IO;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGetSaveBattleData : ComponentBase {
        private const string KEY_UseSaveData = "UseSaveData";
        protected override void OnAwake() {
            MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        }

        private void OnAddLocalGPOCallBack(CM_GPO.AddLocalGPO ent) {
            GetSaveBattleData();
        }

        private void GetSaveBattleData() {
            if (PlayerPrefs.HasKey(KEY_UseSaveData) == false) {
                Debug.LogError("没有该模式下可用战斗数据");
                return;
            }
            long titleIndex = 0;
            var isTrue = long.TryParse(PlayerPrefs.GetString(KEY_UseSaveData), out titleIndex);
            if (isTrue == false) {
                Debug.LogError("UseIndex 转换失败：" + PlayerPrefs.GetString(KEY_UseSaveData));
                return;
            }
            var saveDataUrl = GetLocalUrl(GetBattleDataFileName(titleIndex));
            if (File.Exists(saveDataUrl) == false) {
                Debug.LogError("缺少本地战斗文件：" + saveDataUrl);
                return;
            }
            var byteDatas = File.ReadAllBytes(saveDataUrl);
            MsgRegister.Dispatcher(new CM_Mode.SendSaveBattleData {
                ByteDatas = byteDatas
            });
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            base.OnSetEntityObj(entityBase);
        }

        private string GetBattleDataFileName(long saveTime) {
            return $"BattleData_{saveTime}.txt";
        }

        private string GetLocalUrl(string url) {
            return string.Concat(URI.GetLocalUrl(), "/", url);
        }
    }
}