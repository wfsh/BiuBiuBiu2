using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAISwordTigerSystem : S_AI_Base {
        private GPOM_SwordTiger useMData;
        protected override void OnAwake() {
            useMData = (GPOM_SwordTiger)MData;
            AddComponents();
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ServerAIHateTarget>();
            AddComponent<ServerAIBehaviour>(new ServerAIBehaviour.InitData {
                AIBehaviour = useMData.AiBehavior,
                PetAIBehaviour = useMData.PetBehavior,
            });
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData {
                ATK = useMData.Atk,
                MaxHp = useMData.Hp,
            });
            AddComponent<ServerAIMoveForCharacter>(new ServerAIMoveForCharacter.InitData {
                MoveSpeed = useMData.MoveSpeed,
                MoveRotaSpeed = useMData.RotaSpeed,
            });
            AddComponent<AISwordTigerAttack>();
            AddComponent<ServerSwordTigerAnim>();
            AddComponent<ServerAIDrive>();
            AddComponent<AISwordTigerDriveAttack>();
        }
        
        protected override void OnStart() {
            base.OnStart();
            CreateEntity(AttributeData.Sign + "Server");
        }
    }
}