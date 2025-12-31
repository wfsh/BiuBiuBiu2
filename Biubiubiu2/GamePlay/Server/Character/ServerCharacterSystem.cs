using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterSystem : S_Character_Base {
        public GPOM_Character useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_Character)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntityObj($"Character/Server/ServerCharacter", StageData.GameWorldLayerType.Character);
        }

        private void AddComponents() {
            AddComponent<ServerCharacterDead>();
            AddComponent<ServerGPOFollowPoint>();
            AddComponent<ServerCharacterAnimator>();
            AddComponent<ServerCharacterHurt>();
            AddComponent<ServerCharacterAttribute>();
            AddComponent<ServerCharacterWeapon>();
            AddComponent<ServerCharacterAerocraft>();
            AddComponent<ServerCharacterMove>();
            AddComponent<ServerCharacterDrive>();
            AddComponent<ServerCharacterMode>();
            AddComponent<ServerCharacterSlide>();
            AddComponent<ServerCharacterItemPack>();
            AddComponent<ServerCharacterWeaponPack>();
            AddComponent<ServerCharacterSkill>();       // 超级武器
            AddComponent<ServerGPOSkillSummonDriver>(); // 超级武器-召唤
            AddComponent<ServerGPOSkillHoldOn>();       // 超级武器-手持物品
            AddComponent<ServerGPOSkillCallAI>();       // 超级武器-召唤怪物
            AddComponent<ServerGPOAIMaster>(); // 获得宠物
            AddComponent<ServerGPOIsGodMode>();
            AddComponent<ServerGPOShowEntity>();
            AddComponent<ServerGPODropItem>();
            AddComponent<ServerCharacterDropItemHub>();
            AddComponent<ServerCharacterLostPacket>();
            AddComponent<ServerGPOAbilityEffect>();
            AddComponent<ServerGPORoomState>();
            AddComponent<ServerGPOCheckIsCanCallAI>();
            AddComponent<ServerCharacterTask>();
        }
        
        override protected ITargetRpc RpcCharacter() {
            var heroInfo = (GPOData.HearAttributeData)this.AttributeData;
            return new Proto_Login.TargetRpc_CharacterInfo() {
                playerId = PlayerId,
                speed = heroInfo.Speed,
                jumpHeight = heroInfo.JumpHeight,
                airJumpHeight = heroInfo.AirJumpHeight,
            };
        }
    }
}