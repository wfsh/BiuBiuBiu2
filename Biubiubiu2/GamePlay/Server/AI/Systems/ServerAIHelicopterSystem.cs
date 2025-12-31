using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIHelicopterSystem : S_AI_Base {
        private GPOM_Helicopter useMData;
        protected override void OnAwake() {
            useMData = (GPOM_Helicopter)MData;
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ServerAIBehaviour>(new ServerAIBehaviour.InitData {
                AIBehaviour = useMData.AiBehavior,
                PetAIBehaviour = useMData.PetBehavior,
            });
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData {
                ATK = useMData.Atk,
                AttackRange = useMData.AttackRange,
                MaxHp = useMData.Hp,
            });
            AddComponent<ServerHelicopterMoveForCharacter>();
            AddComponent<ServerAIHelicopterAttack>();
            AddComponent<ServerAIDrive>();
        }
        
        protected override void OnStart() {
            base.OnStart();
            CreateEntity(AttributeData.Sign + "Server");
        }
    }
}