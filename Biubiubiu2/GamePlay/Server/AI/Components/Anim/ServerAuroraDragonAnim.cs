using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using Sofunny.BiuBiuBiu2.Playable.Config;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAuroraDragonAnim : ServerNetworkComponentBase {
        private const float runThresholdAngle = 20f; // 转向多少角度内开始跑步
        private AIData.MoveType moveType = AIData.MoveType.Walk;
        private Vector3 moveDir = Vector3.zero;
        private S_AI_Base aiBase;
        private EntityBase entity = null;
        private int base_layer = -1;
        private string monsterSign = "";
        private int playAnim = 0;
        private int attackAnimId = 0;
        private bool isMovePoint = false;
        private EntityAnimConfig config;
        private SausagePlayable playable;
        
        protected override void OnAwake() {
            mySystem.Register<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            mySystem.Register<SE_GPO.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Register<SE_GPO.Event_MovePointEnd>(OnMovePoineEndCallBack);
            mySystem.Register<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            
            mySystem.Register<SE_AI_AuroraDragon.Event_FireBallStartAnim>(OnFireBallStartCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FireBallAnim>(OnFireBallCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FireBallEndAnim>(OnFireBallEndCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_TreadStartAnim>(OnTreadStartCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_TreadAttackAnim>(OnTreadAttackCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_TreadFireAnim>(OnTreadFireCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FireStartAnim>(OnTreadFireStartCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FireForwardAnim>(OnTreadFireForwardCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FullScreenAOE>(OnFullScreenAOECallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_TornadoAnim>(OnPlayTornadoAnimCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_DragonCarChargeAnim>(OnDragonCarStartCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_DragonCarAttackAnim>(OnDragonCarAttackCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_DragonCarDownAnim>(OnDragonCarAttackCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_DragonCarEndAnim>(OnDragonCarEndCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_AuroraDragonAppearAni>(OnAuroraDragonAppearCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_AuroraDragonOutAni>(OnAuroraDragonOutCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_PlaySkillEnd>(OnSkillEndCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_DelayBlastStartAnim>(OnEvent_DelayBlastStartCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_DelayBlastEndAnim>(OnEvent_DelayBlastEndCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FlyFlameStartAnim>(OnFlyFlameStartCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FlyFlameAnim>(OnFlyFlameCallBack);
            mySystem.Register<SE_AI_AuroraDragon.Event_FlyFlameEndAnim>(OnFlyFlameEndCallBack);
        }
        
        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            aiBase = (S_AI_Base)mySystem;
            monsterSign = aiBase.AttributeData.Sign;
            config = AIAnimConfig.Get(AIAnimConfig.AuroraDragon);
            InitPlayableGraph();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_GPO.Event_SetIsDead>(OnDeadCallBack);
            mySystem.Unregister<SE_GPO.Event_MoveDir>(SetMoveDirCallBack);
            mySystem.Unregister<SE_GPO.Event_MovePointEnd>(OnMovePoineEndCallBack);
            mySystem.Unregister<SE_AI.Event_IsMovePoint>(OnIsMovePointCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FireBallStartAnim>(OnFireBallStartCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FireBallAnim>(OnFireBallCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FireBallEndAnim>(OnFireBallEndCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_TreadStartAnim>(OnTreadStartCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_TreadAttackAnim>(OnTreadAttackCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_TreadFireAnim>(OnTreadFireCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FireStartAnim>(OnTreadFireStartCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FireForwardAnim>(OnTreadFireForwardCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FullScreenAOE>(OnFullScreenAOECallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_TornadoAnim>(OnPlayTornadoAnimCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_DragonCarChargeAnim>(OnDragonCarStartCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_DragonCarAttackAnim>(OnDragonCarAttackCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_DragonCarDownAnim>(OnDragonCarAttackCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_DragonCarEndAnim>(OnDragonCarEndCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_AuroraDragonAppearAni>(OnAuroraDragonAppearCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_AuroraDragonOutAni>(OnAuroraDragonOutCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_PlaySkillEnd>(OnSkillEndCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_DelayBlastStartAnim>(OnEvent_DelayBlastStartCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_DelayBlastEndAnim>(OnEvent_DelayBlastEndCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FlyFlameStartAnim>(OnFlyFlameStartCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FlyFlameAnim>(OnFlyFlameCallBack);
            mySystem.Unregister<SE_AI_AuroraDragon.Event_FlyFlameEndAnim>(OnFlyFlameEndCallBack);
            playable.Dispose();
            playable = null;
        }

        private void OnUpdate(float deltaTime) {
            playable?.OnUpdate(deltaTime);
        }

        protected override ITargetRpc SyncData() {
            return new Proto_AI.TargetRpc_Anim {
                animId = (ushort)playAnim
            };
        }

        private void InitPlayableGraph() {
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            playable = new SausagePlayable();
            playable.Init(entity.transform, animator, config, $"Server_{monsterSign}");
            playable.CloseLoadEffect();
        }

        private void OnDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_EggLose);
        }

        private void OnMovePoineEndCallBack(ISystemMsg body, SE_GPO.Event_MovePointEnd ent) {
            if (attackAnimId != 0) {
                return;
            }
            PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_idle);
        }
        
        private void PlayIdle() {
            PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_idle);
        }
        
        private void SetMoveDirCallBack(ISystemMsg body, SE_GPO.Event_MoveDir ent) {
            if (attackAnimId != 0 || isMovePoint == false) {
                return;
            }

            moveDir = ent.MoveDir;
            var angle = Vector3.Angle(iEntity.GetForward(), moveDir);
            if (angle > runThresholdAngle && angle < 160f) {
                var crossProduct = Vector3.Cross(iEntity.GetForward(), moveDir);
                if (crossProduct.y > 0) {
                    PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_walkRight);
                } else {
                    PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_walkLeft);
                }
            } else {
                PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_walk);
            }
        }
        
        public void OnIsMovePointCallBack(ISystemMsg body, SE_AI.Event_IsMovePoint ent) {
            this.isMovePoint = ent.IsTrue;
            if (this.isMovePoint == false && attackAnimId == 0) {
                PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_idle);
                return;
            }
            this.moveType = ent.MoveType;
        }

        private void OnFireBallStartCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FireBallStartAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireBallStart);
        }
        
        private void OnFireBallCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FireBallAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireBallSpit);
        }
        
        private void OnFireBallEndCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FireBallEndAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireBallOver);
        }

        private void OnTreadStartCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_TreadStartAnim ent) {
            if (ent.isLeftFoot) {
                PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_TreadOnStartLeft);
            } else {
                PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_TreadOnStartRight);
            }
        }

        private void OnTreadAttackCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_TreadAttackAnim ent) {
            if (ent.isLeftFoot) {
                PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_TreadOnLeft);
            } else {
                PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_TreadOnRight);
            }
        }

        private void OnTreadFireCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_TreadFireAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_TreadOnFlame);
        }

        private void OnTreadFireStartCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FireStartAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlameJetStart);
        }
        
        private void OnTreadFireForwardCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FireForwardAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlameJetForward);
        }
        
        private void OnPlayTornadoAnimCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_TornadoAnim ent) {
            var animState = ent.animState;
            PlayAttackAnim(animState);
            
            // PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoStart);// 使用技能立马触发 1.67s 
            // PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoAttack); // 1s
            // PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoLoop);// start 结束后直接播放 loop
            // PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FireTornadoEnd);
        }
        
        private void OnFullScreenAOECallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FullScreenAOE ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_AOE);
        }
        
        private void OnDragonCarStartCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_DragonCarChargeAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlightStart);
        }
        
        private void OnEvent_DelayBlastStartCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_DelayBlastStartAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_DelayBlast);
        }
        
        private void OnEvent_DelayBlastEndCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_DelayBlastEndAnim ent) {
            PlayAttackAnim(0);
        }
        
        private void OnDragonCarAttackCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_DragonCarAttackAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_Flighting);
        }
        
        private void OnDragonCarAttackCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_DragonCarDownAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlightDown);
        }
        
        private void OnDragonCarEndCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_DragonCarEndAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlightOver);
        }

        private void OnAuroraDragonAppearCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_AuroraDragonAppearAni ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_Appear);
        }
        
        private void OnAuroraDragonOutCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_AuroraDragonOutAni ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_Out);
        }
        
        private void OnSkillEndCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_PlaySkillEnd ent) {
            PlayAttackAnim(0);
        }

        private void OnFlyFlameStartCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FlyFlameStartAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlyFlameStart);
        }
        
        private void OnFlyFlameCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FlyFlameAnim ent) {
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlyFlame);
        }

        private void OnFlyFlameEndCallBack(ISystemMsg body, SE_AI_AuroraDragon.Event_FlyFlameEndAnim ent) {
            if (ent.IsTrue == false) {
                PlayAttackAnim(0);
                return;
            }
            PlayAttackAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_FlyFlameEnd);
        }
         
        private void PlayAttackAnim(int anim) {
            attackAnimId = anim;
            if (attackAnimId == 0) {
                PlayAnim(AnimConfig_AuroraDragon.Anim_AuroraDragon_idle);
            } else {
                PlayAnim(anim);
            }
        }
        
        private void PlayAnim(int animId) {
            if (playAnim == AnimConfig_AuroraDragon.Anim_AuroraDragon_Out) {
                return;
            }

            if (playAnim == animId) {
                return;
            }
            playAnim = animId;
            playable?.PlayAnimId(animId);
            Rpc(new Proto_AI.Rpc_Anim {
                animId = (ushort)animId,
            });
        }
    }
}