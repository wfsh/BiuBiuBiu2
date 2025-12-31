using System;
using System.Collections;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CliengGPOOtherTeamMaterial : ComponentBase {
        private int localTeamId = 0;
        protected override void OnAwake() {
            base.OnAwake();
            MsgRegister.Register<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
        }
        
        private void OnAddLocalGPOCallBack(CM_GPO.AddLocalGPO ent) {
            MsgRegister.Unregister<CM_GPO.AddLocalGPO>(OnAddLocalGPOCallBack);
            localTeamId = ent.LocalGPO.GetTeamID();
            UpdateMaterialColor();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            localTeamId = -1;
            MsgRegister.Dispatcher(new CM_GPO.GetLocalGPO {
                CallBack = gpo => {
                    if (gpo != null) {
                        localTeamId = gpo.GetTeamID();
                    }
                }
            });
            UpdateMaterialColor();
        }
        
        private void UpdateMaterialColor() {
            if (localTeamId <= 0 || iGPO.GetTeamID() <= 0 || isSetEntityObj == false) {
                return;
            }
            if (localTeamId != iGPO.GetTeamID()) {
                var entityBase = iEntity as EntityBase;
                var entityMaterial = entityBase.GetComponent<EntityMaterial>();
                ChangeMaterialColor(entityMaterial.GetRenderers(), entityMaterial);
            }
        }

        // 改变所有获取到的材质颜色
        private void ChangeMaterialColor(Renderer[] renderers, EntityMaterial entityMaterial) {
            foreach (var renderer in renderers) {
                if (renderer == null) {
                    continue;
                }
                renderer.material = entityMaterial.GetOtherTeamMaterial();
            }
        }
    }
}