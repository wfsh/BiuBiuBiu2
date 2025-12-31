using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public partial class ServerGPODropItem : ComponentBase {
        private int dropItemId = 0;
        private GpoDropCondition dropCondition = GpoDropCondition.DeadDrop;
        private ushort dropTypeId = 0;
        private int[] dropIdList = Array.Empty<int>();
        private List<IGPO> attackGPOList = new List<IGPO>();

        protected override void OnAwake() {
            mySystem.Register<SE_Item.Event_GetDropBoxId>(GetDropBoxCallBack);
            mySystem.Register<SE_Item.Event_DropItem>(OnDropItemCallBack);
            mySystem.Register<SE_GPO.Event_AfterDownHP>(OnAfterDownHpCallBack);
            mySystem.Register<SE_GPO.Event_ReLife>(OnReLifeCallBack);
            GetDropData();
            AddDropConditionComponent();
        }

        protected override void OnStart() {
            base.OnStart();
            mySystem.Dispatcher(new SE_Item.Event_SetDropData {
                DropIdList = dropIdList,  
                DropTypeId = dropTypeId, 
            });
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_Item.Event_GetDropBoxId>(GetDropBoxCallBack);
            mySystem.Unregister<SE_Item.Event_DropItem>(OnDropItemCallBack);
            mySystem.Unregister<SE_GPO.Event_AfterDownHP>(OnAfterDownHpCallBack);
            mySystem.Unregister<SE_GPO.Event_ReLife>(OnReLifeCallBack);
        }
        
        private void GetDropData() {
            var mData = iGPO.GetMData();
            dropIdList = mData.GetGpoDropId(); // 掉落的物品 ID 列表 (要测试可以改这边)
            dropTypeId = mData.GetGpoDropType(); // 掉落类方式型 ID (要测试可以改这边)
            var gpoDropData = GpoDropTypeSet.GetGpoDropTypeById(dropTypeId);
            dropCondition = (GpoDropCondition)gpoDropData.DropCondition;
        }

        private void GetDropBoxCallBack(ISystemMsg body, SE_Item.Event_GetDropBoxId ent) {
            ent.CallBack?.Invoke(dropItemId);
        }

        private void OnAfterDownHpCallBack(ISystemMsg body, SE_GPO.Event_AfterDownHP ent) {
            SetAttackRole(ent.AttackGPO);
        }

        private void SetAttackRole(IGPO attackGpo) {
            if (attackGpo == null) {
                return;
            }
            if (attackGpo.GetGPOType() == GPOData.GPOType.MasterAI) {
                attackGpo.Dispatcher(new SE_AI.Event_GetMasterGPO {
                    CallBack = masterGPO => {
                        attackGpo = masterGPO;
                    }
                });
            }
            if (attackGPOList.Contains(attackGpo)) {
                return;
            }
            attackGPOList.Add(attackGpo);
        }
        
        private void OnReLifeCallBack(ISystemMsg body, SE_GPO.Event_ReLife ent) {
            attackGPOList.Clear();
        }

        private void OnDropItemCallBack(ISystemMsg body, SE_Item.Event_DropItem ent) {
            dropItemId = ent.DropItemId;
            IGPO attackGpo = null; 
            mySystem.Dispatcher(new SE_GPO.Event_GetLastAttackGpo {
                CallBack = gpo => {
                    attackGpo = gpo;
                }
            });
            MsgRegister.Dispatcher(new SM_Mode.Event_DropItem {
                AttackGPO = attackGpo,
                AttackGPOList = attackGPOList,
                DropGpo = ent.DropGpo,
                DropId = ent.DropTypeId,
                DropItemId = ent.DropItemId,
                DropType = dropCondition,
                OR_DropPoint = ent.OR_DropPoint
            });
        }
    }
}