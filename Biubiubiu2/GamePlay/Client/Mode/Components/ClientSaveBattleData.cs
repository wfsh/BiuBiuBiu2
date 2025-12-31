using System;
using System.Collections.Generic;
using System.IO;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientSaveBattleData : ComponentBase {
        private ClientGPO localGPO;
        private ByteBuffer byteBuffer;
        private string titleSign = "BattleSaveData.txt";

        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<CM_Mode.SaveData>(OnSaveDataCallBack);
            MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
            MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                CallBack = AddLocalGPO
            });
        }

        protected override void OnClear() {
            base.OnClear();
            this.localGPO = null;
            byteBuffer?.Dispose();
            byteBuffer = null;
            MsgRegister.Unregister<CM_Mode.SaveData>(OnSaveDataCallBack);
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        }

        private void OnAddLocalGPOCallBack(CM_GPO.AddLocalGPO ent) {
            AddLocalGPO(ent.LocalGPO);
        }

        private void AddLocalGPO(IGPO localGPO) {
            this.localGPO = (ClientGPO)localGPO;
        }

        private void OnSaveDataCallBack(CM_Mode.SaveData ent) {
            byteBuffer = ByteBuffer.Allocate();
            byteBuffer.Write(TimeUtil.GetMillisecond());
            byteBuffer.Write("临时存档");
            this.localGPO.Dispatcher(new CE_Game.Event_GetSaveItemData {
                CallBack = SaveItemDataCallBack,
            });
            this.localGPO.Dispatcher(new CE_Game.Event_GetSaveWeaponData {
                CallBack = SaveWeaponDataCallBack,
            });
            this.localGPO.Dispatcher(new CE_Game.Event_GetSaveMonsterData {
                CallBack = SaveMonsterDataCallBack,
            });
            var full = GetLocalUrl(titleSign);
            var byteData = new byte[byteBuffer.WriteIndex];
            Array.Copy(byteBuffer.RawBuffer, 0, byteData, 0, byteData.Length);
            Debug.Log("SaveData:" + full + " -- " + byteData.Length);
            SaveBattleData(full, byteData);
        }

        private void SaveItemDataCallBack(List<CE_Game.SaveItemData> list) {
            byteBuffer.Write(list.Count);
            for (int i = 0; i < list.Count; i++) {
                var data = list[i];
                byteBuffer.Write(data.ItemId);
                byteBuffer.Write(data.ItemNum);
                byteBuffer.Write(data.IsQuickUse);
            }
        }

        private void SaveWeaponDataCallBack(List<int> list) {
            byteBuffer.Write(list.Count);
            for (int i = 0; i < list.Count; i++) {
                var data = list[i];
                byteBuffer.Write(data);
            }
        }

        private void SaveMonsterDataCallBack(List<CE_Game.SaveMonsterData> list) {
            byteBuffer.Write(list.Count);
            for (int i = 0; i < list.Count; i++) {
                var data = list[i];
                byteBuffer.Write(data.MonsterSign);
                byteBuffer.Write(data.MonsterLevel);
            }
        }

        private string GetLocalUrl(string url) {
            var path = URI.GetLocalUrl();
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }
            path = string.Concat(path, "/", url);
            return path;
        }

        /// <summary>
        /// 将缓存报告同时写入本地 (防止杀端，崩溃后还未上传的数据丢失)
        /// </summary>
        private void SaveBattleData(string full, byte[] sendData) {
            try {
                if (File.Exists(full)) {
                    File.Delete(full);
                }
                File.WriteAllBytes(full, sendData);
            } catch (Exception e) {
                Debug.LogError("本地 BattleSaveData 数据写入失败，比如磁盘满了等情况。URL:" + full + " [E.Message] " + e.Message);
            }
        }
    }
}