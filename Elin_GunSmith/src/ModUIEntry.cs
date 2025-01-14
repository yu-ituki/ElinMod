using System;
using System.Collections.Generic;

using HarmonyLib;

using UnityEngine.Events;

namespace Elin_Mod
{

#if true
	[HarmonyPatch]
	public class ModUIEntry
	{
		static eTextID[] c_MenuTextIDs;

		public static void Initialize() {
			if (c_MenuTextIDs == null) {
				c_MenuTextIDs = new eTextID[] {
					eTextID.MenuTitle_GunSmith,
				};
			}
		}

		[HarmonyPatch(typeof(ActPlan), "ShowContextMenu")]
		[HarmonyPrefix]
		public static void Prefix(ActPlan __instance) {
			// 銃身加工機のホイールクリックチェック.
			var cellThins = __instance.pos?.cell?.Things;
			if (cellThins == null)
				return;
			var barrelFactory = cellThins.Find(v => v.trait != null && v.id == Const.c_TargetToolName);
			if (barrelFactory == null)
				return;
			
			var textMng = ModTextManager.Instance;

			for (int i = 0; i < c_MenuTextIDs.Length; ++i) {
				int menuIndex = i;
				DynamicAct act = new DynamicAct(textMng.GetText(c_MenuTextIDs[i]), () => {
					switch (c_MenuTextIDs[menuIndex] ) {
						case eTextID.MenuTitle_GunSmith:
							GunSmithManager.Instance.Play_GunSmith();
							break;
					}
					return false;
				}, false);
				__instance.list.Add(new ActPlan.Item { act = act });
			}
		}

		[HarmonyPatch(typeof(ActPlan.Item), "Perform")]
		[HarmonyPrefix]
		public static bool Prefix(ActPlan.Item __instance) {
			Act act = __instance.act;

			DynamicAct val = act as DynamicAct;
			if (val == null)
				return true;
			var textMng = ModTextManager.Instance;
			for (int i = 0; i < c_MenuTextIDs.Length; ++i) {
				if (val.id != textMng.GetText(c_MenuTextIDs[i]))
					continue;
				val.Perform();
				return false;
			}
			return true;
		}
	}
#endif
}
