using BepInEx;
using HarmonyLib;

using UnityEngine.Windows;

namespace Elin_Mod
{

	/// <summary>
	/// Modのエントリポイント.
	/// </summary>
	[BepInPlugin( ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion )]
	public class Plugin : BaseUnityPlugin
	{
		public ModConfig ModConfig { get; private set; } = null;

		public static Plugin Instance { get; private set; }

		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake()
		{
			MyModManager.Instance.Initialize<ModConfig>(this, this.Logger, ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion);
			Instance = this;
		}

		/// <summary>
		/// Mod開放タイミング.
		/// </summary>
		private void Unload()
		{
			MyModManager.Instance.Terminate();
		}
	}
}
