using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class SelectMinAngleGPO : ComponentBase {
        private S_Ability_Base abSystem;
        private List<IGPO> gpoList = null;
        
        protected override void OnStart() {
            base.OnStart();
            abSystem = (S_Ability_Base)mySystem;
            gpoList = abSystem.GPOList;
        }

        protected override void OnClear() {
            base.OnClear();
            abSystem = null;
            gpoList = null;
        }

        public void SetGpoList(List<IGPO> list) {
            gpoList = list;
        }

        public IGPO GetRandGPO(IGPO mainGpo) {
            IGPO igpo = null;
            var checkAngle = 360f;
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                if (gpo.GetGpoID() == mainGpo.GetGpoID() ||
                    (gpo.GetGPOType() != GPOData.GPOType.RoleAI && gpo.GetGPOType() != GPOData.GPOType.Role)) {
                    continue;
                }
                var direction = (gpo.GetPoint() - mainGpo.GetPoint()).normalized;
                var targetRotation = Quaternion.LookRotation(direction);
                var angle = Quaternion.Angle(mainGpo.GetRota(), targetRotation);
                if (angle < checkAngle) {
                    igpo = gpo;
                    checkAngle = angle;
                }
            }
            return igpo;
        }
    }
}