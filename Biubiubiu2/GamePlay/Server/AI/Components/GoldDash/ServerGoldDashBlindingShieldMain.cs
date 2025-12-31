using System;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldDashBlindingShieldMain : ComponentBase {
        private Vector3? tempPoint;
        private Quaternion tempRotation;
        
        protected override void OnAwake() {
            Register<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            Register<SE_AI_BlindingShield.Event_GetShieldPoint>(OnCheckShieldPoint);
            MsgRegister.Register<SM_Sausage.GetIsBlindingShield>(OnGetIsBlindingShield);
        }

        protected override void OnClear() {
            Unregister<SE_GPO.Event_GPOHurt>(OnGPOHurtCallBack);
            Unregister<SE_AI_BlindingShield.Event_GetShieldPoint>(OnCheckShieldPoint);
            MsgRegister.Unregister<SM_Sausage.GetIsBlindingShield>(OnGetIsBlindingShield);
        }
        
        private void OnGetIsBlindingShield(SM_Sausage.GetIsBlindingShield ent) {
            if (ent.GpoId != GpoID) {
                return;
            }
            
            ent.Callback?.Invoke(true);
        }
        
        private void OnGPOHurtCallBack(ISystemMsg msg, SE_GPO.Event_GPOHurt ent) {
            MsgRegister.Dispatcher(new SM_Sausage.CutDownBlindingShield() {
                attackGpo = ent.AttackGPO,
                gpoId = iEntity.GetGPOID(),
                downValue = ent.Hurt,
            });
        }
        
        private void OnCheckShieldPoint(ISystemMsg msg, SE_AI_BlindingShield.Event_GetShieldPoint ent) {
            tempPoint = null;
            tempRotation = Quaternion.identity;
            MsgRegister.Dispatcher(new SM_Sausage.CheckBlindingShieldPoint() {
                gpoId = iEntity.GetGPOID(),
                Callback = OnGetShieldPoint
            });
            if (tempPoint.HasValue) {
                ent.Callback?.Invoke(tempPoint.Value, tempRotation);
            }
        }

        private void OnGetShieldPoint(Vector3 point, Quaternion rotation) {
            tempPoint = point;
            tempRotation = rotation;
        }
    }
}