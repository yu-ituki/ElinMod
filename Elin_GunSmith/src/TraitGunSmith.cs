using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	public class TraitGunSmith : TraitCrafter
	{
		public override string IdSource => "Ex_GunSmith";

		public override string CrafterTitle => "invMod";

		public override AnimeID IdAnimeProgress => AnimeID.Shiver;

		public override string idSoundProgress => "grind";

		public override string idSoundComplete => "grind_finish";

		public override int numIng => 1;

		public override bool StopSoundProgress => true;

		public override bool IsConsumeIng => false;

		public override ToggleType ToggleType => ToggleType.None;

		public override bool ShouldConsumeIng(SourceRecipe.Row item, int index) {
			return false;
		}

		public override bool IsCraftIngredient(Card c, int idx) {
			var ret = base.IsCraftIngredient(c, idx);
			if ( ret ) {
				if (c.trait is TraitThrown)
					return false;

				// ガンランスとゼフィールどうしようかなって思ったけど、まあ良いか.
				if ( !( c.trait is TraitMod ) ) {
					// Modでなくてかつrangedカテゴリでもない特殊物...
					if (!c.category.IsChildOf("ranged")) {
						ret = false;
						// コンフィグでONになってるときだけONにする.
						if (Plugin.Instance.ModConfig.IsEnableGunBlade.Value)
							ret = true;
					}
				}
			}
			return ret;
		}

		public override Thing Craft(AI_UseCrafter ai) {
			var textMng = ModTextManager.Instance;
			var target = ai.ings[0];
			var targetCard = target as Card;
			if (targetCard.encLV >= 99) {
				Msg.SayRaw(textMng.GetText(eTextID.Error_MaxLv));
				return null;
			}

			var config = Plugin.Instance.ModConfig;

			// 手持ちプラチナコイン数取得.
			int haveCost = EClass.pc.GetCurrency(Const.c_UseCurrencyType);


			// Modである.
			if ( target.trait is TraitMod ) {
				// コスト計算.
				int currentLv = targetCard.encLV;
				int useCost = Mathf.FloorToInt(currentLv * config.PowerUpModCost.Value);

				textMng.SetUserData(0, useCost);
				textMng.SetUserData(1, haveCost);
				var bodyText = textMng.GetText(eTextID.Dialog_PowerUpModQ);
				var yesText = textMng.GetText(eTextID.Yes);
				var noText = textMng.GetText(eTextID.No);
				GameUtil.OpenDialog_YesNo(bodyText, yesText, noText, (v) => {
					if (v) {
						// 支払いチェック.
						if (EClass.pc.TryPay(useCost, Const.c_UseCurrencyType)) {
							// +値操作.
							targetCard.ModEncLv(1);
						}
					}
				});
			}
			// 銃である.
			else {
				// スロット追加コスト計算.
				int slotNum = targetCard.sockets.Count;
				int useCost = Mathf.FloorToInt(slotNum * config.AddSlotCost.Value);
				
				textMng.SetUserData(0, useCost);
				textMng.SetUserData(1, haveCost);
				var bodyText = textMng.GetText(eTextID.Dialog_GunSmithGunQ);
				var text1 = textMng.GetText(eTextID.MenuTitle_AddModSocket);
				var text2 = textMng.GetText(eTextID.MenuTitle_ClearMod);
				var text3 = textMng.GetText(eTextID.Cancel);
				GameUtil.OpenDialog_3Button(bodyText, text1, text2, text3, (v) => {
					switch (v) {
						case 0:
							// 支払いチェック.
							if (EClass.pc.TryPay(useCost, Const.c_UseCurrencyType)) {
								// ソケット追加.
								ai.ings[0].AddSocket();
							}
							break;
						case 1:
							// Mod取り外し.
							target.EjectSockets();
							break;
					}
				});
			}



			return null;
		}

	}
}
