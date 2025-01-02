using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.Events;

namespace Elin_AutoExplore
{

	[HarmonyPatch]
	public class AutoExploreConfigUi
	{
		public const string Name = "AutoExplore Settings";

		[HarmonyPatch(typeof(ActPlan), "ShowContextMenu")]
		[HarmonyPrefix]
		public static void Prefix(ActPlan __instance) {
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Expected O, but got Unknown
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			if (!((object)__instance.pos).Equals((object)((Card)EClass.pc).pos)) {
				return;
			}
			AutoExplorerConfig config = Plugin.Instance.AutoExplorerConfig;
			DynamicAct act = new DynamicAct(Translations.GetTranslation("AutoExplore Settings"), (Func<bool>)delegate {
				UIContextMenu val2 = EClass.ui.CreateContextMenu("ContextMenu");
				val2.AddToggle(Translations.GetTranslation("HandleFighting"), config.HandleFighting.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleFighting.Value = val;
				});
				val2.AddToggle(Translations.GetTranslation("HandleHarvestables"), config.HandleHarvestables.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleHarvestables.Value = val;
				});
				val2.AddToggle(Translations.GetTranslation("HandleMineables"), config.HandleMineables.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleMineables.Value = val;
				});
				val2.AddToggle(Translations.GetTranslation("HandleTraps"), config.HandleTraps.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleTraps.Value = val;
				});
				val2.AddToggle(Translations.GetTranslation("HandleShrines"), config.HandleShrines.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleShrines.Value = val;
				});


				val2.AddToggle(Translations.GetTranslation("HandleMineOreOnly"), config.HandleMineOreOnly.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleMineOreOnly.Value = val;
				});

				val2.AddToggle(Translations.GetTranslation("HandleVegetables"), config.HandleVegetables.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleVegetables.Value = val;
				});


				val2.AddSlider(Translations.GetTranslation("HandleHunger"), (Func<float, string>)((float val) => ((AutoExplorerConfig.HungerMode)val/*cast due to .constrained prefix*/).ToString()), (float)config.HandleHunger.Value, (Action<float>)delegate (float val) {
					config.HandleHunger.Value = (AutoExplorerConfig.HungerMode)val;
				}, 0f, 2f, true, false, false);
				val2.AddSeparator(0);
				val2.AddToggle(Translations.GetTranslation("UseMeditation"), config.UseMeditation.Value, (UnityAction<bool>)delegate (bool val) {
					config.UseMeditation.Value = val;
				});
				val2.AddSlider(Translations.GetTranslation("MinMP"), (Func<float, string>)((float val) => val.ToString()), (float)config.MinMP.Value, (Action<float>)delegate (float val) {
					config.MinMP.Value = (int)val;
				}, 0f, 100f, true, false, false);
				val2.AddSlider(Translations.GetTranslation("MinHP"), (Func<float, string>)((float val) => val.ToString()), (float)config.MinHP.Value, (Action<float>)delegate (float val) {
					config.MinHP.Value = (int)val;
				}, 0f, 100f, true, false, false);
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
			Act act = __instance.act;
			
			DynamicAct val = (DynamicAct)(object)((act is DynamicAct) ? act : null);
			if (val != null && val.id == Translations.GetTranslation("AutoExplore Settings")) {
				((Act)val).Perform();
				return false;
			}
			return true;
		}
	}
}
