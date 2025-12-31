using Sofunny.BiuBiuBiu2.Component;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class BulletSelectGPO : MovePointRaycastHit {
        private IGPO fireGPO;
        
        protected override void OnStart() {
            base.OnStart();
            SetLayerMask(LayerData.ServerLayerMask | LayerData.DefaultLayerMask);
            fireGPO = ((S_Ability_Base)mySystem).FireGPO;
            SetIgnoreTeamId(fireGPO.GetTeamID());
        }

        protected override void OnHitGameObj(GameObject gameObj, RaycastHit hitRay) {
            var hitType = gameObj.GetComponent<HitType>();
            if (hitType != null && hitType.MyEntity != null && hitType.MyEntity.GetGPO() != null) {
                var checkHitRatio = Random.Range(0f, 100f);
                var checkHitHeadRatio = Random.Range(0f, 100f);
                var hitRatio = 100f;
                var hitHeadRatio = -1f;
                fireGPO.Dispatcher(new SE_GPO.Event_GetHitRatio {
                    HitGpoType = hitType.MyEntity.GetGPO().GetGPOType(),
                    CallBack = (ratio) => {
                        hitRatio = ratio;
                    }
                });
                if (checkHitRatio > hitRatio) {
                    return;
                }
                
                fireGPO.Dispatcher(new SE_GPO.Event_GetHitHeadAccuracy() {
                    HitGpoType = hitType.MyEntity.GetGPO().GetGPOType(),
                    CallBack = (ratio) => {
                        hitHeadRatio = ratio;
                    }
                });
                
                var hitPartType = GPOData.PartEnum.Body;
                if (checkHitHeadRatio < hitHeadRatio || (Mathf.Approximately(hitHeadRatio, -1f) && hitType.Part == GPOData.PartEnum.Head)) {
                    hitPartType = GPOData.PartEnum.Head;
                }
                mySystem.Dispatcher(new SE_Ability.HitGPO {
                    hitGPO = hitType.MyEntity.GetGPO(),
                    isHead = hitPartType == GPOData.PartEnum.Head,
                    hitPoint = hitRay.point,
                    HurtRatio = 1f,
                });
            } 
        }
    }
}