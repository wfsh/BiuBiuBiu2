using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSpawnerSpawnLockGpo : ServerNetworkComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public string SpawnerGpoSign;
            public int MaxHp;
        }
        private string spawnerGpoSign;
        private IGPO spawnerGpo;
        private int maxHp;
        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            spawnerGpoSign = initData.SpawnerGpoSign;
            maxHp = initData.MaxHp;
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
            SpawnGpo();
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            ClearSpawnGpo();
        }
        
        private void OnUpdate(float deltaTime) {
            if (spawnerGpo == null || spawnerGpo.IsClear()) {
                return;
            }
            iEntity.SetPoint(spawnerGpo.GetPoint());
        }

        private void SpawnGpo() {
            if (string.IsNullOrEmpty(spawnerGpoSign) == true) {
                return;
            }
            MsgRegister.Dispatcher(new SM_AI.Event_AddAI {
                AISign = spawnerGpoSign,
                StartPoint = iGPO.GetPoint(),
                OR_StartRota = iGPO.GetRota(),
                OR_GpoType = GPOData.GPOType.AI,
                OR_TeamId = iGPO.GetTeamID(),
                OR_CallBack = ai => {
                    SetSpawnGpo(ai.GetGPO());
                } 
            });
        }

        private void SetSpawnGpo(IGPO gpo) {
            spawnerGpo = gpo;
            spawnerGpo.Register<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
            if (maxHp > 0) {
                spawnerGpo.Dispatcher(new SE_GPO.Event_SetMaxHP {
                    MaxHp = maxHp,
                    IsSyncSetHp = true,
                });
            }
            mySystem.Dispatcher(new SE_GPOSpawner.Event_AddSpawnerGPOEnd {
                SpawnerGPO = gpo,
            });
        }

        private void ClearSpawnGpo() {
            if (spawnerGpo != null) {
                spawnerGpo.Dispatcher(new SE_AI.Event_OnRemoveAI());
                spawnerGpo.Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDeadCallBack);
                spawnerGpo = null;
            }
        }
        
        private void OnSetIsDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = GpoID,
            });
        }
    }
}