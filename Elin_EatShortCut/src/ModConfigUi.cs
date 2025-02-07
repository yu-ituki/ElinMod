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
		

			ModConfig config = Plugin.Instance.ModConfig;
			DynamicAct act = new DynamicAct(textMng.GetText( eTextID.Config_Title ), ()=>{
				var menu = GameUtil.CreateContextMenu();
				GameUtil.ContextMenu_AddEnumSlider(menu, eTextID.Config_StopEatState, config.StopEatState, Const.s_ConfigTexts_StopEatState);
				GameUtil.ContextMenu_AddEnumSlider(menu, eTextID.Config_EatPriority, config.EatPriority, Const.s_ConfigTexts_EatPriority);
				GameUtil.ContextMenu_AddToggle(menu, eTextID.Config_IsPreferredJustCooked, true, v => config.IsPreferredJustCooked.Value = v);
				GameUtil.ContextMenu_AddToggle(menu, eTextID.Config_IsInstantEat, true, v => config.IsInstantEat.Value = v);
				menu.Show();
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
