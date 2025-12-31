using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerDropCondition_GpoSpawnerWaveEnd : ServerDropCondition_Base {
        private int dropItemId = 0;
        protected override void OnAwake() {
            mySystem.Register<SE_GPOSpawner.Event_WaveEnd>(OnWaveEndCallBack);
        }

        protected override void OnSetDropData() {
            base.OnSetDropData();
            if (dropIdList.Count == 0) {
                Debug.LogError($"[Error] ServerDropCondition_GpoSpawnerWaveEnd dropItemId is ID:{iGPO.GetGpoMID()} : {iGPO.GetMData().GetName()}");
                return;
            }
            dropItemId = dropIdList[0];
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPOSpawner.Event_WaveEnd>(OnWaveEndCallBack);
        }

        private void OnWaveEndCallBack(ISystemMsg body, SE_GPOSpawner.Event_WaveEnd ent) {
            if (ent.IsWin) {
                PlayDropItem(iGPO, dropItemId);
            } 
        }
    }
}