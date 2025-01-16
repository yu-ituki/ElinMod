using System;
using System.Collections.Generic;
using HarmonyLib;
using ReflexCLI;

using UnityEngine.Events;

namespace Elin_Mod
{
	// プレイヤーをホイールクリックしたときのコンフィグUIサンプル.


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
				var val2 = EClass.ui.CreateContextMenu("ContextMenu");
				_AddSlider(val2, eTextID.Config_CostAddSocket, config.AddSlotCost, 0, 500.0f, true);
				_AddSlider(val2, eTextID.Config_CostPowerUpMod, config.PowerUpModCost, 0, 10.0f, false);
				_AddSlider(val2, eTextID.Config_CostCombineMod, config.CombineModCost, 0, 10.0f, false);
				val2.Show();
				return false;
			}, false);
			((List<ActPlan.Item>)(object)__instance.list).Add(new ActPlan.Item {
				act = (Act)(object)act
			});
		}

		static void _AddSlider( UIContextMenu menu, eTextID textID, BepInEx.Configuration.ConfigEntry<float> entry, float min, float max, bool isInt ) {
			var textMng = ModTextManager.Instance;
			menu.AddSlider(
					textMng.GetText(textID),
					(v) => v.ToString(),
					entry.Value,
					(v) => {
						entry.Value = v;
					},
				min, max, isInt, false, false);
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
