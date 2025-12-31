using System.Collections;
using System.Collections.Generic;
using Sofunny.BiuBiuBiu2.Data;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Data {
    public partial class AbilityM_BossFightMusic : AbilityMData {
        public string M_Sign;
        public byte M_BossIn;
        public ushort M_BossInTime;
        public byte[] M_BossFight;
        public ushort[] M_BossFightTime;
        public ushort M_BossFightSpaceTime;
        public byte M_BossFightRandom;
        public byte M_BossOut;
        public ushort M_BossOutTime;
        public byte M_BossDead;
        public ushort M_BossDeadTime;
    }

    public class AbilityIn_BossFightMusic : IAbilityInData {
    }
}
