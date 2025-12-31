using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIShootDuckAISystem : S_AI_Base {
        protected override void OnAwake() {
            base.OnAwake();
            AddComponents();
        }

        protected override void OnStart() {
            base.OnStart();
            CreateEntity(MData.GetAssetSign() + "Server");
        }
        protected override void AddComponents() {
            base.AddComponents();
            AddComponent<ServerCharacterAIMove>(); // AI 移动
            AddComponent<ServerAIItemPack>(); // 物品背包
            AddComponent<ServerGPOWeaponPack>(); // 武器背包
            AddComponent<ServerAIWeapon>(); // 武器
            AddComponent<ServerCharacterAIAnim>(); // 动画
            AddComponent<ServerCharacterAIJump>(); // AI 跳跃
            AddComponent<ServerCharacterAISlide>(); // AI 滑行
            AddComponent<ServerGPOAIMaster>(); // AI 怪物主人
            AddComponent<ServerGPOOnDrive>(); // AI 骑乘
            AddComponent<ServerGPOSkill>();  // 技能
            AddComponent<ServerGPOSkillSummonDriver>();  // 召唤骑乘类型技能
            AddComponent<ServerGPOSkillHoldOn>();       // 手持物品技能
            AddComponent<ServerGPOSkillCallAI>(); // 召唤怪物技能
            AddComponent<ServerGPOFollowPoint>();    // 怪物跟随点组件
            AddComponent<ServerGPORoomState>(); // 怪物房间状态组件
            AddComponent<ServerGPOCheckIsCanCallAI>();
        }
    }
}