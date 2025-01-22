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

			AddFoodDistribution.ApplyPatch(val);

			// データ読み込み.
			//ModUtil.ImportExcel(CommonUtil.GetResourcePath("tables/add_datas.xlsx"), "recipes", EClass.sources.recipes);
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
		private void _Initialize() {

			m_IsInitialized = true;
			ModTextManager.Instance.Initialize();
		}



		public void Update() {
			// 初期化.
			if ( !m_IsInitialized ) {
				if (EClass.core.IsGameStarted) {
					_Initialize();
				}
				else {
					return;
				}
			}

			}
	}
}
