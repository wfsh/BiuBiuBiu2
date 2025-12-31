using System;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerGoldJokerDollBomb : ServerNetworkComponentBase {
        private enum State {
            Down = 0,
            Idle = 1,
            Follow = 2,
            Boom = 3,
            Over = 4
        }
        private SAB_GoldJokerDollBombSystem abSystem;
        private AbilityIn_GoldJokerDollBomb useInData;
        private AbilityM_GoldJokerDollBombSpawner param;
        private ServerGPO fireGPO;
        private CharacterController cc;
        private List<IGPO> gpoList;
        private IGPO lockGPO;
        private State state;
        private int animState = -1;
        private Vector3 fightRangeCenter;
        private float fightRangeRadius;
        private bool blockDamage;
        private float timer;

        protected override void OnAwake() {
            base.OnAwake();
            abSystem = (SAB_GoldJokerDollBombSystem)mySystem;
            useInData = (AbilityIn_GoldJokerDollBomb)abSystem.InData;
            param = useInData.In_Param;
            fireGPO = abSystem.FireGPO;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            var entityBase = (EntityBase)iEntity;
            cc = entityBase.GetComponent<CharacterController>();
        }

        protected override void OnStart() {
            base.OnStart();
            timer = param.M_DownTime;
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = gpos => gpoList = gpos
            });
            
            //战斗区域数据
            fireGPO.Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData {
                CallBack = SetFightRangeDataCallBack
            });
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            fireGPO = null;
            cc = null;
            gpoList = null;
            lockGPO = null;
        }

        private void OnUpdate(float deltaTime) {
            if (cc == null) return;
            if (lockGPO == null) {
                FindLockGPO();
            }
            timer -= deltaTime;
            if (timer <= 0) {
                HandleStateChange();
            }
            if (state == State.Follow && lockGPO != null) {
                FollowTarget(deltaTime);
            }
        }

        private void HandleStateChange() {
            switch (state) {
                case State.Down:
                    state = State.Idle;
                    timer = param.M_IdleTime;
                    SetAnimState(0);
                    break;
                case State.Idle:
                    state = State.Follow;
                    timer = param.M_FollowTime;
                    SetAnimState(3);
                    break;
                case State.Follow:
                    state = State.Boom;
                    timer = param.M_BombWaitTime;
                    SetAnimState(4);
                    PlayBombAudio();
                    break;
                case State.Boom:
                    PlayBoom();
                    state = State.Over;
                    break;
            }
        }

        private void FollowTarget(float deltaTime) {
            var vec = lockGPO.GetPoint() - iEntity.GetPoint();
            var sqrDis = vec.sqrMagnitude;
            if (sqrDis > param.M_FollowDistance * param.M_FollowDistance &&
                sqrDis < param.M_FollowMaxDistance * param.M_FollowMaxDistance) {
                vec.y = 0;
                iEntity.SetRota(Quaternion.Slerp(iEntity.GetRota(), Quaternion.LookRotation(vec), 10 * deltaTime));
                cc.SimpleMove(vec.normalized * param.M_FollowSpeed);
                SetAnimState(3);
            } else {
                SetAnimState(0);
            }
        }

        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool blockDamage) {
            fightRangeCenter = center;
            fightRangeRadius = radius;
            this.blockDamage = blockDamage;
        }

        private bool IsBlockDamage(IGPO gpo) {
            if (!blockDamage) return false;
            var gpoPoint = gpo.GetPoint();
            gpoPoint.y = fightRangeCenter.y;
            var distance = Vector3.Distance(gpoPoint, fightRangeCenter);
            return distance >= fightRangeRadius;
        }

        private void PlayBoom() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_ExpandBoom.CreateForID((byte)param.M_BoomEffectId),
                InData = new AbilityIn_ExpandBoom {
                    In_StartPoint = iEntity.GetPoint(),
                    In_MaxDistance = param.M_AttackRange,
                    In_CheckHight = param.M_AttackRange,
                    In_CheckBlock = true,
                    In_ATK = param.M_ATK,
                    In_LifeTime = param.M_LifeTime - param.M_WarningTime
                }
            });
        }

        private void PlayBombAudio() {
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = fireGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossjorkerClownDollBomb,
                    In_StartPoint = fireGPO.GetPoint(),
                    In_LifeTime = 1.633f,
                }
            });
        }

        private void SetAnimState(int newAnimState) {
            if (animState == newAnimState) return;
            animState = newAnimState;
            Rpc(new Proto_Ability.Rpc_GoldJokerDollBombState {
                state = (byte)newAnimState
            });
        }

        private void FindLockGPO() {
            if (gpoList == null || gpoList.Count == 0) {
                return;
            }
            var minSqrDis = float.MaxValue;
            foreach (var gpo in gpoList) {
                if (gpo.GetTeamID() == fireGPO.GetTeamID()) continue;
                if (gpo.IsDead() || gpo.IsClear()) continue;
                if (IsBlockDamage(gpo)) {
                    return;
                };
                var dis = Vector3.Distance(gpo.GetPoint(), iEntity.GetPoint());
                if (dis > param.M_MaxDistance) continue;
                if (dis < minSqrDis) {
                    lockGPO = gpo;
                    minSqrDis = dis;
                }
            }
        }
    }
}