using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine.Events;

namespace Elin_AutoExplore
{

	[HarmonyPatch]
	public class AutoExploreConfigUi
	{
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
			DynamicAct act = new DynamicAct(Translations.GetTranslation( eModText.Text_AutoExploreSettings ), (Func<bool>)delegate {
				UIContextMenu val2 = EClass.ui.CreateContextMenu("ContextMenu");
				val2.AddToggle(Translations.GetTranslation(eModText.Handle_Fighting), config.HandleFighting.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleFighting.Value = val;
				});
				val2.AddToggle(Translations.GetTranslation(eModText.Handle_Harvestables), config.HandleHarvestables.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleHarvestables.Value = val;
				});
				val2.AddSlider(Translations.GetTranslation(eModText.Handle_Mineables), 
					(Func<float, string>)((float val) => ((AutoExplorerConfig.eMineMode)val).ToString()),
					(float)config.HandleMineables.Value, 
					(Action<float>)delegate (float val) {
						config.HandleMineables.Value = (AutoExplorerConfig.eMineMode)val;
					},
				0f, 2f, true, false, false);

				val2.AddToggle(Translations.GetTranslation(eModText.Handle_Traps), config.HandleTraps.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleTraps.Value = val;
				});
				val2.AddToggle(Translations.GetTranslation(eModText.Handle_Shrines), config.HandleShrines.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleShrines.Value = val;
				});
				
				val2.AddToggle(Translations.GetTranslation(eModText.Handle_Vegetables), config.HandleVegetables.Value, (UnityAction<bool>)delegate (bool val) {
					config.HandleVegetables.Value = val;
				});


				val2.AddSlider(Translations.GetTranslation(eModText.Handle_Hunger), 
					(Func<float, string>)((float val) => ((AutoExplorerConfig.eHungerMode)val).ToString()),
					(float)config.HandleHunger.Value,
					(Action<float>)delegate (float val) {
						config.HandleHunger.Value = (AutoExplorerConfig.eHungerMode)val;
					},
				0f, 2f, true, false, false);
				
				val2.AddSeparator(0);
				
				val2.AddToggle(Translations.GetTranslation(eModText.Handle_Meditation), config.UseMeditation.Value, (UnityAction<bool>)delegate (bool val) {
					config.UseMeditation.Value = val;
				});
				val2.AddSlider(Translations.GetTranslation(eModText.Text_MinMP), (Func<float, string>)((float val) => val.ToString()), (float)config.MinMP.Value, (Action<float>)delegate (float val) {
					config.MinMP.Value = (int)val;
				}, 0f, 100f, true, false, false);
				val2.AddSlider(Translations.GetTranslation(eModText.Text_MinHP), (Func<float, string>)((float val) => val.ToString()), (float)config.MinHP.Value, (Action<float>)delegate (float val) {
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
			if (val != null && val.id == Translations.GetTranslation(eModText.Text_AutoExploreSettings)) {
				((Act)val).Perform();
				return false;
			}
			return true;
		}
	}
}
