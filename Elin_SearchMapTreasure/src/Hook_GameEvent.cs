using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static ActPlan;

namespace Elin_Mod
{
	[HarmonyPatch]
	public class Hook_GameEvent
	{
		static TraitScrollMapTreasure s_LastActiveTraitTreasure;

		/// <summary>
		/// 宝の地図を開いたタイミングをフック.
		/// </summary>
		/// <param name="c"></param>
		[HarmonyPatch(typeof(LayerTreasureMap), "SetMap")]
		[HarmonyPostfix]
		public static void Postfix(TraitScrollMapTreasure trait) {
			s_LastActiveTraitTreasure = trait;

			if (!(EClass._zone is Region)) {
				return;
			}

			_SayTreasureMapPos(trait);
		}

		/// <summary>
		/// 各マップに入ったときをフック.
		/// </summary>
		/// <param name="c"></param>
		[HarmonyPatch(typeof(Zone), "Activate")]
		[HarmonyPostfix]
		public static void Postfix() {
			if (!(EClass._zone is Region)) {
				return;
			}

			// 開いてるかチェック.
			var layerMap = EClass.ui.layerFloat.GetLayer<LayerTreasureMap>();
			if (layerMap != null && layerMap.isActiveAndEnabled) {
				_SayTreasureMapPos(s_LastActiveTraitTreasure);
			}
		}


		/// <summary>
		/// メッセージ表示本体.
		/// </summary>
		/// <param name="trait"></param>
		static void _SayTreasureMapPos(TraitScrollMapTreasure trait) {
			if (trait == null)
				return;
			var tboxPos = trait.GetDest( true );
			var playerPos = EClass.pc.pos;
		//	var playerRegionPos = new RegionPoint(EClass.pc.pos);
			var textMng = ModTextManager.Instance;
			var currentZone = EClass._zone;

#if false
			Zone nearestZone = null;
			int nearestDist = int.MaxValue;
			foreach (Zone zone in EClass.game.spatials.Zones) {
				if (zone == currentZone)
					continue;
				int dist = zone.Dist(playerRegionPos);
				if (nearestDist < dist)
					continue;
				nearestDist = dist;
				nearestZone = zone;
			}

			string textNearName = null;
			if (nearestZone != null) {
				textNearName = nearestZone.Name;
			} else {
				textNearName = textMng.GetText(eTextID.Text_NoneNearZone);
			}
#endif

			eTextID textLR = eTextID.Text_L;
			int diffX = tboxPos.x - playerPos.x;
			if (diffX > 0)
				textLR = eTextID.Text_R;

			eTextID textUD = eTextID.Text_U;
			int diffZ = playerPos.z - tboxPos.z;
			if (diffZ > 0)
				textUD = eTextID.Text_D;

			var textLRBody = textMng.GetText(textLR);
			var textUDBody = textMng.GetText(textUD);

			diffZ = Mathf.Abs(diffZ);
			diffX = Mathf.Abs(diffX);

			textMng.SetUserData(0, textLRBody);
			textMng.SetUserData(1, diffX);
			textMng.SetUserData(2, textUDBody);
			textMng.SetUserData(3, diffZ);
		//	textMng.SetUserData(4, textNearName);

			var textBody = textMng.GetText(eTextID.Text_Main);
			Msg.SayRaw(textBody);
		}
	}
}
