using System;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerAbilityMissileBombSpawner : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public ServerGPO FireGPO;
            public AbilityData.PlayAbility_Missile InData;
        }
        private float timer;
        private bool bombing;
        private bool isAllSpawn;
        private int bombingCount;
        private Vector2 prevOffset;
        private ServerGPO fireGPO;
        private AbilityData.PlayAbility_Missile inData;
        private const float WAIT_BOMB_END_DURATION = 5f;

        public void Init(ServerGPO fireGPO, AbilityData.PlayAbility_Missile inData) {
            timer = 0f;
            bombingCount = 0;
            bombing = false;
            isAllSpawn = false;
            prevOffset = Vector2.zero;
            this.fireGPO = fireGPO;
            this.inData = inData;
        }

        protected override void OnAwake() {
            Register<SE_Ability.Ability_StartMissileBombing>(OnStartMissileBombing);
            var initData = (InitData)initDataBase;
            Init(initData.FireGPO, initData.InData);
        }

        protected override void OnStart() {
            base.OnStart();
            AddUpdate(OnUpdate);
        }

        protected override void OnClear() {
            RemoveUpdate(OnUpdate);
            Unregister<SE_Ability.Ability_StartMissileBombing>(OnStartMissileBombing);
        }

        private void OnStartMissileBombing(ISystemMsg body, SE_Ability.Ability_StartMissileBombing ent) {
            bombing = true;
        }

        public void OnUpdate(float deltaTime) {
            if (!bombing) {
                return;
            }
            if (!isAllSpawn) {
                UpdateBombing(deltaTime);
            } else {
                timer += deltaTime;
                if (timer < WAIT_BOMB_END_DURATION && bombingCount != 0) {
                    return;
                }
                bombing = false;
                Dispatcher(new SE_Ability.Ability_EndMissileBombing());
            }
        }

        private void UpdateBombing(float deltaTime) {
            int prevCount = (int)Math.Floor(timer / inData.In_BombingInterval);
            timer += deltaTime;
            int newCount = (int)Math.Floor(timer / inData.In_BombingInterval);
            int spawnCount = newCount - prevCount;
            if (spawnCount > 0) {
                SpawnBomb(spawnCount);
            }
            if (timer >= inData.M_BombingDuration + inData.In_RandDuration || ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                timer = 0;
                isAllSpawn = true;
            }
        }

        private void SpawnBomb(int count) {
            for (int i = 0; i < count; i++) {
                var offset = GetRandomOffsetFarWayPrevious();
                var endPos = inData.In_Points[^1];
                var finalPos = endPos + new Vector3(offset.x, -2 * inData.M_BombSpawnHeight, offset.y);
                if (PhysicsUtil.CheckBlocked(new Vector3(finalPos.x, inData.M_BombSpawnHeight, finalPos.z),
                        new Vector3(finalPos.x, -2 * inData.M_BombSpawnHeight, finalPos.z),
                        IgnoreFunc,
                        ~LayerData.ClientLayerMask, out var hitPoint)) {
                    finalPos = hitPoint;
                }
                bombingCount++;
                MsgRegister.Dispatcher(new SM_Ability.PlayAbilityOld {
                    FireGPO = fireGPO,
                    CallBack = OnPlayAbilityMissileBomb,
                    AbilityMData = new AbilityData.PlayAbility_MissileBomb {
                        ConfigId = AbilityConfig.MissileBomb,
                        In_Points = new []{ new Vector3(finalPos.x, inData.M_BombSpawnHeight, finalPos.z), new Vector3(finalPos.x, -2 * inData.M_BombSpawnHeight, finalPos.z) },
                        In_WeaponItemId = ItemSet.Id_Missile,
                        In_AttackRangeRange = inData.In_AttackRange,
                        In_ATK = inData.In_ATK
                    },
                });
            }
        }

        private bool IgnoreFunc(RaycastHit hit) {
            var hitType = hit.collider.gameObject.GetComponent<HitType>();
            if (hitType != null) {
                if (hitType.Layer == GPOData.LayerEnum.Ignore || (hitType.MyEntity != null && hitType.MyEntity.GetTeamID() == fireGPO.GetTeamID())) {
                    return true;
                }
            }
            return false;
        }

        private Vector2 GetRandomOffsetFarWayPrevious() {
            Vector2 offset;
            do {
                offset = GetRandomOffset();
            } while((offset - prevOffset).sqrMagnitude < 1);
            prevOffset = offset;
            return offset;
        }

        private Vector2 GetRandomOffset() {
            var angle = Random.Range(0, 2 * Mathf.PI);
            var radius = Random.Range(0f, inData.In_FinalAreaRadius);
            var y = Mathf.Sin(angle) * radius;
            var x = Mathf.Cos(angle) * radius;
            return new Vector2(x, y);
        }

        private void OnPlayAbilityMissileBomb(IAbilitySystem abilitySystem) {
            var missileBombSystem = abilitySystem as SAB_MissileBombSystem;
            missileBombSystem.GetEntity().SetRota(Quaternion.Euler(90, 0, 0));
            missileBombSystem.SetPlayExplosive(OnPlayExplosive);
        }

        private void OnPlayExplosive(IAbilitySystem abilitySystem) {
            abilitySystem.Dispatcher(new SE_Ability.Ability_InitGetHurtRatioFunc {
                getHurtRatioFunc = GetHurtRatio
            });
            abilitySystem.Dispatcher(new SE_Ability.Ability_SetClearCallback {
                Callback = () => {
                    bombingCount--;
                }
            });
        }

        private float GetHurtRatio(IGPO hitGpo, float distance) {
            if (hitGpo.GetGpoMTypeID() == GpoSet.Id_Helicopter || hitGpo.GetGpoMTypeID() == GpoSet.Id_Tank) {
                return inData.M_VehicleHurtRatio;
            }
            return 1;
        }
    }
}