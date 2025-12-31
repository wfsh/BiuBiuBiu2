using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class S_GPO_Base : SystemBase {
        public GPOData.AttributeData AttributeData { get; private set; }
        public int TeamId { get; private set; }
        public IGPOM MData { get; private set; }
        public IGPOInData InData { get; private set; }
        public GPOData.GPOType GpoType { get; private set; }
        public int GpoId {
            get {
                if (Gpo == null) {
                    return 0;
                }
                return Gpo.GetGpoID();
            }
        }

        protected override void OnStartClear() {
            Dispatcher(new SE_GPO.Event_StartRemoveGPO());
        }
        
        protected override void OnClearBase() {
            base.OnClearBase();
            AttributeData = null;
        }

        
        public void SetAttributeData(GPOData.AttributeData attributeData) {
            this.AttributeData = attributeData;
            this.AttributeData.GpoId = Gpo.GetGpoID();
            this.AttributeData.SkinSign = this.MData.GetSign();
            this.AttributeData.Sign = this.MData.GetSign();
            OnSetAttributeData(this.AttributeData);
        }
        
        virtual protected void OnSetAttributeData(GPOData.AttributeData attributeData) {
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
        #if UNITY_EDITOR
            var entity = (EntityBase)iEnter;
            entity.SetName(this.MData.GetName() + "_Server");
       #endif
        }

        public void SetGPOData (int gpoId, GPOData.GPOType gpoType, IGPOM gpoMData, IGPOInData inData, int teamId) {
            this.MData = gpoMData;
            this.InData = inData;
            this.TeamId = teamId;
            this.GpoType = gpoType;
            AddComponent<ServerGPO>(new ServerGPO.InitData {
                GpoType = GpoType,
                GpoMData = MData,
                TeamId = teamId,
                SetGpoID = gpoId,
            });
        }
    }
}