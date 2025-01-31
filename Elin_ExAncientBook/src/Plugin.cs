﻿using BepInEx;

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
		public WalletGachaCoin MyWallet { get; private set; }



		/// <summary>
		/// Modのエントリポイント.
		/// </summary>
		private void Awake() {
			Instance = this;
			NyModManager.Instance.Initialize<ModConfig>(this, this.Logger, ModInfo.c_ModFullName, ModInfo.c_ModName, ModInfo.c_ModVersion);
			NyModManager.Instance.RegisterOnStartGameAction(OnStartGame);
			NyModManager.Instance.RegisterOnLoadTableAction(OnLoadTable);
			NyModManager.Instance.RegisterOnLoadCardTableAction(OnLoadCard);

			TraitMerchantEx_AncientResearcher.Initialize();
		}

		/// <summary>
		/// Mod開放タイミング.
		/// </summary>
		void Unload() {
			NyModManager.Instance?.Terminate();
			NyModManager.DeleteInstance();
		}

		/// <summary>
		/// Card系テーブル読み込み専用タイミング.
		/// </summary>
		void OnLoadCard() {
			TraitMerchantEx_AncientResearcher.LoadCards();
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
			// 財布の初期化.
			MyWallet?.Terminate();
			MyWallet = null;
			MyWallet = new WalletGachaCoin();
			MyWallet.Initialize(ModConfig);

			// ショップ周り初期化.
			TraitMerchantEx_AncientResearcher.OnStartGame();
		}


#if false
		public void Update() {
			if (CommonUtil.GetKeyDown(UnityEngine.KeyCode.F10)) {
				//Test.Run();
				
				//var pos = EClass.pc.pos;
				//DebugUtil.LogError( $"X={pos.x} Z={pos.z}" );
			}
		}
#endif
	}
}
