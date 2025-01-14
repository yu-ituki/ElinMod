using BepInEx.Configuration;
using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig
	{
		public ConfigEntry<float> AddSlotCost { get; set; }
		public ConfigEntry<float> PowerUpModCost { get; set; }

		public ModConfig( ConfigFile config )
		{
			AddSlotCost = config.Bind("Regen", "AddSlotCost", 100.0f, "Cost required to add one slot.");
			PowerUpModCost = config.Bind("Regen", "PowerUpModCost", 2.0f, "Cost required to power up a mod by +1.");
		}
	}
}
