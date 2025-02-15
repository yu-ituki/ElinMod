using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.UI;
using UnityEngine;

using static ActPlan;

namespace Elin_Mod
{
	public class TraitRuneSmith : TraitCrafter
	{
		public override string IdSource => "Ex_RuneSmith";

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


		public override Thing Craft(AI_UseCrafter ai) {
			var textMng = ModTextManager.Instance;
			var target = ai.ings[0];
			var targetCard = target as Card;
			var config = Plugin.Instance.ModConfig;

			// 手持ちプラチナコイン数取得.
			int haveCost = EClass.pc.GetCurrency(Const.c_UseCurrencyType);


			// Runeである.
			if ( target.trait is TraitRune ) {
				// コスト計算.
				int currentLv = targetCard.encLV;
				int usePowerUpCost = Mathf.FloorToInt(currentLv * config.PowerUpRuneCost.Value);
				
				textMng.SetUserData(0, usePowerUpCost);
				textMng.SetUserData(1, haveCost);
				var bodyText = textMng.GetText(eTextID.Dialog_PowerUpRuneQ);
				var yesText = textMng.GetText(eTextID.Yes);
				var noText = textMng.GetText(eTextID.No);
				GameUtil.OpenDialog_YesNo(bodyText, yesText, noText, (v) => {
					if (v) {
						// 支払いチェック.
						if (EClass.pc.TryPay(usePowerUpCost, Const.c_UseCurrencyType)) {
							// +値操作.
							targetCard.ModEncLv(1);
						}
					}
				});
			}
			// 装備である.
			else {
				// スロット追加コスト計算.
				int slotNum = targetCard.socketList?.Count ?? 0;
				int useCost = Mathf.FloorToInt(slotNum * config.AddSlotCost.Value);

				var listRune = targetCard.elements.ListRune();

				textMng.SetUserData(0, useCost);
				textMng.SetUserData(1, haveCost);
				var bodyText = textMng.GetText(eTextID.Dialog_RuneSmithEquipQ);
				var text1 = textMng.GetText(eTextID.MenuTitle_AddRuneSocket);
				var text2 = textMng.GetText(eTextID.MenuTitle_ClearAllRune);
				var text3 = textMng.GetText(eTextID.MenuTitle_ClearOneRune);
				var text4 = textMng.GetText(eTextID.Cancel);
				GameUtil.OpenDialog_Buttons(bodyText, (v) => {
					switch (v) {
						case 0:
							// 支払いチェック.
							if (EClass.pc.TryPay(useCost, Const.c_UseCurrencyType)) {
								if (targetCard.socketList == null)
									targetCard.socketList = new List<SocketData>();
								// ソケット追加.
								ai.ings[0].elements.ModBase(Const.c_RuneSlotElem, 1);
							}
							break;
						case 1:
							// Rune全取り外し.
							for ( int i = 0; i < listRune.Count; ++i ) {
								var rune = listRune[i];
								if (!_PopRune(targetCard, rune))
									continue;
								if ( targetCard.elements.list.Remove(rune.id) )
									--i;
							}
							EClass.pc.PlaySound("intonation");
							EClass.pc.PlayEffect("intonation");
							break;

						case 2:
							EClass.ui.AddLayer<LayerList>().SetList2(listRune, (Element a) => a.Name, delegate (Element a, ItemGeneral b)
							{
								_PopRune(targetCard, a);
								EClass.pc.PlaySound("intonation");
								EClass.pc.PlayEffect("intonation");
							}, delegate (Element a, ItemGeneral b)
							{
								b.SetSubText((a.vBase + a.vSource).ToString() ?? "", 200, FontColor.Default, UnityEngine.TextAnchor.MiddleRight);
								b.Build();
								if (a.HasTag("noRune")) {
									b.button1.interactable = false;
									b.button1.mainText.gameObject.AddComponent<CanvasGroup>().alpha = 0.5f;
								}
							}).SetSize(500f)
				.SetOnKill(delegate {
				})
				.SetTitles("wRuneMold");
							break;
					}
				}, text1, text2, text3, text4 );
			}



			return null;
		}


		bool _PopRune( Card baseCard, Element elem ) {
			if (elem.HasTag(Const.c_Tag_ModRange))
				return false;
			Thing thing8 = ThingGen.Create("rune");
			thing8.ChangeMaterial(baseCard.material);
			thing8.refVal = elem.id;
			thing8.encLV = elem.vBase + elem.vSource;
			EClass.pc.Pick(thing8);

			baseCard.elements.Remove(elem.id);
			return true;
		}
	}
}
