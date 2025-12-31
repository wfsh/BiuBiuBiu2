using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class C_GPO_Base : SystemBase {
        public GPOData.AttributeData AttributeData { get; private set; }
        public IGPOM MData { get; private set; }
        public IProto_Doc InData { get; private set; }
        public int TeamId { get; private set; }
        public int GpoId {
            get {
                if (Gpo == null) {
                    return 0;
                }
                return Gpo.GetGpoID();
            }
        }
        protected override void OnClearBase() {
            base.OnClearBase();
            AttributeData = null;
            this.MData = null;
        }

        public void SetAttributeData(GPOData.AttributeData attributeData) {
            this.AttributeData = attributeData;
            this.AttributeData.GpoId = Gpo.GetGpoID();
            this.AttributeData.Sign = this.MData.GetSign();
            this.AttributeData.SkinSign = this.MData.GetSign();
            OnSetAttributeData(this.AttributeData);
        }

        virtual protected void OnSetAttributeData(GPOData.AttributeData attributeData) {
        }
        
        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
#if UNITY_EDITOR
            var entity = (EntityBase)iEnter;
            entity.SetName(this.MData.GetName() + "_Client");
#endif
        }
        
        public void SetGPOData (int gpoId, IGPOM gpoMData, GPOData.GPOType gpoType, IProto_Doc inData, int teamId, bool isLocal) {
            this.MData = gpoMData;
            this.InData = inData;
            this.TeamId = teamId;
            if (Gpo == null) {
                AddComponent<ClientGPO>(new ClientGPO.InitData {
                    GpoId = gpoId,
                    TeamId = teamId,
                    IsLocalGPO = isLocal,
                    GpoType = gpoType,
                    GpoMData = MData,
                });
            }
        }
    }
}
