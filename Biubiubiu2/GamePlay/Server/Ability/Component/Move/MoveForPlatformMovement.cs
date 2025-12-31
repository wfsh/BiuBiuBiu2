using System;
using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.ServerMessage;

using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class MoveForPlatformMovement : ComponentBase {
        protected override void OnAwake() {
            this.mySystem.Register<SE_Ability.PlatformMovement>(OnPlatformMovementCallBack);
        }

        protected override void OnClear() {
            this.mySystem.Unregister<SE_Ability.PlatformMovement>(OnPlatformMovementCallBack);
            base.OnClear();
        }

        private void OnPlatformMovementCallBack(ISystemMsg body, SE_Ability.PlatformMovement entData) {
            var point = iEntity.GetPoint();
            point += entData.move;
            iEntity.SetPoint(point);
        }
    }
}