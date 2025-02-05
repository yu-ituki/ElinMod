using BepInEx.Configuration;


using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig : ModConfigBase
	{
		//public ConfigEntry<KeyCode> ActiveKey { get; set; }

		public override void Initialize( ConfigFile config )
		{
			//ActiveKey = config.Bind( "General", "Key_Activation", (KeyCode)108, "Key to start and stop autoexplore." );
		}
	}
}
