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

		bool m_IsInitialized = false;

		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake()
		{
			Harmony val = new Harmony( ModInfo.c_ModFullName );
			val.PatchAll();
			Instance = this;
			m_IsInitialized = false;

			ModConfig = new ModConfig(Config);
			DebugUtil.Initialize(Logger);
			CommonUtil.Initialize(Info);
		}

		/// <summary>
		/// Mod開放タイミング.
		/// </summary>
		private void Unload()
		{
			m_IsInitialized = false;
			Harmony val = new Harmony( ModInfo.c_ModFullName );
			val.UnpatchSelf();

			ModTextManager.Instance.Terminate();
			ModTextManager.DeleteInstance();
			GunSmithManager.Instance.Terminate();
			GunSmithManager.DeleteInstance();
		}


		/// <summary>
		/// プラグインの実初期化処理.
		/// ゲーム開始直前に呼び出される.
		/// </summary>
		private void _OnStartGame() {

			m_IsInitialized = true;
			ModTextManager.Instance.Initialize();
			GunSmithManager.Instance.Initialize();
		}



		public void Update() {
			// 初期化.
			if ( !m_IsInitialized ) {
				if (EClass.core.IsGameStarted) {
					_OnStartGame();
				}
				else {
					return;
				}
			}

			if (CommonUtil.GetKeyDown(UnityEngine.KeyCode.F10))
				GameUtil.Cheat_AllItemJIdentify();

#if false
			if (UnityEngine.Input.GetKeyDown(UnityEngine.KeyCode.F10)) {
				Debug_AnalyzeElin.Dump_ElinCategoriesAll("D:\\categories.tsv");

			}
#endif

#if false
			// だんぷ.
			if ( UnityEngine.Input.GetKeyDown( UnityEngine.KeyCode.F10 ) ) {
				var grid = LayerDragGrid.Instance;
				if ( grid == null ) {
					DebugUtil.LogError("1");
					return;
				}
				DebugUtil.LogError(grid.ToString());
				if (grid.owner == null) {
					DebugUtil.LogError("2");
					return;
				}
				DebugUtil.LogError(grid.owner.ToString());
				if (grid.owner.owner == null) {
					DebugUtil.LogError("3");
					return;
				}
				DebugUtil.LogError(grid.owner.owner.ToString());
				if (grid.owner.owner.trait == null) {
					DebugUtil.LogError("4");
					return;
				}
				DebugUtil.LogError(grid.owner.owner.trait.ToString());
			}
#endif
			}
	}
}
