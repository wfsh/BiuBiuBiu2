using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.ServerMessage;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.ServerGamePlay {
    public class ServerSausageCharacterAIAttribute : ServerAIAttribute {
        private MonsterArmedCustom customCfg;
        private bool isInit;
        
        protected override void OnAwake() {
            base.OnAwake();
            mySystem.Register<SE_GPO.Event_GetHitHeadAccuracy>(OnGetHitHeadAccuracy);
            mySystem.Register<SE_GPO.Event_GetHeadHurtRate>(GetHitHeadHurtRate);
            mySystem.Register<SE_AI.Event_GetArmedCustomData>(GetArmedCustomCfgCallBack);
        }

        protected override void OnClear() {
            base.OnClear();
            mySystem.Unregister<SE_GPO.Event_GetHitHeadAccuracy>(OnGetHitHeadAccuracy);
            mySystem.Unregister<SE_GPO.Event_GetHeadHurtRate>(GetHitHeadHurtRate);
            mySystem.Unregister<SE_AI.Event_GetArmedCustomData>(GetArmedCustomCfgCallBack);
        }

        protected override void OnStart() {
            base.OnStart();
            InitArmedData();
        }

        private void InitArmedData() {
            var monsterSign = AISystem.AttributeData.Sign;
            var customConfigs = MonsterArmedCustomSet.GetMonsterArmedCustomsByMonsterSignMatchMode(monsterSign, ModeData.MatchId);
            if (customConfigs.Count > 0) {
                int index = UnityEngine.Random.Range(0, customConfigs.Count);
                var randomCustomCfg = customConfigs[index];
                SetTemplateCustomData(randomCustomCfg);
            }
        }

        override protected void OnGetHitRatioCallBack(ISystemMsg body, SE_GPO.Event_GetHitRatio ent) {
            ent.CallBack(customCfg.Accuracy);
        }

        public void OnGetHitHeadAccuracy(ISystemMsg body, SE_GPO.Event_GetHitHeadAccuracy ent) {
            ent.CallBack(customCfg.HitHeadAccuracy);
        }

        private void GetHitHeadHurtRate(ISystemMsg body, SE_GPO.Event_GetHeadHurtRate ent) {
            ent.CallBack.Invoke(OnGetHitHeadHurtRate());
        }

        private void GetArmedCustomCfgCallBack(ISystemMsg body, SE_AI.Event_GetArmedCustomData ent) {
            ent.CallBack.Invoke(isInit, customCfg);
        }

        private float OnGetHitHeadHurtRate() {
            if (useWeapon != null) {
                var hurtRate = 0.0f;
                useWeapon.Dispatcher(new SE_Weapon.Event_GetHitHeadMultiplier() {
                    CallBack = value => {
                        hurtRate = value;
                    }
                });
                return hurtRate;
            }
            return 1;
        }

        /// <summary>
        /// 根据枪械属性生成枪械
        /// </summary>
        /// <param name="cfg"></param>
        private void SetTemplateCustomData(MonsterArmedCustom cfg) {
            customCfg = cfg;
            var weaponData = WeaponData.Get(cfg.WeaponItemId);
            AISystem.Dispatcher(new SE_Item.Event_AddWeaponForMode() {
                WeaponData = new SE_Mode.PlayModeCharacterWeapon {
                    WeaponItemId = weaponData.ItemId,
                    Index = 1,
                    IsSuperWeapon = false,
                    HitHeadMultiplier = weaponData.HitHeadMultiplier,
                    RandAttack = Mathf.FloorToInt(WeaponData.GetHitDamageRatio(customCfg) * 100),
                    RandHp = 0,
                }
            });
            isInit = true;
        }
    }
}