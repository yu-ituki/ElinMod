using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	public class RangedModData_Scope
	{
		public int bestDist;
		public int eleID;

		public static readonly RangedModData_Scope[] c_Datas = new RangedModData_Scope[] {
			new RangedModData_Scope(){ eleID = 1000001, bestDist = 1},
			new RangedModData_Scope(){ eleID = 1000002, bestDist = 2},
			new RangedModData_Scope(){ eleID = 1000003, bestDist = 3},
			new RangedModData_Scope(){ eleID = 1000004, bestDist = 4},
			new RangedModData_Scope(){ eleID = 1000005, bestDist = 5},
			new RangedModData_Scope(){ eleID = 1000006, bestDist = 6},
		};
	}

	[HarmonyPatch]
	class NewRangedMod_Scope
	{
		/// <summary>
		/// TraittoolRangeのBestDistプロパティをハック.
		/// </summary>
		/// <param name="__instance"></param>
		/// <param name="__result"></param>
		[HarmonyPatch(typeof(TraitToolRange), "get_BestDist")]
		[HarmonyPostfix]
		public static void Postfix(TraitToolRange __instance, ref int __result) {
			// スコープ検索.
			var things = __instance.owner.Thing;
			if (things == null)
				return;
			var datas = RangedModData_Scope.c_Datas;
			for (int i = 0; i < datas.Length; ++i) {
				var elem = things.elements.GetElement(datas[i].eleID);
				if (elem == null)
					continue;
				__result = Mathf.Max(__result, datas[i].bestDist);
			}
		}
	}
}
