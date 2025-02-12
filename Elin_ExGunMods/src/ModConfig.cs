using BepInEx.Configuration;
using System.Collections.Generic;

using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig : ModConfigBase
	{
		public ConfigEntry<float> ModElement_DmgFactor { get; set; }
		public ConfigEntry<float> ModBarrel_DistReductionFactor { get; set; }

		public override void Initialize( ConfigFile config )
		{
			ModElement_DmgFactor = config.Bind( "General", "ModElement_DmgFactor", 1.0f, "Coefficient of Element Damage");
			ModBarrel_DistReductionFactor = config.Bind("General", "ModBarrel_DistReductionFactor", 0.5f, "Correction factor for the effect of distance attenuation due to scope");

			var textMng = ModTextManager.Instance;
			ModConfigMenu.Instance.AddMenu(new ModConfigMenu.MenuInfo() {
				m_TabName = textMng.GetText(eTextID.Config_Title),
				m_Menus = new List<System.Action<UIContextMenu>>() {
					
					menu => GameUtil.ContextMenu_AddButton(menu, eTextID.Config_UnInstall, () => {
						menu.parent.Hide();
						GameUtil.OpenDialog_YesNo(
							textMng.GetText(eTextID.Config_UnInstallMain),
							textMng.GetText(eTextID.Yes),
							textMng.GetText(eTextID.No),
							(v) => {
								if (v) {
									if (NewRangedModManager.Instance.Uninstall()) {
										GameUtil.OpenDialog_1Button(textMng.GetText(eTextID.Config_UnInstallEnd), textMng.GetText(eTextID.Yes), () => {
											EClass.game.GotoTitle(false);
										});
									}
								}
							}
						);
					}),
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_ElemDmgFactor, ModElement_DmgFactor, 0.1f, 5.0f),
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_BarrelDistReductFactor, ModBarrel_DistReductionFactor, 0.1f, 5.0f ),
				}
			});
		}
	}
}
