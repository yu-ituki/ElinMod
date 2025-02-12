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
		//public ConfigEntry<KeyCode> ActiveKey { get; set; }

		public override void Initialize( ConfigFile config )
		{
#if false
			ActiveKey = config.Bind( "General", "Key_Activation", (KeyCode)108, "Key to start and stop autoexplore." );

			var textMng = ModTextManager.Instance;
			ModConfigMenu.Instance.AddMenu(new ModConfigMenu.MenuInfo() {
				m_TabName = textMng.GetText(eTextID.Config_Title),
				m_Menus = new List<System.Action<UIContextMenu>>() {
					//menu => GameUtil.ContextMenu_AddSlider(menu, eTextID.Config_Worth_GachaCoin_Copper, Worth_GachaCoin_Copper, 0, 1000),
				}
			});
#endif
		}
	}
}
