using BepInEx.Configuration;


using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig : ModConfigBase
	{
		public ConfigEntry<int> EffectiveRange { get; set; }
		public ConfigEntry<float> AddMusicLvFactor { get; set; }

		public override void Initialize( ConfigFile config )
		{
			EffectiveRange = config.Bind( "General", "Effective range", 4, "Effective range.");
			AddMusicLvFactor = config.Bind("General", "Performance level coefficient", 1.0f, "Performance level coefficient.");
		}
	}
}
