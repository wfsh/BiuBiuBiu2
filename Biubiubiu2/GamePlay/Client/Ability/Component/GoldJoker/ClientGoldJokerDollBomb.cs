using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.NetworkMessage;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientGoldJokerDollBomb : ComponentBase {
        public struct InitData : SystemBase.IComponentInitData {
            public float AttackRange;
        }

        private static readonly int StateHash = Animator.StringToHash("State");
        private static readonly int StateOnHash = Animator.StringToHash("StateOn");
        private int state = -1;
        private Animator anim;
        private Transform[] effects;
        private Transform rootBody;

        // 警告特效
        private GameObject warningEffect;
        private string warningUrl;
        private float attackRange;

        protected override void OnAwake() {
            base.OnAwake();
            var initData = (InitData)initDataBase;
            attackRange = initData.AttackRange;
        }

        protected override void OnStart() {
        }

        protected override void OnSetNetwork() {
            AddProtoCallBack(Proto_Ability.Rpc_GoldJokerDollBombState.ID, OnSetState);
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveProtoCallBack(Proto_Ability.Rpc_GoldJokerDollBombState.ID, OnSetState);
            ClearFollowWarningEffect();
            rootBody = null;
            effects = null;
            anim = null;
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            rootBody = iEntity.GetBodyTran(GPOData.PartEnum.RootBody);
            var body = iEntity.GetBodyTran(GPOData.PartEnum.Body);
            anim = rootBody.GetComponentInChildren<Animator>();
            effects = new Transform[2] {
                body.GetChild(1), rootBody.GetChild(1)
            };
            if (state == -1) {
                SetState(1);
            }
        }

        private void OnSetState(INetwork network, IProto_Doc docData) {
            var data = (Proto_Ability.Rpc_GoldJokerDollBombState)docData;
            SetState(data.state);
        }

        private void SetState(int newState) {
            if (state == newState) {
                return;
            }
            state = newState;
            if (state > 1) {
                AddFollowWarningEffect();
            }
            SetAnimState();
            SetEffectState();
        }

        private void AddFollowWarningEffect() {
            if (warningEffect != null) {
                return;
            }
            warningUrl = $"{AssetURL.GamePlay}/Ability/CircleWarningEffect.prefab";
            PrefabPoolManager.OnGetPrefab(
                warningUrl,
                rootBody,
                gameObj => {
                    if (IsClear()) {
                        PrefabPoolManager.OnReturnPrefab(warningUrl, gameObj);
                        return;
                    }
                    warningEffect = gameObj;
                    warningEffect.transform.localScale = Vector3.one * attackRange;
                });
        }

        private void ClearFollowWarningEffect() {
            if (string.IsNullOrEmpty(warningUrl) || warningEffect == null) {
                return;
            }
            PrefabPoolManager.OnReturnPrefab(warningUrl, warningEffect);
            warningEffect = null;
        }

        private void SetAnimState() {
            if (anim == null) return;
            anim.SetInteger(StateHash, state);
            anim.SetTrigger(StateOnHash);
        }

        private void SetEffectState() {
            if (effects == null) return;
            foreach (var effect in effects) {
                if (effect != null) {
                    effect.gameObject.SetActive(false);
                }
            }
            switch (state) {
                case 0:
                case 1:
                    effects[0]?.gameObject.SetActive(true);
                    break;
                case 2:
                case 3:
                    effects[1]?.gameObject.SetActive(true);
                    break;
            }
        }
    }
}