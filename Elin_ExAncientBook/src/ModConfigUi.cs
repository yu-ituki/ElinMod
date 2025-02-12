using System;
using System.Collections.Generic;
using HarmonyLib;
using ReflexCLI;

using UnityEngine.Events;

namespace Elin_Mod
{
	// プレイヤーをホイールクリックしたときのコンフィグUIサンプル.


#if false
	[HarmonyPatch]
	public class ModConfigUi
	{
		[HarmonyPatch(typeof(ActPlan), "ShowContextMenu")]
		[HarmonyPrefix]
		public static void Prefix(ActPlan __instance) {

			if (!__instance.pos.Equals(EClass.pc.pos)) {
				return;
			}

			var textMng = ModTextManager.Instance;
			ModConfig config = Plugin.Instance.ModConfig;
			DynamicAct act = new DynamicAct(textMng.GetText( eTextID.Config_Title ), ()=>{
				var menu = GameUtil.CreateContextMenu();
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_GachaCoin_Copper, config.Worth_GachaCoin_Copper, 0, 1000);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_GachaCoin_Silver, config.Worth_GachaCoin_Silver, 0, 1000);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_GachaCoin_Gold, config.Worth_GachaCoin_Gold, 0, 1000);
			//	GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_GachaCoin_Plat, config.Worth_GachaCoin_Plat, 0, 1000);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_AncientBook, config.Worth_AncientBook, 0.01f, 20.0f, 0.1f);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_SkillBook, config.Worth_SkillBook, 0.01f, 10.0f, 0.01f);
			//	GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesNum_SkillBook_Base, config.SalesNum_SkillBook_Base, 1, 10);
			//	GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesNum_SkillBook_Add, config.SalesNum_SkillBook_Add, 1, 15);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesLvLotRate_SkillBook_High, config.SalesLvLotRate_SkillBook_High, 0, 1000);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesLvLotRate_SkillBook_Middle, config.SalesLvLotRate_SkillBook_Middle, 0, 1000);
				GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesLvLotRate_SkillBook_Low, config.SalesLvLotRate_SkillBook_Low, 0, 1000);
			//	GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesLv_SkillBook_High, config.SalesLv_SkillBook_High, -99, 99);
			//	GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesLv_SkillBook_Middle, config.SalesLv_SkillBook_Middle, -99, 99);
			//	GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_SalesLv_SkillBook_Low, config.SalesLv_SkillBook_Low, -99, 99);
				menu.Show();
				return false;
			}, false);
			((List<ActPlan.Item>)(object)__instance.list).Add(new ActPlan.Item {
				act = (Act)(object)act
			});
		}

		

		[HarmonyPatch(typeof(ActPlan.Item), "Perform")]
		[HarmonyPrefix]
		public static bool Prefix(ActPlan.Item __instance) {
			var val = __instance.act as DynamicAct;
			if (val != null && val.id ==ModTextManager.Instance.GetText(eTextID.Config_Title)) {
				((Act)val).Perform();
				return false;
			}
			return true;
		}
	}
#endif
}
