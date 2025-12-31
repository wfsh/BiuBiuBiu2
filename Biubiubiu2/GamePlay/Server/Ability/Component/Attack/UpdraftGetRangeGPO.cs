using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class UpdraftGetRangeGPO : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public Vector3 CheckPoint;
            public float RangeXZ;
            public float RangeY;
            public float Power;
        }
        private List<IGPOAbilityEffectData> effectList = new List<IGPOAbilityEffectData>();
        private float rangeXZ = 0f;
        private float rangeY = 0f;
        private float countCheckTime = 0.0f;
        private float power = 0.0f;
        private Vector3 checkPoint = Vector3.zero;
        private List<IGPO> gpoList;
        private S_Ability_Base abSystem;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            SetData(initData.CheckPoint, initData.RangeXZ, initData.RangeY, initData.Power);
        }

        protected override void OnStart() {
            base.OnStart();
            abSystem = (S_Ability_Base)mySystem;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            effectList.Clear();
            abSystem = null;
            base.OnClear();
        }
        
        public void SetData(Vector3 checkPoint, float rangeXZ, float rangeY, float power) {
            if (rangeXZ <= 0 || rangeY <= 0) {
                Debug.Log("GetRangeGPO 范围小于 0");
                return;
            }
            this.rangeXZ = rangeXZ;
            this.rangeY = rangeY;
            this.checkPoint = checkPoint;
            this.power = power;
        }

        private void OnUpdate(float delayTime) {
            if (countCheckTime >= 0) {
                countCheckTime -= Time.deltaTime;
            } else {
                countCheckTime = 0.5f;
                ClearEffect();
                var checkPointXZ = new Vector3(this.checkPoint.x, 0f, this.checkPoint.z);
                var gpoList = abSystem.GPOList;
                for (int i = 0; i < gpoList.Count; i++) {
                    var gpo = (IGPO)gpoList[i];
                    var targetPointXZ = new Vector3(gpo.GetPoint().x, 0f, gpo.GetPoint().z);
                    var distance = Vector3.Distance(checkPointXZ, targetPointXZ);
                    var distanceY = gpo.GetPoint().y - this.checkPoint.y;
                    if (distance <= rangeXZ && distanceY >= -0.2f &&  distanceY <= this.rangeY) {
                        gpo.Dispatcher(new SE_AbilityEffect.Event_AddEffect {
                            Effect = AbilityEffectData.Effect.UpdraftPoint,
                            Value = this.power,
                            CallBack = data => {
                                effectList.Add(data);
                            }
                        });
                    }
                }
            }
        }
        
        private void ClearEffect() {
            for (int i = 0; i < effectList.Count; i++) {
                var data = effectList[i];
                data.Remove();
            }
            effectList.Clear();
        }
    }
}
