using System;
using System.Collections.Generic;
using HarmonyLib;
using ReflexCLI;

using UnityEngine.Events;

namespace Elin_Mod
{
	// プレイヤーをホイールクリックしたときのコンフィグUIサンプル.


#if false
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
			DynamicAct act = new DynamicAct(textMng.GetText( eTextID.ConfigTitle ), ()=>{
				UIContextMenu val2 = EClass.ui.CreateContextMenu("ContextMenu");
				val2.AddToggle( textMng.GetText(eTextID.hoge ), config.Hoge.Value, (v) => {
					config.Hoge.Value = val;
				});
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
