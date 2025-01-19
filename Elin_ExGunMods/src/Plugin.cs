using BepInEx;
using HarmonyLib;

using UnityEngine;
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
			ModTextManager.Instance.Initialize();


			// データ読み込み.
			ModUtil.ImportExcel(CommonUtil.GetResourcePath("tables/add_datas.xlsx"), "elements", EClass.sources.elements);
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
		}



		/// <summary>
		/// プラグインの実初期化処理.
		/// ゲーム開始直前に呼び出される.
		/// </summary>
		private void _OnStartGame() {

			m_IsInitialized = true;
			NewRangedModManager.Instance.Initialize();
		}




		public void Update() {

#if false
			if (UnityEngine.Input.GetKeyDown(KeyCode.F10)) {
				Debug_AnalyzeElin.Dump_ElinFactionAll("D:\\faction.tsv");
				Debug_AnalyzeElin.Dump_ElinElementAll("D:\\elements.tsv");
				Debug_AnalyzeElin.Dump_ElinRecipeAll("D:\\recipe.tsv");
				Debug_AnalyzeElin.Dump_ElinThingAll("D:\\things.tsv");
				Debug_AnalyzeElin.Dump_ElinLangGeneral("D:\\langGeneral.tsv");
				Debug_AnalyzeElin.Dump_ElinLangList("D:\\langList.tsv");
				Debug_AnalyzeElin.Dump_ElinLangWord("D:\\langWord.tsv");
				Debug_AnalyzeElin.Dump_ElinLangGame("D:\\langGame.tsv");
				Debug_AnalyzeElin.Dump_ElinLangNote("D:\\langNote.tsv");
			}
#endif
			// 初期化.
			if ( !m_IsInitialized ) {
				if (EClass.core.IsGameStarted) {
					_OnStartGame();
				}
				else {
					return;
				}
			}

		}
	}
}
