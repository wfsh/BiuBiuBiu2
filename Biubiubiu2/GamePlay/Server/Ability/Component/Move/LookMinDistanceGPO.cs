using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using UnityEngine;


namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class LookMinDistanceGPO : ComponentBase {
        private S_Ability_Base abSystem;
        private IGPO targetGPO;
        private float checkTime = 0.0f;
        private int ignoreGpoID = 0;
        private List<IGPO> gpoList = null;

        protected override void OnStart() {
            base.OnStart();
            abSystem = (S_Ability_Base)mySystem;
            gpoList = abSystem.GPOList;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            gpoList = null;
            abSystem = null;
            RemoveUpdate(OnUpdate);
        }
        
        public void SetGpoList(List<IGPO> list, int ignoreGpoID) {
            gpoList = list;
            this.ignoreGpoID = ignoreGpoID;
        }

        private void OnUpdate(float delta) {
            if (checkTime >= 0f) {
                checkTime -= Time.deltaTime;
            } else {
                checkTime = 1.0f;
                GetMinDistanceGPO();
            }
            if (targetGPO != null && targetGPO.IsClear() == false) {
                var entity = (EntityData)iEntity;
                entity.LookAT(targetGPO.GetPoint());
            }
        }
        
        public void GetMinDistanceGPO() {
            targetGPO = null;
            var checkDistance = -1f;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetGpoID() != ignoreGpoID && (
                        gpo.GetGPOType() == GPOData.GPOType.RoleAI || gpo.GetGPOType() == GPOData.GPOType.Role)) {
                    var distance = Vector3.Distance(iEntity.GetPoint(), gpo.GetPoint());
                    if (checkDistance <= 0f) {
                        targetGPO = gpo;
                        checkDistance = distance;
                    } else {
                        if (distance < checkDistance) {
                            targetGPO = gpo;
                            checkDistance = distance;
                        }
                    }
                }
            }
        }
    }
}
