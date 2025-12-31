using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class TargetDownHpForTime : ComponentBase {
        public class InitData : SystemBase.IComponentInitData {
            public float DownHpValue = 0;
            public float DownHpSpace = -1f;
        }
        private float downHp = 0f;
        private float downHpSpace = -1f;
        private float countDownHpSpace = 0f;
        private IGPO targetGPO; // 目标GPO
        private IGPO fireGPO;

        protected override void OnAwake() {
            var initData = (InitData)initDataBase;
            SetData(initData.DownHpValue, initData.DownHpSpace);
        }

        protected override void OnStart() {
            base.OnStart(); 
            var ability = (S_Ability_Base)mySystem;
            targetGPO = ability.TargetGPO;
            fireGPO = ability.FireGPO;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            fireGPO = null;
            targetGPO = null;
        }

        public void SetData(float downHpValue, float downHpSpace) {
            this.downHp = downHpValue;
            this.downHpSpace = downHpSpace;
        }

        private void OnUpdate(float deltaTime) {
            if (targetGPO == null || targetGPO.IsClear()) {
                return;
            }
            if (downHpSpace < 0f) {
                return;
            }
            countDownHpSpace -= Time.deltaTime;
            if (countDownHpSpace <= 0f) {
                countDownHpSpace = downHpSpace;
                Debug.Log("TargetDownHpForTime OnUpdate downHp = " + downHp);
                targetGPO.Dispatcher(new SE_GPO.Event_GPOHurt {
                    Hurt = Mathf.CeilToInt(this.downHp),
                    IsHead = false,
                    AttackGPO = fireGPO,
                    AttackItemId = 0
                });
            }
        }
    }
}