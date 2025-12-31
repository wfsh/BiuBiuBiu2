using System;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Asset;
using Sofunny.BiuBiuBiu2.ClientMessage;
using Sofunny.BiuBiuBiu2.CoreGamePlay;
using Sofunny.BiuBiuBiu2.CoreMessage;
using Sofunny.BiuBiuBiu2.Data;
using Sofunny.BiuBiuBiu2.Message;
using Sofunny.BiuBiuBiu2.Template;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Component;
using Sofunny.BiuBiuBiu2.Playable.Config;

namespace Sofunny.BiuBiuBiu2.ClientGamePlay {
    public class ClientAIMachineGunAttack : ComponentBase {
        struct MachineGunEffect {
            public GameObject effect;
            public float lifeTime;
            public string url;
        }

        enum MachineEffectType {
            Fire = 1,
            Unload
        }
        
        private bool enabledDriveMove = false;
        private ClientGPO driveGPO;
        private bool isDrive = false;
        private bool isFireDown = false;
        private float sendTargetPointTime = 0.3f;
        private float audioTimer = 0;
        private int fireIndex = 0;
        private string fireEffectUrl;
        private string unloadEffectUrl;
        private List<MachineGunEffect> allEffects = new List<MachineGunEffect>();
        private List<Transform> fireTran = new List<Transform>();
        private List<Transform> unloadTran = new List<Transform>();
        private Transform currentFireTrans = null;
        private Transform currentUnloadTrans = null;
        private SausagePlayable playable;
        private EntityBase entity;
        private C_AI_Base aiBase;
        private GPOM_MachineGun useMData;
        private string monsterSign = "";
        private EntityAnimConfig config;
        private float checkIsFireTime = -10f;

        private float playEffectCount = 0;
        //过热
        private float countFireOverHotTimer = 0f; // 持续射击时间
        private float fireOverHotTimer = 0f; // 持续射击时间
        private float fireOverHotCDTimer = 0f; // 冷却时间
        private bool isOverHotCoolDown = false;
        protected bool isCoolingDown {
            get {
                return isOverHotCoolDown;
            }
            set {
                if (isOverHotCoolDown != value) {
                    isOverHotCoolDown = value;
                    if (isOverHotCoolDown) {
                        PlayerOverHotAudio();
                    }
                }
            }

        }  // 是否处于冷却
        private WeaponData.GunData gunData;

        protected override void OnAwake() {
            Register<CE_GPO.Event_DriveGPO>(OnDriveGPOCallBack);
            Register<CE_GPO.Event_OnInputDeviceFire>(OnDeviceStartFireCallBack);
            Register<CE_GPO.Event_GetDeviceSkillCD>(OnGetDeviceSkillCDCallBack);
            Register<CE_Weapon.Event_BulletSetFireGPO>(OnBulletSetFireGPOCallBack);
            Register<CE_Weapon.GetFireBox>(OnGetFireBoxCallBack);
            aiBase = (C_AI_Base)mySystem;
            useMData = (GPOM_MachineGun)aiBase.MData;
            monsterSign = aiBase.AttributeData.Sign;
        }

        protected override void OnStart() {
            base.OnStart();
            gunData = (WeaponData.GunData)WeaponData.Get(ItemSet.Id_MachineGun);
            fireOverHotTimer = gunData.FireOverHotTime;
            unloadEffectUrl = AssetURL.GetEffect("fx_machinegun_cartridge_case");
            fireEffectUrl = AssetURL.GetEffect("fx_machinegun_fire");
            mySystem.Dispatcher(new CE_GPO.Event_DeviceSkillCD {
                NowCD = 0f, MaxCD = 0f,
            });
            mySystem.Dispatcher(new CE_Weapon.GetFireOverHotTimer() {
                CallBack = value => {
                    fireOverHotTimer = value;
                } 
            });
            AddUpdate(OnUpdate);
        }
        
        private void OnUpdate(float delta) {
            audioTimer -= delta;
            audioTimer = Mathf.Max(audioTimer, 0);

            RecycleEffect(delta);
            CheckIsStopFire(delta);
            if (enabledDriveMove == false || isSetEntityObj == false) {
                return;
            }
            UpdateFireOverHotCD();
            CountFireOverHotTimer();
            if (sendTargetPointTime > 0) {
                sendTargetPointTime -= delta;
                return;
            }

            if (isFireDown == false || ModeData.PlayGameState != ModeData.GameStateEnum.RoundStart) {
                return;
            }

            sendTargetPointTime = 0.3f;
            if (isCoolingDown == false) {
                MsgRegister.Dispatcher(new CM_Camera.GetCameraCenterObjPoint {
                    CallBack = GetCameraTargetPoint,
                    FarDistance = useMData.MaxAttackDistance,
                    IgnoreTeamId = iGPO.GetTeamID(),
                    CheckForwardPoint = currentFireTrans.position,
                });
            }
        }

