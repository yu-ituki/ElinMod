using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	[HarmonyPatch]
	class NewRangedMod_Elements : NewRangedModBase
	{
		public class Data : BaseData
		{
			public int sourceEleID;

			public static readonly Data[] c_Datas = new Data[] {
				new Data(){ alias = "itukiyu_modEX_eleFire",  sourceEleID = 910},
				new Data(){ alias = "itukiyu_modEX_eleCold",  sourceEleID = 911},
				new Data(){ alias = "itukiyu_modEX_eleLightning",  sourceEleID = 912},
				new Data(){ alias = "itukiyu_modEX_eleDarkness",  sourceEleID = 913},
				new Data(){ alias = "itukiyu_modEX_eleMind",  sourceEleID = 914},
				new Data(){ alias = "itukiyu_modEX_elePoison",  sourceEleID = 915},
				new Data(){ alias = "itukiyu_modEX_eleNether",  sourceEleID = 916},
				new Data(){ alias = "itukiyu_modEX_eleSound",  sourceEleID = 917},
				new Data(){ alias = "itukiyu_modEX_eleNerve",  sourceEleID = 918},
				new Data(){ alias = "itukiyu_modEX_eleHoly",  sourceEleID = 919},
				new Data(){ alias = "itukiyu_modEX_eleChaos",  sourceEleID = 920},
				new Data(){ alias = "itukiyu_modEX_eleMagic",  sourceEleID = 921},
				new Data(){ alias = "itukiyu_modEX_eleEther",  sourceEleID = 922},
				new Data(){ alias = "itukiyu_modEX_eleAcid",  sourceEleID = 923},
				new Data(){ alias = "itukiyu_modEX_eleCut",  sourceEleID = 924},
				new Data(){ alias = "itukiyu_modEX_eleImpact",  sourceEleID = 925},
			};

		}


		public override void Initialize() {
			base.Initialize();
			for (int i = 0; i < Data.c_Datas.Length; ++i) {
				Data.c_Datas[i].Load();
			}
		}

		public int GetElementModDataIndex( int id ) {
			for (int i = 0; i < Data.c_Datas.Length; ++i) {
				if (Data.c_Datas[i].id != id)
					continue;
				return i;
			}
			return -1;
		}


		/// <summary>
		/// CardのDamageHPをハック.
		/// </summary>
		/// <param name="__instance"></param>
		/// <param name="__result"></param>
		[HarmonyPatch(typeof(Card), "DamageHP", 
			new System.Type[] { typeof(int), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool) })]
		[HarmonyPrefix]
		public static bool Prefix(Card __instance, int dmg, ref int ele, int eleP = 100, AttackSource attackSource = AttackSource.None, Card origin = null, bool showEffect = true) {

			// ダメージ計算時に属性IDだけ変換して.
			// あとは元の処理にバイパスしてやる.
			// そのためにeleをrefにしておく.
			if ( NewRangedModManager.Instance.IsNewRangeModIDBand(ele)) {
				var datas = Data.c_Datas;
				for (int i = 0; i < datas.Length; ++i) {
					if (datas[i].id != ele)
						continue;
					dmg = Mathf.CeilToInt((float)dmg * Plugin.Instance.ModConfig.ModElement_DmgFactor.Value);
					ele = datas[i].sourceEleID;
					break;
				}
			}
			return true;
		}
	}
}
