using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilitySausageHurtGPO : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public int GunAutoItemId;
            public Vector3 StartPoint;
            public int BuffDamage;
        }
        private int gunAutoItemId;
        private Vector3 startPoint;
        private S_Ability_Base abSystem;
        private int buffDamage;
        private Vector3 hitPoint = Vector3.zero;
        private IGPO hitGPO = null;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (S_Ability_Base)mySystem;
            mySystem.Register<SE_Ability.HitGPO>(HitCallBack);
            var initData = (InitData)initDataBase;
            SetData(initData.GunAutoItemId, initData.StartPoint, initData.BuffDamage);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_Ability.HitGPO>(HitCallBack);
            ClearDownHpRegister(hitGPO);
        }

        public void SetData(int gunAutoItemId, Vector3 startPoint, int buffDamage) {
            this.gunAutoItemId = gunAutoItemId;
            this.startPoint = startPoint;
            this.buffDamage = buffDamage;
        }

        void HitCallBack(ISystemMsg body, SE_Ability.HitGPO entData) {
            if (entData.hitGPO.IsClear()) {
                return;
            }
            hitPoint = entData.hitPoint;
            hitGPO = entData.hitGPO;
            if (hitPoint == Vector3.zero) {
                var tran = entData.hitGPO.GetBodyTran(GPOData.PartEnum.Head);
                if (tran != null) {
                    hitPoint = tran.position;
                } else {
                    hitPoint = entData.hitGPO.GetPoint();
                    hitPoint.y += 0.5f;
                }
            }
            ClearDownHpRegister(entData.hitGPO);
            entData.hitGPO.Register<SE_GPO.Event_DownHP>(OnGPODownHpCallBack);
            Hit(entData.hitGPO, entData.isHead, hitPoint);
        }
        
        private void OnGPODownHpCallBack(ISystemMsg body, SE_GPO.Event_DownHP ent) {
            ClearDownHpRegister(ent.DownHpGPO);
            if (ent.DownHp > 0) {
                PlayEffect(ent.DownHpGPO, hitPoint);
            }
        }
        
        private void ClearDownHpRegister(IGPO hitGPO) {
            hitGPO?.Unregister<SE_GPO.Event_DownHP>(OnGPODownHpCallBack);
        }

        private void Hit(IGPO hitGPO, bool isHead, Vector3 hitPoint) {
            if (abSystem.FireGPO == null || abSystem.FireGPO.IsClear() || hitGPO.IsDead() || hitGPO.IsGodMode()) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Sausage.SausageHit() {
                AttackGPO = abSystem.FireGPO,
                TargetGPO = hitGPO,
                GunAutoItemId = gunAutoItemId,
                IsHead = isHead,
                Distance = (startPoint - hitPoint).magnitude,
                BuffDamage = buffDamage,
            });
        }

        private void PlayEffect(IGPO hitGpo, Vector3 hitPoint) {
            if (hitGpo.IsClear() || hitGpo.IsDead()) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = abSystem.FireGPO,
                MData = AbilityM_PlayBloodSplatter.CreateForID(AbilityM_PlayBloodSplatter.ID_BloodSplatter),
                InData = new AbilityIn_PlayBloodSplatter {
                    In_HitGpoId = hitGpo.GetGpoID(),
                    In_HitPoint = hitPoint,
                    In_DiffPos = hitPoint - hitGpo.GetPoint(),
                }
            });
        }
    }
}