using BepInEx.Configuration;


using UnityEngine;

using static Elin_Mod.ModConfig;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig : ModConfigBase
	{
		public enum eHungerState {
			Bloated,
			Filled,
			Normal,
			Hungry,
			VeryHungry,
			Starving,
			MAX
		}

		public enum eEatPriority {
			/// <summary>適当.</summary>
			Normal,
			/// <summary>栄養が高い順.</summary>
			HighNutrition,
			/// <summary>栄養が低い順.</summary>
			LowNutrition,

			/// <summary> 筋力高い順 </summary>.
			HighSTR,
			/// <summary> 耐久高い順 </summary>.
			HighEND,
			/// <summary> 器用高い順 </summary>.
			HighDEX,
			/// <summary> 感覚高い順 </summary>.
			HighPER,
			/// <summary> 学習高い順 </summary>.
			HighLER,
			/// <summary> 意志高い順 </summary>.
			HighWIL,
			/// <summary> 魔力高い順 </summary>.
			HighMAG,
			/// <summary> 魅力高い順 </summary>.
			HighCHA,

			MAX
		}

		public ConfigEntry<bool> IsAutoEat { get; set; }
		public ConfigEntry<eHungerState> AutoEatState { get; set; }
		public ConfigEntry<eEatPriority> EatPriority { get; set; }
		public ConfigEntry<bool> IsInstantEat { get; set; }
		public ConfigEntry<bool> IsPreferredJustCooked { get; set; }

		public override void Initialize( ConfigFile config )
		{
			IsAutoEat = config.Bind("General", "IsAutoEat", true, "Auto Eat Enable");
			AutoEatState = config.Bind( "General", "AutoEatState", eHungerState.Bloated, "Hunger to auto eating");
			EatPriority = config.Bind("General", "EatPriority", eEatPriority.Normal, "Eating Priority");
			IsInstantEat = config.Bind("General", "IsInstantEat", true, "Instantly finishes the meal.");
			IsPreferredJustCooked = config.Bind("General", "IsPreferredJustCooked", false, "Priority is given to eating freshly prepared food.");
		}
	}
}
