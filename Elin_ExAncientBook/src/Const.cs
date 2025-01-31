using System.Numerics;

using UnityEngine;

namespace Elin_Mod
{


	public class Const
	{
		// 専用ShopType.
		public const ShopType c_ShopType_ExAncientResearcher = (ShopType)(250000);
		// 専用CurrencyType.
		public const CurrencyType c_CurrencyType_GachaCoin = (CurrencyType)(200000);
		public static readonly int c_CurrencyTypeHash_GachaCoin = c_CurrencyType_GachaCoin.ToString().ToLower().GetHashCode();
		// キャラID.
		public const string c_CharaID_AncientShop = "itukiyu_charaEx_AncientShop";
		// キャラ出現箇所.
		public const string c_SpawnZone_AncientShop = "lumiest"; //< ルミエストに出す.
		public static readonly Vector2Int c_SpawnPos_AncientShop = new Vector2Int( 77, 83 ); //< 噴水近くの端っこに出す.

		public const string c_ThingID_GachaCoin = "gacha_coin";

		public const string c_ThingID_BookAncient = "book_ancient";
		public static readonly int c_ThingIDHash_BookAncient = c_ThingID_BookAncient.GetHashCode();
		public const string c_ThingID_BookSkill = "book_skill";
		public static readonly int c_ThingIDHash_BookSkill = c_ThingID_BookSkill.GetHashCode();



	}
}
