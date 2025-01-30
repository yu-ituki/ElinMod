using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace Elin_Mod
{
	/// <summary>
	/// Trait_古文書研究家.
	/// </summary>
	[HarmonyPatch]
	public class TraitMerchantEx_AncientResearcher : TraitMerchant
	{
		enum eSkillBookLv {
			High,
			Middle,
			Low,
		}

		/// <summary> 店タイプ. </summary>
		public override ShopType ShopType => Const.c_ShopType_ExAncientResearcher;

		/// <summary> 通貨. </summary>
		public override CurrencyType CurrencyType => Const.c_CurrencyType_GachaCoin;

		/// <summary> 売却可. </summary>
		public override bool AllowSell => true;

		/// <summary> 投資可. </summary>
		public override bool CanInvest => true;


		static int c_CurrencyTypeIDHash;
		
		public static void Initialize() {
			c_CurrencyTypeIDHash = Const.c_CurrencyType_GachaCoin.ToString().ToLower().GetHashCode();
			// フォールバック機構が増えたよ、やったね.
			// とりあえずパン屋にしておこう.
			ModUtil.RegisterSerializedTypeFallback(ModInfo.c_ModName, "TraitMerchantEx_AncientResearcher", "TraitMerchantBread");

		}

		public static void LoadTable() {
			//	var addCharaTable = CommonUtil.LoadTableNoneReset<SourceChara>("add_datas.xlsx", "chara");
			//	CommonUtil.AddElinTableData(addCharaTable.map, EClass.sources.charas.map);
			CommonUtil.LoadTable("add_datas.xlsx", "chara", EClass.sources.charas);
			
		}

		public static void OnGameStart() {
			// キャラが既に登録されているかチェック.
			var globalCharas = EClass.game.cards.globalCharas;
			var chara = globalCharas.Find(Const.c_CharaID_AncientShop);
			if (chara == null) {
				// いなければ生成.
				var zone = EClass.game.spatials.Find(Const.c_SpawnZone_AncientShop);
				chara = CharaGen.Create(Const.c_CharaID_AncientShop);
				var pos = Const.c_SpawnPos_AncientShop;
				chara.SetGlobal(zone, pos.x, pos.y);
			}
		}




		[HarmonyPatch(typeof(Card), "ModCurrency")]
		[HarmonyPrefix]
		public static bool Prefix_CardModCurrency(Card __instance, int a, string id) {
			if (id.GetHashCode() != c_CurrencyTypeIDHash)
				return false;


			// ガチャコインは専用支払い処理に流す.
			Plugin.Instance.MyWallet.Pay(a);

			return true;
		}



		[HarmonyPatch(typeof(Trait), "OnBarter")]
		[HarmonyPrefix]
		public static bool Prefix_TraitOnBarter(Trait __instance) {
			var trait = __instance as TraitMerchantEx_AncientResearcher;
			if (trait == null)
				return false;

			// この辺は元の処理のコピペ.
			Thing traitInventory = trait.owner.things.Find("chest_merchant");
			if (traitInventory == null) {
				traitInventory = ThingGen.Create("chest_merchant");
				trait.owner.AddThing(traitInventory);
			}
			traitInventory.c_lockLv = 0;
			if (!EClass.world.date.IsExpired(trait.owner.c_dateStockExpire)) {
				return true;
			}
			trait.owner.c_dateStockExpire = EClass.world.date.GetRaw(24 * trait.RestockDay);
			trait.owner.isRestocking = true;
			traitInventory.things.DestroyAll((Thing _t) => _t.GetInt(101) != 0);
			foreach (Thing thing7 in traitInventory.things) {
				thing7.invX = -1;
			}

			// ここからオリジナル.

			// 並べる個数を決めて.
			var config = Plugin.Instance.ModConfig;
			int maxNum = config.SalesNum_SkillBook_Max.Value;
			int minNum = config.SalesNum_SkillBook_Min.Value;
			int salesNum = EClass.rnd(maxNum - minNum) + minNum;

			// 並べる.
			CardBlueprint.SetNormalRarity();
			for ( int i = 0; i < salesNum; ++i ) {
				// 排出レベルを抽選.
				int lv = trait.ShopLv;
				switch ( _LotSkillBookLv(config) ) {
					case eSkillBookLv.High:		lv += config.SalesLv_SkillBook_High.Value; break;
					case eSkillBookLv.Middle:	lv += config.SalesLv_SkillBook_Middle.Value; break;
					case eSkillBookLv.Low:		lv += config.SalesLv_SkillBook_Low.Value; break;
				}
				lv = Mathf.Clamp(lv, 1, 99);
				
				// こいつのインベントリに突っ込む.
				Thing thing = ThingGen.Create(Const.c_ThingID_BookSkill, -1, lv);
				thing.SetNum(1);
				thing.idSkin = 0;
				traitInventory.AddThing(thing);
			}

			return true;
		}

		static eSkillBookLv _LotSkillBookLv( ModConfig config ) {
			int rateHigh = config.SalesLvLotRate_SkillBook_High.Value;
			int rateMiddle = config.SalesLvLotRate_SkillBook_Middle.Value;
			int rateLow = config.SalesLvLotRate_SkillBook_Low.Value;
			int rateSum = rateHigh + rateMiddle + rateLow;

			// 良くある全足し割合計算で抽選.
			int currentRate = GameUtil.GetRand(rateSum);
			currentRate -= rateHigh;
			if (currentRate <= 0)
				return eSkillBookLv.High;
			currentRate -= rateMiddle;
			if (currentRate <= 0)
				return eSkillBookLv.Middle;

			return eSkillBookLv.Low;
		}



		[HarmonyPatch(typeof(Card), "GetPrice")]
		[HarmonyPrefix]
		public static bool Prefix_CardGetPrice(Card __instance, CurrencyType currency, bool sell, PriceType priceType, Chara c, ref int __result ) {
			// とりあえず整数値カリング.
			if (currency != Const.c_CurrencyType_GachaCoin)
				return false;
			// あとタイプでカリング.
			var trait = c.trait as TraitMerchantEx_AncientResearcher;
			if (trait == null)
				return false;

			// 以降、古文書研究家の売買価格決定.
			__result = 0;
			var config = Plugin.Instance.ModConfig;
			if (sell) {
				// 売却(古文書以外は買わない).
				if ( __instance.id == Const.c_ThingID_BookAncient ) {
					// 売却価格は切り上げ.
					__result = Mathf.CeilToInt(__instance.refVal * config.Worth_AncientBook.Value);
				}
			} else {
				// 購入価格( 技術書以外は売らない ).
				if ( __instance.id == Const.c_ThingID_BookSkill) {
					// 購入価格は切り下げ.
					__result = Mathf.FloorToInt( __instance.GetValue() * config.Worth_SkillBook.Value );
				}
			}

			return true;
		}


	}

}
