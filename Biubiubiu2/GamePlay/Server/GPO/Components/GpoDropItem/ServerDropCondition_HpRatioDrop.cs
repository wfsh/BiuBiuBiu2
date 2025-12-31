using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerDropCondition_HpRatioDrop : ServerDropCondition_Base {
        private float maxHp = 0f;
        private float countDownHp = 0f;
        private int dropItemId = 0;
        private float dropTypeValue = 0f;

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_AfterDownHP>(OnAfterDownHpCallBack);
            mySystem.Register<SE_GPO.Event_MaxHPChange>(OnMaxHPChangeCallBack);
        }
        protected override void OnSetDropData() {
            base.OnStart();
            if (dropIdList.Count == 0) {
                Debug.LogError($"[Error] ServerGPODropItem_HpFixedRatioDrop 没有配置掉落物品 ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                return;
            }
            if (dropTypeValueList.Count == 0) {
                Debug.LogError($"[Error] ServerGPODropItem_HpFixedRatioDrop 没有配置掉落条件数值 ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                return;
            }
            dropItemId = dropIdList[0];
            dropTypeValue = dropTypeValueList[0];
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPO.Event_AfterDownHP>(OnAfterDownHpCallBack);
            mySystem.Unregister<SE_GPO.Event_MaxHPChange>(OnMaxHPChangeCallBack);
        }
        
        private void OnAfterDownHpCallBack(ISystemMsg body, SE_GPO.Event_AfterDownHP ent) {
            PlayHpRatioDrop(ent.DownHp);
        }
        
        private void OnMaxHPChangeCallBack(ISystemMsg body, SE_GPO.Event_MaxHPChange ent) {
            maxHp = ent.MaxHp;
        }

        // GPO 血量下降一定比例 XXX 后掉落物品 数值录入：0.X
        private void PlayHpRatioDrop(float downHp) {
            if (dropTypeValueList.Count == 0) {
                return;
            }
            countDownHp += downHp;
            var downHpRate = countDownHp / maxHp;
            if (downHpRate >= dropTypeValue) {
                countDownHp = 0f;
                PlayDropItem(iGPO, dropItemId);
            }
        }
    }
}