using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerCharacterToSausageMan : ServerCharacterComponent {
        private S_Character_Base characterSystem;
        private long playerId;
        private IGPO attackGPO;
        private int attackItemId;
        private bool isKnockedDown;
        private bool isBeRatBuffNoNeedFindByBoss;
        private IGPO lockGPO;

        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_Character.AddSausageRoleMoveForce>(OnAddSausageRoleMoveForce);
            mySystem.Register<SE_Character.GetKnockDownStatus>(OnGetKnockDownStatusCallBack);
            mySystem.Register<SE_AI_FightBoss.Event_HurtOutOfFightRange>(OnHurtOutOfFightRangeCallBack);
            mySystem.Register<SE_AI_FightBoss.Event_ChangeFightRangeStage>(OnHurtOutOfFightRangeCallBack);
            mySystem.Register<SE_Character.GetIsBeRatBuffNoNeedFindByBoss>(OnGetIsBeRatBuffNoNeedFindByBossCallBack);
            mySystem.Register<SE_GPO.Event_DownHP>(OnDownHPCallBack);
            MsgRegister.Register<SM_Sausage.SausageFireBullet>(OnSausageFireBulletCallBack);
            MsgRegister.Register<SM_Sausage.SausagePlayAbility>(OnSausagePlayAbilityCallBack);
            MsgRegister.Register<SM_Sausage.SausagePlayAbilityEffect>(OnSausagePlayAbilityEffectCallBack);
            MsgRegister.Register<SM_Sausage.SausageTakeMeleeDamageToMonster>(OnSausageTakeMeleeDamageToMonsterCallBack);
            MsgRegister.Register<SM_Sausage.SausageSetKnockDownStatus>(OnSausageSetKnockDownStatusCallBack);
            MsgRegister.Register<SM_Sausage.SausageSetIsBeRatBuffNoNeedFindByBoss>(OnSausageSetIsBeRatBuffNoNeedFindByBossCallBack);
            MsgRegister.Register<SM_Sausage.SausageDead>(OnSausageDeadCallBack);
            MsgRegister.Register<SM_Sausage.SausageReLife>(OnSausageReLifeCallBack);
            MsgRegister.Register<SM_Sausage.SausageSummerMasterMonster>(OnSausageSummerMasterMonster);
            MsgRegister.Register<SM_Sausage.SausageStandType>(OnSausageStandTypeCallBack);
            MsgRegister.Register<SM_Sausage.SausageTakeGunDamageToMonster>(OnSausageTakeGunDamageToMonsterCallBack);
            mySystem.Register<SE_Character.SetSausageLockGPO>(OnLookGPOIDCallBack);
            mySystem.Register<SE_Character.PlayBubble>(OnPlayBubble);
            mySystem.Register<SE_Character.PlayAudio>(OnPlayAudio);
        }

        protected override void OnStart() {
            base.OnStart();
            characterSystem = (S_Character_Base)mySystem;
            playerId = characterSystem.PlayerId;
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            attackGPO = null;
            mySystem.Unregister<SE_GPO.Event_DownHP>(OnDownHPCallBack);
            mySystem.Unregister<SE_Character.GetKnockDownStatus>(OnGetKnockDownStatusCallBack);
            mySystem.Unregister<SE_Character.AddSausageRoleMoveForce>(OnAddSausageRoleMoveForce);
            mySystem.Unregister<SE_AI_FightBoss.Event_HurtOutOfFightRange>(OnHurtOutOfFightRangeCallBack);
            mySystem.Unregister<SE_AI_FightBoss.Event_ChangeFightRangeStage>(OnHurtOutOfFightRangeCallBack);
            mySystem.Unregister<SE_Character.GetIsBeRatBuffNoNeedFindByBoss>(OnGetIsBeRatBuffNoNeedFindByBossCallBack);
            MsgRegister.Unregister<SM_Sausage.SausagePlayAbility>(OnSausagePlayAbilityCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageFireBullet>(OnSausageFireBulletCallBack);
            MsgRegister.Unregister<SM_Sausage.SausagePlayAbilityEffect>(OnSausagePlayAbilityEffectCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageTakeMeleeDamageToMonster>(OnSausageTakeMeleeDamageToMonsterCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSetKnockDownStatus>(OnSausageSetKnockDownStatusCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSetIsBeRatBuffNoNeedFindByBoss>(OnSausageSetIsBeRatBuffNoNeedFindByBossCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageSummerMasterMonster>(OnSausageSummerMasterMonster);
            MsgRegister.Unregister<SM_Sausage.SausageDead>(OnSausageDeadCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageReLife>(OnSausageReLifeCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageStandType>(OnSausageStandTypeCallBack);
            MsgRegister.Unregister<SM_Sausage.SausageTakeGunDamageToMonster>(OnSausageTakeGunDamageToMonsterCallBack);
            mySystem.Unregister<SE_Character.SetSausageLockGPO>(OnLookGPOIDCallBack);
            mySystem.Unregister<SE_Character.PlayBubble>(OnPlayBubble);
            mySystem.Unregister<SE_Character.PlayAudio>(OnPlayAudio);
            RemoveUpdate(OnUpdate);
        }
        
        private void OnPlayBubble(ISystemMsg body, SE_Character.PlayBubble ent) {
            if (playerId != ent.PlayerId) {
                return;
            }
            
            TargetRpc(networkBase, new Proto_AI.TargetRpc_PlayBubble() {
                effectSign = ent.EffectSign,
                effectPos = ent.EffectPos,
                lifeTime = ent.LifeTime,
            });
        }
        
        private void OnPlayAudio(ISystemMsg body, SE_Character.PlayAudio ent) {
            if (playerId != ent.PlayerId) {
                return;
            }

            TargetRpc(networkBase, new Proto_AI.TargetRpc_PlayAudio() {
                WwiseId = ent.WwiseId,
            });
        }
        
        private void OnHurtOutOfFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_HurtOutOfFightRange ent) {
            TargetRpc(networkBase, new Proto_AI.TargetRpc_GPOHurtOutOfFightRange());
        }
        
        private void OnHurtOutOfFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_ChangeFightRangeStage ent) {
            TargetRpc(networkBase, new Proto_AI.TargetRpc_GPOChangeFightRangeStage() {
                isInFightRange = ent.isInRange
            });
        }

        private void OnLookGPOIDCallBack(ISystemMsg body, SE_Character.SetSausageLockGPO ent) {
            if (ent.LockGpo == null || ent.LockGpo == iGPO) {
                lockGPO = null;
            } else {
                lockGPO = ent.LockGpo;
            }
        }
        
        private void OnUpdate(float deltaTime) {
            if (networkBase == null || networkBase.IsDestroy()) {
                return;
            }
            if (lockGPO != null) {
                var networkPoint = lockGPO.GetPoint();
                if (networkPoint != Vector3.zero) {
                    iEntity.SetPoint(lockGPO.GetPoint());
                }
                iEntity.SetRota(lockGPO.GetRota());
            }
        }

        private void OnGetKnockDownStatusCallBack(ISystemMsg body, SE_Character.GetKnockDownStatus ent) {
            ent.CallBack?.Invoke(isKnockedDown);
        }

        private void OnSausageSetKnockDownStatusCallBack(SM_Sausage.SausageSetKnockDownStatus ent) {
            if (playerId != 0 && ent.PlayerId == playerId) {
                isKnockedDown = ent.IsKnockedDown;
            }
        }
        
        private void OnAddSausageRoleMoveForce(ISystemMsg body, SE_Character.AddSausageRoleMoveForce ent) {
            MsgRegister.Dispatcher(new SM_Sausage.AddSausageRoleMoveForce() {
                CenterPoint = ent.CenterPoint,
                PlayerId = playerId,
            });
        }

        private void OnGetIsBeRatBuffNoNeedFindByBossCallBack(ISystemMsg body, SE_Character.GetIsBeRatBuffNoNeedFindByBoss ent) {
            ent.CallBack?.Invoke(isBeRatBuffNoNeedFindByBoss);
        }

        private void OnSausageSetIsBeRatBuffNoNeedFindByBossCallBack(SM_Sausage.SausageSetIsBeRatBuffNoNeedFindByBoss ent) {
            if (playerId != 0 && ent.PlayerId == playerId) {
                isBeRatBuffNoNeedFindByBoss = ent.IsBeRatBuffNoNeedFindByBoss;
            }
        }
        
        private void OnSausageTakeMeleeDamageToMonsterCallBack(SM_Sausage.SausageTakeMeleeDamageToMonster ent) {
            if (ent.AttackerPlayerId != playerId || playerId == 0) {
                return;
            }

            IGPO targetGpo = null;
            IGPO attackerGpo = null;
            MsgRegister.Dispatcher(new SM_GPO.GetGPO() {
                GpoId = ent.TargetGPOId,
                CallBack = gpo => { targetGpo = gpo; },
            });
            if (targetGpo == null) {
                if (ent.TargetGPOId == 0) {
                    Debug.LogError("近战伤害传入了为 0 的目标 GPO ID");
                } else {
                    Debug.LogError($"找不到近战伤害的目标 GPO: {ent.TargetGPOId}");
                }
                return;
            }
            if (targetGpo.GetGPOType() != GPOData.GPOType.AI &&
                targetGpo.GetGPOType() != GPOData.GPOType.MasterAI) {
                return;
            }

            targetGpo.Dispatcher(new SE_GPO.Event_GPOHurt() {
                Hurt = ent.Hurt,
                IsHead = false,
                AttackGPO = iGPO,
                AttackItemId = -1,
            });
        }

        private void OnSausageTakeGunDamageToMonsterCallBack(SM_Sausage.SausageTakeGunDamageToMonster ent) {
            if (ent.AttackerPlayerId != playerId || playerId == 0) {
                return;
            }

            IGPO targetGpo = null;
            MsgRegister.Dispatcher(new SM_GPO.GetGPO() {
                GpoId = ent.TargetGPOId,
                CallBack = gpo => { targetGpo = gpo; },
            });
            if (targetGpo == null || targetGpo.IsClear()) {
                return;
            }
            if (targetGpo.GetGPOType() != GPOData.GPOType.AI &&
                targetGpo.GetGPOType() != GPOData.GPOType.MasterAI) {
                return;
            }
            targetGpo.Dispatcher(new SE_GPO.Event_GPOHurt {
                Hurt = Mathf.CeilToInt(ent.Hurt),
                AttackGPO = iGPO,
                AttackItemId = ent.AttackItemId,
                IsHead = ent.IsHead,
                HeadAddPowerRatio = ent.HeadAddPowerRatio,
            });
        }

        private void OnSausageFireBulletCallBack(SM_Sausage.SausageFireBullet ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            var abilityData = ent.BulletData;
            abilityData.ConfigId = AbilityConfig.SausageBullet;
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = iGPO, AbilityMData = abilityData
            });
            MsgRegister.Dispatcher(new SM_Weapon.Event_Fire() {
                FireGpo = iGPO,
            });
        }

        private void OnSausagePlayAbilityCallBack(SM_Sausage.SausagePlayAbility ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO, MData = ent.MData, InData = ent.InData,
            });
        }

        private void OnSausagePlayAbilityEffectCallBack(SM_Sausage.SausagePlayAbilityEffect ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityEffect {
                FireGPO = iGPO,
                TargetGPO = ent.TargetGPO,
                MData = ent.MData,
                InData = ent.InData,
            });
        }
        
        private void OnSausageDeadCallBack(SM_Sausage.SausageDead ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            mySystem.Dispatcher(new SE_GPO.Event_DownHP {
                AttackGPO = attackGPO, DownHp = 9999999, AttackItemId = attackItemId, DownHpGPO = iGPO
            });
        }
        
        private void OnSausageReLifeCallBack(SM_Sausage.SausageReLife ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            isKnockedDown = false;
            mySystem.Dispatcher(new SE_GPO.Event_ReLife {
                UpHp = ent.Hp,
            });
        }
        
        private void OnDownHPCallBack(ISystemMsg body, SE_GPO.Event_DownHP ent) {
            attackGPO = ent.AttackGPO;
        }

        private void OnSausageStandTypeCallBack(SM_Sausage.SausageStandType ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            mySystem.Dispatcher(new SE_Character.StandTypeChange {
                StandType = ent.StandType,
            });
        }

        private void OnSausageSummerMasterMonster(SM_Sausage.SausageSummerMasterMonster ent) {
            if (ent.PlayerId != playerId || playerId == 0) {
                return;
            }
            MsgRegister.Dispatcher(new SM_AI.Event_AddMasterAI {
                AISign = ent.MonsterSign,
                MasterGPO = iGPO,
                StartPoint = iEntity.GetPoint(),
            });
        }
    }
}