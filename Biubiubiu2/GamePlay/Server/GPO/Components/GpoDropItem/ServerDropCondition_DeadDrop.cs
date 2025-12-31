using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerDropCondition_DeadDrop : ServerDropCondition_Base {
        private int dropItemId = 0;
        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
        }

        protected override void OnSetDropData() {
            base.OnSetDropData();
            if (dropCondition == GpoDropCondition.DeadDrop) {
                if (dropIdList.Count == 0) {
                    Debug.LogError($"[Error] ServerDropCondition_DeadDrop 没有配置掉落物品 ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                    return;
                }
                dropItemId = dropIdList[0];
            }
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
        }
        
        private void OnDeadCallBack( ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            PlayDeadDropType();
        }

        private void PlayDeadDropType() {
            if (dropCondition == GpoDropCondition.DeadDropNoSetItem) {
                PlayDropItem(iGPO, 0);
            } else {
                if (dropIdList.Count > 0) {
                    PlayDropItem(iGPO, dropItemId);
                } else {
                    Debug.LogError("ServerGPODropItem_DeadDrop PlayDeadDropType dropIdList is empty");
                }
            }
        }
    }
}