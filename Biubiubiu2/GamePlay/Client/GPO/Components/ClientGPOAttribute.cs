using System;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGPOAttribute : ComponentBase {
        protected GPOData.AttributeData attributeData;
        protected C_GPO_Base gpoSystem;
        
        protected override void OnAwake() {
            gpoSystem = (C_GPO_Base)mySystem;
            mySystem.Register<CE_GPO.Event_GetAttribute>(OnGetAttribute);
            mySystem.Register<CE_GPO.Event_GetHPData>(OnGetHpData);
            mySystem.Register<CE_GPO.Event_GetLevel>(OnGetLevelCallBack);
            attributeData = CreateAttribute();
            OnCreateAttribute(attributeData, gpoSystem.MData);
            gpoSystem.SetAttributeData(attributeData);
        }

        protected override void OnStart() {
            base.OnStart();
            ChangeHP();
            ChangeMaxHp();
        }
        
        protected override void OnClear() {
            RemoveProtoCallBack(Proto_GPO.Rpc_ChangeHP.ID, OnChangeHP);
            RemoveProtoCallBack(Proto_GPO.Rpc_ChangeMaxHP.ID, OnChangeMaxHP);
            RemoveProtoCallBack(Proto_GPO.Rpc_AfterDownHP.ID, OnAfterDownHP);
            RemoveProtoCallBack(Proto_GPO.Rpc_ChangeLevel.ID, OnChangeLevelCallBack);
            mySystem.Unregister<CE_GPO.Event_GetAttribute>(OnGetAttribute);
            mySystem.Unregister<CE_GPO.Event_GetHPData>(OnGetHpData);
            mySystem.Unregister<CE_GPO.Event_GetLevel>(OnGetLevelCallBack);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_GPO.Rpc_ChangeHP.ID, OnChangeHP);
            AddProtoCallBack(Proto_GPO.Rpc_ChangeMaxHP.ID, OnChangeMaxHP);
            AddProtoCallBack(Proto_GPO.Rpc_AfterDownHP.ID, OnAfterDownHP);
            AddProtoCallBack(Proto_GPO.Rpc_ChangeLevel.ID, OnChangeLevelCallBack);
        }

        virtual protected void OnCreateAttribute(GPOData.AttributeData data, IGPOM gpoMData) {
        }
        
        virtual protected GPOData.AttributeData CreateAttribute() {
            var data = new GPOData.AttributeData();
            data.Level = 1;
            data.maxHp = 1;
            data.nowHp = 1;
            return data;
        }

        private void OnGetAttribute(ISystemMsg body, CE_GPO.Event_GetAttribute ent) {
            ent.CallBack(attributeData);
        }

        private void OnGetHpData(ISystemMsg body, CE_GPO.Event_GetHPData ent) {
            ent.CallBack(new CE_GPO.Para_GetHpData {
                Hp = attributeData.nowHp,
                MaxHp = attributeData.maxHp,
            });
        }
        
        private void OnChangeHP(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_GPO.Rpc_ChangeHP)docData;
            if (attributeData == null) {
                return;
            }
            if (attributeData.nowHp != rpcData.nowHP) {
                attributeData.nowHp = rpcData.nowHP;
                ChangeHP();
            }
        }

        private void OnChangeMaxHP(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_GPO.Rpc_ChangeMaxHP)docData;
            attributeData.maxHp = rpcData.nowMaxHP;
            ChangeMaxHp();
        }

        private void OnAfterDownHP(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_GPO.Rpc_AfterDownHP)docData;
            IGPO attackGPO = null;
            MsgRegister.Dispatcher(new CM_GPO.GetGPO() {
                GpoId = rpcData.attackerGPOId,
                CallBack = result => {
                    attackGPO = result;
                }
            });
            MsgRegister.Dispatcher(new CM_GPO.Event_AfterDownHP {
                AttackGPO = attackGPO,
                TargetGPO = mySystem.GetGPO(),
                ActualDownHp = rpcData.DownHp,
                IsHead = rpcData.isHead,
            });
        }

        virtual protected void ChangeMaxHp() {
            mySystem.Dispatcher(new CE_GPO.Event_MaxHPChange {
                MaxHp = attributeData.maxHp,
            });
        }

        virtual protected void ChangeHP() {
            mySystem.Dispatcher(new CE_GPO.Event_HPChange {
                NowHp = attributeData.nowHp,
            });
        }

        private void OnChangeLevelCallBack(INetwork network, IProto_Doc docData) {
            var rpcData = (Proto_GPO.Rpc_ChangeLevel)docData;
            if (attributeData.Level != rpcData.level) {
                attributeData.Level = rpcData.level;
                ChangeLevel();
            }
        }

        private void OnGetLevelCallBack(ISystemMsg body, CE_GPO.Event_GetLevel entData) {
            var level = attributeData == null ? 0 : attributeData.Level;
            entData.CallBack.Invoke(level);
        }

        virtual protected void ChangeLevel() {
            mySystem.Dispatcher(new CE_GPO.Event_LevelChange {
                Level = attributeData.Level
            });
        }
    }
}