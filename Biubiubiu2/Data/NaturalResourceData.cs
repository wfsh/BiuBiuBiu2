using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public class NaturalResourceData {
        public static ushort Index = 1;
        public const string ExplosiveBulletGrass = "ExplosiveBulletGrass";
        public const string RPGBulletGrass = "RPGBulletGrass";
        public class Data {
            public ushort ID = Index++;
            public string Sign; 
            // 一次性可采集的次数
            public int GatheringLimit;
            // 一次采集可获得的数量
            public ushort HarvestNum;
            // 每次采集需要的时间
            public float HarvestTime;
            // 所有次数采集完后再生成的 CD 时间
            public float RespawnCooldown;
            // 可以采集的物品名称
            public string[] HarvestItems;
        }
        
        public static List<Data> Datas = new List<Data>() {
            new Data() {
                Sign = ExplosiveBulletGrass, GatheringLimit = 3, HarvestNum = 2, HarvestTime = 3f, RespawnCooldown = 30f, HarvestItems = new []{"ExplosiveBullet"},
            },
            new Data() {
                Sign = RPGBulletGrass, GatheringLimit = 3, HarvestNum = 2, HarvestTime = 3f, RespawnCooldown = 30f, HarvestItems = new []{"RPGBullet"},
            },
        };
        
        public static Data GetData(string sign) {
            for (int i = 0; i < Datas.Count; i++) {
                var data = Datas[i];
                if (data.Sign == sign) {
                    return data;
                }
            }
            Debug.LogError("缺少采集资源数据:" + sign);
            return null;
        }
        
        public static Data GetData(ushort id) {
            for (int i = 0; i < Datas.Count; i++) {
                var data = Datas[i];
                if (data.ID == id) {
                    return data;
                }
            }
            Debug.LogError("缺少采集资源数据:" + id);
            return null;
        }
    }
}
