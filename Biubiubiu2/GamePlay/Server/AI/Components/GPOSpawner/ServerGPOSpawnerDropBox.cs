using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSpawnerDropBox : ServerNetworkComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int DropBoxId;
        }
        private int dropBoxId = 0;
        protected override void OnAwake() {
            mySystem.Register<SE_GPOSpawner.Event_WaveEnd>(OnWaveEndCallBack);
            var initData = (InitData)initDataBase;
            dropBoxId = initData.DropBoxId;
        }
        
        protected override void OnClear() {
            mySystem.Unregister<SE_GPOSpawner.Event_WaveEnd>(OnWaveEndCallBack);
        }

        private void OnWaveEndCallBack(ISystemMsg body, SE_GPOSpawner.Event_WaveEnd ent) {
            if (ent.IsWin) {
                // MsgRegister.Dispatcher(new SM_Mode.Event_DropItem {
                //     DropBoxId = dropBoxId,
                //     DropGpo = iGPO,
                // });
            } 
        }
    }
}