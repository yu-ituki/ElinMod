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
		public ConfigEntry<float> PowerUpModCost { get; set; }
		public ConfigEntry<float> CombineModCost { get; set; }

		public ConfigEntry<bool> IsEnableGunBlade { get; set; }

		public override void Initialize( ConfigFile config )
		{
			AddSlotCost = config.Bind("Regen", "AddSlotCost", 100.0f, "Cost required to add one slot.");
			PowerUpModCost = config.Bind("Regen", "PowerUpModCost", 2.0f, "Cost required to power up a mod by +1.");
			CombineModCost = config.Bind("Regen", "CombineModCost", 1.0f, "Cost required for one time combination of mod.");
			IsEnableGunBlade = config.Bind("General", "IsEnableGunBlade", false, "Whether to allow custom gunblades");


			var textMng = ModTextManager.Instance;
			ModConfigMenu.Instance.AddMenu(new ModConfigMenu.MenuInfo() {
				m_TabName = textMng.GetText(eTextID.Config_Title),
				m_Menus = new List<System.Action<UIContextMenu>>() {
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_CostAddSocket, AddSlotCost, 0, 500.0f, 1.0f),
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_CostPowerUpMod, PowerUpModCost, 0, 10.0f, 0.1f),
					menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_CostCombineMod, CombineModCost, 0, 10.0f, 0.1f),
				}
			});

		}
	}
}
