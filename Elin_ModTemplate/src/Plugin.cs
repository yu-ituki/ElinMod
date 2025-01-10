using BepInEx;
using HarmonyLib;

namespace Elin_ModTemplate
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
			Harmony val = new Harmony( ModInfo.c_ModFullName );
			val.PatchAll();
			Instance = this;

			ModConfig = new ModConfig( Config );
			DebugUtil.Initialize( Logger );
			CommonUtil.Initialize( Info );
			ModTextManager.Instance.Initialize();
		}

		/// <summary>
		/// Mod開放タイミング.
		/// </summary>
		private void Unload()
		{
			Harmony val = new Harmony( ModInfo.c_ModFullName );
			val.UnpatchSelf();
		}
	}
}
