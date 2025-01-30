using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnityEngine.Windows;

namespace Elin_Mod
{
	[HarmonyPatch]
	/// <summary>
	/// Mod管理クラス.
	/// </summary>
	public class NyModManager : Singleton<NyModManager>
	{
		public enum eState {
			None,
			Initializing,
			Idle,
		}

		BaseUnityPlugin m_Plugin;
		string m_ModName;
		string m_ModFullName;
		string m_ModVersion;
		Harmony m_Harmony;

		eState m_State;
		ModConfigBase m_Config;

		System.Action m_OnLoadTable;
		System.Action m_OnStartGame;


		/// <summary>
		/// 初期化.
		/// </summary>
		/// <typeparam name="ModConfigType"></typeparam>
		/// <param name="plugin"></param>
		/// <param name="logger"></param>
		/// <param name="modFullName"></param>
		/// <param name="modName"></param>
		/// <param name="modVersion"></param>
		public void Initialize<
			ModConfigType
		>(
			BaseUnityPlugin plugin, 
			BepInEx.Logging.ManualLogSource logger, 
			string modFullName, 
			string modName, 
			string modVersion
		) 
			where ModConfigType : ModConfigBase, new()
		{
			m_State = eState.Initializing;
			m_Plugin = plugin;
			m_ModName = modName;
			m_ModFullName = modFullName;
			m_ModVersion = modVersion;

			m_Harmony = new Harmony(modFullName);
			m_Harmony.PatchAll();

			m_Config = new ModConfigType();
			m_Config.Initialize(plugin.Config);
			DebugUtil.Initialize(logger);
			CommonUtil.Initialize(plugin.Info);
		}

		/// <summary>
		/// 終了処理.
		/// </summary>
		public void Terminate() {
			m_Harmony?.UnpatchSelf();
			m_Harmony = null;
			ModTextManager.Instance.Terminate();
			ModTextManager.DeleteInstance();
		}

		/// <summary>
		/// コンフィグ取得.
		/// </summary>
		/// <returns></returns>
		public ModConfigBase GetConfig() {
			return m_Config;
		}




		/// <summary>
		/// ゲーム開始時コールバック登録.
		/// </summary>
		/// <param name="onStartGame"></param>
		public void RegisterOnStartGameAction( System.Action onStartGame ) {
			m_OnStartGame += onStartGame;
		}

		/// <summary>
		/// テーブル読み込みコールバック登録.
		/// </summary>
		/// <param name="onLoadTable"></param>
		public void RegisterOnLoadTableAction(System.Action onLoadTable) {
			m_OnLoadTable += onLoadTable;
		}



		//------
		// 以下コールバック群....
		//------

		[HarmonyPatch(typeof(Game), "OnBeforeInstantiate")]
		[HarmonyPostfix]
		static void PostFix_OnBeforeInstantiate() {
			switch (Instance.m_State) {
				case eState.Initializing:
					ModTextManager.Instance.Initialize();
					Instance.m_OnLoadTable?.Invoke();
					break;
			}
		}

		[HarmonyPatch(typeof(Scene), "Init")]
		[HarmonyPostfix]
		static void PostFix_OnScebeInit( Scene.Mode newMode) {
			if (newMode != Scene.Mode.StartGame)
				return;
			Instance.m_OnStartGame?.Invoke();
			Instance.m_State = eState.Idle;
		}
	}
}
