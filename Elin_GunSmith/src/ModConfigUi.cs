using System;
using System.Collections.Generic;
using HarmonyLib;
using ReflexCLI;

using UnityEngine.Events;

namespace Elin_Mod
{
#if true
	[HarmonyPatch]
	public class ModConfigUi
	{
		[HarmonyPatch(typeof(ActPlan), "ShowContextMenu")]
		[HarmonyPrefix]
		public static void Prefix(ActPlan __instance) {

			if (!__instance.pos.Equals(EClass.pc.pos)) {
				return;
			}
			var textMng = ModTextManager.Instance;
			var config = Plugin.Instance.ModConfig;
			DynamicAct act = new DynamicAct(textMng.GetText(eTextID.Config_Title), ()=> {
				var val2 = GameUtil.CreateContextMenu();
				GameUtil.ContextMenu_AddSlider(val2, eTextID.Config_CostAddSocket, config.AddSlotCost, 0, 500.0f, 1.0f);
				GameUtil.ContextMenu_AddSlider(val2, eTextID.Config_CostPowerUpMod, config.PowerUpModCost, 0, 10.0f, 0.1f);
				GameUtil.ContextMenu_AddSlider(val2, eTextID.Config_CostCombineMod, config.CombineModCost, 0, 10.0f, 0.1f);
				val2.Show();
				return false;
			}, false);
			((List<ActPlan.Item>)(object)__instance.list).Add(new ActPlan.Item {
				act = (Act)(object)act
			});
		}

	
		[HarmonyPatch(typeof(ActPlan.Item), "Perform")]
		[HarmonyPrefix]
		public static bool Prefix(ActPlan.Item __instance) {
			var val = __instance.act as DynamicAct;
			if (val != null && val.id ==ModTextManager.Instance.GetText(eTextID.Config_Title)) {
				((Act)val).Perform();
				return false;
			}
			return true;
		}
	}
#endif
}
