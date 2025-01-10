using BepInEx.Configuration;
using UnityEngine;

namespace Elin_ModTemplate
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig
	{
		//public ConfigEntry<KeyCode> ActiveKey { get; set; }

		public ModConfig( ConfigFile config )
		{
			//ActiveKey = config.Bind( "General", "Key_Activation", (KeyCode)108, "Key to start and stop autoexplore." );
		}
	}
}
