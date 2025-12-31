using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sofunny.BiuBiuBiu2.Template;

namespace Sofunny.BiuBiuBiu2.Data {
    public class SkillData {
        public static int Skill_SummonDriverSwordTiger = 1;
        public static int Skill_SummonDriverTank = 2;
        public static int Skill_SummonDriverHelicopter = 3;
        public static int Skill_Missile = 4;
        public static int Skill_UAV = 5;
        public static int Skill_MachineGun = 6;

        public enum SkillTypeEnum {
            SummonDriver,
            HoldOn,    // 手持物品
            CallMonster
        }
        public enum GetSkillPointType {
            GetModePoint, // 获取模式点数
            ContinueGetModePoint, // 持续获取模式点数
            TimeGetModePoint, // 时间获取模式点数 (1 秒获取 1 点)
        }

        public class Data {
            public int ID = 0;
            public int ItemId = 0;
            public string ShowName = "";
            public string Sign = "";
            public SkillTypeEnum SkillType;
            public float SkillCD; // 技能冷却时间， 使用后下次使用的时间
            public float SkillDuration; // 技能持续时间, 0 为无限时间
            public int SkillActivePoint; // 激活技能所需点数
            public GetSkillPointType GetSkillPointType; // 获取技能点数类型
            public int GetSkillPoint; // 单次触发获取的技能点数 
            public int MaxSaveSkillPoint; // 最大保存技能点数
            public int SummonNum;//数量
            public bool IsCanCancel;
            public SkillNeedArea NeedArea =new SkillNeedArea();
        }
        
        public class UseSkillData {
            public Data ModeData;
            public float CurrectSkillCD; // 当前技能 CD
            public float CurrectSkillDuration; // 当前技能持续时间
            public int CurrectSkillPoint; // 当前技能点数
        }

        public class HoldOnData {
            public int skillId;
            public ushort abilityConfigId;
            public float maxDistance; // 扔的最远距离
            public string targetArea; // 目标指示区
            public string progressTip; // 提示
        }
        
        public class CallAIData {
            public int skillId;
            public ushort abilityConfigId;
            public string progressTip; // 提示
        }
        public class  SkillNeedArea {
            public float width;
            public float height;
        }
        private static List<Data> SkillDataList = new List<Data> {
            new Data {
                ID = Skill_SummonDriverTank, ItemId = ItemSet.Id_Tank ,Sign =  GPOM_TankSet.Sign_Tank, SkillType = SkillTypeEnum.SummonDriver, SkillCD = 5f, SkillDuration = 100f,
                SkillActivePoint = 3, MaxSaveSkillPoint = 3, GetSkillPoint = 1, GetSkillPointType = GetSkillPointType.ContinueGetModePoint, ShowName = "坦克"
            },
            new Data {
                ID = Skill_SummonDriverHelicopter, ItemId = ItemSet.Id_Helicopter ,Sign = GPOM_HelicopterSet.Sign_Helicopter, SkillType = SkillTypeEnum.SummonDriver, SkillCD = 5f, SkillDuration = 40f,
                SkillActivePoint = 3, MaxSaveSkillPoint = 3, GetSkillPoint = 1, GetSkillPointType = GetSkillPointType.ContinueGetModePoint, ShowName = "直升机"
            },
            new Data {
                ID = Skill_Missile, ItemId = ItemSet.Id_Missile , Sign = "MissileBeacon", SkillType = SkillTypeEnum.HoldOn, SkillCD = 1f, SkillDuration = float.MaxValue,
                SkillActivePoint = 3, MaxSaveSkillPoint = 3, GetSkillPoint = 1, GetSkillPointType = GetSkillPointType.ContinueGetModePoint, ShowName = "空袭导弹",IsCanCancel = true
            },
            new Data {
                ID = Skill_UAV, ItemId = ItemSet.Id_Uav , Sign = GPOM_UavSet.Sign_Uav, SkillType = SkillTypeEnum.CallMonster, SkillCD = 1f, SkillDuration = 40f,
                SkillActivePoint = 3, MaxSaveSkillPoint = 3, GetSkillPoint = 1, GetSkillPointType = GetSkillPointType.ContinueGetModePoint, ShowName = "无人机",SummonNum = 2
            },
            new Data {
                ID = Skill_MachineGun, ItemId = ItemSet.Id_MachineGun ,Sign = GPOM_MachineGunSet.Sign_MachineGun, SkillType = SkillTypeEnum.CallMonster, SkillCD = 5f, SkillDuration = 40f,
                SkillActivePoint = 3, MaxSaveSkillPoint = 3, GetSkillPoint = 1, GetSkillPointType = GetSkillPointType.ContinueGetModePoint, ShowName = "重型防御机枪",SummonNum = 1,NeedArea = new SkillNeedArea{width = 1.6f,height = 2}//IsCanCancel = true
            },
        };

        private static List<HoldOnData> HoldOnDataList = new List<HoldOnData> {
            new HoldOnData { skillId = Skill_Missile, abilityConfigId = AbilityConfig.Missile, maxDistance = 50, targetArea = "fx_missile_aim", progressTip = "空袭持续期间，所有击杀不计入连杀"}
        };
        private static List<CallAIData> CallMonsterDataList = new List<CallAIData> {
            new CallAIData { skillId = Skill_UAV, abilityConfigId = AbilityConfig.UAVTrackingMissle, progressTip = "导弹无人机持续期间，所有击杀不计入连杀"},
            new CallAIData { skillId = Skill_MachineGun, abilityConfigId = AbilityConfig.UAVTrackingMissle, progressTip = "重型机枪持续期间，所有击杀不计入连杀"}
        };

        private static List<int> DisableUseInRoomList = new List<int> {
            Skill_SummonDriverTank, Skill_SummonDriverHelicopter,Skill_MachineGun
        };

        public static Data GetData(int id) {
            var length = SkillDataList.Count;
            for (var i = 0; i < length; i++) {
                if (SkillDataList[i].ID == id) {
                    return SkillDataList[i];
                }
            }
            Debug.LogError("缺少技能数据 ID:" + id);
            return null;
        }
        
        public static Data GetDataForItemId(int itemId) {
            var length = SkillDataList.Count;
            for (var i = 0; i < length; i++) {
                if (SkillDataList[i].ItemId == itemId) {
                    return SkillDataList[i];
                }
            }
            Debug.LogError("缺少技能数据 itemId:" + itemId);
            return null;
        }
        public static CallAIData GetCallMonsterData(int skillId) {
            foreach (var callMonsterData in CallMonsterDataList) {
                if (callMonsterData.skillId == skillId) {
                    return callMonsterData;
                }
            }
            Debug.LogError("缺少技能数据 skillId:" + skillId);
            return null;
        }
        public static HoldOnData GetHoldOnData(int skillId) {
            foreach (var holdOnData in HoldOnDataList) {
                if (holdOnData.skillId == skillId) {
                    return holdOnData;
                }
            }
            Debug.LogError("缺少技能数据 skillId:" + skillId);
            return null;
        }

        public static IAbilityMData GetHoldOnConfigId(int itemId) {
            var skillData = GetDataForItemId(itemId);
            var holdOnData = GetHoldOnData(skillData.ID);
            return AbilityConfig.GetAbilityModData(holdOnData.abilityConfigId);
        }

        public static bool IsDisableUseInRoom(int skillId) {
            return DisableUseInRoomList.Contains(skillId);
        }
    }
}