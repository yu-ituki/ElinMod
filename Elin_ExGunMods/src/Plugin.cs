using BepInEx;

using HarmonyLib;

using UnityEngine.Windows;

using static CoreConfig;

namespace Elin_Mod
{
	/// <summary>
	/// Modのエントリポイント.
	/// </summary>
	[BepInPlugin(ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion)]
	public class Plugin : BaseUnityPlugin
	{
		public static Plugin Instance { get; private set; }

		public ModConfig ModConfig { get => MyModManager.Instance.GetConfig() as ModConfig; }



		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake() {
			Instance = this;
			MyModManager.Instance.Initialize<ModConfig>(this, this.Logger, ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion);
			MyModManager.Instance.RegisterOnStartGameAction(OnStartGame);
			MyModManager.Instance.RegisterOnLoadTableAfterAction(OnLoadTableAfter);
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
		void Unload() {
			MyModManager.Instance?.Terminate();
			MyModManager.DeleteInstance();
		}

		void OnStartCore() {
			NewRangedModManager.Instance.OnStartCore();
		}

		/// <summary>
		/// テーブル読み込みタイミング.
		/// 各ゲーム内テーブル読み込み完了後、かつプレイヤー等の生成直前.
		/// </summary>
		void OnLoadTableAfter() {
			NewRangedModManager.Instance.OnLoadTableAfter();
		}

		/// <summary>
		/// ゲーム開始直前.
		/// 初期ゾーン読み込み完了直後.
		/// </summary>
		void OnStartGame() {
		}


#if false
		public void Update() {
			if (CommonUtil.GetKeyDown(UnityEngine.KeyCode.F10)) {
				Debug_AnalyzeElin.Dump_ElinElementAll("D:\\elements.tsv");
				Debug_AnalyzeElin.Dump_ElinThingAll("D:\\things.tsv");
				Debug_AnalyzeElin.Dump_ElinRecipeAll("D:\\recipies.tsv");
			}
		}
#endif
	}
}
