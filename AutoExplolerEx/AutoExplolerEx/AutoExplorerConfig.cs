using System;
using BepInEx.Configuration;
using UnityEngine;

namespace Elin_AutoExplore
{

	public class AutoExplorerConfig
	{
		public enum eMode
		{
			Explore,
			Harvest,
			Mine,
			
			EX_MineOnlyOre,
			EX_Vegetables,
		}

		static readonly (eMode, eModText)[] s_ModeToText = new(eMode, eModText)[] {
			( eMode.Explore, eModText.Mode_Exploring ),
			( eMode.Harvest, eModText.Mode_Harvesting ),
			( eMode.Mine, eModText.Mode_Mining ),
			( eMode.EX_MineOnlyOre, eModText.Mode_MineOreOnly ),
			( eMode.EX_Vegetables, eModText.Mode_Vegetables ),
		};


		public enum eHungerMode
		{
			Ignore,
			AutoEat,
			StopAutoExplore
		}

		public enum eMineMode
		{
			Ignore,
			All,
			OreOnly,
		}






		public ConfigEntry<KeyCode> Key_Activation { get; set; }

		public ConfigEntry<KeyboardShortcut> Key_ToggleChangeMode { get; set; }

		public ConfigEntry<KeyCode> GoDownKey { get; set; }

		public ConfigEntry<KeyCode> GoUpKey { get; set; }

		public ConfigEntry<KeyboardShortcut> MenuKey { get; set; }

		public ConfigEntry<bool> UseMeditation { get; set; }

		public ConfigEntry<int> MinMP { get; set; }

		public ConfigEntry<int> MinHP { get; set; }

		public ConfigEntry<bool> HandleTraps { get; set; }

		public ConfigEntry<bool> HandleFighting { get; set; }

		public ConfigEntry<bool> HandleHarvestables { get; set; }

		public ConfigEntry<eMineMode> HandleMineables { get; set; }

		public ConfigEntry<bool> HandleShrines { get; set; }

		public ConfigEntry<string> GatheringRestrictionList { get; set; }

		public ConfigEntry<string> MiningRestrictionList { get; set; }

		public ConfigEntry<eHungerMode> HandleHunger { get; set; }


		public ConfigEntry<bool> HandleVegetables { get; set; }



		int m_ModeIndex;

		static eMode[] s_Modes;




		public AutoExplorerConfig(ConfigFile config) {
			
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			Key_Activation = config.Bind<KeyCode>("General", "Key_Activation", (KeyCode)108, "Key to start and stop autoexplore.");
			Key_ToggleChangeMode = config.Bind<KeyboardShortcut>("General", "Key_ToggleChangeMode", new KeyboardShortcut((KeyCode)108, (KeyCode[])(object)new KeyCode[1] { (KeyCode)304 }), "Key to toggle between just exploring, harvesting and mining mode.");
			GoDownKey = config.Bind<KeyCode>("General", "GoDownKey", (KeyCode)44, "Key to move to stairs down.");
			GoUpKey = config.Bind<KeyCode>("General", "GoUpKey", (KeyCode)46, "Key to move to stairs up.");
			HandleTraps = config.Bind<bool>("Toggles", "HandleTraps", true, "Should autoexplore disarm traps?");
			UseMeditation = config.Bind<bool>("Toggles", "UseMeditation", true, "Should autoexplore meditate for HP/MP regen?");
			HandleFighting = config.Bind<bool>("Toggles", "HandleFighting", true, "Should autoexplore fight enemies?");
			HandleHarvestables = config.Bind<bool>("Toggles", "HandleHarvestables", false, "Should autoexplore harvest?");
			HandleMineables = config.Bind<eMineMode>("Regen", "HandleMineables2", eMineMode.Ignore, "Should autoexplore mine?");
			HandleShrines = config.Bind<bool>("Toggles", "HandleShrines", true, "Should autoexplore use shrines?");
			HandleHunger = config.Bind<eHungerMode>("Toggles", "HandleHunger", eHungerMode.AutoEat, "Should autoexplore eat food?");
			MinMP = config.Bind<int>("Regen", "minMP", 90, "Percentage of MP to start meditation.");
			MinHP = config.Bind<int>("Regen", "minHP", 100, "Percentage of HP to start meditation.");
			GatheringRestrictionList = config.Bind<string>("Restrictions", "GatheringRestrictionList", "wreck,chemicals,wall frame,debris,moss grass,a vine,pebble,stalagmite,chunk", "Comma separated list of things to ignore when harvesting.");
			MiningRestrictionList = config.Bind<string>("Restrictions", "MiningRestrictionList", "metal block(copper),soil block(soil),soil block covered with vines (soil)", "Comma separated list of things to ignore when mining.");

			HandleVegetables = config.Bind<bool>("Toggles", "HandleVegetables", false, "Should autoexplore Vegetable?");
			m_ModeIndex = 0;

			if (s_Modes == null) {
				var modes = System.Enum.GetValues(typeof(eMode));
				s_Modes = new eMode[modes.Length];
				for (int i = 0; i < modes.Length; ++i)
					s_Modes[i] = (eMode)modes.GetValue(i);
			}
		}

		private eMode GetMode() {
			return s_Modes[m_ModeIndex];
		}

		public void SetNextMode() {

			++m_ModeIndex;
			if (m_ModeIndex >= s_Modes.Length)
				m_ModeIndex = 0;
			SetMode(m_ModeIndex);

			var currentMode = GetMode();
			for ( int i = 0; i < s_ModeToText.Length; ++i ) {
				if (s_ModeToText[i].Item1 != currentMode)
					continue;

				((Card)ELayer.pc).TalkRaw(Translations.GetTranslation(s_ModeToText[i].Item2), (string)null, (string)null, false);
				break;
			}
		}

		private void SetMode(int modeIndex) {
			m_ModeIndex = modeIndex;
			var currentMode = GetMode();

			HandleHarvestables.Value = false;
			HandleMineables.Value = eMineMode.Ignore;
			HandleVegetables.Value = false;

			switch (currentMode) {
				case eMode.Explore:
					break;
				case eMode.Harvest:
					HandleHarvestables.Value = true;
					break;
				case eMode.Mine:
					HandleMineables.Value = eMineMode.All;
					break;
				case eMode.EX_MineOnlyOre:
					HandleMineables.Value = eMineMode.OreOnly;
					break;
				case eMode.EX_Vegetables:
					HandleVegetables.Value = true;
					break;
			}
		}
	}
}
