using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	[HarmonyPatch]
	class NewRangedMod_Barrel : NewRangedModBase
	{
		public class Data : BaseData
		{
			public int bestDist;
			
			public static readonly Data[] c_Datas = new Data[] {
				new Data(){ alias ="itukiyu_modEX_barrel_plus1", bestDist = 1},
				new Data(){ alias ="itukiyu_modEX_barrel_plus2", bestDist = 2},
				new Data(){ alias ="itukiyu_modEX_barrel_plus3", bestDist = 3},
				new Data(){ alias ="itukiyu_modEX_barrel_plus4", bestDist = 4},
				new Data(){ alias ="itukiyu_modEX_barrel_plus5", bestDist = 5},
				new Data(){ alias ="itukiyu_modEX_barrel_minus1", bestDist = -1},
				new Data(){ alias ="itukiyu_modEX_barrel_minus2", bestDist = -2},
				new Data(){ alias ="itukiyu_modEX_barrel_minus3", bestDist = -3},
				new Data(){ alias ="itukiyu_modEX_barrel_minus4", bestDist = -4},
				new Data(){ alias ="itukiyu_modEX_barrel_minus5", bestDist = -5},
			};
		}

		public override void Initialize() {
			base.Initialize();
			for ( int i = 0; i < Data.c_Datas.Length; ++i ) {
				Data.c_Datas[i].Load();
			}
		}


		/// <summary>
		/// TraittoolRangeのBestDistプロパティをフック.
		/// </summary>
		/// <param name="__instance"></param>
		/// <param name="__result"></param>
		[HarmonyPatch(typeof(TraitToolRange), "get_BestDist")]
		[HarmonyPostfix]
		public static void Postfix(TraitToolRange __instance, ref int __result) {
			// スコープは最適距離を変更する.
			var things = __instance.owner.Thing;
			if (things == null)
				return;
			var datas = Data.c_Datas;
			for (int i = 0; i < datas.Length; ++i) {
				var elem = things.elements.GetElement(datas[i].id);
				if (elem == null)
					continue;
				__result = Mathf.Max(__result + datas[i].bestDist, 0 );
			}
		}

		/// <summary>
		/// CardのEvalueをフック.
		/// </summary>
		/// <param name="__instance"></param>
		/// <param name="__result"></param>
		[HarmonyPatch(typeof(Card), "Evalue", new Type[] { typeof(int) })]
		[HarmonyPostfix]
		public static void Postfix(Card __instance, int ele, ref int __result) {
			// スコープを付けていたら少しだけ距離減衰補正を強化する...
			if (ele != 605)
				return;

			// 最も高レベルのものを選択.
			// TODO: バカスカ呼ばれる場所だからできればCardのどこかにキャッシュしたほうが良い...
			var datas = Data.c_Datas;
			var elemDict = __instance.elements.dict;
			int valMax = 0;
			for (int i = 0; i < datas.Length; ++i) {
				Element elem = null;
				if (!elemDict.TryGetValue(datas[i].id, out elem))
					continue;
				valMax = Mathf.Max(valMax, elem.Value);
			}

			if ( valMax > 0 ) {
				var addScopeValue = Mathf.CeilToInt((float)valMax * Plugin.Instance.ModConfig.ModBarrel_DistReductionFactor.Value);
				__result += addScopeValue;
			}
		}
	}
}
