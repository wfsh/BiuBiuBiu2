using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSceneElementPlayAbility : ComponentBase {
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Scene.PlayAbilityForElementType>(OnPlayAbilityForElementTypeCallBack);
            mySystem.Register<SE_Scene.CanPlayAbilityForElementType>(OnCanPlayAbilityForElementTypeCallBack);
        }
        
        protected override void OnStart() {
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_Scene.PlayAbilityForElementType>(OnPlayAbilityForElementTypeCallBack);
            mySystem.Unregister<SE_Scene.CanPlayAbilityForElementType>(OnCanPlayAbilityForElementTypeCallBack);
        }

        private void OnPlayAbilityForElementTypeCallBack(ISystemMsg body, SE_Scene.PlayAbilityForElementType ent) {
            GetElement(ent.ElementType, ent.PickGPO);
        }

        /// <summary>
        /// 判断是否可以使用技能
        /// </summary>
        /// <param name="ent"></param>
        private void OnCanPlayAbilityForElementTypeCallBack(ISystemMsg body, SE_Scene.CanPlayAbilityForElementType ent) {
            var pickGpo = ent.PickGPO;
            var isTrue = true;
            switch (ent.ElementType) {
                case SceneData.ElementEnum.Ability_UpHp:
                    pickGpo.Dispatcher(new SE_GPO.Event_GetAttributeData {
                        CallBack = attr => {
                            isTrue = AbilityM_UpHPForFireRole.CheckCanUse(attr.nowHp, attr.maxHp);
                        }
                    });
                    break;
            }
            ent.CallBack(isTrue);
        }
        
        // / <summary>
        // / 通过元素类型使用技能
        // / </summary>
        private void GetElement(SceneData.ElementEnum elementType, IGPO gpo) {
            switch (elementType) {
                case SceneData.ElementEnum.Ability_UpHp:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                        FireGPO = iGPO,
                        MData = AbilityM_UpHPForFireRole.Create(),
                        InData = new AbilityIn_UpHPForFireRole {
                            In_UpHPValue = 1000
                        }
                    });
                    break;
                case SceneData.ElementEnum.AbilityEffect_SpeedRatio:
                    MsgRegister.Dispatcher(new SM_Ability.PlayAbilityEffect {
                        TargetGPO = gpo,
                        MData = AbilityM_MoveSpeedRate.CreateForID(AbilityM_MoveSpeedRate.ID_SceneUpMoveSpeedRate),
                    });
                    break;
            }
        }
    }
}