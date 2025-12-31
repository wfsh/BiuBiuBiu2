// Generated automatically by csv-gen: do not edit manually
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Sofunny.BiuBiuBiu2.Template {
    public interface IGPOM {
		/// <summary>
		/// GPO资产标识
		/// </summary>
		public string GetAssetSign();
		/// <summary>
		/// GPO掉落ID
		/// </summary>
		public int[] GetGpoDropId();
		/// <summary>
		/// GPO掉落类型
		/// </summary>
		public ushort GetGpoDropType();
		/// <summary>
		/// GPO类型
		/// </summary>
		public int GetGpoType();
		/// <summary>
		/// GPOID
		/// </summary>
		public int GetId();
		/// <summary>
		/// 匹配模式
		/// </summary>
		public int GetMatchMode();
		/// <summary>
		/// GPO名称
		/// </summary>
		public string GetName();
		/// <summary>
		/// GPO 品质
		/// </summary>
		public byte GetQuality();
		/// <summary>
		/// GPO唯一标识
		/// </summary>
		public string GetSign();
    }

	public partial  class GpoSet {
		public static IGPOM GetGPOMData(int gpoId, int matchMode = 0) {
			var data = GpoSet.GetGpoById(gpoId);
			if (data.Id == 0) {
				Debug.LogError("缺少 GPOM 数据 ID:" + gpoId);
				return null;
			}

			return GetGPOMData(data.GpoType, data.Id, matchMode);
		}

		public static IGPOM GetGPOMData(string gpoSign, int matchMode = 0) {
			var data = GpoSet.GetGpoBySign(gpoSign);
			if (data.Id == 0) {
				Debug.LogError("缺少 GPOM 数据 Sign:" + gpoSign);
				return null;
			}

			return GetGPOMData(data.GpoType, data.Id, matchMode);
		}
		
		public static IGPOM GetGPOMData(int gpoTypeId, int gpoId, int matchMode = 0) {
			IGPOM mData = null;
			switch (gpoTypeId) {
				
				case GpoTypeSet.Id_AceJoker:
					mData = GPOM_AceJokerSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_AuroraDragon:
					mData = GPOM_AuroraDragonSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_BlindingShield:
					mData = GPOM_BlindingShieldSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_Character:
					mData = GPOM_CharacterSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_FeiYu:
					mData = GPOM_FeiYuSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_Gpospawner:
					mData = GPOM_GpospawnerSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_GiantDaDa:
					mData = GPOM_GiantDaDaSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_GoldenEgg:
					mData = GPOM_GoldenEggSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_Helicopter:
					mData = GPOM_HelicopterSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_JokerUav:
					mData = GPOM_JokerUavSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_MachineGun:
					mData = GPOM_MachineGunSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_RexKing:
					mData = GPOM_RexKingSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_Sniper:
					mData = GPOM_SniperSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_SwordTiger:
					mData = GPOM_SwordTigerSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_Tank:
					mData = GPOM_TankSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_Uav:
					mData = GPOM_UavSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
				
				case GpoTypeSet.Id_WuGui:
					mData = GPOM_WuGuiSet.GetGPOMByIdAndMatchMode(gpoId, matchMode);
					break;
			}

			return mData;
		}
	}

}