        protected override void OnClear() {
            base.OnClear();
            RemoveUpdate(OnUpdate);
            Unregister<CE_GPO.Event_DriveGPO>(OnDriveGPOCallBack);
            Unregister<CE_GPO.Event_OnInputDeviceFire>(OnDeviceStartFireCallBack);
            Unregister<CE_GPO.Event_GetDeviceSkillCD>(OnGetDeviceSkillCDCallBack);
            Unregister<CE_Weapon.Event_BulletSetFireGPO>(OnBulletSetFireGPOCallBack);
            Unregister<CE_Weapon.GetFireBox>(OnGetFireBoxCallBack);
            fireTran = null;
            unloadTran = null;
            playable = null;
            config = null;
            gunData = null;
            currentFireTrans = null;
            currentUnloadTrans = null;
            ClearAllEffects();
        }

        private void ClearAllEffects() {
            for (int i = 0; i < allEffects.Count; i++) {
                MachineGunEffect machineGunEffect = allEffects[i];
                PrefabPoolManager.OnReturnPrefab(machineGunEffect.url, machineGunEffect.effect);
            }
            allEffects.Clear();
        }

        private void CheckIsStopFire(float delta) {
            if (checkIsFireTime > -5) {
                if (checkIsFireTime <= 0) {
                    checkIsFireTime = -10;
                    playable?.StopSign(AnimConfig_MachineGun.Play_Fire);
                } else {
                    checkIsFireTime -= delta;
                }
            }
        }

        protected override void OnSetNetwork() {
        }

        protected override void OnSetEntityObj(IEntity iEntity) {
            base.OnSetEntityObj(iEntity);
            config = AIAnimConfig.Get(monsterSign);
            fireTran = iEntity.GetBodyTranList(GPOData.PartEnum.AttactPoint1);
            unloadTran = iEntity.GetBodyTranList(GPOData.PartEnum.LeftHand);
            currentFireTrans = fireTran[fireIndex];
            currentUnloadTrans = unloadTran[fireIndex];
            InitPlayableGraph();
        }

        private void InitPlayableGraph() {
            entity = (EntityBase)iEntity;
            var animator = entity.GetComponentInChildren<Animator>(true);
            playable = new SausagePlayable();
            playable.Init(entity.transform, animator, config, $"Client_{monsterSign}");
        }

        private void PlayAnimSign(string animSign) {
            if (playable == null) {
                return;
            }
            playable.PlayAnimSign(animSign);
        }
        private void PlayerOverHotAudio() {
            //播放过热音效
            if (driveGPO != null && driveGPO.IsLocalGPO()) {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("WP_G_MiniGun_Ovht"), driveGPO.GetPoint());
            }
        }
        private void OnGetFireBoxCallBack(ISystemMsg body, CE_Weapon.GetFireBox ent) {
            if (isSetEntityObj == false || IsClear() || fireTran == null) {
                return;
            }

            fireIndex++;
            if (fireIndex >= fireTran.Count) {
                fireIndex = 0;
            }

            currentFireTrans = fireTran[fireIndex];
            currentUnloadTrans = unloadTran[fireIndex];
            ent.CallBack(currentFireTrans);
            if (checkIsFireTime < -5) {
                PlayAnimSign(AnimConfig_MachineGun.Play_Fire);
            }
            checkIsFireTime = useMData.AttackIntervalTime * 2;
            PlayEffect();
        }

        private void ShakeCamera() {
            if (driveGPO != null && driveGPO.IsLocalGPO()) {
                MsgRegister.Dispatcher(new CM_Camera.ShakeCamera() {
                    Duration = 0.2f,
                    Magnitude = 0.1f,
                    DampingSharpness = 1.6f,
                    NoiseFrequency = new Vector3(10, 10, 0),
                    AffectRotation = true,
                    MaxRotationAngle = 10,
                });
            }
        }

        private void PlayEffect() {
            switch (QualityData.GetQualityType()) {
                case QualityData.QualityType.Low:
                    return;
                case QualityData.QualityType.High:
                    LoadAndInitMachineGunEffect(fireEffectUrl,MachineEffectType.Fire,fireIndex,0.2f);
                    LoadAndInitMachineGunEffect(unloadEffectUrl,MachineEffectType.Unload,fireIndex,0.5f);
                    break;
                case QualityData.QualityType.Medium:
                    playEffectCount++;
                    int fireCount = fireTran.Count;
                    if (playEffectCount <= fireCount) {
                        LoadAndInitMachineGunEffect(fireEffectUrl,MachineEffectType.Fire,fireIndex,0.2f);
                        LoadAndInitMachineGunEffect(unloadEffectUrl,MachineEffectType.Unload,fireIndex,0.4f);
                    }else if (playEffectCount >= fireCount * 5) {
                        playEffectCount = 0;
                    }
                    break;
            }
        }

