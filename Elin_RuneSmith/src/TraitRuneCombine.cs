using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using UnityEngine;

namespace Elin_Mod
{
	public class TraitRuneCombine : TraitCrafter
	{
		public override string IdSource => "Ex_RuneCombine";

		public override string CrafterTitle => "invMod";

		public override AnimeID IdAnimeProgress => AnimeID.Shiver;

		public override string idSoundProgress => "grind";

		public override string idSoundComplete => "grind_finish";

		public override int numIng => 2;

		public override bool StopSoundProgress => true;

		public override bool IsConsumeIng => false;

		public override ToggleType ToggleType => ToggleType.None;

		public override bool ShouldConsumeIng(SourceRecipe.Row item, int index) {
			return false;
		}


		public override bool IsCraftIngredient(Card c, int idx) {
			var ret = base.IsCraftIngredient(c, idx);
			switch ( idx) {
				case 0:
					break;
				case 1:
				default:
					if ( ret ) {
						ret = false;
						// 0番と同一の種別か調べる.
						var mod1 = LayerDragGrid.Instance.buttons[0].Card?.trait as TraitRune;
						var mod2 = c.trait as TraitRune;
						if (mod1 != null && mod2 != null) {
							ret = mod1.source.id == mod2.source.id;
						}
					}
					break;
			}
			return ret;
		}


		public override Thing Craft(AI_UseCrafter ai) {
			var textMng = ModTextManager.Instance;
			var target1Card = ai.ings[0] as Card;
			var target2Card = ai.ings[1] as Card;

			Card targetCard = target1Card;
			Card baitCard = target2Card;
			if ( target1Card.encLV < target2Card.encLV ) {
				targetCard = target2Card;
				baitCard = target1Card;
			}

			if (targetCard.encLV >= 999) {
				Msg.SayRaw(textMng.GetText(eTextID.Error_MaxLv));
				return null;
			}

			var config = Plugin.Instance.ModConfig;

			// 手持ちプラチナコイン数取得.
			int haveCost = EClass.pc.GetCurrency(Const.c_UseCurrencyType);

			// コスト計算.
			int currentLv = targetCard.encLV;
			int combineCost = Mathf.FloorToInt(currentLv * config.CombineRuneCost.Value);

			textMng.SetUserData(0, combineCost);
			textMng.SetUserData(1, haveCost);
			var bodyText = textMng.GetText(eTextID.Dialog_CombineRuneQ);
			var yes = textMng.GetText(eTextID.Yes);
			var no = textMng.GetText(eTextID.No);
			GameUtil.OpenDialog_YesNo(bodyText, yes, no, (v) => {
				if (v) {
					// 支払いチェック.
					if (EClass.pc.TryPay(combineCost, Const.c_UseCurrencyType)) {
						// +値操作.
						targetCard.ModEncLv(1);
						// 餌パーツ2削除.
						baitCard.Destroy();
					}
				}
			});

			return null;
		}
	}
}
