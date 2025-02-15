using BepInEx;
using HarmonyLib;

using UnityEngine.Windows;

namespace Elin_Mod
{

	/// <summary>
	/// Modのエントリポイント.
	/// </summary>
	[BepInPlugin(ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion)]
	public class Plugin : BaseUnityPlugin
	{
		public ModConfig ModConfig { get; private set; } = null;

		public static Plugin Instance { get; private set; }


		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake() {
			MyModManager.Instance.Initialize<ModConfig>(this, this.Logger, ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion);
			MyModManager.Instance.RegisterOnStartGameAction(_OnStartGame);
			MyModManager.Instance.RegisterOnLoadTableAfterAction(_OnLoadedTable);
			this.ModConfig = MyModManager.Instance.GetConfig() as ModConfig;
			Instance = this;
		}

		/// <summary>
		/// コンフィグメニュー表示コールバック.
		/// </summary>
		void _ModConfigMenu_OnAddCallback(object menu) {
			ModConfigMenu.Instance.OnCallback_AddMenu(menu);
		}

		/// <summary>
		/// Mod開放タイミング.
		/// </summary>
		private void Unload() {
			MyModManager.Instance.Terminate();
			RuneSmithManager.Instance.Terminate();
			RuneSmithManager.DeleteInstance();
		}

		/// <summary>
		/// テーブル読み込み直後.
		/// </summary>

		private void _OnLoadedTable() {
			RuneSmithManager.Instance.OnLoadedTables();
		}

		/// <summary>
		/// プラグインの実初期化処理.
		/// ゲーム開始直前に呼び出される.
		/// </summary>
		private void _OnStartGame() {

			RuneSmithManager.Instance.OnStartGame();
		}
	}
}
