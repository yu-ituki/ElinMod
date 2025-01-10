using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.Events;

namespace Elin_ModTemplate
{
	// プレイヤーをホイールクリックしたときのコンフィグUIサンプル.


#if false
	[HarmonyPatch]
	public class ModConfigUi
	{
		[HarmonyPatch(typeof(ActPlan), "ShowContextMenu")]
		[HarmonyPrefix]
		public static void Prefix(ActPlan actPlan) {
			// 自身をホイールクリックしているか否か.
			if (actPlan.pos != EClass.pc.pos) {
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
			actPlan.list.Add(new ActPlan.Item { act = act });
		}

		[HarmonyPatch(typeof(ActPlan.Item), "Perform")]
		[HarmonyPrefix]
		public static bool Prefix(ActPlan.Item planItem) {
			Act act = planItem.act;
			
			DynamicAct val = (DynamicAct)(object)((act is DynamicAct) ? act : null);
			if (val != null && val.id == ModTextManager.Instance.GetText( eTextID.ConfigTitle )) {
				val.Perform();
				return false;
			}
			return true;
		}
	}
#endif
}
