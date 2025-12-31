using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIToSausageWarReport : ComponentBase {
        private S_AI_Base aiBase;
        private float deltaPointTime = 0f;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_AfterDownHP>(AfterDownHPCallBack);
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Register<SE_GPO.Event_PlayAnimIdEnd>(OnPlayAnimIdEndCallBack);
            MsgRegister.Register<SM_Sausage.LoadWarReportMoveToPoint>(OnWarReportMoveToPointCallBack);
            MsgRegister.Register<SM_Sausage.LoadWarReportMoveToRota>(OnLoadWarReportMoveToRotaCallBack);
            MsgRegister.Register<SM_Sausage.LoadWarReportSetNowHp>(OnWarReportSetNowHpCallBack);
            MsgRegister.Register<SM_Sausage.LoadWarReportSetIsDead>(OnLoadWarReportSetIsDeadCallBack);
            MsgRegister.Register<SM_Sausage.LoadWarReportPlayAnimId>(OnLoadWarReportPlayAnimIdCallBack);
            aiBase = (S_AI_Base)mySystem;
        }

        protected override void OnStart() {
            base.OnStart();

            MsgRegister.Dispatcher(new SM_Sausage.SaveWarReportCreatePVEAI {
                GpoId = GpoID,
                GpoType = (int)iGPO.GetGPOType(),
                TeamId = aiBase.TeamId,
                MonsterSign = aiBase.AttributeData.Sign,
                Point = iGPO.GetPoint(),
                Level = aiBase.AttributeData.Level
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            SetIsDead();
            RemoveUpdate(OnUpdate);
            MsgRegister.Unregister<SM_Sausage.LoadWarReportMoveToPoint>(OnWarReportMoveToPointCallBack);
            MsgRegister.Unregister<SM_Sausage.LoadWarReportSetNowHp>(OnWarReportSetNowHpCallBack);
            MsgRegister.Unregister<SM_Sausage.LoadWarReportMoveToRota>(OnLoadWarReportMoveToRotaCallBack);
            MsgRegister.Unregister<SM_Sausage.LoadWarReportSetIsDead>(OnLoadWarReportSetIsDeadCallBack);
            MsgRegister.Unregister<SM_Sausage.LoadWarReportPlayAnimId>(OnLoadWarReportPlayAnimIdCallBack);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_AfterDownHP>(AfterDownHPCallBack);
            mySystem.Unregister<SE_GPO.Event_PlayAnimIdEnd>(OnPlayAnimIdEndCallBack);
        }
        
        private void OnUpdate(float deltaTime) {
            if (deltaPointTime > 0f) {
                deltaPointTime -= Time.deltaTime;
                return;
            }
            deltaPointTime = 0.1f;
            MsgRegister.Dispatcher(new SM_Sausage.SaveWarReportSetPointRota {
                GpoId = GpoID,
                Point = iGPO.GetPoint(),
                Rota = iGPO.GetRota(),
            });
        }

        private void AfterDownHPCallBack(ISystemMsg body, SE_GPO.Event_AfterDownHP ent) {
            MsgRegister.Dispatcher(new SM_Sausage.SaveWarReportSetHpChange {
                GpoId = GpoID,
                NowHp = ent.NowHp,
            });
        }

        private void OnPlayAnimIdEndCallBack(ISystemMsg body, SE_GPO.Event_PlayAnimIdEnd ent) {
            MsgRegister.Dispatcher(new SM_Sausage.SaveWarReportPlayAnimId {
                GpoId = GpoID,
                AnimId = ent.AnimId,
            });
        }
        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            SetIsDead();
        }

        private void SetIsDead() {
            MsgRegister.Dispatcher(new SM_Sausage.SaveWarReportSetIsDead {
                GpoId = GpoID,
            });
        }

        private void OnWarReportMoveToPointCallBack(SM_Sausage.LoadWarReportMoveToPoint ent) {
            if (ent.GpoId != GpoID) return;
            iEntity.SetPoint(ent.Point);
        }

        private void OnLoadWarReportMoveToRotaCallBack(SM_Sausage.LoadWarReportMoveToRota ent) {
            if (ent.GpoId != GpoID) return;
            iEntity.SetRota(ent.Rota);
        }
        
        private void OnWarReportSetNowHpCallBack(SM_Sausage.LoadWarReportSetNowHp ent) {
            if (ent.GpoId != GpoID) return;
            mySystem.Dispatcher(new SE_GPO.Event_SetHP {
                Hp = ent.NowHp,
            });
        }

        private void OnLoadWarReportSetIsDeadCallBack(SM_Sausage.LoadWarReportSetIsDead ent) {
            if (ent.GpoId != GpoID) return;
            mySystem.Dispatcher(new SE_GPO.Event_SetHP {
                Hp = 0,
            });
            var quality = (AIData.AIQuality)iGPO.GetMData().GetQuality();
            if (quality == AIData.AIQuality.Boss) {
                mySystem.Dispatcher(new SE_Entity.Event_SetShowEntity {
                    IsShow = false,
                });
            }
        }
            
        private void OnLoadWarReportPlayAnimIdCallBack(SM_Sausage.LoadWarReportPlayAnimId ent) {
            if (ent.GpoId != GpoID) return;
            mySystem.Dispatcher(new SE_GPO.Event_PlayWarReportAnimIdStart {
                AnimId = ent.AnimId,
            });
        }
    }
}