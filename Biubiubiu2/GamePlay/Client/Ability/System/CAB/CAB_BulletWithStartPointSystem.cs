using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_BulletWithStartPointSystem : C_Ability_Base {
        public Vector3 startPoint;
        public Vector3 targetPoint;
        public Vector3 targetNormal;
        public ushort abilityModId;
        public float speed;
        private bool isShowDecal = false;
        private string effectSign = "";
        private string bulletDecal = "";

        protected override void OnAwake() {
            base.OnAwake();
            InitProtoData();
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnStart() {
            base.OnStart();
            var data = AbilityConfig.GetAbilityModData(abilityModId);
            if (data == null) {
                Debug.LogError($"Bullet mod data is null, id: {abilityModId}");
                return;
            }
            var modData = (AbilityData.PlayAbility_BulletWithStartPos)data;
            effectSign = modData.M_EffectSign;
            bulletDecal = modData.M_HitEffect;
            CreateEntityToPool(effectSign);
        }

        private void InitProtoData() {
            if (InData.GetID() == Proto_Ability.Rpc_BulletFireWithStartPoint.ID) {
                var abilityData = (Proto_Ability.Rpc_BulletFireWithStartPoint)InData;
                startPoint = abilityData.startPoint;
                targetPoint = abilityData.targetPoint;
                abilityModId = abilityData.abilityModId;
                speed = abilityData.speed * 0.1f;
                iEntity.SetPoint(abilityData.startPoint);
            } else {
                var abilityData = (Proto_Ability.Rpc_BulletFireDecalWithStartPoint)InData;
                startPoint = abilityData.startPoint;
                targetPoint = abilityData.targetPoint;
                targetNormal = abilityData.targetNormal;
                abilityModId = abilityData.abilityModId;
                speed = abilityData.speed * 0.1f;
                isShowDecal = true;
                iEntity.SetPoint(abilityData.startPoint);
            }
        }

        private void AddComponents() {
            AddComponent<ClientResetTrailRenderer>();
            AddComponent<TimeReduce>();
            AddComponent<ClientBulletDecal>();
            AddComponent<ClientBulletStartPoint>(new ClientBulletStartPoint.InitData {
                TargetPoint = targetPoint,
                Speed = speed,
                StartPoint = this.startPoint,
                CallBack = () => {
                    LifeTimeEnd();
                },
            });
            AddComponent<MoveLinePointSpeed>(new MoveLinePointSpeed.InitData {
                Speed = speed * 0.8f,
                TargetPoint = targetPoint,
                CallBack = () => {
                    this.Dispatcher(new CE_Ability.AddBulletDecal() {
                        TargetPoint = targetPoint,
                        IsShowDecal = isShowDecal,
                        BulletDecal = bulletDecal,
                        TargetNormal = targetNormal,
                    });
                    LifeTimeEnd();
                }
            });
        }
        private void LifeTimeEnd() {
            this.Dispatcher(new CE_Ability.RemoveAbility() {
                AbilityId = this.AbilityId
            });
        }
    }
}