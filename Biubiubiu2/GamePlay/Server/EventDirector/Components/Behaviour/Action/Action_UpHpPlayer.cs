using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Action_UpHpPlayer : EventDirectorAction {
        public struct InitData : IEventDirectorData {
            public int UpHpValue;
            public void Serialize (string value) {
                var arr = value.Split('&');
                UpHpValue = int.Parse(arr[0]);
            }
        }
        private InitData useMData;
        private IGPO monsterGPO;
        protected override void OnAwake() {
            base.OnAwake();
            useMData = SerializeData<InitData>();
        }

        protected override void OnEnterAction() {
            for (int i = 0; i < gpoList.Count; i++) {
                var gpo = gpoList[i];
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = gpo,
                    MData = AbilityM_UpHPForFireRole.Create(),
                    InData = new AbilityIn_UpHPForFireRole {
                        In_UpHPValue = useMData.UpHpValue
                    }
                });
            }
            QuitAction();
        }
    }
}