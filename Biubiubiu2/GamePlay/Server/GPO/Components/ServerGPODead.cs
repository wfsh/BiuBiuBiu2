using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;
using Random = UnityEngine.Random;
using Sofunny.BiuBiuBiu2.NetworkMessage;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPODead : ServerNetworkComponentBase {
        private bool isDead = false;
        private EntityBase entity;
        protected IGPO attackGPO = null;
        protected int attackWeaponId = 0;
        protected ModeData.MessageEnum killRoleMessage = ModeData.MessageEnum.WeaponKillRole;
        private byte deadEffectRowId = 0;

        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_HPChange>(OnHPChangeCallBack);
            mySystem.Register<SE_GPO.Event_DownHP>(OnDownLifeCallBack, 1);
            mySystem.Register<SE_GPO.Event_ReLife>(OnReLifeCallBack);
            mySystem.Register<SE_GPO.Event_GetIsDead>(OnIsDeadCallBack);
            mySystem.Register<SE_GPO.Event_OnSetDead>(OnOnSetDeadCallBack);
            mySystem.Register<SE_GPO.Event_GetLastAttackGpo>(OnGetLastAttackGpoCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            entity = (EntityBase)iEntity;
            if (isDead == true) {
                entity.SetActive(false);
            }
        }

        protected override void OnClear() {
            mySystem.Unregister<SE_GPO.Event_HPChange>(OnHPChangeCallBack);
            mySystem.Unregister<SE_GPO.Event_DownHP>(OnDownLifeCallBack);
            mySystem.Unregister<SE_GPO.Event_ReLife>(OnReLifeCallBack);
            mySystem.Unregister<SE_GPO.Event_GetIsDead>(OnIsDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_OnSetDead>(OnOnSetDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_GetLastAttackGpo>(OnGetLastAttackGpoCallBack);
        }

        private void OnOnSetDeadCallBack(ISystemMsg body, SE_GPO.Event_OnSetDead ent) {
            if (isDead == true) {
                return;
            }
            attackGPO = iGPO;
            Dead();
        }

        private void OnHPChangeCallBack(ISystemMsg body, SE_GPO.Event_HPChange ent) {
            if (ent.HP > 0 || isDead) {
                return;
            }
            Dead();
        }

        private void Dead() {
            isDead = true;
            AddModeScore();
            PlayDeadEffect();
            Rpc(new Proto_GPO.Rpc_Dead());
            mySystem.Dispatcher(new SE_GPO.Event_SetIsDead {
                DeadGpo = iGPO, IsDead = true,
            });
            MsgRegister.Dispatcher(new SM_GPO.GpoDeadEnd {
                DeadGpo = iGPO,
            });
            attackGPO?.Dispatcher(new SE_GPO.Event_KillGPO {
                AttackGPO = attackGPO,
                DeadGPO = iGPO,
            });
            OnDead();
        }

        virtual protected void OnDead() {
        }

        private void OnReLifeCallBack(ISystemMsg body, SE_GPO.Event_ReLife ent) {
            int reLifeHp = 10;
            if (ent.UpHp > 0) {
                reLifeHp = ent.UpHp;
            }
            mySystem.Dispatcher(new SE_GPO.Event_UpHP {
                UpHp = reLifeHp,
            });
            if (isDead == false) {
                return;
            }
            isDead = false;
            Rpc(new Proto_GPO.Rpc_ReLife() {
                relifePos = iGPO.GetPoint()
            });
            Dispatcher(new SE_GPO.Event_SetIsDead {
                IsDead = false, DeadGpo = iGPO,
            });
            entity?.SetActive(true);
        }

        private void OnIsDeadCallBack(ISystemMsg body, SE_GPO.Event_GetIsDead ent) {
            ent.CallBack(isDead);
        }

        private void OnDownLifeCallBack(ISystemMsg body, SE_GPO.Event_DownHP ent) {
            if (ent.AttackGPO == null) {
                return;
            }
            attackWeaponId = ent.AttackItemId;
            if (ent.AttackGPO.GetGPOType() == GPOData.GPOType.MasterAI) {
                SetAttackMasterGpo(ent.AttackGPO);
            } else {
                SetAttackGpo(ent.AttackGPO);
            }
        }

        private void SetAttackMasterGpo(IGPO attackGpo) {
            killRoleMessage = ModeData.MessageEnum.SuperWeaponKillRole;
            attackGpo.Dispatcher(new SE_AI.Event_GetMasterGPO {
                CallBack = masterGPO => {
                    attackGPO = masterGPO;
                }
            });
        }

        private void SetAttackGpo(IGPO attackGpo) {
            attackGPO = attackGpo;
            killRoleMessage = ModeData.MessageEnum.WeaponKillRole;
        }
        
        private void OnGetLastAttackGpoCallBack (ISystemMsg body, SE_GPO.Event_GetLastAttackGpo ent) {
            ent.CallBack(attackGPO);
        }

        private void AddModeScore() {
            if (attackGPO == null || iGPO.GetTeamID() == attackGPO.GetTeamID()) {
                return;
            }
            OnAddModeScore();
        }

        virtual protected void OnAddModeScore() {
        }

        private Vector3 GetEffectPosition() {
            Vector3 effectPos = Vector3.zero;
            if (iEntity is AIEntity) {
                if (entity != null) {
                    var rootBody = entity.GetBodyTran(GPOData.PartEnum.RootBody);
                    if (rootBody != null) {
                        effectPos = rootBody.position;
                    } else {
                        effectPos = iEntity.GetPoint();   
                    }
                } else {
                    effectPos = iEntity.GetPoint();
                }
            } else {
                effectPos = iEntity.GetPoint();
            }
            return effectPos;
        }

        virtual protected void PlayDeadEffect() {
            var effectPos = GetEffectPosition();
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_PlayEffect.CreateForID(GetDeadRowId()),
                InData = new AbilityIn_PlayEffect {
                    In_StartPoint = effectPos,
                    In_StartRota = Quaternion.identity,
                }
            });
        }
        
        virtual protected byte GetDeadRowId() {
            return AbilityM_PlayEffect.ID_Dead;
        }
    }
}