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
			Thing tent = EClass.pc.things.Find<TraitTent>();
			if ( tent == null ) {
				tent = EClass._zone.map.FindThing(( v ) => v.trait is TraitTent);
				if (tent == null)
					return;
				EClass.pc.AddCard(tent);
			} else {
				ItemPosition posTent = ItemPosition.Get(tent);
				EClass._zone.AddCard(tent, EClass.pc.pos).Install();
			}
		}
	}

}
