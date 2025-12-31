using System.Collections.Generic;
using System.Linq;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerDropCondition_Base : ComponentBase {
        private int[] originalDropIdList = null; // 原始掉落物品 ID 列表
        protected List<int> dropIdList = null;
        protected List<float> dropTypeValueList = null;
        protected GpoDropCondition dropCondition = GpoDropCondition.DeadDrop;
        protected GpoDropType gpoDropTypeData;
        protected int dropTypeId;

        protected override void OnAwakeBase() {
            base.OnAwakeBase();
            mySystem.Register<SE_Item.Event_SetDropData>(OnSetDropData);
        }

        protected override void OnClearBase() {
            base.OnClearBase();
            mySystem.Unregister<SE_Item.Event_SetDropData>(OnSetDropData);
        }

        private void OnSetDropData(ISystemMsg sys, SE_Item.Event_SetDropData ent) {
            dropTypeId = ent.DropTypeId;
            originalDropIdList = ent.DropIdList;
            gpoDropTypeData = GpoDropTypeSet.GetGpoDropTypeById((ushort)ent.DropTypeId);
            dropCondition = (GpoDropCondition)gpoDropTypeData.DropCondition;
            SetDropData();
        }

        protected void SetDropData() {
            dropIdList = originalDropIdList.ToList();
            dropTypeValueList = gpoDropTypeData.DropTypeValues.ToList();
            OnSetDropData();
        }

        virtual protected void OnSetDropData() {
        }

        virtual protected void PlayDropItem(IGPO dropGpo, int dropItemId) {
            mySystem.Dispatcher(new SE_Item.Event_DropItem {
                DropTypeId = dropTypeId,
                DropGpo = dropGpo,
                DropItemId = dropItemId,
            });
        }
    }
}