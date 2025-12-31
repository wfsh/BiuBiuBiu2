using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldDashAISystem : S_AI_Base {
        private GPOM_Character useMData;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = (GPOM_Character)MData;
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            switch ((AIData.AIQuality)useMData.Quality) {
                case AIData.AIQuality.Normal:
                    CreateEntityToPool("CharacterAIServer");
                    break;
                case AIData.AIQuality.Senior:
                    CreateEntityToPool("CharacterAI_MidHumanoidServer");
                    break;
                case AIData.AIQuality.Elite:
                case AIData.AIQuality.Boss:
                    CreateEntityToPool("CharacterAI_HighHumanoidServer");
                    break;
            }
        }

        protected override void OnLoadEntityEnd(IEntity iEnter) {
            base.OnLoadEntityEnd(iEnter);
            
            switch ((AIData.AIQuality)useMData.Quality) {
                case AIData.AIQuality.Senior:
                    iEntity.SetLocalScale(Vector3.one * 1.2f);
                    break;
                case AIData.AIQuality.Elite:
                case AIData.AIQuality.Boss:
                    iEntity.SetLocalScale(Vector3.one * 1.5f);
                    break;
            }
        }

        protected override void AddComponents() {  
            base.AddComponents();
            // 怪物属性组件
            AddComponent<ServerSausageCharacterAIAttribute>(new ServerAIAttribute.InitData {
                ATK = 100,
                AttackRange = 0,
                MaxHp = useMData.Hp,
            });
            if (WarReportData.IsStartSausageWarReport == false) {
                var behaviorData = MonsterBehaviorSet.GetMonsterBehaviorByMonsterSign(useMData.Sign);
                AddComponent<ServerAIBehaviour>(new ServerAIBehaviour.InitData {
                    AIBehaviour = behaviorData.BehaviorSign,
                    PetAIBehaviour = behaviorData.BehaviorSign,
                });
            }
            AddComponent<ServerGoldDashAIHateTarget>();
            AddComponent<ServerAIItemPack>(); // 物品背包
            AddComponent<ServerGPOWeaponPack>(); // 武器背包
            AddComponent<ServerCharacterAIAnim>(); // 动画
            AddComponent<ServerGoldDashCharacterAIJump>(); // AI 跳跃
            AddComponent<ServerGoldDashAIWeapon>(); // 武器
            AddComponent<ServerGoldDashCharacterAIMove>(); // AI 移动
            AddComponent<ServerGoldDashCharacterAIPointCheck>(); // AI 坐标验证
            AddComponent<ServerGoldDashAIActivate>(); // 激活状态管理（警戒/战斗）
            AddComponent<ServerGoldDashAIFireCycle>(); // 武器周期性射击组件
            AddComponent<ServerGoldDashAISight>(); // 怪物视野组件
            AddComponent<ServerGoldDashAITouch>();// 怪物触觉组件
        }
    }
}