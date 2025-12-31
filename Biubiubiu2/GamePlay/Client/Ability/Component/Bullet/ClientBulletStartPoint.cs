using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Util;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientBulletStartPoint : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public Vector3 TargetPoint;
            public float Speed;
            public Action CallBack;
            public Vector3 StartPoint;
        }
        private int fireGpoId;
        private bool isSetStartPoint;
        private Vector3 startPoint;
        private Vector3 targetPoint;
        private float speed;
        private Action onEndCallBack;

        protected override void OnAwake() {
            base.OnAwake();
            var abilitySystem = (C_Ability_Base)mySystem;
            fireGpoId = abilitySystem.FireGpoId;
            var initData = (InitData)initDataBase;
            SetData(initData.TargetPoint, initData.Speed, initData.CallBack);
            if (initData.StartPoint != Vector3.zero) {
                SetStartPoint(initData.StartPoint);
            }
        }
        
        protected override void OnClear() {
            base.OnClear();
            this.onEndCallBack = null;
        }

        public void SetData(Vector3 targetPoint, float speed, Action onEndCallBack) {
            this.targetPoint = targetPoint;
            this.speed = speed;
            this.onEndCallBack = onEndCallBack;
        }

        public void SetStartPoint(Vector3 startPoint) {
            isSetStartPoint = true;
            this.startPoint = startPoint;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            if (!isSetStartPoint) {
                MsgRegister.Dispatcher(new CM_GPO.GetGPO {
                    GpoId = fireGpoId, CallBack = SetFireGPO
                });
            } else {
                SetFireBox(startPoint);
            }
        }
        private void SetFireGPO(IGPO gpo) {
            if (gpo != null) {
                var isSetFireBox = false;
                gpo.Dispatcher(new CE_Weapon.GetFireBox {
                    CallBack = fireBox => {
                        SetFireBox(fireBox);
                        isSetFireBox = true;
                    }
                });
                gpo.Dispatcher(new CE_Weapon.Event_BulletSetFireGPO());
                if (isSetFireBox) {
                    return;
                }
                var fireTranList = gpo.GetBodyTranList(GPOData.PartEnum.AttactPoint1);
                if (fireTranList.Count > 0) {
                    var fireIndex = Random.Range(0, fireTranList.Count);
                    var fireTran = fireTranList[fireIndex];
                    SetFireBox(fireTran);
                }
            } 
            if (iEntity.GetPoint() == Vector3.zero) {
                End();
            }
        }

        private void SetFireBox(Transform fireBox) {
            if (fireBox == null) {
                return;
            }
            SetFireBox(fireBox.position);
        }

        public void SetFireBox(Vector3 position) {
            iEntity.SetPoint(position);
            iEntity.SetRota(GetStartRotation(position, targetPoint));
            mySystem.Dispatcher(new CE_Ability.SetEntityStartPoint {
                StartPoint = position
            });
            var time = Mathf.Max(1f, Vector3.Distance(iEntity.GetPoint(), targetPoint) / speed);
            Dispatcher(new CE_Ability.SetLifeTime {
                LifeTime = time,
                OnLifeTimeEnd = End
            });
        }

        private void End() {
            this.onEndCallBack?.Invoke();
        }

        private Quaternion GetStartRotation(Vector3 startPoint, Vector3 targetPoint) {
            var dir = (targetPoint - startPoint).normalized;
            if (dir == Vector3.zero) {
                return Quaternion.identity;
            }
            var rot = Quaternion.LookRotation(dir);
            return rot;
        }
    }
}