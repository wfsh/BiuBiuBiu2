using System;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIDead : ServerGPODead {
        protected bool isBoss = false;
        private bool isEnabledDeadToRemove = true;
        protected S_AI_Base aiSystem;
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_AI.Event_OnRemoveAI>(OnRemoveMonsterCallBack);
            mySystem.Register<SE_AI.Event_DisabledDeadToRemove>(OnDisabledDeadToRemoveCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            aiSystem = (S_AI_Base)mySystem;
            isBoss = false;
            mySystem.Dispatcher(new SE_AI.Event_GetIsBoss {
                CallBack = (b) => {
                    isBoss = b;
                }
            });
        }
        
        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_AI.Event_OnRemoveAI>(OnRemoveMonsterCallBack);
            mySystem.Unregister<SE_AI.Event_DisabledDeadToRemove>(OnDisabledDeadToRemoveCallBack);
            aiSystem = null;
        }
        
        private void OnDisabledDeadToRemoveCallBack(ISystemMsg body, SE_AI.Event_DisabledDeadToRemove ent) {
            isEnabledDeadToRemove = false;
        }

        protected override void OnDead() {
            base.OnDead();
            if (isEnabledDeadToRemove) {
                MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                    GpoId = GpoID,
                });
            }
        }

        protected override void OnAddModeScore() {
            var lastAttackGPOID = attackGPO.GetGpoID();
            if (aiSystem.MasterGPO != null) {
                MsgRegister.Dispatcher(new SM_Mode.AddScore {
                    Channel = ModeData.GetScoreChannelEnum.KillMasterMonster,
                    GpoId = lastAttackGPOID,
                    AttackItemId = attackWeaponId
                });
                MsgRegister.Dispatcher(new SM_Mode.SuperWeaponDestroyed {
                    killGpoId = lastAttackGPOID,
                    beKillGpoId = iGPO.GetGpoID(),
                    beKillMasterGpoId = aiSystem.MasterGPO.GetGpoID(),
                    ItemId = attackWeaponId,
                });
            } else {
                if (isBoss) {
                    MsgRegister.Dispatcher(new SM_Mode.AddScore {
                        Channel = ModeData.GetScoreChannelEnum.KillBoss,
                        GpoId = lastAttackGPOID,
                        AttackItemId = attackWeaponId
                    });
                }
                if (iGPO.GetGPOType() == GPOData.GPOType.RoleAI) {
                    MsgRegister.Dispatcher(new SM_Mode.Event_ModeMessage {
                        mainGpoId = lastAttackGPOID,
                        subGpoId = iGPO.GetGpoID(),
                        ItemId = attackWeaponId,
                        MessageState = killRoleMessage,
                    });
                    MsgRegister.Dispatcher(new SM_Mode.AddScore {
                        Channel = ModeData.GetScoreChannelEnum.KillRoleAI,
                        GpoId = lastAttackGPOID,
                        AttackItemId = attackWeaponId
                    });
                } else {
                    MsgRegister.Dispatcher(new SM_Mode.AddScore {
                        Channel = ModeData.GetScoreChannelEnum.KillMonster,
                        GpoId = lastAttackGPOID,
                        AttackItemId = attackWeaponId
                    });
                }
            }
        }
        
        private void OnRemoveMonsterCallBack(ISystemMsg body, SE_AI.Event_OnRemoveAI ent) {
            MsgRegister.Dispatcher(new SM_AI.Event_RemoveAI {
                GpoId = iGPO.GetGpoID(),
            });
        }
    }
}