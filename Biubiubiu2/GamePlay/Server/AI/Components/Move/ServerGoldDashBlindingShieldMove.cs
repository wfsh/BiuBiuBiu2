using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    // 设置盾的位置和朝向
    public class ServerGoldDashBlindingShieldMove : ComponentBase {
        private S_AI_Base aiSystem;
        private EntityBase entity;
        
        protected override void OnAwake() {
            aiSystem = (S_AI_Base)mySystem;
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
        }

        private void OnUpdate(float delta) {
            UpdatePoint();
        }

        private void UpdatePoint() {
            if (iEntity == null || iEntity.IsClear()) {
                return;
            }
            
            aiSystem.Dispatcher(new SE_AI_BlindingShield.Event_GetShieldPoint() {
                Callback = OnGetPoint
            });
        }

        private void OnGetPoint(Vector3 point, Quaternion rotation) {
            iEntity.SetPoint(point);
            iEntity.SetRota(rotation);
        }
    }
}