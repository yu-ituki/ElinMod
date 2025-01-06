using System;
using System.Collections.Generic;
using HarmonyLib;

namespace Elin_AutoExplore
{

	[HarmonyPatch]
	public class IgnoreListPatch
	{
		private static List<string> actNames = new List<string> { "Remove from Gathering Ignore List", "Add to Gathering Ignore List", "Remove from Mining Ignore List", "Add to Mining Ignore List" };

		[HarmonyPatch(typeof(ActPlan), "GetAction")]
		[HarmonyPrefix]
		public static void Prefix(ActPlan __instance) {
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Expected O, but got Unknown
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Expected O, but got Unknown
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Expected O, but got Unknown
			if (!EInput.isShiftDown) {
				return;
			}
			TaskHarvest lang = TaskHarvest.TryGetAct(ELayer.pc, __instance.pos);
			if (lang != null) {
				string targetName = (lang.IsObj ? ((SourceData.BaseRow)__instance.pos.cell.sourceObj).GetName() : ((Card)((BaseTaskHarvest)lang).target).Name);
				bool ignored = Plugin.Instance.IgnoreList.IsIgnoredFromGathering(targetName);
				string text = (ignored ? "Remove from Gathering Ignore List" : "Add to Gathering Ignore List");
				DynamicAct act = new DynamicAct(text ?? "", (Func<bool>)delegate {
					if (ignored) {
						Plugin.Instance.IgnoreList.RemoveFromGatheringIgnoreList(targetName);
					} else {
						Plugin.Instance.IgnoreList.AddToGatheringIgnoreList(targetName);
					}
					return true;
				}, false);
				((List<ActPlan.Item>)(object)__instance.list).Add(new ActPlan.Item {
					act = (Act)(object)act
				});
			}
			if (!TaskMine.CanMine(__instance.pos, (Card)(object)((Card)ELayer.pc).Tool)) {
				return;
			}
			string targetName2 = __instance.pos.cell.GetBlockName();
			bool ignored2 = Plugin.Instance.IgnoreList.IsIgnoredFromMining(targetName2);
			string text2 = (ignored2 ? "Remove from Mining Ignore List" : "Add to Mining Ignore List");
			DynamicAct act2 = new DynamicAct(text2 ?? "", (Func<bool>)delegate {
				if (ignored2) {
					Plugin.Instance.IgnoreList.RemoveFromMiningIgnoreList(targetName2);
				} else {
					Plugin.Instance.IgnoreList.AddToMiningIgnoreList(targetName2);
				}
				return true;
			}, false);
			((List<ActPlan.Item>)(object)__instance.list).Add(new ActPlan.Item {
				act = (Act)(object)act2
			});
		}

		[HarmonyPatch(typeof(ActPlan.Item), "Perform")]
		[HarmonyPrefix]
		public static bool Prefix(ActPlan.Item __instance) {
			Act act = __instance.act;
			DynamicAct val = (DynamicAct)(object)((act is DynamicAct) ? act : null);
			if (val != null && actNames.Contains(val.id)) {
				((Act)val).Perform();
				return false;
			}
			return true;
		}
	}

}
