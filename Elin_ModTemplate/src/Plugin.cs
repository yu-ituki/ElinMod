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
		public static Plugin Instance { get; private set; }

		public ModConfig ModConfig { get => MyModManager.Instance.GetConfig() as ModConfig; }


		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake() {
			Instance = this;
			MyModManager.Instance.Initialize<ModConfig>(this, this.Logger, ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion);
			MyModManager.Instance.RegisterOnStartGameAction(OnStartGame);
			MyModManager.Instance.RegisterOnLoadTableAction(OnLoadTable);
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


		/// <summary>
		/// テーブル読み込みタイミング.
		/// 各ゲーム内テーブル読み込み完了後、かつプレイヤー等の生成直前.
		/// </summary>
		void OnLoadTable() {
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
				Debug_AnalyzeElin.Dump_ElinCategoriesAll("D:\\categories.tsv");
				Debug_AnalyzeElin.Dump_ElinCharaAll("D:\\charas.tsv");
				Debug_AnalyzeElin.Dump_ElinElementAll("D:\\elements.tsv");
				Debug_AnalyzeElin.Dump_ElinFactionAll("D:\\factions.tsv");
				Debug_AnalyzeElin.Dump_ElinZoneAll("D:\\zones.tsv");
				Debug_AnalyzeElin.Dump_ElinRecipeAll("D:\\recipies.tsv");
				Debug_AnalyzeElin.Dump_ElinThingAll("D:\\things.tsv");
				Debug_AnalyzeElin.Dump_ElinLangGame("D:\\lang_game.tsv");
				Debug_AnalyzeElin.Dump_ElinLangGeneral("D:\\lang_general.tsv");
				Debug_AnalyzeElin.Dump_ElinLangList("D:\\lang_list.tsv");
				Debug_AnalyzeElin.Dump_ElinLangNote("D:\\lang_note.tsv");
				Debug_AnalyzeElin.Dump_ElinLangWord("D:\\lang_word.tsv");
			}
		}
#endif
	}
}