        private void LoadAndInitMachineGunEffect(string effectUrl,MachineEffectType type,int index, float lifeTime) {
            PrefabPoolManager.OnGetPrefab(effectUrl, null, (obj) => {
                if (isClear) {
                    PrefabPoolManager.OnReturnPrefab(effectUrl, obj);
                    return;
                }

                if (obj != null) {
                    Transform effectTransform = obj.transform;
                    MsgRegister.Dispatcher(new M_Stage.SetGamePlayWorldLayer {
                        layer = StageData.GameWorldLayerType.Ability, transform = effectTransform
                    });
                    var fireBox = fireTran[index];
                    var unloadBox = unloadTran[index];
                    switch (type) {
                        case MachineEffectType.Fire:
                            effectTransform.position = fireBox.position;
                            effectTransform.rotation = fireBox.rotation;
                            break;
                        case MachineEffectType.Unload:
                            effectTransform.position = unloadBox.position;
                            effectTransform.rotation = unloadBox.rotation;
                            break;
                    }

                    MachineGunEffect machineGunEffect = new MachineGunEffect {
                        effect = obj,
                        lifeTime = lifeTime,
                        url = effectUrl
                    };
                    allEffects.Add(machineGunEffect);
                }
            });
        }

        private void OnGetDeviceSkillCDCallBack(ISystemMsg body, CE_GPO.Event_GetDeviceSkillCD ent) {
            ent.CallBack(0f, 0f);
        }

        private void OnBulletSetFireGPOCallBack(ISystemMsg body, CE_Weapon.Event_BulletSetFireGPO ent) {
            if (isSetEntityObj == false || audioTimer > 0 || IsClear()) {
                return;
            }

            audioTimer = 0.05f;
            if (driveGPO != null && driveGPO.IsLocalGPO()) {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("WP_SA_Machinegun_Fire"), currentFireTrans.position);
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio1P("WP_SA_Machinegun_Unload"), currentUnloadTrans.position);
            } else {
                AudioPoolManager.OnPlayAudio(AssetURL.GetAudio3P("WP_SA_Machinegun_Fire_3P"), currentFireTrans.position);
            }
        }

        private void OnDriveGPOCallBack(ISystemMsg body, CE_GPO.Event_DriveGPO ent) {
            driveGPO = (ClientGPO)ent.PlayerDriveGPO;
            if (driveGPO == null) {
                enabledDriveMove = false;
            } else {
                enabledDriveMove = driveGPO.IsLocalGPO();
            }
        }

        private void RecycleEffect(float delta) {
            for (int i = allEffects.Count - 1; i >= 0; i--) {
                MachineGunEffect machineGunEffect = allEffects[i];
                if (machineGunEffect.lifeTime > 0) {
                    machineGunEffect.lifeTime -= delta;
                    allEffects[i] = machineGunEffect;
                } else {
                    PrefabPoolManager.OnReturnPrefab(machineGunEffect.url, machineGunEffect.effect);
                    allEffects.RemoveAt(i);
                }
            }
        }
        private void UpdateFireOverHotCD() {
            if (isCoolingDown == false) {
                return;
            }
            fireOverHotCDTimer -= Time.deltaTime;
            if (fireOverHotCDTimer <= 0f) {
                isCoolingDown = false;
            }
            aiBase.Dispatcher(new CE_Weapon.OutFireOverHotTimer {
                NowOverHotTimer = fireOverHotCDTimer, MaxOverHotTime = gunData.FireOverHotCDTime, IsCooling = true
            });
        }
        
        private void CountFireOverHotTimer() {
            if (fireOverHotTimer <= 0) {
                return;
            }
            if (isFireDown) {
                if (countFireOverHotTimer >= fireOverHotTimer) {
                    StartCoolingDown();
                } else {
                    countFireOverHotTimer += Time.deltaTime;
                }
            } else {
                if (countFireOverHotTimer > 0f) {
                    countFireOverHotTimer -= Time.deltaTime;
                }
            }
            if (isCoolingDown == false) {
                aiBase.Dispatcher(new CE_Weapon.OutFireOverHotTimer {
                    NowOverHotTimer = countFireOverHotTimer, MaxOverHotTime = fireOverHotTimer, IsCooling = false
                });
            }
        }
        
        private void StartCoolingDown() {
            isCoolingDown = true;
            countFireOverHotTimer = 0f;
            fireOverHotCDTimer = gunData.FireOverHotCDTime;
        }

        private void GetCameraTargetPoint(Vector3 targetPoint, bool isHit) {
            var firePoints = new Vector3[1];
            firePoints[0] = targetPoint;
            mySystem.Dispatcher(new CE_GPO.Event_OnDeviceFire {
                Points = firePoints,
            });
        }

        private void OnDeviceStartFireCallBack(ISystemMsg body, CE_GPO.Event_OnInputDeviceFire ent) {
            if (enabledDriveMove == false || isSetEntityObj == false) {
                return;
            }
            isFireDown = ent.IsDown;
        }
    }
}