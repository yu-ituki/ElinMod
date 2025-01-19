using BepInEx.Configuration;
using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig
	{
		public ConfigEntry<float> ModElement_DmgFactor { get; set; }
		public ConfigEntry<float> ModBarrel_DistReductionFactor { get; set; }

		public ModConfig( ConfigFile config )
		{
			ModElement_DmgFactor = config.Bind( "General", "ModElement_DmgFactor", 1.0f, "Coefficient of Element Damage");
			ModBarrel_DistReductionFactor = config.Bind("General", "ModBarrel_DistReductionFactor", 0.5f, "Correction factor for the effect of distance attenuation due to scope");
		}
	}
}
