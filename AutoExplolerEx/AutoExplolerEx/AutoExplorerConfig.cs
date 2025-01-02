using System;
using BepInEx.Configuration;
using UnityEngine;

namespace Elin_AutoExplore
{

	public class AutoExplorerConfig
	{
		public enum Mode
		{
			Explore,
			Harvest,
			Mine,
			HarvestAndMine,

			EX_MineOnlyOre,
			EX_Vegetables,
		}

		public enum HungerMode
		{
			Ignore,
			AutoEat,
			StopAutoExplore
		}

		public ConfigEntry<KeyCode> ActivationKey { get; set; }

		public ConfigEntry<KeyboardShortcut> ToggleHarvestingAndMiningMode { get; set; }

		public ConfigEntry<KeyCode> GoDownKey { get; set; }

		public ConfigEntry<KeyCode> GoUpKey { get; set; }

		public ConfigEntry<KeyboardShortcut> MenuKey { get; set; }

		public ConfigEntry<bool> UseMeditation { get; set; }

		public ConfigEntry<int> MinMP { get; set; }

		public ConfigEntry<int> MinHP { get; set; }

		public ConfigEntry<bool> HandleTraps { get; set; }

		public ConfigEntry<bool> HandleFighting { get; set; }

		public ConfigEntry<bool> HandleHarvestables { get; set; }

		public ConfigEntry<bool> HandleMineables { get; set; }

		public ConfigEntry<bool> HandleShrines { get; set; }

		public ConfigEntry<string> GatheringRestrictionList { get; set; }

		public ConfigEntry<string> MiningRestrictionList { get; set; }

		public ConfigEntry<HungerMode> HandleHunger { get; set; }


		public ConfigEntry<bool> HandleMineOreOnly { get; set; }
		public ConfigEntry<bool> HandleVegetables { get; set; }


		public AutoExplorerConfig(ConfigFile config) {
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			ActivationKey = config.Bind<KeyCode>("General", "ActivationKey", (KeyCode)108, "Key to start and stop autoexplore.");
			ToggleHarvestingAndMiningMode = config.Bind<KeyboardShortcut>("General", "ToggleHarvestingAndMiningMode", new KeyboardShortcut((KeyCode)108, (KeyCode[])(object)new KeyCode[1] { (KeyCode)304 }), "Key to toggle between just exploring, harvesting and mining mode.");
			GoDownKey = config.Bind<KeyCode>("General", "GoDownKey", (KeyCode)44, "Key to move to stairs down.");
			GoUpKey = config.Bind<KeyCode>("General", "GoUpKey", (KeyCode)46, "Key to move to stairs up.");
			HandleTraps = config.Bind<bool>("Toggles", "HandleTraps", true, "Should autoexplore disarm traps?");
			UseMeditation = config.Bind<bool>("Toggles", "UseMeditation", true, "Should autoexplore meditate for HP/MP regen?");
			HandleFighting = config.Bind<bool>("Toggles", "HandleFighting", true, "Should autoexplore fight enemies?");
			HandleHarvestables = config.Bind<bool>("Toggles", "HandleHarvestables", false, "Should autoexplore harvest?");
			HandleMineables = config.Bind<bool>("Toggles", "HandleMineables", false, "Should autoexplore mine?");
			HandleShrines = config.Bind<bool>("Toggles", "HandleShrines", true, "Should autoexplore use shrines?");
			HandleHunger = config.Bind<HungerMode>("Toggles", "HandleHunger", HungerMode.AutoEat, "Should autoexplore eat food?");
			MinMP = config.Bind<int>("Regen", "minMP", 90, "Percentage of MP to start meditation.");
			MinHP = config.Bind<int>("Regen", "minHP", 100, "Percentage of HP to start meditation.");
			GatheringRestrictionList = config.Bind<string>("Restrictions", "GatheringRestrictionList", "wreck,chemicals,wall frame,debris,moss grass,a vine,pebble,stalagmite,chunk", "Comma separated list of things to ignore when harvesting.");
			MiningRestrictionList = config.Bind<string>("Restrictions", "MiningRestrictionList", "metal block(copper),soil block(soil),soil block covered with vines (soil)", "Comma separated list of things to ignore when mining.");


			HandleMineOreOnly = config.Bind<bool>("Toggles", "HandleMineOreOnly", false, "Should autoexplore Ore?");
			HandleVegetables = config.Bind<bool>("Toggles", "HandleVegetables", false, "Should autoexplore Vegetable?");
		}

		private Mode GetMode() {
			if (HandleMineOreOnly.Value) {
				return Mode.EX_MineOnlyOre;
			}
			if (HandleVegetables.Value) {
				return Mode.EX_Vegetables;
			}

			if (HandleHarvestables.Value && HandleMineables.Value) {
				return Mode.HarvestAndMine;
			}
			if (HandleHarvestables.Value) {
				return Mode.Harvest;
			}
			if (HandleMineables.Value) {
				return Mode.Mine;
			}
			return Mode.Explore;
		}

		public void SetNextMode() {
			switch (GetMode()) {
				case Mode.Explore:
					SetMode(Mode.Harvest);
					((Card)ELayer.pc).TalkRaw(Translations.GetTranslation("Harvesting mode"), (string)null, (string)null, false);
					break;
				case Mode.Harvest:
					SetMode(Mode.Mine);
					((Card)ELayer.pc).TalkRaw(Translations.GetTranslation("Mining mode"), (string)null, (string)null, false);
					break;
				case Mode.Mine:
					SetMode(Mode.HarvestAndMine);
					((Card)ELayer.pc).TalkRaw(Translations.GetTranslation("Harvesting and mining mode"), (string)null, (string)null, false);
					break;
				case Mode.HarvestAndMine:
					//SetMode(Mode.Explore);
					SetMode(Mode.EX_MineOnlyOre);
					((Card)ELayer.pc).TalkRaw(Translations.GetTranslation("HandleMineOreOnly"), (string)null, (string)null, false);
					break;




				case Mode.EX_MineOnlyOre:
					SetMode(Mode.EX_Vegetables);
					((Card)ELayer.pc).TalkRaw(Translations.GetTranslation("HandleVegetables"), (string)null, (string)null, false);
					break;

				case Mode.EX_Vegetables:
					SetMode(Mode.Explore);
					((Card)ELayer.pc).TalkRaw(Translations.GetTranslation("Exploring mode"), (string)null, (string)null, false);
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void SetMode(Mode mode) {
			HandleMineOreOnly.Value = false;
			HandleVegetables.Value = false;
			switch (mode) {
				case Mode.Explore:
					HandleHarvestables.Value = false;
					HandleMineables.Value = false;
					break;
				case Mode.Harvest:
					HandleHarvestables.Value = true;
					HandleMineables.Value = false;
					break;
				case Mode.Mine:
					HandleHarvestables.Value = false;
					HandleMineables.Value = true;
					break;
				case Mode.HarvestAndMine:
					HandleHarvestables.Value = true;
					HandleMineables.Value = true;
					break;
				case Mode.EX_MineOnlyOre:
					HandleHarvestables.Value = false;
					HandleMineables.Value = false;
					HandleMineOreOnly.Value = true;
					HandleVegetables.Value = false;
					break;
				case Mode.EX_Vegetables:
					HandleHarvestables.Value = false;
					HandleMineables.Value = false;
					HandleMineOreOnly.Value = false;
					HandleVegetables.Value = true;
					break;
			}
		}
	}
}
