// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sofunny.BiuBiuBiu2.Template
{
    /// <summary>
    /// 2代WWise 音频
    /// </summary>
	public struct WwiseAudio
	{
		/// <summary>
		/// 主键id
		/// </summary>
		public readonly ushort Id { get; }
		/// <summary>
		/// 标识
		/// </summary>
		public readonly string Sign { get; }
		/// <summary>
		/// wwise事件
		/// </summary>
		public readonly string WwiseEvent { get; }
		/// <summary>
		/// 音频类型
		/// </summary>
		public readonly sbyte AudioType { get; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sign"></param>
        /// <param name="wwiseEvent"></param>
        /// <param name="audioType"></param> )
		public WwiseAudio( ushort id, string sign, string wwiseEvent, sbyte audioType )
		{
			Id = id;
			Sign = sign;
			WwiseEvent = wwiseEvent;
			AudioType = audioType;
		}
	}

    /// <summary>
    /// WwiseAudioSet that holds all the table data
    /// </summary>
    public partial class WwiseAudioSet
    {
        public static readonly WwiseAudio[] Data;
		/// <summary>
		/// 常量类型
		/// </summary>
		
		
		public const ushort Id_GoldDashBossSkill1AttackAir = 1;
		public const ushort Id_GoldDashBossSkill2Keyboard = 2;
		public const ushort Id_GoldDashBossSkill2BugFall = 3;
		public const ushort Id_GoldDashBossSkill2BugImpact = 4;
		public const ushort Id_GoldDashBossSkill3RockRoll = 5;
		public const ushort Id_GoldDashBossSkill4HammerHit = 6;
		public const ushort Id_GoldDashBossSkill4ElectWave = 7;
		public const ushort Id_GoldDashBossSkill5AttackElectStart = 8;
		public const ushort Id_GoldDashBossAct02HammerHit = 9;
		public const ushort Id_GoldDashBossFightDefeat = 10;
		public const ushort Id_GoldDashBossFightMusicEndingTrigger = 11;
		public const ushort Id_GoldDashBossFightRtpcMusicBossFightStage = 12;
		public const ushort Id_GoldDashBossAdragonTreadCom = 13;
		public const ushort Id_GoldDashBossAdragonDelayBlastLv1 = 14;
		public const ushort Id_GoldDashBossAdragonDelayBlastLv2 = 15;
		public const ushort Id_GoldDashBossAdragonAoe = 16;
		public const ushort Id_GoldDashBossAdragonTread01 = 17;
		public const ushort Id_GoldDashBossAdragonFireCom = 18;
		public const ushort Id_GoldDashBossAdragonEruption = 19;
		public const ushort Id_GoldDashBossAdragonSkill3Cast = 20;
		public const ushort Id_GoldDashBossAdragonFlyStrike = 21;
		public const ushort Id_GoldDashBossMusicAuroraDragon = 22;
		public const ushort Id_GoldDashBossMusicAncientDragon = 23;
		public const ushort Id_GoldDashBossAdragonActIn = 24;
		public const ushort Id_GoldDashBossAdragonActOut = 25;
		public const ushort Id_GoldDashBossAdragonSkill1Attack = 26;
		public const ushort Id_GoldDashBossAdragonSkill2Charge = 27;
		public const ushort Id_GoldDashBossAdragonSkill3Fly = 28;
		public const ushort Id_GoldDashBossAdragonSkill3Swing = 29;
		public const ushort Id_GoldDashBossAdragonSkill3Explosion = 30;
		public const ushort Id_GoldDashBossAdragonSkill4Fly = 31;
		public const ushort Id_GoldDashBossAdragonSkill4SprintUp = 32;
		public const ushort Id_GoldDashBossAdragonSkill4Fall = 33;
		public const ushort Id_GoldDashBossAdragonSkill4FlyFireStart = 34;
		public const ushort Id_GoldDashJorkerDroneFlyLoop = 35;
		public const ushort Id_GoldDashBossjorkerGunAim = 36;
		public const ushort Id_GoldDashBossjorkerFlyKnifeStart = 37;
		public const ushort Id_GoldDashJorkerDroneSpades = 38;
		public const ushort Id_GoldDashBossjorkerActOut = 39;
		public const ushort Id_GoldDashBossjorkerClownDollStart = 40;
		public const ushort Id_GoldDashBossjorkerClownDollBomb = 41;
		public const ushort Id_GoldDashSilverJorkerEntranceVoice = 42;
		public const ushort Id_GoldDashSilverJorkerBattleVoice = 43;
		public const ushort Id_GoldDashSilverJorkerResetVoice = 44;
		public const ushort Id_GoldDashSilverJorkerEliminatedVoice = 45;
		public const ushort Id_GoldDashGoldenJorkerEntranceVoice = 46;
		public const ushort Id_GoldDashGoldenJorkerBattleVoice = 47;
		public const ushort Id_GoldDashGoldenJorkerResetVoice = 48;
		public const ushort Id_GoldDashGoldenJorkerEliminatedVoice = 49;
		public const ushort Id_GoldDashBossjorkerVoice02 = 50;
		public const ushort Id_GoldDashBossjorkerVoice03 = 51;
		public const ushort Id_GoldDashBossjorkerVoice04 = 52;
		public const ushort Id_GoldDashBossjorkerVoice08 = 53;
		public const ushort Id_GoldDashBossjorkerVoice09 = 54;
		public const ushort Id_GoldDashBossjorkerVoice10 = 55;
		public const ushort Id_GoldDashBossjorkerVoice11 = 56;
		public const ushort Id_GoldDashJorkerDroneSpadesFire = 57;
		public const ushort Id_BeastcampAiAdcGuard3P = 58;
		public const ushort Id_BeastcampAiTankGuard3P = 59;
		public const ushort Id_BeastcampAiGoblinGuard3P = 60;
		public const ushort Id_BeastcampAiMobGuard3P = 61;
		public const ushort Id_BatAttack3P = 62;
		public const ushort Id_BeastcampAiTankAttack3P = 63;
		public const ushort Id_PlungerAttack3P = 64;
		public const ushort Id_BeastcampAiTankShieldHit3P = 65;
		public const ushort Id_GoldDashAiAttackHint = 66;
		public const ushort Id_GoldDashAiFightHint = 69;

        /// <summary>
        /// 构造函数
        /// </summary>
        static WwiseAudioSet()
        {
            Data = new WwiseAudio[]
            {
					 new WwiseAudio( 1, "GoldDash_Boss_Skill1_Attack_Air", "GoldDash_Boss_Skill1_Attack_Air", 1 )
					, new WwiseAudio( 2, "GoldDash_Boss_Skill2_Keyboard", "GoldDash_Boss_Skill2_Keyboard", 1 )
					, new WwiseAudio( 3, "GoldDash_Boss_Skill2_BUG_Fall", "GoldDash_Boss_Skill2_BUG_Fall", 1 )
					, new WwiseAudio( 4, "GoldDash_Boss_Skill2_BUG_Impact", "GoldDash_Boss_Skill2_BUG_Impact", 1 )
					, new WwiseAudio( 5, "GoldDash_Boss_Skill3_Rock_Roll", "GoldDash_Boss_Skill3_Rock_Roll", 1 )
					, new WwiseAudio( 6, "GoldDash_Boss_Skill4_Hammer_Hit", "GoldDash_Boss_Skill4_Hammer_Hit", 1 )
					, new WwiseAudio( 7, "GoldDash_Boss_Skill4_Elect_Wave", "GoldDash_Boss_Skill4_Elect_Wave", 1 )
					, new WwiseAudio( 8, "GoldDash_Boss_Skill5_Attack_Elect_Start", "GoldDash_Boss_Skill5_Attack_Elect_Start", 1 )
					, new WwiseAudio( 9, "GoldDash_Boss_Act_02_Hammer_Hit", "GoldDash_Boss_Act_02_Hammer_Hit", 1 )
					, new WwiseAudio( 10, "GoldDash_BossFight_Defeat", "GoldDash_Boss_Defeat", 1 )
					, new WwiseAudio( 11, "GoldDash_BossFight_MusicEnding_Trigger", "MusicEnding", 1 )
					, new WwiseAudio( 12, "GoldDash_BossFight_RTPC_Music_BossFight_Stage", "RTPC_Music_BossFight_Stage", 1 )
					, new WwiseAudio( 13, "GoldDash_BOSS_ADragon_TreadCom", "GoldDash_BOSS_ADragon_TreadCom", 1 )
					, new WwiseAudio( 14, "GoldDash_BOSS_ADragon_DelayBlastLv1", "GoldDash_BOSS_ADragon_DelayBlastLv1", 1 )
					, new WwiseAudio( 15, "GoldDash_BOSS_ADragon_DelayBlastLv2", "GoldDash_BOSS_ADragon_DelayBlastLv2", 1 )
					, new WwiseAudio( 16, "GoldDash_BOSS_ADragon_AOE", "GoldDash_BOSS_ADragon_AOE", 1 )
					, new WwiseAudio( 17, "GoldDash_BOSS_ADragon_Tread_01", "GoldDash_BOSS_ADragon_Tread_01", 1 )
					, new WwiseAudio( 18, "GoldDash_BOSS_ADragon_FireCom", "GoldDash_BOSS_ADragon_FireCom", 1 )
					, new WwiseAudio( 19, "GoldDash_BOSS_ADragon_Eruption", "GoldDash_BOSS_ADragon_Eruption", 1 )
					, new WwiseAudio( 20, "GoldDash_BOSS_ADragon_Skill3_Cast", "GoldDash_BOSS_ADragon_Skill3_Cast", 1 )
					, new WwiseAudio( 21, "GoldDash_BOSS_ADragon_FlyStrike", "GoldDash_BOSS_ADragon_FlyStrike", 1 )
					, new WwiseAudio( 22, "GoldDash_BOSS_Music_AuroraDragon", "GoldDash_Boss_Music_AuroraDragon", 1 )
					, new WwiseAudio( 23, "GoldDash_BOSS_Music_AncientDragon", "GoldDash_Boss_Music_AncientDragon", 1 )
					, new WwiseAudio( 24, "GoldDash_BOSS_ADragon_ActIn", "GoldDash_BOSS_ADragon_ActIn", 1 )
					, new WwiseAudio( 25, "GoldDash_BOSS_ADragon_ActOut", "GoldDash_BOSS_ADragon_ActOut", 1 )
					, new WwiseAudio( 26, "GoldDash_BOSS_ADragon_Skill1_Attack", "GoldDash_BOSS_ADragon_Skill1_Attack_3P", 1 )
					, new WwiseAudio( 27, "GoldDash_BOSS_ADragon_Skill2_Charge", "GoldDash_BOSS_ADragon_Skill2_Charge_3P", 1 )
					, new WwiseAudio( 28, "GoldDash_BOSS_ADragon_Skill3_Fly", "GoldDash_BOSS_ADragon_Skill3_Fly_3P", 1 )
					, new WwiseAudio( 29, "GoldDash_BOSS_ADragon_Skill3_Swing", "GoldDash_BOSS_ADragon_Skill3_Swing_3P", 1 )
					, new WwiseAudio( 30, "GoldDash_BOSS_ADragon_Skill3_Explosion", "GoldDash_BOSS_ADragon_Skill3_Explosion_3P", 1 )
					, new WwiseAudio( 31, "GoldDash_BOSS_ADragon_Skill4_Fly", "GoldDash_BOSS_ADragon_Skill4_Fly_3P", 1 )
					, new WwiseAudio( 32, "GoldDash_BOSS_ADragon_Skill4_Sprint_Up", "GoldDash_BOSS_ADragon_Skill4_Sprint_Up_3P", 1 )
					, new WwiseAudio( 33, "GoldDash_BOSS_ADragon_Skill4_Fall", "GoldDash_BOSS_ADragon_Skill4_Fall_3P", 1 )
					, new WwiseAudio( 34, "GoldDash_BOSS_ADragon_Skill4_FlyFireStart", "GoldDash_BOSS_ADragon_Skill4_FlyFireStart", 1 )
					, new WwiseAudio( 35, "GoldDash_JorkerDrone_FlyLoop", "GoldDash_JorkerDrone_FlyLoop", 1 )
					, new WwiseAudio( 36, "GoldDash_BOSSJorker_Gun_Aim", "GoldDash_BOSSJorker_Gun_Aim", 1 )
					, new WwiseAudio( 37, "GoldDash_BOSSJorker_FlyKnife_Start", "GoldDash_BOSSJorker_FlyKnife_Start", 1 )
					, new WwiseAudio( 38, "GoldDash_JorkerDrone_Spades", "GoldDash_JorkerDrone_Spades", 1 )
					, new WwiseAudio( 39, "GoldDash_BOSSJorker_ActOut", "GoldDash_BOSSJorker_ActOut", 1 )
					, new WwiseAudio( 40, "GoldDash_BOSSJorker_ClownDoll_Start", "GoldDash_BOSSJorker_ClownDoll_Start", 1 )
					, new WwiseAudio( 41, "GoldDash_BOSSJorker_ClownDoll_Bomb", "GoldDash_BOSSJorker_ClownDoll_Bomb", 1 )
					, new WwiseAudio( 42, "GoldDash_SilverJorker_Entrance_Voice", "GoldDash_SilverJorker_Entrance_Voice", 1 )
					, new WwiseAudio( 43, "GoldDash_SilverJorker_Battle_Voice", "GoldDash_SilverJorker_Battle_Voice", 1 )
					, new WwiseAudio( 44, "GoldDash_SilverJorker_Reset_Voice", "GoldDash_SilverJorker_Reset_Voice", 1 )
					, new WwiseAudio( 45, "GoldDash_SilverJorker_Eliminated_Voice", "GoldDash_SilverJorker_Eliminated_Voice", 1 )
					, new WwiseAudio( 46, "GoldDash_GoldenJorker_Entrance_Voice", "GoldDash_GoldenJorker_Entrance_Voice", 1 )
					, new WwiseAudio( 47, "GoldDash_GoldenJorker_Battle_Voice", "GoldDash_GoldenJorker_Battle_Voice", 1 )
					, new WwiseAudio( 48, "GoldDash_GoldenJorker_Reset_Voice", "GoldDash_GoldenJorker_Reset_Voice", 1 )
					, new WwiseAudio( 49, "GoldDash_GoldenJorker_Eliminated_Voice", "GoldDash_GoldenJorker_Eliminated_Voice", 1 )
					, new WwiseAudio( 50, "GoldDash_BOSSJorker_Voice_02", "GoldDash_BOSSJorker_Voice_02", 1 )
					, new WwiseAudio( 51, "GoldDash_BOSSJorker_Voice_03", "GoldDash_BOSSJorker_Voice_03", 1 )
					, new WwiseAudio( 52, "GoldDash_BOSSJorker_Voice_04", "GoldDash_BOSSJorker_Voice_04", 1 )
					, new WwiseAudio( 53, "GoldDash_BOSSJorker_Voice_08", "GoldDash_BOSSJorker_Voice_08", 1 )
					, new WwiseAudio( 54, "GoldDash_BOSSJorker_Voice_09", "GoldDash_BOSSJorker_Voice_09", 1 )
					, new WwiseAudio( 55, "GoldDash_BOSSJorker_Voice_10", "GoldDash_BOSSJorker_Voice_10", 1 )
					, new WwiseAudio( 56, "GoldDash_BOSSJorker_Voice_11", "GoldDash_BOSSJorker_Voice_11", 1 )
					, new WwiseAudio( 57, "GoldDash_JorkerDrone_Spades_Fire", "GoldDash_JorkerDrone_Spades_Fire", 1 )
					, new WwiseAudio( 58, "Beastcamp_AI_ADC_Guard_3P", "Beastcamp_AI_ADC_Guard_3P", 1 )
					, new WwiseAudio( 59, "Beastcamp_AI_Tank_Guard_3P", "Beastcamp_AI_Tank_Guard_3P", 1 )
					, new WwiseAudio( 60, "Beastcamp_AI_Goblin_Guard_3P", "Beastcamp_AI_Goblin_Guard_3P", 1 )
					, new WwiseAudio( 61, "Beastcamp_AI_Mob_Guard_3P", "Beastcamp_AI_Mob_Guard_3P", 1 )
					, new WwiseAudio( 62, "Bat_Attack_3P", "Bat_Attack_3P", 1 )
					, new WwiseAudio( 63, "Beastcamp_AI_Tank_Attack_3P", "Beastcamp_AI_Tank_Attack_3P", 1 )
					, new WwiseAudio( 64, "Plunger_Attack_3P", "Plunger_Attack_3P", 1 )
					, new WwiseAudio( 65, "Beastcamp_AI_Tank_Shield_Hit_3P", "Beastcamp_AI_Tank_Shield_Hit_3P", 1 )
					, new WwiseAudio( 66, "GoldDash_AI_Attack_Hint", "GoldDash_AI_Attack_Hint", 1 )
					, new WwiseAudio( 69, "GoldDash_AI_Fight_Hint", "GoldDash_AI_Fight_Hint", 1 )
            };
        }
		/// <summary>
		/// 根据指定条件获取单个 WwiseAudio
		/// </summary>
		/// <param name="Sign"></param>
		public static WwiseAudio GetWwiseAudioBySign( string sign )
		{
			foreach (WwiseAudio data in Data)
			{
				if ( data.Sign == sign )
				{
					return data;
				}
			}
			return default(WwiseAudio);
		}

		/// <summary>
		/// 根据指定条件判断单个 WwiseAudio 是否存在
		/// </summary>
		/// <param name="Sign"></param>
		public static bool HasWwiseAudioBySign( string sign )
		{
			foreach (WwiseAudio data in Data)
			{
				if ( data.Sign == sign )
				{
					return true;
				}
			}
			return false;
		}
		/// <summary>
		/// 根据指定条件获取单个 WwiseAudio
		/// </summary>
		/// <param name="Id"></param>
		public static WwiseAudio GetWwiseAudioById( ushort id )
		{
			foreach (WwiseAudio data in Data)
			{
				if ( data.Id == id )
				{
					return data;
				}
			}
			return default(WwiseAudio);
		}

		/// <summary>
		/// 根据指定条件判断单个 WwiseAudio 是否存在
		/// </summary>
		/// <param name="Id"></param>
		public static bool HasWwiseAudioById( ushort id )
		{
			foreach (WwiseAudio data in Data)
			{
				if ( data.Id == id )
				{
					return true;
				}
			}
			return false;
		}
    }
}
