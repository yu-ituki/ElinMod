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

		public ConfigEntry<eHungerState> StopEatState { get; set; }
		public ConfigEntry<eEatPriority> EatPriority { get; set; }
		public ConfigEntry<bool> IsPreferredJustCooked { get; set; }

		public ConfigEntry<bool> IsInstantEat { get; set; }

		public override void Initialize( ConfigFile config )
		{
			StopEatState = config.Bind( "General", "StopEatState", eHungerState.Bloated, "Hunger to stop eating");
			EatPriority = config.Bind("General", "EatPriority", eEatPriority.Normal, "Eating Priority");
			IsPreferredJustCooked = config.Bind("General", "IsPreferredJustCooked", true, "Priority is given to eating freshly prepared food.");
			IsInstantEat = config.Bind("General", "IsInstantEat", false, "Instantly finishes the meal.");
		}
	}
}
