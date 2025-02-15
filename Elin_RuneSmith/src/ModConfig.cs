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
		public ConfigEntry<float> AddSlotCost { get; set; }
		public ConfigEntry<float> PowerUpRuneCost { get; set; }
		public ConfigEntry<float> CombineRuneCost { get; set; }

		
		public override void Initialize( ConfigFile config )
		{
			AddSlotCost = config.Bind("Regen", "AddSlotCost", 50f, "Cost required to add one slot.");
			PowerUpRuneCost = config.Bind("Regen", "PowerUpRuneCost", 2.0f, "Cost required to power up a rune by +1.");
			CombineRuneCost = config.Bind("Regen", "CombineRuneCost", 1.0f, "Cost required for one time combination of rune.");
			
			var textMng = ModTextManager.Instance;
			ModConfigMenu.Instance.AddMenu(new ModConfigMenu.MenuInfo() {
				m_TabName = textMng.GetText(eTextID.Config_Title),
				m_Menus = new List<System.Action<UIContextMenu>>() {
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_CostAddSocket, AddSlotCost, 0, 500.0f, 1.0f),
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_CostPowerUpRune, PowerUpRuneCost, 0, 10.0f, 0.1f),
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_CostCombineRune, CombineRuneCost, 0, 10.0f, 0.1f),
				}
			});

		}
	}
}
