using HarmonyLib;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static Lang;

namespace Elin_Mod
{
	[HarmonyLib.HarmonyPatch]
	public class EatShortCut
	{
		[HarmonyPatch(typeof(WidgetHotbar), "SetShortcutMenu")]
		[HarmonyPostfix]
		public static void Postfix_SetShortcutMenu(WidgetHotbar __instance, ButtonHotItem b, UIContextMenu m) {
			m.AddButton(ModTextManager.Instance.GetText(eTextID.Msg_Title), ()=>
			{
				__instance.SetItem(b, new HotItemActionEat());
			});

		}


	}


	public class HotItemActionEat : HotAction
	{
		public override string Id => "Eat";
		public override string Name => ModTextManager.Instance.GetText(eTextID.Msg_Title);

		public override string pathSprite => "icons_48_11";	//< 飯アイコン.

		public override bool CanName => false;

		public override void Perform() {

			var textMng = ModTextManager.Instance;
			var config = Plugin.Instance.ModConfig;
			var pc = EClass.pc;

			// 食事停止ステータスチェック.
			var stopEatState = config.StopEatState.Value;
			int orgEatState = 0;
			switch (stopEatState) {
				case ModConfig.eHungerState.Normal: orgEatState = StatsHunger.Normal; break;
				case ModConfig.eHungerState.Hungry: orgEatState = StatsHunger.Hungry; break;
				case ModConfig.eHungerState.VeryHungry: orgEatState = StatsHunger.VeryHungry; break;
				case ModConfig.eHungerState.Starving: orgEatState = StatsHunger.Starving; break;
				case ModConfig.eHungerState.Bloated: orgEatState = StatsHunger.Bloated; break;
				case ModConfig.eHungerState.Filled: orgEatState = StatsHunger.Filled; break;
			}
			if (pc.hunger.GetPhase() <= orgEatState) {
				textMng.SetUserData(0, Const.s_ConfigTexts_StopEatState[orgEatState]);
				var stopText = textMng.GetText(eTextID.Msg_Stop);
				Msg.SayRaw(stopText);
				return;
			}

			// 飯リストアップ.
			Thing food = null;
			var foods = pc.things.List((Thing a) => pc.CanEat(a, shouldEat: true) && !a.c_isImportant);
			if (foods.Count <= 0) {
				// 飯ねンだわ.
				Msg.SayRaw(textMng.GetText(eTextID.Msg_NoneFood));
				return;
			}

			// 食事優先度チェック.
			switch ( config.EatPriority.Value) {
				case ModConfig.eEatPriority.Normal:
					break;

				case ModConfig.eEatPriority.HighLER:
					_SortThings(foods, v => v.LER);
					break;

				case ModConfig.eEatPriority.HighSTR:
					_SortThings(foods, v => v.STR);
					break;

				case ModConfig.eEatPriority.HighWIL:
					_SortThings(foods, v => v.WIL);
					break;

				case ModConfig.eEatPriority.HighMAG:
					_SortThings(foods, v => v.MAG);
					break;

				case ModConfig.eEatPriority.HighPER:
					_SortThings(foods, v => v.PER);
					break;

				case ModConfig.eEatPriority.HighCHA:
					_SortThings(foods, v => v.CHA);
					break;

				case ModConfig.eEatPriority.HighDEX:
					_SortThings(foods, v => v.DEX);
					break;

				case ModConfig.eEatPriority.HighEND:
					_SortThings(foods, v => v.END);
					break;

				case ModConfig.eEatPriority.HighNutrition:
					_SortThings(foods, v => v.Evalue(10));
					break;

		//		case ModConfig.eEatPriority.HighPrice:
		//			_SortThings(foods, v => v.GetPrice());
		//			break;

				case ModConfig.eEatPriority.LowNutrition:
					_SortThings(foods, v => -v.Evalue(10));
					break;

		//		case ModConfig.eEatPriority.LowPrice:
		//			_SortThings(foods, v => -v.GetPrice());
		//			break;
			}

			food = foods[0];

			// 食う.
			if ( config.IsInstantEat.Value )
				pc.InstantEat(null, true);
			else 
				pc.SetAI(new AI_Eat() { target = food });
		}

		static void _SortThings( List<Thing> list, System.Func<Thing, int> func ) {
			var config = Plugin.Instance.ModConfig;
			list.Sort((a, b) => {
				int aa = func(a);
				int bb = func(b);
				if (aa > bb)
					return -1;
				else if ( aa < bb )
					return 1;
				return 0;

			});
		//	DebugUtil.LogError($"{config.EatPriority.Value} : {func(list[0])} : {func ( list[list.Count - 1] )}");
		}
	}

}
