using BepInEx.Configuration;

using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Modコンフィグ用.
	/// </summary>
	public class ModConfig : ModConfigBase
	{
		public ConfigEntry<int> Worth_GachaCoin_Copper { get; set; }
		public ConfigEntry<int> Worth_GachaCoin_Silver { get; set; }
		public ConfigEntry<int> Worth_GachaCoin_Gold { get; set; }
		public ConfigEntry<int> Worth_GachaCoin_Plat { get; set; }

		public ConfigEntry<float> Worth_AncientBook { get; set; }
		public ConfigEntry<float> Worth_SkillBook { get; set; }


		public ConfigEntry<int> SalesNum_SkillBook_Base { get; set; }
		public ConfigEntry<int> SalesNum_SkillBook_Add { get; set; }

		public ConfigEntry<int> SalesLvLotRate_SkillBook_High { get; set; }
		public ConfigEntry<int> SalesLvLotRate_SkillBook_Middle { get; set; }
		public ConfigEntry<int> SalesLvLotRate_SkillBook_Low { get; set; }

		public ConfigEntry<int> SalesLv_SkillBook_High { get; set; }
		public ConfigEntry<int> SalesLv_SkillBook_Middle { get; set; }
		public ConfigEntry<int> SalesLv_SkillBook_Low { get; set; }




		public override void Initialize( ConfigFile config ) 
		{
			Worth_GachaCoin_Copper = config.Bind( "General", "Worth_GachaCoin_Copper", 1, "Gacha CoinData \"Copper\" Value per CoinData. If zero is specified, it is not used..");
			Worth_GachaCoin_Silver = config.Bind("General", "Worth_GachaCoin_Silver", 10, "Gacha CoinData \"Silver\" Value per CoinData. If zero is specified, it is not used..");
			Worth_GachaCoin_Gold = config.Bind("General", "Worth_GachaCoin_Gold", 50, "Gacha CoinData \"Gold\" Value per CoinData. If zero is specified, it is not used.");
			Worth_GachaCoin_Plat = config.Bind("General", "Worth_GachaCoin_Plat", 200, "Gacha CoinData \"Plat\" Value per CoinData. If zero is specified, it is not used.");
			Worth_AncientBook = config.Bind("General", "Worth Ancient Book", 5.0f, "Sale price coefficient of \"ancient book\". Multiplied by the level.");
			Worth_SkillBook = config.Bind("General", "Worth Skill Book", 0.1f, "Buy price coefficient of \"skill book\". Multiplied by the base worth.");

			SalesNum_SkillBook_Base = config.Bind("General", "Minimum display count of technical books for sale.", 5, "Minimum display count of technical books for sale.");
			SalesNum_SkillBook_Add = config.Bind("General", "Additional draw display count for technical books for sale.", 5, "Additional draw display count for technical books for sale.");

			SalesLvLotRate_SkillBook_High = config.Bind("General", "High-level technical book sales Lottery probability", 200, "High-level technical book sales Lottery probability.");
			SalesLvLotRate_SkillBook_Middle = config.Bind("General", "Middle-level technical book sales Lottery probability", 400, "Middle-level technical book sales Lottery probability.");
			SalesLvLotRate_SkillBook_Low = config.Bind("General", "Low-level technical book sales Lottery probability", 300, "Low-level technical book sales Lottery probability.");

			SalesLv_SkillBook_High = config.Bind("General", "Bulk level value of high-level technical books", 10, "Bulk level value of high-level technical books.");
			SalesLv_SkillBook_Middle = config.Bind("General", "Bulk level value of middle-level technical books", 0, "Bulk level value of middle-level technical books.");
			SalesLv_SkillBook_Low = config.Bind("General", "Bulk level value of low-level technical books", -10, "Bulk level value of low-level technical books.");
		}
	}
}
