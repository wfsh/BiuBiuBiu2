using System.Linq;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerDropCondition_HpFixedRatioDrop : ServerDropCondition_Base {
        private float maxHp = 0f;
        private float nowHp = 0f;
        private int dropItemId = 0;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_AfterDownHP>(OnAfterDownHpCallBack);
            mySystem.Register<SE_GPO.Event_MaxHPChange>(OnMaxHPChangeCallBack);
            mySystem.Register<SE_GPO.Event_ReLife>(OnReLifeCallBack);
        }

        protected override void OnSetDropData() {
            base.OnStart();
            if (dropIdList.Count == 0) {
                Debug.LogError($"[Error] ServerGPODropItem_HpFixedRatioDrop 没有配置掉落物品 ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                return;
            }
            if (dropTypeValueList.Count == 0) {
                Debug.LogError($"[Error] ServerGPODropItem_HpFixedRatioDrop 没有配置掉落血量比例 ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                return;
            }
            dropItemId = dropIdList[0];
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_AfterDownHP>(OnAfterDownHpCallBack);
            mySystem.Unregister<SE_GPO.Event_MaxHPChange>(OnMaxHPChangeCallBack);
            mySystem.Unregister<SE_GPO.Event_ReLife>(OnReLifeCallBack);
        }

        private void OnAfterDownHpCallBack(ISystemMsg body, SE_GPO.Event_AfterDownHP ent) {
            PlayHpFixedRatioDrop(ent.NowHp);
        }
        
        private void OnMaxHPChangeCallBack(ISystemMsg body, SE_GPO.Event_MaxHPChange ent) {
            maxHp = ent.MaxHp;
        }
        
        private void OnReLifeCallBack(ISystemMsg body, SE_GPO.Event_ReLife ent) {
            SetDropData();
        }

        private void PlayHpFixedRatioDrop(float nowHp) {
            if (dropTypeValueList.Count == 0) {
                return;
            }
            var nowHpRate = nowHp / maxHp;
            for (int i = 0; i < dropTypeValueList.Count; i++) {
                var checkRate = dropTypeValueList[i];
                if (nowHpRate <= checkRate) {
                    dropTypeValueList.RemoveAt(i);
                    i--;
                    PlayDropItem(iGPO, dropItemId);
                }
            }
        }
    }
}