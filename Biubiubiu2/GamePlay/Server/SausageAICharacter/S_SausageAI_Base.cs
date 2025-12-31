using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class S_SausageAI_Base : S_GPO_Base {
        public int AIId = 0;
        public string NickName = "";

        public void SetCharacterData(int aiId, string name, int teamId) {
            this.NickName = name;
            this.AIId = aiId;
            var mData = GPOM_CharacterSet.GetGPOMByIdAndMatchMode(GPOM_CharacterSet.Id_Character);
            SetGPOData(0, GPOData.GPOType.RoleAI, mData, null, teamId);
        }
        
        public override NetworkData.SpawnConnType GetSpawnConnType() {
            return NetworkData.SpawnConnType.Role;
        }
    }
}