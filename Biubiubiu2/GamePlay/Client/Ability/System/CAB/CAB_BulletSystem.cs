using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class CAB_BulletSystem : C_Ability_Base {
        public Vector3 targetPoint;
        public Vector3 targetNormal;
        public float speed;
        private bool isShowDecal = false;
        private string effectSign = "";
        private string bulletDecal = "";
        private AbilityM_Bullet useMData;

        protected override void OnAwake() {
            base.OnAwake();
            useMData = (AbilityM_Bullet)MData;
            InitProtoData();
            AddComponents();
        }

        protected override void OnClear() {
            base.OnClear();
        }

        protected override void OnStart() {
            base.OnStart();
            SetInvokeClearPoolEntityTime(0.1f);
            effectSign = useMData.M_EffectSign;
            bulletDecal = useMData.M_HitEffect;
            CreateEntityToPool(effectSign);
        }

        private void InitProtoData() {
            if (InData.GetID() == Proto_Ability.Rpc_BulletFire.ID) {
                var abilityData = (Proto_Ability.Rpc_BulletFire)InData;
                targetPoint = abilityData.targetPoint;
                speed = abilityData.speed * 0.1f;
            } else {
                var abilityData = (Proto_Ability.Rpc_BulletFireDecal)InData;
                targetPoint = abilityData.targetPoint;
                targetNormal = abilityData.targetNormal;
                speed = abilityData.speed * 0.1f;
                isShowDecal = true;
            }
        }

        private void AddComponents() {
            AddComponent<ClientResetTrailRenderer>();
            AddComponent<TimeReduce>();
            AddComponent<ClientBulletDecal>();
            AddComponent<ClientBulletStartPoint>(new ClientBulletStartPoint.InitData {
                TargetPoint = targetPoint,
                Speed = speed,
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