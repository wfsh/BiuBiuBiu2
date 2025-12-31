using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    [TaskName("设置自动开火")]
    [TaskCategory("AI")]
    public class ActionSetAutoFire : ActionComponent {
        [SerializeField]
        private bool targetStatus;
        [SerializeField]
        private float startFireWaitTime;
        private List<IWeapon> weaponList;

        public override void OnAwake() {
            base.OnAwake();
            iGPO.Dispatcher(new SE_GPO.Event_GetPackWeaponList() {
                CallBack = list => {
                    weaponList = list;
                }
            });
        }

        protected override void OnClear() {
        }

        public override TaskStatus OnUpdate() {
            for (int i = 0, count = weaponList.Count; i < count; i++) {
                var weapon = (S_Weapon_Base)weaponList[i];
                weapon.Dispatcher(new SE_Weapon.Event_EnabledAutoFire() {
                    IsTrue = targetStatus,
                    StartFireWaitTime = startFireWaitTime,
                });
            }

            return TaskStatus.Success;
        }
    }
}