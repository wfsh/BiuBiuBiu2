using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    /// <summary>
    /// 模拟本地开火逻辑
    /// </summary>
    public class ServerAIWeaponFire : ServerNetworkComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_OnFire1>(OnFire1CallBack);
            mySystem.Register<SE_GPO.Event_OnFire2>(OnFire2CallBack);
        }

        protected override void OnSetNetwork() {
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_OnFire1>(OnFire1CallBack);
            mySystem.Unregister<SE_GPO.Event_OnFire2>(OnFire2CallBack);
        }

        private void OnFire1CallBack(ISystemMsg body, SE_GPO.Event_OnFire1 ent) {
        }

        private void OnFire2CallBack(ISystemMsg body, SE_GPO.Event_OnFire2 ent) {
        }
    }
}