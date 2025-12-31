using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGPOSkillHoldOn : ServerNetworkComponentBase {
        private int skillId;
        private float areaRadius;
        private string holdOnSign;
        private float throwTimer;
        private SkillData.HoldOnData holdOnData;
        private IGPO targetGpo;
        private Transform lHandTran;
        private SE_Mode.PlayModeCharacterWeapon skillWeapon;
        private float atkRate = 1;

        protected override void OnAwake() {
            Register<SE_Skill.Event_AddSkillEnd>(OnAddSkillEnd);
            Register<SE_Skill.Event_UseSkill>(OnUseSkill);
            Register<SE_Skill.Event_SkillOver>(OnSkillOver);
            Register<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            Register<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            Unregister<SE_Skill.Event_AddSkillEnd>(OnAddSkillEnd);
            Unregister<SE_Skill.Event_UseSkill>(OnUseSkill);
            Unregister<SE_Skill.Event_SkillOver>(OnSkillOver);
            Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            Unregister<SE_Behaviour.Event_MaxHateTarget>(OnMaxHateTargetCallBack);
            RemoveProtoCallBack(Proto_Character.Cmd_Throw.ID, OnThrow);
            RemoveUpdate(OnUpdate);
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Character.Cmd_Throw.ID, OnThrow);
        }

        protected override void OnSetEntityObj(IEntity entityBase) {
            lHandTran = iEntity.GetBodyTran(GPOData.PartEnum.LeftHand);
            RoleAICheckThrow();
        }

        protected override void Sync(List<INetworkCharacter> networks) {
            if (iGPO.GetGPOType() == GPOData.GPOType.RoleAI && IsHoldOn()) {
                TargetRpcList(networks, new Proto_Skill.TargetRpc_HoldOnSign {
                    holdOnSign = holdOnSign
                });
            } else if (iGPO.GetGPOType() == GPOData.GPOType.Role) {
                TargetRpc(networkBase, new Proto_Skill.TargetRpc_SkillAreaRadius {
                    areaRadius = areaRadius
                });
            }
        }

        private void OnUpdate(float deltaTime) {
            if (!IsHoldOn() || iGPO.GetGPOType() != GPOData.GPOType.RoleAI) {
                return;
            }
            throwTimer -= deltaTime;
            if (throwTimer <= 0) {
                RoleAICheckThrow();
            }
        }

        private void OnAddSkillEnd(ISystemMsg body, SE_Skill.Event_AddSkillEnd ent) {
            var skillData = SkillData.GetDataForItemId(ent.SkillData.WeaponItemId);
            if (skillData.SkillType != SkillData.SkillTypeEnum.HoldOn) {
                return;
            }
            holdOnData = SkillData.GetHoldOnData(skillData.ID);
            var abilityData = (AbilityData.PlayAbility_Missile)AbilityConfig.GetAbilityModData(holdOnData.abilityConfigId);
            areaRadius = ent.SkillData.RandMissileRange + abilityData.M_AreaRadius;
            if (iGPO.GetGPOType() == GPOData.GPOType.Role) {
                TargetRpc(networkBase, new Proto_Skill.TargetRpc_SkillAreaRadius {
                    areaRadius = areaRadius
                });
            }
        }

        private void OnUseSkill(ISystemMsg body, SE_Skill.Event_UseSkill ent) {
            if (ent.SkillData.SkillType != SkillData.SkillTypeEnum.HoldOn) {
                return;
            }
            skillId = ent.SkillData.ID;
            holdOnSign = WeaponSkinData.GetSkinSfx(ent.WeaponData.WeaponSkinItemId, ent.SkillData.Sign);
            skillWeapon = ent.WeaponData;
            Dispatcher(new SE_Character.Event_SetHoldOnSign {
                HoldOnSign = holdOnSign
            });
            // 玩家通过 CharacterSync 同步
            if (iGPO.GetGPOType() == GPOData.GPOType.RoleAI) {
                Rpc(new Proto_Skill.Rpc_HoldOnSign {
                    holdOnSign = holdOnSign
                });
                throwTimer = 2f;
            }
        }

        private void OnSkillOver(ISystemMsg body, SE_Skill.Event_SkillOver ent) {
            if (ent.SkillType != SkillData.SkillTypeEnum.HoldOn) {
                return;
            }
            skillId = 0;
            holdOnSign = string.Empty;
            skillWeapon = null;
            Dispatcher(new SE_Character.Event_SetHoldOnSign {
                HoldOnSign = string.Empty
            });
            // 玩家通过 CharacterSync 同步
            if (iGPO.GetGPOType() == GPOData.GPOType.RoleAI) {
                throwTimer = 0f;
                Rpc(new Proto_Skill.Rpc_HoldOnSign {
                    holdOnSign = string.Empty
                });
            }
        }

        private void OnSetIsDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (!IsHoldOn()) {
                return;
            }
            Dispatcher(new SE_Skill.Event_RequestSkillOver {
                SkillId = skillId,
            });
        }

        private void OnMaxHateTargetCallBack(ISystemMsg body, SE_Behaviour.Event_MaxHateTarget ent) {
            targetGpo = ent.TargetGPO;
            RoleAICheckThrow();
        }

        private void OnThrow(INetwork network, IProto_Doc proto) {
            if (!IsHoldOn()) {
                return;
            }
            var cmd = (Proto_Character.Cmd_Throw)proto;
            Throw(cmd.points);
        }

        private void Throw(Vector3[] points) {
            Rpc(new Proto_Weapon.Rpc_Throw());
            var weapon = (WeaponData.GunData)WeaponData.Get(skillWeapon.WeaponItemId);
            MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                FireGPO = iGPO,
                AbilityMData = new AbilityData.PlayAbility_Missile {
                    ConfigId = holdOnData.abilityConfigId,
                    In_Points = points,
                    In_SkinItemId = skillWeapon.WeaponSkinItemId,
                    In_ATK = weapon.ATK,
                    In_BombingInterval = weapon.IntervalTime * 0.001f,
                    In_RandDuration = skillWeapon.RandMissileDuration,
                    In_AttackRange = skillWeapon.RandAttackRange + weapon.AttackRange,
                    In_FinalAreaRadius = areaRadius
                },
            });
            Dispatcher(new SE_Skill.Event_ReduceSkillPoint {
                SkillId = skillId,
                Value = SkillData.GetData(skillId).SkillActivePoint
            });
            Dispatcher(new SE_Skill.Event_RequestSkillOver {
                SkillId = skillId
            });
        }

        private void RoleAICheckThrow() {
            if (iGPO.GetGPOType() != GPOData.GPOType.RoleAI
                || !IsHoldOn()
                || targetGpo == null
                || lHandTran == null
                || throwTimer > 0f) {
                return;
            }
            var distanceSqr = (lHandTran.position - targetGpo.GetPoint()).sqrMagnitude;
            if (distanceSqr > holdOnData.maxDistance * holdOnData.maxDistance) {
                return;
            }
            var targetPoints = targetGpo.GetPoint();
            var points = CurveUtil.GetPointsForEndPoint(lHandTran.position, targetPoints, 10, 1, ~LayerData.ClientLayerMask);
            if (points.Length == 0 || !points[^1].Equals(targetPoints)) {
                return;
            }
            Throw(points);
        }

        private bool IsHoldOn() {
            return skillId != 0;
        }
    }
}