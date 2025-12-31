using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIAuroraDragonSkill : ComponentBase {
        private S_AI_Base aiBase;
        private string sceneTypeSign = "";

        protected override void OnAwake() {
            aiBase = (S_AI_Base)mySystem;
            Register<SE_AI.Event_PlayDragonSkill>(OnPlayDragonSkill);
        }

        protected override void OnStart() {
            base.OnStart();
            GetCurSceneData(SceneData.Get(ModeData.SceneId));
        }

        protected override void OnClear() {
            Unregister<SE_AI.Event_PlayDragonSkill>(OnPlayDragonSkill);
            aiBase = null;
            sceneTypeSign = "";
        }
        private void GetCurSceneData(SceneData.Data sceneData) {
            sceneTypeSign = ExtractPart(sceneData.ElementConfig);
        }  
        private void OnPlayDragonSkill(ISystemMsg body, SE_AI.Event_PlayDragonSkill ent) {
            var bossType = (byte)(iGPO.GetGpoMID() == GPOM_AuroraDragonSet.Id_AuroraDragon ? 1 : 2);
            switch (ent.SkillType) {
                case AIDragonSkillType.Tread: // 踩踏连击
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_AuroraDragonTreadSpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case AIDragonSkillType.FireBall: // 喷射火球
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_AuroraDragonFireBallSpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case AIDragonSkillType.Fire: // 旋转喷火（激光）
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_AuroraDragonFire.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case AIDragonSkillType.DragonCar: // 飞行冲撞
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_AuroraDragonDragonCar.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case AIDragonSkillType.Tornado: // 地火
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_DragonFireTornado.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case AIDragonSkillType.DelayBlast: // 延迟爆炸（球体）
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_DragonDelayBlastSpawner.CreateForKey(bossType, sceneTypeSign),
                        InData = new AbilityIn_DragonDelayBlastSpawner() {
                            In_MonsterSign = iGPO.GetSign(),
                        }
                    });
                    break;
                case AIDragonSkillType.AOE: // 全屏AOE（特有表演）
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_DragonFullScreenAOESpawner.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
                case AIDragonSkillType.FlyFlame: // 飞行喷火
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_AncientDragonFlyFlame.CreateForKey(bossType, sceneTypeSign),
                    });
                    break;
            }
        }
        
        private string ExtractPart(string input) {
            var parts = input.Split('_');
            if (parts.Length >= 2) {
                // 取最后两个部分组合
                return $"{parts[parts.Length - 2]}_{parts[parts.Length - 1]}";
            }
            return input; // 格式不符合时返回原始字符串
        }  
    }
}
