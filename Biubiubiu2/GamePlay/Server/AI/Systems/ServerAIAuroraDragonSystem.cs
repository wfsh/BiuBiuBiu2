using System;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIAuroraDragonSystem : S_AI_Base {
        private GPOM_AuroraDragon useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_AuroraDragon)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetAssetSign() + "Server");
        }

        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ServerAIAttribute>(new ServerAIAttribute.InitData { // 怪物属性
                ATK = 100,
                AttackRange = 0,
                MaxHp = useMData.Hp,
            });
            // 玩法互动组件
            AddComponent<ServerAIAuroraDragonFightRange>();
            AddComponent<ServerAIAuroraDragonMove>();
            AddComponent<ServerAuroraDragonAnim>();
            AddComponent<ServerAIAuroraDragonTimeline>();
            AddComponent<ServerAIAuroraDragonSkill>();
        }
    }
}