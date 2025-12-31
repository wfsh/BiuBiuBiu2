using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAIAuroraDragonTimeline : ComponentBase {
        private enum State {
            Disable,
            Enable,
            Move,
            LockTarget,
            UseSkill,
            WaitSkill,
            Destroy
        }

        private S_AI_Base aiBase;
        private AIDragonConfig config;
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
        private List<IGPO> lockGpoList;
        private IGPO lockGpo;

        private List<AIDragonSkillGroup> skillGroups;
        private Dictionary<int, float> skillCDDict;
        private Dictionary<int, AIDragonSkillData> skillDataDict;
        private List<int> indexList;
        private float minFollowTimer;
        private string lastAbilityHasValidHit;
        private bool syncBoolCallBackCache = false;
        private Action<bool> getIsBeRatBuffNoNeedFindByBossCallBack;
        protected override void OnAwake() {
            aiBase = (S_AI_Base)mySystem;
            Register<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            Register<SE_AI_AuroraDragon.Event_GetAttackTarget>(OnGetAttackTargetCallBack);
            Register<SE_AI.Event_BossAbilityHit>(OnBossAbilityHitCallBack);
            getIsBeRatBuffNoNeedFindByBossCallBack = isBeRatBuffNoNeedFindByBossCallBack => {
                syncBoolCallBackCache = isBeRatBuffNoNeedFindByBossCallBack;
            };
        }
        
        protected override void OnStart() {
            AddUpdate(OnUpdate);
            MsgRegister.Dispatcher(new SM_GPO.GetGPOList {
                CallBack = (gpos => gpoList = gpos)
            });
            MsgRegister.Dispatcher(new SM_Sausage.HideGlodDashBossWall{});
            Dispatcher(new SE_Entity.Event_SetDeadAutoHideEntity() {
                isDeadAutoHideEntity = false,
            });
            Dispatcher(new SE_Entity.Event_SetShowEntity {
                IsShow = true,
            });

            AssetManager.LoadAISO(aiBase.AttributeData.Sign+"Config", LoadConfigComplete);
        }
        
        private void LoadConfigComplete(ScriptableObject so) {
            config = (AIDragonConfig)so;
            lockGpoList = new List<IGPO>(4);
            skillGroups = new List<AIDragonSkillGroup>();
            indexList = new List<int>(skillGroups.Count);
            foreach (var group in config.SkillGroups) {
                skillGroups.Add(new AIDragonSkillGroup() {
                    SkillTypes = new List<AIDragonSkillType>(group.SkillTypes),
                    MaxDistance = group.MaxDistance
                });

                indexList.Add(0);
            }
            
            for (int i = 0; i < skillGroups.Count; i++) {
                var group = skillGroups[i];
                group.SkillTypes = Shuffle(group.SkillTypes);
            }

            skillCDDict = new Dictionary<int, float>();
            skillDataDict = new Dictionary<int, AIDragonSkillData>();
            foreach (var skill in config.Skills) {
                skillDataDict.Add((int)skill.SkillType, skill);
            }
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
            Unregister<SE_GPO.Event_SetIsDead>(OnSetIsDead);
            Unregister<SE_AI_AuroraDragon.Event_GetAttackTarget>(OnGetAttackTargetCallBack);
            Unregister<SE_AI.Event_BossAbilityHit>(OnBossAbilityHitCallBack);
            RemoveUpdate(OnUpdate);
        }

        private void SetFightRangeDataCallBack(Vector3 center, float radius, float endTime, bool isInFight) {
            fightCenter = center;
            fightRadius = radius;
            sqrFightRadius = fightRadius * fightRadius;
        }

        private void PlayNextSkill() {
            if (lockGpo == null) {
                return;
            }

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

        private bool PlaySkill(AIDragonSkillType type) {
            skillCDDict.TryGetValue((int)type, out var time);
            var data = skillDataDict[(int)type];
            if (Time.time >= time + data.SkillCD) {
                skillCDDict[(int)type] = Time.time;
                nextPlaySkillTime = data.NextTime;
                animTime = data.AnimTime;
                Dispatcher(new SE_AI.Event_PlayDragonSkill() {
                    SkillType = type
                });
                return true;
            }

            return false;
        }

        private void OnSetIsDead(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead) {
                timer = config.DestroyTime;
                PlayOutAni();
                nextState = State.Destroy;
            }
        }

        private void OnUpdate(float delta) {
            // if (Input.GetKeyDown(KeyCode.K)) {
            //     Dispatcher(new SE_Monster.Event_PlayDragonSkill() {
            //          SkillType = MonsterDragonSkillType.FireBall
            //     });
            // }
            // return;
            if (config == null) {
                return;
            }
            
            timer -= delta;
            if (timer <= 0) {
                currState = nextState;
                switch (nextState) {
                    case State.Disable:
                        timer = config.AwakeTime;
                        PlayAwakeAni(true);
                        nextState = State.Enable;
                        break;
                    case State.Enable:
                        PlayAwakeAni(false);
                        nextState = State.Move;
                        break;
                    case State.Move:
                        if (nextPlaySkillTime > 0) {
                            timer = nextPlaySkillTime;
                            nextPlaySkillTime = 0;
                        }

                        nextState = State.LockTarget;
                        Dispatcher(new SE_AI_AuroraDragon.Event_PlaySkillEnd());
                        break;
                    case State.LockTarget:
                        if (FindTarget()) {
                            timer = config.LockTime;
                            nextState = State.UseSkill;
                            Dispatcher(new SE_Behaviour.Event_StopMove());
                        }

                        break;
                    case State.UseSkill:
                        PlayNextSkill();
                        nextState = State.WaitSkill;
                        break;
                    case State.WaitSkill:
                        timer = animTime;
                        nextState = State.Move;
                        minFollowTimer = 0;
                        break;
                    case State.Destroy:
                        Dispatcher(new SE_Entity.Event_SetDeadAutoHideEntity {
                            isDeadAutoHideEntity = false,
                        });
                        Dispatcher(new SE_Entity.Event_SetShowEntity {
                            IsShow = false,
                        });
                        iGPO.Dispatcher(new SE_AI_FightBoss.Event_LeaveComplete() {
                            isShowTime = true,
                            languageSign = "PickBossItem_CountDown"
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
                    Follow(delta);
                    break;
            }
        }

        private void PlayAwakeAni(bool isTrue) {
            Dispatcher(new SE_AI_AuroraDragon.Event_AuroraDragonAppearAni {
                IsTrue = isTrue
            });
            if (isTrue) {
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_PlayWWiseAudio.Create(),
                    InData = new AbilityIn_PlayWWiseAudio() {
                        In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonActIn,
                        In_StartPoint = iGPO.GetPoint(),
                        In_LifeTime = 5f
                    }
                });
                var rowId = aiBase.AttributeData.Sign == GPOM_AuroraDragonSet.Sign_AuroraDragon
                    ? AbilityM_PlayEffect.ID_AuroraDragonAppear
                    : AbilityM_PlayEffect.ID_AncientDragonAppear;
                MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                    FireGPO = iGPO,
                    MData = AbilityM_PlayEffect.CreateForID(rowId),
                    InData = new AbilityIn_PlayEffect {
                        In_StartPoint = iGPO.GetPoint(),
                        In_StartRota = iGPO.GetRota(),
                        In_LifeTime = 5f,
                    }
                });
            }
        }
        
        private void PlayOutAni() {
            Dispatcher(new SE_AI_AuroraDragon.Event_AuroraDragonOutAni {});
            MsgRegister.Dispatcher(new SM_Ability.PlayAbility {
                FireGPO = iGPO,
                MData = AbilityM_PlayWWiseAudio.Create(),
                InData = new AbilityIn_PlayWWiseAudio() {
                    In_WWiseId = WwiseAudioSet.Id_GoldDashBossAdragonActOut,
                    In_StartPoint = iGPO.GetPoint(),
                    In_LifeTime = 10f
                }
            });
        }
        
        private bool FindTarget() {
            lockGpoList.RemoveAll(IsRemoveLockRole);

            if (lockGpoList.Count > 0) {
                var index = lockGpoList.IndexOf(lockGpo);
                index = (index + 1) % lockGpoList.Count;
                lockGpo = lockGpoList[index];
                return true;
            }

            if (fightRadius <= 0) {
                Dispatcher(new SE_AI_FightBoss.Event_GetFightRangeData() {
                    CallBack = SetFightRangeDataCallBack
                });
            }

            var minDis = sqrFightRadius;
            
            lockGpo = null;
            lockGpoList.Clear();
            foreach (var gpo in gpoList) {
                gpo.Dispatcher(new SE_Character.GetIsBeRatBuffNoNeedFindByBoss() {
                    CallBack = getIsBeRatBuffNoNeedFindByBossCallBack
                });
                bool isBeRatBuffNoNeedFindByBoss = syncBoolCallBackCache;
                if (gpo.IsDead() || gpo.IsClear() || isBeRatBuffNoNeedFindByBoss) {
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

                lockGpoList.Add(gpo);
            }

            return lockGpo != null;
        }

        private bool IsRemoveLockRole(IGPO gpo) {
            var isWeak = false;
            gpo.Dispatcher(new SE_Character.GetSausageRoleIsWeak() {
                Callback = v => isWeak = v
            });
            gpo.Dispatcher(new SE_Character.GetIsBeRatBuffNoNeedFindByBoss() {
                CallBack = getIsBeRatBuffNoNeedFindByBossCallBack
            });
            bool isBeRatBuffNoNeedFindByBoss = syncBoolCallBackCache;
            if (gpo.IsDead() ||
                gpo.IsClear() ||
                isBeRatBuffNoNeedFindByBoss ||
                Vector3.SqrMagnitude(gpo.GetPoint() - fightCenter) > sqrFightRadius) {
                return true;
            }

            return isWeak;
        }

        private void LockTarget(float delta) {
            if (lockGpo == null) {
                return;
            }

            var vec = lockGpo.GetPoint() - iEntity.GetPoint();
            vec.y = 0;
            var targetForward = Vector3.RotateTowards(iEntity.GetForward(), vec, delta * config.AngleSpeed, 0.1f);
            iEntity.SetRota(Quaternion.LookRotation(targetForward));
        }

        private void Follow(float delta) {
            if (lockGpo == null) {
                return;
            }

            if (Vector3.SqrMagnitude(iGPO.GetPoint() - lockGpo.GetPoint()) < config.MinFollowDistance * config.MinFollowDistance) {
                Dispatcher(new SE_Behaviour.Event_StopMove());
                minFollowTimer = config.MinFollowTime;
                return;
            }

            if (minFollowTimer > 0) {
                minFollowTimer -= delta;
                return;
            }

            Dispatcher(new SE_Behaviour.Event_MovePoint() {
                movePoint = lockGpo.GetPoint(),
                MoveType = AIData.MoveType.Run
            });
        }
        
        private void OnGetAttackTargetCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_GetAttackTarget ent) {
            ent.CallBack(lockGpo);
        }
        
        private void OnBossAbilityHitCallBack(ISystemMsg body, SE_AI.Event_BossAbilityHit ent) {
            if (ent.sourceAbilityType != lastAbilityHasValidHit) {
                MsgRegister.Dispatcher(new SM_Sausage.MonsterAbilityHit() {
                    FireGPO = iGPO,
                    SourceAbilityType = ent.sourceAbilityType,
                });
                lastAbilityHasValidHit = ent.sourceAbilityType;
            }
        }
    }
}
