using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Util;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGetSaveBattleData : ComponentBase {
        private List<Vector3> startPointList;

        protected override void OnAwake() {
            MsgRegister.Register<SM_Mode.Event_GetSaveBattleData>(OnGetSaveBattleDataCallBack);
        }

        protected override void OnStart() {
        }

        protected override void OnClear() {
            MsgRegister.Unregister<SM_Mode.Event_GetSaveBattleData>(OnGetSaveBattleDataCallBack);
        }

        private void OnGetSaveBattleDataCallBack(SM_Mode.Event_GetSaveBattleData ent) {
            LoadSaveData(ent.CharacterGPO, ent.ByteDatas);
        }

        private void LoadSaveData(IGPO gpo, byte[] bytesData) {
            using (var byteBuffer = ByteBuffer.Wrap(bytesData)) {
                var saveTime = byteBuffer.ReadLong();
                var saveName = byteBuffer.ReadString();
                GetItemData(gpo, byteBuffer);
                GetWeaponData(gpo, byteBuffer);
                GetMonsterData(gpo, byteBuffer);
            }
        }

        private void GetItemData(IGPO gpo, ByteBuffer byteBuffer) {
            var len = byteBuffer.ReadInt();
            for (int i = 0; i < len; i++) {
                var itemId = byteBuffer.ReadInt();
                var itemNum = byteBuffer.ReadUShort();
                var isQuickUse = byteBuffer.ReadBoolean();
                var modData = ItemData.GetData(itemId);
                if (modData.Sign == "MonsterBall") {
                    continue;
                }
                gpo.Dispatcher(new SE_Item.Event_AddPickItem {
                    ItemId = itemId, ItemNum = itemNum, IsQuickUse = isQuickUse,
                });
            }
        }

        private void GetWeaponData(IGPO gpo, ByteBuffer byteBuffer) {
            var len = byteBuffer.ReadInt();
            for (int i = 0; i < len; i++) {
                var weaponItemId = byteBuffer.ReadInt();
                var modData = ItemData.GetData(weaponItemId);
                gpo.Dispatcher(new SE_GPO.AddWeaponPack {
                    AddWeaponId = modData.Id,
                    AddWeaponSkinId = WeaponSkinData.GetDefaultSkinItemId(weaponItemId)
                });
            }
        }

        private void GetMonsterData(IGPO gpo, ByteBuffer byteBuffer) {
            var len = byteBuffer.ReadInt();
            for (int i = 0; i < len; i++) {
                var monsterSign = byteBuffer.ReadString();
                var monsterLevel = byteBuffer.ReadInt();
                // var data = MonsterData.CreateMonsterForLevel(monsterSign, monsterLevel);
                // gpo.Dispatcher(new SE_Monster.Event_CatchMonsterState {
                //     CatchMonsterData = data, IsSuccess = true
                // });
            }
        }
    }
}