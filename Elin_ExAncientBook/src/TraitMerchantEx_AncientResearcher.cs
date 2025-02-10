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

		public override PriceType PriceType => PriceType.Default;

		/// <summary> 売却可. </summary>
		public override bool AllowSell => true;

		/// <summary> 投資可. </summary>
		public override bool CanInvest => true;


		static bool s_IsOpenBatterMenu;


		public static void Initialize() {
			// フォールバック機構が増えたよ、やったね.
			// とりあえずパン屋にしておこう.
			ModUtil.RegisterSerializedTypeFallback(ModInfo.c_ModName, "TraitMerchantEx_AncientResearcher", "TraitMerchantBread");
		}

		public static void LoadCards() {
			CommonUtil.LoadTable("add_datas.xlsx", "chara", EClass.sources.charas);
		}

		public static void OnStartGame() {
			GameUtil.CreateModChara<TraitMerchantEx_AncientResearcher>(
				Const.c_CharaID_AncientShop,
				Const.c_SpawnZone_AncientShop,
				Const.c_SpawnPos_AncientShop.x,
				Const.c_SpawnPos_AncientShop.y
			);
		}

		/// <summary>
		/// ◯円の表記部分.
		/// </summary>
		[HarmonyPatch(typeof(Lang), "_currency", new Type[] { typeof(object), typeof(string) })]
		[HarmonyPrefix]
		public static bool Prefix_Lang_currency(object a, string IDCurrency, ref string __result) {
			if (!s_IsOpenBatterMenu)
				return true;

			if (IDCurrency.GetHashCode() != Const.c_CurrencyTypeHash_GachaCoin)
				return true;
			ModTextManager.Instance.SetUserData(0, a.ToString());
			__result = ModTextManager.Instance.GetText(eTextID.Say_HaveCoin);
			return false;
			//return  ("u_currency_" + IDCurrency).lang($"{a:#,0}");
		}

		/// <summary>
		/// 所持金額の取得.
		/// </summary>
		[HarmonyPatch(typeof(Card), "GetCurrency")]
		[HarmonyPrefix]
		public static bool Prefix_CardGetCurrency(Card __instance, string id, ref int __result ) {
			if (!s_IsOpenBatterMenu)
				return true;
			if (id.GetHashCode() != Const.c_CurrencyTypeHash_GachaCoin)
				return true;
			if (__instance == EClass.pc) {
				__result = Plugin.Instance.MyWallet.GetHave();  //< 重かったら直す.
				return false;
			} else {
				return true;	//< TODO...
			}
		}

		/// <summary>
		/// 所持金変動.
		/// </summary>
		[HarmonyPatch(typeof(Card), "ModCurrency")]
		[HarmonyPrefix]
		public static bool Prefix_CardModCurrency(Card __instance, int a, string id) {
			if (id.GetHashCode() != Const.c_CurrencyTypeHash_GachaCoin)
				return true;

		//	DebugUtil.LogError( $" mod currency {a} : {id}", true );

			// ガチャコインは専用処理に流す.
			var wallet = Plugin.Instance.MyWallet;
			if ( a > 0 ) {
				wallet.Add(a);
			} else {
				wallet.Pay(-a);
			}
			return false;
		}

		/// <summary>
		/// 支払い処理を閉じる.
		/// </summary>
		[HarmonyPatch(typeof(ShopTransaction), "OnEndTransaction")]
		[HarmonyPostfix]
		public static void Postfix_ShopClose() {
		//	DebugUtil.LogError("!!!! Close Shop !!!!");
			s_IsOpenBatterMenu = false;
		}

		/// <summary>
		/// ショップメニューを開く.
		/// </summary>
		[HarmonyPatch(typeof(Trait), "OnBarter")]
		[HarmonyPrefix]
		public static bool Prefix_TraitOnBarter(Trait __instance) {
			var trait = __instance as TraitMerchantEx_AncientResearcher;
			if (trait == null)
				return true;

			// 現在の手持ち金額をログに流す.
			s_IsOpenBatterMenu = true;
			ModTextManager.Instance.SetUserData(0, Plugin.Instance.MyWallet.GetHave());
			Msg.SayRaw(ModTextManager.Instance.GetText(eTextID.Msg_HaveCoin));

			//-----
			// この辺は元の処理のコピペ.
			//-----
			// 商人用のチェストを追加.
			Thing traitInventory = trait.owner.things.Find("chest_merchant");
			if (traitInventory == null) {
				traitInventory = ThingGen.Create("chest_merchant");
				trait.owner.AddThing(traitInventory);
			}
			traitInventory.c_lockLv = 0;
			if (!EClass.world.date.IsExpired(trait.owner.c_dateStockExpire)) {
				return false;	//< 期限チェック.
			}
			// 新規期限設定.
			trait.owner.c_dateStockExpire = EClass.world.date.GetRaw(24 * trait.RestockDay);
			// チェストを一度空にする.
			trait.owner.isRestocking = true;
			traitInventory.things.DestroyAll((Thing _t) => _t.GetInt(101) != 0);
			foreach (Thing thing7 in traitInventory.things) {
				thing7.invX = -1;
			}

			//-----
			// ここまでコピペ.
			//-----
			// ここからオリジナル.

			// 並べる個数を決めて.
			var config = Plugin.Instance.ModConfig;
			int addNum = config.SalesNum_SkillBook_Add.Value;
			int baseNum = config.SalesNum_SkillBook_Base.Value;
			int salesNum = EClass.rnd(addNum) + baseNum;

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

			return false;
		}

		/// <summary> 技術書抽選処理. </summary>
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

		/// <summary>
		/// 手持ち金のUI.
		/// </summary>
		/// <param name="__instance"></param>
		[HarmonyPatch(typeof(UICurrency), "Build", new Type[] {})]
		[HarmonyPostfix]
		public static void Postfix_UICurrencyBuild(UICurrency __instance ) {
			if (!s_IsOpenBatterMenu)
				return;
			__instance.Add(__instance.icons[0], Const.c_ThingID_GachaCoin, () => { return Plugin.Instance.MyWallet.GetHave().ToString(); });
			__instance.layout.RebuildLayout();
		}

		/// <summary>
		/// Cardの値段チェック時処理.
		/// </summary>
		[HarmonyPatch(typeof(Card), "GetPrice")]
		[HarmonyPrefix]
		public static bool Prefix_CardGetPrice(Card __instance, CurrencyType currency, bool sell, PriceType priceType, Chara c, ref int __result ) {
			if (!s_IsOpenBatterMenu)
				return true;

			// とりあえず整数値カリング.
			if (currency != Const.c_CurrencyType_GachaCoin)
				return true;
#if false
			// あとタイプでカリング.
			var trait = c.trait as TraitMerchantEx_AncientResearcher;
			if (trait == null)
				return false;
#endif
			// 以降、古文書研究家の売買価格決定.
			__result = _CalcPrice(__instance, sell);
			
			return false;
		}


		static int _CalcPrice( Card card, bool isSell ) {
			int ret = 0;
			var config = Plugin.Instance.ModConfig;
			int idHash = card.id.GetHashCode();
			if (isSell) {
				// 売却は古文書のみ.
				if (idHash == Const.c_ThingIDHash_BookAncient) {
					// 売却価格は切り上げ.
					ret = Mathf.CeilToInt((card.refVal + 1 ) * config.Worth_AncientBook.Value);
			//		DebugUtil.LogError($"{ret} = {card.refVal} : {config.Worth_AncientBook.Value}");
				}
			} else {
				// 購入は技術書のみ.
				if (idHash == Const.c_ThingIDHash_BookSkill) {
					// 購入価格は切り下げ.
					ret = Mathf.FloorToInt(card.GetValue() * config.Worth_SkillBook.Value);
			//		DebugUtil.LogError($"{ret} = {card.GetValue()} : {config.Worth_SkillBook.Value}");
				}
				// 売った古文書もあったわ.
				if ( idHash == Const.c_ThingIDHash_BookAncient ) {
					ret = Mathf.CeilToInt((card.refVal + 1) * config.Worth_AncientBook.Value); //< 同額かな...
				}
			}
			return ret;
		}

	}

}
