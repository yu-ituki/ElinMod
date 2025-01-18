using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using static ActPlan;
using static TextureReplace;
using static UnityEngine.UI.GridLayoutGroup;

#if true
namespace Elin_Mod
{
	[HarmonyPatch]
	class ElinOverrides
	{
		// HarmonyPatch置き場.

#if false
		[HarmonyPatch(typeof(ElementContainer), "AddNote")]
		[HarmonyPrefix]
		public static void Prefix(ElementContainer __instance, UINote n, Func<Element, bool> isValid = null, Action onAdd = null, ElementContainer.NoteMode mode = ElementContainer.NoteMode.Default, bool addRaceFeat = false, Func<Element, string, string> funcText = null, Action<UINote, Element> onAddNote = null) {
			List<Element> list = new List<Element>();
			foreach (Element value2 in __instance.dict.Values) {
				if ((isValid == null || isValid(value2)) && (mode != ElementContainer.NoteMode.CharaMake || value2.ValueWithoutLink != 0) && (value2.Value != 0 || mode == ElementContainer.NoteMode.CharaMakeAttributes) && (!value2.HasTag("hidden") || EClass.debug.showExtra)) {
					list.Add(value2);
				}
			}
			if (addRaceFeat) {
				Element element = Element.Create(29, 1);
				element.owner = __instance;
				list.Add(element);
			}
			DebugUtil.LogError("!!!!!!!!!!!!!! " + list.Count);
			if (list.Count == 0) {
				return;
			}
			onAdd?.Invoke();
			switch (mode) {
				case ElementContainer.NoteMode.CharaMake:
				case ElementContainer.NoteMode.CharaMakeAttributes:
					list.Sort((Element a, Element b) => a.GetSortVal(UIList.SortMode.ByElementParent) - b.GetSortVal(UIList.SortMode.ByElementParent));
					break;
				case ElementContainer.NoteMode.Trait:
					list.Sort((Element a, Element b) => ElementContainer.GetSortVal(b) - ElementContainer.GetSortVal(a));
					break;
				default:
					list.Sort((Element a, Element b) => a.SortVal() - b.SortVal());
					break;
			}
			string text = "";
			foreach (Element e in list) {
				switch (mode) {
					case ElementContainer.NoteMode.Domain:
						n.AddText(e.Name, FontColor.Default);
						continue;
					case ElementContainer.NoteMode.Default:
					case ElementContainer.NoteMode.Trait: {
							bool flag = e.source.tag.Contains("common");
							string categorySub = e.source.categorySub;
							bool flag2 = false;
							bool flag3 = (e.source.tag.Contains("neg") ? (e.Value > 0) : (e.Value < 0));
							int num = Mathf.Abs(e.Value);
							bool flag4 = __instance.Card != null && __instance.Card.ShowFoodEnc;
							bool flag5 = __instance.Card != null && __instance.Card.IsWeapon && e is Ability;


							DebugUtil.LogError(
								$"{e.id} : {flag} : {flag2} : {flag3} : {flag4} : {flag5} : {e.IsTrait} : {categorySub} : {e.source.textPhase} : {e.source.id} : {e.Value}"
							);
							break;
						}
				}
			}
		}
#endif

	}
}
#endif
