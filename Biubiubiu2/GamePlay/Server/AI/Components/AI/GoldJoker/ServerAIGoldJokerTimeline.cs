using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Playable.Config;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIGoldJokerTimeline : ComponentBase {
        private enum State {
            Disable,
            Enable,
            Move,
            LockTarget,
            UseSkill,
            WaitSkill,
            Destroy,
            waitHideEntity
        }

        private S_AI_Base aiBase;
        private AIGoldJokerConfig config;
        private State currState;
        private State nextState;
        private float timer;
        private float nextPlaySkillTime;
        private float animTime;
        private bool canTeleport;
        private float teleportTime;
        private float teleportTimer;
        private List<Vector3> points;
        private List<Vector3> tempPoints;
        private Vector3 fightCenter;
        private float fightRadius;
        private float sqrFightRadius;
        private List<IGPO> gpoList;
        private IGPO lockGpo;
        private bool isLooking;
        private Vector3 startPos;
        private float moveRatio;
        
        private List<GoldJokerSkillGroup> skillGroups;
        private Dictionary<int, float> skillCDDict;
        private Dictionary<int, GoldJokerSkillData> skillDataDict;
        private List<int> indexList;
        private bool syncBoolCallBackCache = false;
        private Action<bool> getIsBeRatBuffNoNeedFindByBossCallBack;
        
        protected override void OnAwake() {
            Register<SE_AI.Event_GetBossFightArea>(OnGetBossFightRadius);
            Register<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            Register<SE_AI.Event_GoldJokerLookAtTarget>(OnGoldJokerLookAtTarget);
            Register<SE_AI_FightBoss.Event_SetAllGpoInFightRange>(OnSetAllGpoInFightRangeCallBack);
            Register<SE_AI.Event_AIGoldJokerShieldStateChange>(OnMonsterGoldJokerShieldStateChange);
            MsgRegister.Register<SM_Sausage.BossFightFailed>(OnBossFightFailedCallBack);
            aiBase = (S_AI_Base)mySystem;
            getIsBeRatBuffNoNeedFindByBossCallBack = isBeRatBuffNoNeedFindByBossCallBack => {
                syncBoolCallBackCache = isBeRatBuffNoNeedFindByBossCallBack;
            };
        }
        
        protected override void OnStart() {
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = (gpos => gpoList = gpos)
            });
            Dispatcher(new SE_Entity.Event_SetDeadAutoHideEntity() {
                isDeadAutoHideEntity = false,
            });
            startPos = iEntity.GetPoint();
            AssetManager.LoadAISO(aiBase.AttributeData.Sign+"Config", LoadConfigComplete);
            fightCenter = iGPO.GetPoint();
            AddUpdate(OnUpdate);
        }
        
        
        private void LoadConfigComplete(ScriptableObject so) {
            config = (AIGoldJokerConfig)so;
            skillGroups = new List<GoldJokerSkillGroup>();
            indexList = new List<int>(skillGroups.Count);
            foreach (var group in config.SkillGroups) {
                skillGroups.Add(new GoldJokerSkillGroup() {
                    SkillTypes = new List<GoldJokerSkillType>(group.SkillTypes),
                    MaxDistance = group.MaxDistance
                });

                indexList.Add(0);
            }
                        
            for (int i = 0; i < skillGroups.Count; i++) {
                var group = skillGroups[i];
                group.SkillTypes = Shuffle(group.SkillTypes);
            }

            skillCDDict = new Dictionary<int, float>();
            skillDataDict = new Dictionary<int, GoldJokerSkillData>();
            foreach (var skill in config.Skills) {
                skillDataDict.Add((int)skill.SkillType, skill);
            }
            
            Dispatcher(new SE_AI.Event_SetShieldParam() {
                ConfigId = AbilityM_GoldJokerFollowEffect.ID_GoldJokerFollowBossEffect,
                UpHpConfigId = AbilityM_PlayEffect.ID_BossAceJokerAddHpBuff,
                UpHp = config.UpHp,
                UpHpInterval = config.UpHpInterval,
                Distance = config.ShieldDistance
            });
            moveRatio = config.teleportRandom;
            BossFightRadius();
        }

        private List<T> Shuffle<T>(List<T> original) {
            System.Random randomNum = new System.Random();
            int index;
            T temp;
            for (int i = 0; i < original.Count; i++) {
                index = randomNum.Next(original.Count);
                if (index != i) {
                    temp = original[i];
                    original[i] = original[index];
                    original[index] = temp;
                }
            }

            return original;
        }

        protected override void OnClear() {
            Unregister<SE_AI.Event_GetBossFightArea>(OnGetBossFightRadius);
            Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            Unregister<SE_AI.Event_GoldJokerLookAtTarget>(OnGoldJokerLookAtTarget);
            MsgRegister.Unregister<SM_Sausage.BossFightFailed>(OnBossFightFailedCallBack);
            Unregister<SE_AI_FightBoss.Event_SetAllGpoInFightRange>(OnSetAllGpoInFightRangeCallBack);
            Unregister<SE_AI.Event_AIGoldJokerShieldStateChange>(OnMonsterGoldJokerShieldStateChange);
            RemoveUpdate(OnUpdate);
        }
        
        private void OnSetAllGpoInFightRangeCallBack(ISystemMsg body, SE_AI_FightBoss.Event_SetAllGpoInFightRange e) {
            gpoList = e.GpoList;
        }

        private void OnMonsterGoldJokerShieldStateChange(ISystemMsg body, SE_AI.Event_AIGoldJokerShieldStateChange ent) {
            if (ent.isOpen) {
                Dispatcher(new SE_AI.Event_PlayFightFailedMusic());
                iGPO.Dispatcher(new SE_AI_FightBoss.Event_AddBattleCnt {
                    AddCnt = 0,
                    OutCnt = 1
                });
            } else {
                Dispatcher(new SE_AI.Event_PlayBossInMusic());
                iGPO.Dispatcher(new SE_AI_FightBoss.Event_AddBattleCnt {
                    AddCnt = 1,
                    OutCnt = 0
                });
            }
        }
        
        private void PlayNextSkill() {
            var sqrDis = Vector3.SqrMagnitude(lockGpo.GetPoint() - iGPO.GetPoint());
            for (int i = 0; i < skillGroups.Count; i++) {
                var group = skillGroups[i];
                var index = indexList[i];
                if (sqrDis > group.MaxDistance * group.MaxDistance) {
                    continue;
                }

                int loop = 0;
                var skillTypes = group.SkillTypes;
                while (!PlaySkill(skillTypes[index]) && loop++ < skillTypes.Count) {
                    index = (index + 1) % skillTypes.Count;
                }
                
                index = (index + 1) % skillTypes.Count;
                indexList[i] = index;
                break;
            }
        }

        private bool PlaySkill(GoldJokerSkillType type) {
            skillCDDict.TryGetValue((int)type, out var time);
            var data = skillDataDict[(int)type];
            if (Time.time >= time + data.SkillCD) {
                skillCDDict[(int)type] = Time.time;
                nextPlaySkillTime = data.NextTime;
                animTime = data.AnimTime;
                Dispatcher(new SE_AI.Event_PlayGoldJokerSkill() {
                    SkillType = type
                });
                return true;
            }

            return false;
        }
        
        private void OnBossFightFailedCallBack(SM_Sausage.BossFightFailed ent) {
            Dispatcher(new SE_AI.Event_PlayFightFailedMusic());
        }
        
        private void BossFightRadius() {
            fightRadius = 25;
            sqrFightRadius = fightRadius * fightRadius;
            points = new List<Vector3>();
            tempPoints = new List<Vector3>();
            var forward = iEntity.GetRota() * Vector3.forward;
            for (int i = 0; i < 5; i++) {
                var angle = Random.Range(0, 360);
                var radius = Random.Range(config.fightMinDis, 10);
                var dir = Quaternion.Euler(0, angle, 0) * forward;
                points.Add(startPos + dir * radius);
            }
        }

        private void OnGetBossFightRadius(ISystemMsg body, SE_AI.Event_GetBossFightArea ent) {
            ent.Callback(fightCenter, fightRadius);
        }
        
        private void OnSetIsDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                nextState = State.Destroy;
                timer = -1;
                var isAceJoker = aiBase.AttributeData.Sign == GPOM_AceJokerSet.Sign_AceJoker;
                var leaveEffect = isAceJoker ? AbilityM_PlayEffect.ID_AceJokerLeaveEffect : AbilityM_PlayEffect.ID_GoldJokerLeaveEffect;
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility() {
                    FireGPO = ent.DeadGpo,
                    MData = AbilityM_PlayEffect.CreateForID(leaveEffect),
                    InData = new AbilityIn_PlayEffect() {
                        In_StartPoint = ent.DeadGpo.GetPoint(),
                        In_StartRota = ent.DeadGpo.GetRota(),
                        In_LifeTime = 2.5f
                    }
                });
                
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = ent.DeadGpo,
                    MData = AbilityM_PlayWWiseAudio.Create(),
                    InData = new AbilityIn_PlayWWiseAudio() {
                        In_WWiseId = WwiseAudioSet.Id_GoldDashBossjorkerActOut,
                        In_StartPoint = ent.DeadGpo.GetPoint(),
                        In_LifeTime = 4f,
                    }
                });
                Dispatcher(new SE_AI.Event_PlayBossDeadMusic());
                Dispatcher(new SE_AI.Event_PlayBossAnim() {
                    Id = AnimConfig_GoldDash_BOSSAceJoker.Anim_BOSSAceJoker_Leave
                });
            }
        }

        private void OnUpdate(float delta) {
            if (Input.GetKeyDown(KeyCode.K)) {
                // Dispatcher(new SE_Monster.Event_PlayGoldJokerSkill() {
                //      SkillType = GoldJokerSkillType.CardTrick
                // });
                Dispatcher(new SE_AI.Event_PlayBossInMusic());
            }
            
            
            if (config == null ||
                ModeData.PlayGameState == ModeData.GameStateEnum.ModeOver) {
                return;
            }

            timer -= delta;
            if (timer <= 0) {
                currState = nextState;
                switch (nextState) {
                    case State.Disable:
                        timer = config.awakeTime;
                        nextState = State.Enable;
                        break;
                    case State.Enable:
                        nextState = State.Move;
                        break;
                    case State.Move:
                        if (nextPlaySkillTime > 0) {
                            timer = nextPlaySkillTime;
                            nextPlaySkillTime = 0;
                            canTeleport = true;
                            teleportTime = Random.Range(config.teleportTime.x, config.teleportTime.y);
                            teleportTimer = 0;
                            PlayMove();
                        }

                        nextState = State.LockTarget;
                        break;
                    case State.LockTarget:
                        if (FindTarget()) {
                            timer = config.lockTime;
                            nextState = State.UseSkill;
                            Dispatcher(new SE_Behaviour.Event_StopMove());
                            if (aiBase.AttributeData.Sign == GPOM_AceJokerSet.Sign_AceJoker) {
                                MsgRegister.Dispatcher(new SM_Sausage.SyncGoldJokerBossState() {
                                    GpoId = 1,
                                    State = 4,
                                    Pos = startPos,
                                });
                            }
                        } else {
                            nextPlaySkillTime = config.moveTime;
                            nextState = State.Move;
                            if (aiBase.AttributeData.Sign == GPOM_AceJokerSet.Sign_AceJoker) {
                                MsgRegister.Dispatcher(new SM_Sausage.SyncGoldJokerBossState() {
                                    GpoId = 1,
                                    State = 2,
                                    Pos = startPos,
                                });
                            }
                        }

                        break;
                    case State.UseSkill:
                        PlayNextSkill();
                        nextState = State.WaitSkill;
                        break;
                    case State.WaitSkill:
                        timer = animTime;
                        animTime = 0;
                        nextState = State.Move;
                        Dispatcher(new SE_AI.Event_PlayFightRandomMusic());
                        break;
                    case State.Destroy:
                        nextState = State.waitHideEntity;
                        var isAceJoker = aiBase.AttributeData.Sign == GPOM_AceJokerSet.Sign_AceJoker;
                        var languageSign = isAceJoker ? "AceJokerPickBossItem_CountDown" : "PickBossItem_CountDown";
                        var isShowTime = !isAceJoker;
                        
                        iGPO.Dispatcher(new SE_AI_FightBoss.Event_LeaveComplete() {
                            isShowTime = isShowTime,
                            languageSign = languageSign
                        });
                        timer = 2.8f;
                        if (!isAceJoker) {
                            MsgRegister.Dispatcher(new SM_Sausage.SyncGoldJokerBossState() {
                                GpoId = 1,
                                State = 5,
                                Pos = iGPO.GetPoint(),
                            });
                        }
                        break;
                    case State.waitHideEntity:
                        Dispatcher(new SE_Entity.Event_SetShowEntityForAnim {
                            IsShow = false,
                        });
                        RemoveUpdate(OnUpdate);
                        break;
                }
            }

            switch (currState) {
                case State.LockTarget:
                    LockTarget(delta);
                    break;
                case State.Move:
                    if (canTeleport && lockGpo != null) {
                        teleportTimer += delta;
                        if (teleportTimer >= teleportTime) {
                            canTeleport = false;
                            if (Random.Range(0, 100) <= moveRatio) {
                                moveRatio = config.teleportRandom;
                                timer = skillDataDict[(int)GoldJokerSkillType.Flash].AnimTime;
                                nextState = State.LockTarget;
                                Dispatcher(new SE_AI.Event_PlayBossTeleport() {
                                    EndPoint = FindPoint()
                                });
                            } else {
                                moveRatio += config.teleportRandom;
                            }
                        }
                    }

                    break;
                case State.WaitSkill:
                    if (isLooking) {
                        LockTarget(delta);
                    }

                    break;
            }
        }

        private void PlayMove() {
            Dispatcher(new SE_Behaviour.Event_MovePoint() {
                movePoint = FindPoint()
            });
        }

        private bool FindTarget() {
            lockGpo = null;
            if (gpoList == null || gpoList.Count == 0) {
                return false;
            }
            var minDis = sqrFightRadius;
            foreach (var gpo in gpoList) {
                gpo.Dispatcher(new SE_Character.GetIsBeRatBuffNoNeedFindByBoss() {
                    CallBack = getIsBeRatBuffNoNeedFindByBossCallBack
                });
                bool isBeRatBuffNoNeedFindByBoss = syncBoolCallBackCache;
                if (gpo.IsDead() || gpo.IsClear() || isBeRatBuffNoNeedFindByBoss) {
                    continue;
                }

                var isWeak = false;
                gpo.Dispatcher(new SE_Character.GetSausageRoleIsWeak() {
                    Callback = v => isWeak = v
                });
                
                if (isWeak) {
                    continue;
                }
                
                if (gpo.GetTeamID() == iGPO.GetTeamID()) {
                    continue;
                }

                var sqrDis = Vector3.SqrMagnitude(gpo.GetPoint() - fightCenter);
                if (sqrDis > sqrFightRadius) {
                    continue;
                }

                if (minDis > sqrDis) {
                    minDis = sqrDis;
                    lockGpo = gpo;
                }
            }

            return lockGpo != null;
        }

        private void LockTarget(float delta) {
            if (lockGpo == null) {
                return;
            }

            var vec = lockGpo.GetPoint() - iEntity.GetPoint();
            vec.y = 0;
            iEntity.SetRota(Quaternion.RotateTowards(iEntity.GetRota(), Quaternion.LookRotation(vec), delta * config.angleSpeed));
        }

        private Vector3 FindPoint() {
            tempPoints.AddRange(points);
            tempPoints.RemoveAll(point => Vector3.SqrMagnitude(point - iEntity.GetPoint()) < config.fightMinDis);
            var result = iEntity.GetPoint();
            if (tempPoints.Count > 0) {
                result = tempPoints[Random.Range(0, tempPoints.Count)];
                tempPoints.Clear();
            }
            result.y = iEntity.GetPoint().y;
            return result;
        }

        private void OnGoldJokerLookAtTarget(ISystemMsg body, SE_AI.Event_GoldJokerLookAtTarget ent) {
            isLooking = ent.IsLooking;
        }
    }
}