using System;
using System.Collections.Generic;
using HarmonyLib;
using ReflexCLI;

using UnityEngine.Events;

namespace Elin_Mod
{
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
				UIContextMenu val2 = EClass.ui.CreateContextMenu("ContextMenu");
				val2.AddButton(() => textMng.GetText(eTextID.Config_UnInstall), () => {

					GameUtil.OpenDialog_YesNo(
						textMng.GetText(eTextID.Config_UnInstallMain),
						textMng.GetText(eTextID.Yes),
						textMng.GetText(eTextID.No),
						(v) => {
							if (v)
								Uninstall();
						});

				});
				val2.Show();
				return false;
			}, false);
			((List<ActPlan.Item>)(object)__instance.list).Add(new ActPlan.Item {
				act = (Act)(object)act
			});
		}

		static void Uninstall() {
			Plugin.Instance.Uninstall();
			EClass.game.Save(true);

			var textMng = ModTextManager.Instance;
			GameUtil.OpenDialog_1Button(textMng.GetText(eTextID.Config_UnInstallEnd), textMng.GetText(eTextID.Yes), () => {
				EClass.game.GotoTitle(false);
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
}
