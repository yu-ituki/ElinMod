using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	public class RangedModData_Elements
	{
		public const int c_BaseElementID = 1000000;

		public int eleID;
		public int sourceEleID;
		

		public static readonly RangedModData_Elements[] c_Datas = new RangedModData_Elements[] {
			new RangedModData_Elements(){ eleID = 1000910, sourceEleID = 910},
			new RangedModData_Elements(){ eleID = 1000911, sourceEleID = 911},
			new RangedModData_Elements(){ eleID = 1000912, sourceEleID = 912},
			new RangedModData_Elements(){ eleID = 1000913, sourceEleID = 913},
			new RangedModData_Elements(){ eleID = 1000914, sourceEleID = 914},
			new RangedModData_Elements(){ eleID = 1000915, sourceEleID = 915},
			new RangedModData_Elements(){ eleID = 1000916, sourceEleID = 916},
			new RangedModData_Elements(){ eleID = 1000917, sourceEleID = 917},
			new RangedModData_Elements(){ eleID = 1000918, sourceEleID = 918},
			new RangedModData_Elements(){ eleID = 1000919, sourceEleID = 919},
			new RangedModData_Elements(){ eleID = 1000920, sourceEleID = 920},
			new RangedModData_Elements(){ eleID = 1000921, sourceEleID = 921},
			new RangedModData_Elements(){ eleID = 1000922, sourceEleID = 922},
			new RangedModData_Elements(){ eleID = 1000923, sourceEleID = 923},
			new RangedModData_Elements(){ eleID = 1000924, sourceEleID = 924},
			new RangedModData_Elements(){ eleID = 1000925, sourceEleID = 925},
		};
	}

	[HarmonyPatch]
	class NewRangedMod_Elements
	{
		/// <summary>
		/// CardのDamageHPをハック.
		/// </summary>
		/// <param name="__instance"></param>
		/// <param name="__result"></param>
		[HarmonyPatch(typeof(Card), "DamageHP", 
			new System.Type[] { typeof(int), typeof(int), typeof(int), typeof(AttackSource), typeof(Card), typeof(bool) })]
		[HarmonyPrefix]
		public static bool Prefix(Card __instance, int dmg, ref int ele, int eleP = 100, AttackSource attackSource = AttackSource.None, Card origin = null, bool showEffect = true) {

			DebugUtil.LogWarning($"!!!!!! --> {dmg} : {ele} : {eleP} : {attackSource} : {showEffect}" );

			// ダメージ計算時に属性IDだけ変換して.
			// あとは元の処理にバイパスしてやる.
			if ( ele >= RangedModData_Elements.c_BaseElementID ) {
				var datas = RangedModData_Elements.c_Datas;
				for (int i = 0; i < datas.Length; ++i) {
					if (datas[i].eleID != ele)
						continue;
					ele = datas[i].sourceEleID;
					break;
				}
			}

			//	__instance.DamageHP(dmg, ele, eleP, attackSource, origin, showEffect);
			//	return false; //< こっちで呼び出しているのでfalseで終了.
			return true;
		}
	}
}
