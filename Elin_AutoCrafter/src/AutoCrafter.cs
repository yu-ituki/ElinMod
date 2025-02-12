using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

using JetBrains.Annotations;

namespace Elin_Mod
{
	[HarmonyPatch]
	class AutoCrafter : Singleton<AutoCrafter>
	{
		bool m_IsPlayingAutoCraft;
		LayerCraft m_LastCraftLayer;
		TraitCrafter m_LastUseCrafter;
		Recipe m_LastUseRecipe;

		IEnumerator m_CraftCoroutine;

		// AI_UseCrafterが食材足りないときにCancelにならないっぽくて.
		// AutoCraferが停止しないので仕方なくHarmonyPatchに頼る...
		[HarmonyPatch(typeof(LayerCraft), "ClearButtons")]
		[HarmonyPostfix]
		static void _Postfix_LayerCraftClearbuttons() {
			Instance?._EndAutoCraft();
		}

		[HarmonyPatch(typeof(LayerCraft), "RefreshCurrentGrid")]
		[HarmonyPostfix]
		static void _Postfix_LayerCraftRefreshCurrentGrid() {
			Instance?._EndAutoCraft();
		}
		

		[HarmonyPatch(typeof(LayerCraft), "OnClickCraft")]
		[HarmonyPostfix]
		static void _Postfix_LayerCraftOnClickCraft(LayerCraft __instance) {
			if (__instance.gameObject.activeSelf)
				return;

			var ai = ELayer.pc.ai as AI_UseCrafter;
			if (ai == null)
				return;

			// メニューを閉じててAI_UseCrafterがセットされてたら判定開始.
			if ( !CommonUtil.GetKey(KeyCode.LeftShift) 
				&& !CommonUtil.GetKey(KeyCode.RightShift)) 
			{
				return;
			}
			
			Instance._PlayAutoCraft(__instance, ai.crafter, ai.recipe);
		}


		void _PlayAutoCraft(LayerCraft layer, TraitCrafter crafter, Recipe recipe ) {
		//	DebugUtil.LogWarning("!!!! register !!!!!");
			m_IsPlayingAutoCraft = true;
			m_LastCraftLayer = layer;
			m_LastUseCrafter = crafter;
			m_LastUseRecipe = recipe;
		}


		public void UpdateAutoCraft() {
			if (!m_IsPlayingAutoCraft)
				return;

			if (m_CraftCoroutine == null )
				m_CraftCoroutine = _Run();
			if (!m_CraftCoroutine.MoveNext())
				_EndAutoCraft();
		}

		void _EndAutoCraft() {
			m_CraftCoroutine = null;
			m_IsPlayingAutoCraft = false;
			m_LastUseCrafter = null;
			m_LastUseRecipe = null;
			m_LastCraftLayer = null;
			ActionMode.Adv.EndTurbo();
		}

		IEnumerator _Run() {
			var pc = EClass.pc;

			var config = Plugin.Instance.ModConfig;
			while ( true ) {
			//	DebugUtil.LogWarning("!!!! run !!!!!");
				yield return null;

				// 寝そうなら止める.
				if (config.IsStopCanSleep.Value && GameUtil.IsCanSleepPlayer())
					break;
				// 腹減ってたら止める.
				if (config.IsStopHunger.Value && GameUtil.IsHungerPlayer())
					break;
				// 疲れてても止める.
				if (config.IsStopZeroStumina.Value && pc.stamina.value <= 0)
					break;
				// ボタン押したら強制解除.
				if (CommonUtil.GetKeyAnyDown() )
					break;

				var ai = ELayer.pc.ai;
				if ( ai != null ) {
					// AI起動中は待つ.
					if (ai.status == AIAct.Status.Running)
						continue;
					// 失敗したら終了.
					if (ai.status == AIAct.Status.Fail)
						break;
				}

				// UIのアクティブ待ち(tweenでやってるっぽいのでこっちでポーリング待ちする).
				var layerGo = m_LastCraftLayer.gameObject;
				while (layerGo != null && !layerGo.activeSelf) {
					yield return null;
				}
				if (layerGo == null)
					break;

				// UI閉じ続ける.
				layerGo.SetActive(false);

				// AIセット.
				ELayer.pc.SetAI(new AI_UseCrafter {
					crafter = m_LastUseCrafter,
					layer = m_LastCraftLayer,
					recipe = m_LastUseRecipe,
					num = 1,
				});
				ActionMode.Adv.SetTurbo(); //< 早く.
			}

			// UI戻す.
			if (m_LastCraftLayer != null ) {
				m_LastCraftLayer.gameObject.SetActive(true);
			}
			// おわり.
		//	DebugUtil.LogError("!!!! end !!!!! " + (EClass.pc.ai?.status ?? (AIAct.Status)(-1) ) );
		}
	
	}
}
