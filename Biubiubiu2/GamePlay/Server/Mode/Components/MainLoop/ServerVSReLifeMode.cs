using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerVSReLifeMode : ComponentBase {
        public struct RelifeData {
            public float RelifeDownTime;
            public IGPO RelifeGPO;
        }
        private List<PlayerSpawnPoint> team1PointList;
        private List<PlayerSpawnPoint> useTeam1PointList;
        private List<PlayerSpawnPoint> team2PointList;
        private List<PlayerSpawnPoint> useTeam2PointList;
        private List<SE_Mode.PlayModeCharacterData> characterList;
        private float protectTime = 3f;
        private ModeData.GameStateEnum gameState = ModeData.GameStateEnum.None;
        private List<RelifeData> relifeList = new List<RelifeData>();

        protected override void OnAwake() {
            mySystem.Register<SE_Mode.Event_GameState>(OnGameStateCallBack);
            mySystem.Register<SE_Mode.Event_AddCharacterFinish>(OnAddCharacterCallBack);
            mySystem.Register<SE_Mode.Event_SendCharacterScore>(OnSendCharacterScoreCallBack);
            MsgRegister.Register<SM_Mode.GetStartPoint>(OnGetStartPointCallBack);
            MsgRegister.Register<SM_Mode.StartMode>(OnStartModeCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            mySystem.Unregister<SE_Mode.Event_GameState>(OnGameStateCallBack);
            mySystem.Unregister<SE_Mode.Event_AddCharacterFinish>(OnAddCharacterCallBack);
            mySystem.Unregister<SE_Mode.Event_SendCharacterScore>(OnSendCharacterScoreCallBack);
            MsgRegister.Unregister<SM_Mode.GetStartPoint>(OnGetStartPointCallBack);
            MsgRegister.Unregister<SM_Mode.StartMode>(OnStartModeCallBack);
        }

        private void OnGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState e) {
            gameState = e.GameState;
            switch (e.GameState) {
                case ModeData.GameStateEnum.Wait:
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    RoundStart();
                    break;
                case ModeData.GameStateEnum.WaitNextRound:
                    break;
                case ModeData.GameStateEnum.RoundEnd:
                    break;
                case ModeData.GameStateEnum.WaitModeOver:
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    break;
            }
        }

        private void OnAddCharacterCallBack(ISystemMsg body, SE_Mode.Event_AddCharacterFinish ent) {
            ent.Data.CharacterGPO.Register<SE_GPO.Event_SetIsDead>(OnCharacterDeadCallBack);
            AddWeapon(ent.Data);
            if (gameState == ModeData.GameStateEnum.RoundStart && ent.Data.IsAI) {
                ent.Data.CharacterGPO.Dispatcher(new SE_GPO.Event_BecomeTargetProtectTime {
                    ProtectTime = protectTime,
                });
            }
        }

        /// <summary>
        /// 角色死亡
        /// </summary>
        /// <param name="ent"></param>
        private void OnCharacterDeadCallBack(ISystemMsg body, SE_GPO.Event_SetIsDead ent) {
            if (ent.IsDead == false) {
                return;
            }
            relifeList.Add(new RelifeData {
                RelifeDownTime = 1f,
                RelifeGPO = ent.DeadGpo,
            });
        }
        
        public void OnUpdate(float deltatTime) {
            for (int i = 0; i < relifeList.Count; i++) {
                var data = relifeList[i];
                data.RelifeDownTime -= Time.deltaTime;
                if (data.RelifeDownTime <= 0) {
                    relifeList.RemoveAt(i);
                    i--;
                    RelifeGPO(data.RelifeGPO);
                } else {
                    relifeList[i] = data;
                }
            }
        }

        private void RelifeGPO(IGPO DeadGpo) {
            ResetCharacterState(DeadGpo);
            AddItem(DeadGpo, ItemSet.Id_FocusGunBullet, 100);
            AddItem(DeadGpo, ItemSet.Id_GunBullet, 100);
            DeadGpo.Dispatcher(new SE_GPO.Event_BecomeTargetProtectTime {
                ProtectTime = protectTime,
            });
        }

        private void OnStartModeCallBack(SM_Mode.StartMode ent) {
            GetStartPoint();
        }

        private void GetStartPoint() {
            MsgRegister.Dispatcher(new SM_Scene.GetPlayerPointList {
                TeamIndex = 1, CallBack = GetStartPoint1s
            });
            MsgRegister.Dispatcher(new SM_Scene.GetPlayerPointList {
                TeamIndex = 2, CallBack = GetStartPoint2s
            });
        }

        private void OnGetStartPointCallBack(SM_Mode.GetStartPoint ent) {
            var startPoint = GetStartPoint(ent.CharacterGPO);
            ent.CallBack(startPoint.Point, startPoint.Rota);
        }

        private void GetStartPoint1s(List<PlayerSpawnPoint> pointList) {
            team1PointList = pointList;
        }

        private void GetStartPoint2s(List<PlayerSpawnPoint> pointList) {
            team2PointList = pointList;
        }

        private void ResetCharacterState(IGPO iGpo) {
            var data = GetStartPoint(iGpo);
            iGpo.Dispatcher(new SE_Entity.SyncPointAndRota {
                Point = data.Point,
                Rota = data.Rota,
            });
            iGpo.Dispatcher(new SE_GPO.Event_ReLife {
                UpHp = 9999999,
            });
        }

        private PlayerSpawnPoint GetStartPoint(IGPO iGpo) {
            if (team1PointList.Count <= 0 || team2PointList.Count <= 0) {
                GetStartPoint();
            }
            PlayerSpawnPoint data = null;
            if (iGpo.GetTeamID() == 1) {
                if (useTeam1PointList == null || useTeam1PointList.Count <= 0) {
                    useTeam1PointList = new List<PlayerSpawnPoint>(team1PointList);
                }
                var index = Random.Range(0, useTeam1PointList.Count);
                data = useTeam1PointList[index];
                useTeam1PointList.RemoveAt(index);
            } else {
                if (useTeam2PointList == null || useTeam2PointList.Count <= 0) {
                    useTeam2PointList = new List<PlayerSpawnPoint>(team2PointList);
                }
                var index = Random.Range(0, useTeam2PointList.Count);
                data = useTeam2PointList[index];
                useTeam2PointList.RemoveAt(index);
            }
            return data;
        }

        private void RoundStart() {
            mySystem.Dispatcher(new SE_Mode.Event_GetCharacterList {
                CallBack = OnGetCharacterListCallBack,
            });
        }

        private void OnGetCharacterListCallBack(List<SE_Mode.PlayModeCharacterData> characterList) {
            this.characterList = characterList;
            foreach (var data in characterList) {
                var iGpo = data.CharacterGPO;
                if (iGpo != null) {
                    ResetCharacterState(iGpo);
                }
            }
        }

        private void OnSendCharacterScoreCallBack(ISystemMsg body, SE_Mode.Event_SendCharacterScore ent) {
            AddItem(ent.Data.CharacterGPO, ItemSet.Id_GunBullet, 50);
        }

        private void AddItem(IGPO characterGpo, int itemId, ushort itemNum, bool isProtect = false) {
            characterGpo.Dispatcher(new SE_Item.Event_AddPickItem {
                ItemId = itemId, ItemNum = itemNum, IsProtect = isProtect, IsQuickUse = true,
            });
        }
        
        
        /// -------------------------------------------------------------------------------------------------
        ///                                         添加武器
        /// -------------------------------------------------------------------------------------------------
        
        private void AddWeapon(SE_Mode.PlayModeCharacterData data) {
            for (int i = 0; i < data.WeaponList.Count; i++) {
                var weapon = data.WeaponList[i];
                if (weapon.IsSuperWeapon == false) {
                    data.CharacterGPO.Dispatcher(new SE_Item.Event_AddWeaponForMode {
                        WeaponData = weapon
                    });
                } else {
                    data.CharacterGPO.Dispatcher(new SE_Skill.Event_AddSkill {
                        SkillData = weapon,
                    });
                }
            }
            AddItem(data.CharacterGPO, ItemSet.Id_GunBullet, 999);
        }
    }
}