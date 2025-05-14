using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elin_Mod
{
	[HarmonyLib.HarmonyPatch]
	public class TentShortCut
	{
		public static void Initialize() {
			ModUtil.RegisterSerializedTypeFallback(ModInfo.c_ModName, "Elin_Mod.HotItemActionTent", "HotItemActionSleep");
		}

		[HarmonyPatch(typeof(WidgetHotbar), "SetShortcutMenu")]
		[HarmonyPostfix]
		public static void Postfix_SetShortcutMenu(WidgetHotbar __instance, ButtonHotItem b, UIContextMenu m) {
			m.AddButton(ModTextManager.Instance.GetText(eTextID.Msg_Title), ()=>
			{
				__instance.SetItem(b, new HotItemActionTent());
			});

		}


	}


	public class HotItemActionTent : HotAction
	{
		public override string Id => "Tent";
		public override string Name => ModTextManager.Instance.GetText(eTextID.Msg_Title);

		public override string pathSprite => "icon_LayerChara";

		public override bool CanName => false;

		public override void Perform() {
			var pcPos = EClass.pc.pos;

			// ワールドマップ上では使えないようにする.
			if (EClass._zone.IsRegion)
				return;

			// ゾーンにテントがあるかを調べる.
			var zoneTents = EClass._zone.map.ListThing<TraitTent>();
			if (zoneTents.Count > 0) {
				// 自身の持ち物以外は除去.
				for ( int i = 0; i < zoneTents.Count; ++i ) {
					if (!zoneTents[i].isNPCProperty)
						continue;
					zoneTents.RemoveAt(i);
					--i;
				}
				// プレイヤーに近い順にソート.
				zoneTents.Sort((a, b) => {
					var aDist = a.pos.Distance(pcPos);
					var bDist = b.pos.Distance(pcPos);
					if (aDist < bDist)
						return -1;
					if (aDist > bDist)
						return 1;
					return 0;
				});
			}
			// 自身に近い位置にテントが既にあったら回収を優先する.
			if (zoneTents.Count > 0 && zoneTents[0].pos.Distance(pcPos) <= 2 ) {
				EClass.pc.AddCard(zoneTents[0]);
			} else {
				// そうじゃなければテントを持っているか調べる.
				var haveTent = _FindTent(EClass.pc.things);
				if (haveTent != null) {
					if (haveTent.Num > 1) { //< 無いとは思うが複数持ってたら.
						haveTent = haveTent.Split(1); //< 一個だけ使う.
					}
					ItemPosition posTent = ItemPosition.Get(haveTent);
					EClass._zone.AddCard(haveTent, pcPos).Install();
				} else if (zoneTents.Count > 0 ) {
					// 一番近いものをしまって終了.
					EClass.pc.AddCard(zoneTents[0]);
				}
			}
		}

		static Thing _FindTent( ThingContainer things ) {
			// インベ直のチェック.
			var haveTents = things.FindAll((v) => v.trait is TraitTent);
			if (haveTents.Count > 0)
				return haveTents[0];

			// インベ内インベをチェック.
			var invInInvs = things.FindAll((v) => v.trait is TraitContainer);
			foreach (var itr in invInInvs) {
				var ret = _FindTent(itr.things);
				if (ret == null)
					continue;
				return ret;
			}

			// ねンだわ.
			return null;
		}
	}

}
