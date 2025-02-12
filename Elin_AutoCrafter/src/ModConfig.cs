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
		public ConfigEntry<bool> IsStopCanSleep { get; set; }
		public ConfigEntry<bool> IsStopHunger { get; set; }
		public ConfigEntry<bool> IsStopZeroStumina { get; set; }
		
		public override void Initialize( ConfigFile config )
		{
			IsStopCanSleep = config.Bind("General", "IsStopCanSleep", true, "Stop Condition: Can Sleep");
			IsStopHunger = config.Bind("General", "IsStopHunger", true, " Stop Condition: Hungry");
			IsStopZeroStumina = config.Bind("General", "IsStopZeroStumina", true, "Stop Condition: Stamina Zero");

			var textMng = ModTextManager.Instance;
			ModConfigMenu.Instance.AddMenu(new ModConfigMenu.MenuInfo() {
				m_TabName = textMng.GetText( eTextID.Config_Title ),
				m_Menus = new List<System.Action<UIContextMenu>>() {
					v => GameUtil.ContextMenu_AddToggle( v, eTextID.Config_IsStopCanSleep, IsStopCanSleep),
					v => GameUtil.ContextMenu_AddToggle( v, eTextID.Config_IsStopHunger, IsStopHunger),
					v => GameUtil.ContextMenu_AddToggle( v, eTextID.Config_IsStopZeroStumina, IsStopZeroStumina),
				}
			});
		}
	}
}
