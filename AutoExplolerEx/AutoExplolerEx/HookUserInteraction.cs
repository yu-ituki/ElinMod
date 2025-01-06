using System.Collections.Generic;
using HarmonyLib;

namespace Elin_AutoExplore
{

	[HarmonyPatch(typeof(AM_Adv), "TryCancelInteraction")]
	public static class HookUserInteraction
	{
		public static readonly List<AIAct> UserCanceledAiActs = new List<AIAct>();

		[HarmonyPostfix]
		public static void PostFix(AM_Adv __instance, bool __result) {
			if (__result) {
				UserCanceledAiActs.Add(EClass.pc.ai);
			}
		}
	}
}
