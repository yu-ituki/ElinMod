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

		public ModConfig ModConfig { get => NyModManager.Instance.GetConfig() as ModConfig; }


		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake() {
			Instance = this;
			NyModManager.Instance.Initialize<ModConfig>(this, this.Logger, ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion);
			NyModManager.Instance.RegisterOnStartGameAction(OnStartGame);
			NyModManager.Instance.RegisterOnLoadTableAction(OnLoadTable);

			Const.Initialize();
			AutoEat.Instance.Initialize();
		}

		/// <summary>
		/// Mod開放タイミング.
		/// </summary>
		void Unload() {
			NyModManager.Instance?.Terminate();
			NyModManager.DeleteInstance();
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


		

#if true
		public void Update() {
			if (!GameUtil.IsPlayingGame())
				return;
			AutoEat.Instance.CheckAutoEat();
		}
#endif
	}
}
