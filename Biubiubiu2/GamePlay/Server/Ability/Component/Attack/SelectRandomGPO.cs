using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;


namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SelectRandomGPO : ComponentBase {
        private S_Ability_Base abSystem;
        private float arge = -1f;
        
        protected override void OnStart() {
            base.OnStart();
            abSystem = (S_Ability_Base)mySystem;
        }

        protected override void OnClear() {
            base.OnClear();
            abSystem = null;
        }
        public void SetArge(int arge) {
            this.arge = arge;
        }

        public IGPO GetRandGPO(int ignoreGpoID) {
            var gpoList = abSystem.GPOList;
            var newList = new List<IGPO>();
            var point = iEntity.GetPoint();
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (ignoreGpoID >= 0 && gpo.GetGpoID() == ignoreGpoID) {
                    continue;
                }
                if (gpo.GetGPOType() == GPOData.GPOType.RoleAI) {
                    newList.Add(gpo);
                }
                if (gpo.GetGPOType() == GPOData.GPOType.Role) {
                    if (arge > 0f) {
                        var distance = Vector3.Distance(point, gpo.GetPoint());
                        if (distance > arge) {
                            continue;;
                        }
                    }
                    newList.Add(gpo);
                }
            }
            if (newList.Count <= 0) {
                return null;
            } 
            var index =  UnityEngine.Random.Range(0, newList.Count);
            return newList[index];
        }
    }
}