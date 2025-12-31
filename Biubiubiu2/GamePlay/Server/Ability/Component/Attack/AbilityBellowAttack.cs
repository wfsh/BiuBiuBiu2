using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class AbilityBellowAttack : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float AttackDelayTime;
            public float Range;
        }
        private float attackDelayTime = 0f;
        private float countAttackDelayTime = 0f;
        private float range = 0f;
        private S_Ability_Base abSystem;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.AttackDelayTime, initData.Range);
        }

        protected override void OnStart() {
            abSystem = (S_Ability_Base)mySystem;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            abSystem = null;
        }

        private void OnUpdate(float delayTime) {
            if (countAttackDelayTime > 0f) {
                countAttackDelayTime -= Time.deltaTime;
                return;
            }
            countAttackDelayTime = this.attackDelayTime;
            if (abSystem.FireGPO.IsClear()) {
                return;
            }
            var gpoList = abSystem.GPOList;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.IsClear() || gpo.GetTeamID() == abSystem.FireGPO.GetTeamID()) {
                    continue;
                }
                var distance = Vector3.Distance(abSystem.FireGPO.GetPoint(), gpo.GetPoint());
                if (distance <= range) {
                    mySystem.Dispatcher(new SE_Ability.HitGPO {
                        hitGPO = gpo,
                        hitPoint = Vector3.zero
                    });
                }
            }
        }

        public void SetData(float attackDelayTime, float range) {
            this.attackDelayTime = attackDelayTime;
            this.range = range;
        }
    }
}