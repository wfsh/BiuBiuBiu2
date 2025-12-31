using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using Sofunny.BiuBiuBiu2.Message;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class Server1v1ModeWeaponInit : ComponentBase {
        public bool firstRound;
        private bool isCollectedThisRound = false;
        private List<SE_Mode.PlayModeCharacterData> characterDataList;
        private Dictionary<long, List<WeaponResultInfo>> prevWeaponsDict;

        private struct WeaponResultInfo {
            public int id;
            public int level;
            public int fireBullets;
            public int hitBullets;
        }

        protected override void OnAwake() {
            firstRound = true;
            prevWeaponsDict = new Dictionary<long, List<WeaponResultInfo>>();
            Register<SE_Mode.Event_GameState>(OnGameStateCallBack);
            Register<SE_Mode.Event_GetWeaponResult>(OnGetWeaponResultCallBack);
            Register<SE_Mode.Event_OverGameMode>(OnOverGameModeCallBack);
        }

        protected override void OnClear() {
            Unregister<SE_Mode.Event_GameState>(OnGameStateCallBack);
            Unregister<SE_Mode.Event_GetWeaponResult>(OnGetWeaponResultCallBack);
            Unregister<SE_Mode.Event_OverGameMode>(OnOverGameModeCallBack);
        }

        private void OnGameStateCallBack(ISystemMsg body, SE_Mode.Event_GameState e) {
            switch (e.GameState) {
                case ModeData.GameStateEnum.None:
                    break;
                case ModeData.GameStateEnum.Wait:
                    break;
                case ModeData.GameStateEnum.WaitStartDownTime:
                    break;
                case ModeData.GameStateEnum.WaitRoundStart:
                    CheckInitWeapon();
                    break;
                case ModeData.GameStateEnum.RoundStart:
                    break;
                case ModeData.GameStateEnum.RoundEnd:
                    isCollectedThisRound = true;
                    CollectionWeapon();
                    break;
                case ModeData.GameStateEnum.WaitNextRound:
                    break;
                case ModeData.GameStateEnum.WaitModeOver:
                    break;
                case ModeData.GameStateEnum.ModeOver:
                    break;
                case ModeData.GameStateEnum.QuitApp:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnGetWeaponResultCallBack(ISystemMsg body, SE_Mode.Event_GetWeaponResult e) {
            List<SM_FunnyDB.WeaponResultInfo> result;
            if (prevWeaponsDict.TryGetValue(e.PlayerId, out var prevWeaponList)) {
                result = new List<SM_FunnyDB.WeaponResultInfo>(prevWeaponList.Count);
                foreach (var weaponResultInfo in prevWeaponList) {
                    var weaponItem = ItemData.GetData(weaponResultInfo.id);
                    result.Add(new SM_FunnyDB.WeaponResultInfo {
                        Weapon = weaponItem.Sign,
                        WeaponLevel = weaponResultInfo.level,
                        FireBullets = weaponResultInfo.fireBullets,
                        HitBullets = weaponResultInfo.hitBullets
                    });
                }
            } else {
                result = new List<SM_FunnyDB.WeaponResultInfo>();
                Debug.Log($"[WeaponMode-Error] playerId: {e.PlayerId} not found in prevWeaponsDict");
            }
            e.CallBack(result);
        }

        private void CheckInitWeapon() {
            isCollectedThisRound = false;
            CheckGetCharacterList();
            if (ModeData.WeaponSource == ModeData.WeaponSourceEnum.RandomRedWeapon) {
                if (!firstRound && !ModeData.PlayData.PerRoundRandWeapon) {
                    return;
                }
                firstRound = false;
                WeaponData.GetRandomRedWeapon(out var weapon1, out var weapon2, out var superWeapon);
                foreach (var characterData in characterDataList) {
                    var characterWeapons = characterData.WeaponList;
                    characterWeapons.Clear();
                    characterWeapons.Add(new SE_Mode.PlayModeCharacterWeapon {
                        Index = 1,
                        WeaponItemId = weapon1,
                        IsSuperWeapon = false,
                        Level = WeaponQualitySet.Quality_Red,
                    });
                    characterWeapons.Add(new SE_Mode.PlayModeCharacterWeapon {
                        Index = 2,
                        WeaponItemId = weapon2,
                        IsSuperWeapon = false,
                        Level = WeaponQualitySet.Quality_Red,
                    });
                    characterWeapons.Add(new SE_Mode.PlayModeCharacterWeapon {
                        Index = 3,
                        WeaponItemId = superWeapon,
                        IsSuperWeapon = true,
                        Level = WeaponQualitySet.Quality_Red,
                    });
                }
            }
        }

        private void CheckGetCharacterList() {
            if (characterDataList != null) {
                return;
            }
            Dispatcher(new SE_Mode.Event_GetCharacterList {
                CallBack = list => characterDataList = list
            });
        }

        private void CollectionWeapon() {
            foreach (var characterData in characterDataList) {
                if (characterData.IsAI) {
                    continue;
                }
                var characterWeapons = characterData.WeaponList;
                foreach (var weapon in characterWeapons) {
                    if (weapon.IsSuperWeapon) {
                        continue;
                    }
                    CollectionWeapon(characterData.PlayerId, weapon);
                }
            }
        }

        private void CollectionWeapon(long playerId, SE_Mode.PlayModeCharacterWeapon weapon) {
            int fireCount = 0, hitCount = 0;
            if (weapon.iWeapon != null) {
                weapon.iWeapon.Dispatcher(new SE_Weapon.Event_GetGunFireRecord {
                    CallBack = (count, hit) => {
                        fireCount = count;
                        hitCount = hit;
                    }
                });
            } else {
                Debug.Log($"[WeaponMode-Error] weapon.iWeapon is null, weaponItemId: {weapon.WeaponItemId}");
                return;
            }
            if (!prevWeaponsDict.TryGetValue(playerId, out var prevWeaponList)) {
                prevWeaponList = new List<WeaponResultInfo>();
                prevWeaponsDict.Add(playerId, prevWeaponList);
            }
            for (int index = 0; index < prevWeaponList.Count; index++) {
                var weaponResultInfo = prevWeaponList[index];
                if (weaponResultInfo.id == weapon.WeaponItemId) {
                    weaponResultInfo.fireBullets += fireCount;
                    weaponResultInfo.hitBullets += hitCount;
                    prevWeaponList[index] = weaponResultInfo;
                    return;
                }
            }
            prevWeaponList.Add(new WeaponResultInfo {
                id = weapon.WeaponItemId,
                level = weapon.Level,
                fireBullets = fireCount,
                hitBullets = hitCount
            });
        }

        private void OnOverGameModeCallBack(ISystemMsg body, SE_Mode.Event_OverGameMode ent) {
            if (!isCollectedThisRound) {
                isCollectedThisRound = true;
                CollectionWeapon();
            }
        }
    }
}