using B83.Win32;

using HarmonyLib;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;

using static TableData;
using static ThingContainer;
using static UnityEngine.UI.GridLayoutGroup;

namespace Elin_Mod
{
	[HarmonyPatch]
	class AddFoodDistribution
	{
		static readonly int c_HashOnlyRottable = "onlyRottable".GetHashCode();

		static Window.SaveData s_LastShowDistributionWindowSaveData;
		static bool m_IsGuardRecursiveCall;
		static UIContextMenu s_LastAddToggleContextMenu;
		static Thing s_LastGetDestThing;
		static ThingContainer s_LastGetDestThingContainer;



		public static ModPatchInfo[] s_ManualPatches = new ModPatchInfo[] {
			new ModPatchInfo() {
				m_TargetType = typeof(ThingContainer),
				m_Regex = "TrySearchContainer",
				m_Postfix = CommonUtil.ToMethodInfo((Action<Card>)Postfix_TrySearchContainer),
			},
		};


		/// <summary>
		/// ローカル関数を無理やりパッチ化する対処.
		/// </summary>
		/// <param name="harmony"></param>
		public static void ApplyPatch(Harmony harmony) {
			CommonUtil.ApplyHarmonyPatches(harmony, s_ManualPatches);
		}



		/// <summary>
		/// 振り分けメニュー表示時をフック.
		/// </summary>
		[HarmonyPatch(typeof(UIInventory), "ShowDistribution")]
		[HarmonyPrefix]
		public static void Prefix(UIContextMenu dis, Window.SaveData data) {
			// 最後二振り分けメニューを表示したコンテキストメニューのセーブデータを保持.
			s_LastShowDistributionWindowSaveData = data;
		}


		/// <summary>
		/// 振り分けメニュー表示時をフック.
		/// </summary>
		[HarmonyPatch(typeof(UIInventory), "ShowDistribution")]
		[HarmonyPostfix]
		public static void Postfix(UIContextMenu dis, Window.SaveData data) {
	//		s_LastShowDistributionWindowSaveData = null;
		}

		/// <summary>
		/// コンテキストメニューのトグル追加をフック.
		/// </summary>
		[HarmonyPatch(typeof(UIContextMenu), "AddToggle")]
		[HarmonyPostfix]
		public static void Postfix(UIContextMenu __instance, string idLang = "", bool isOn = false, UnityAction<bool> action = null) {
			// もうここしかフックできそうな場所が見つからなかった...
			// 「腐敗が進むアイテムのみ入れる」が追加されたタイミングをフック.
			if (m_IsGuardRecursiveCall) {
				return;
			}
			int hash = idLang.GetHashCode();
			if ( hash == c_HashOnlyRottable) {
				s_LastAddToggleContextMenu = __instance;
				m_IsGuardRecursiveCall = true;  //< AddToggle無限ループガード.

				// 子メニュー化する？.
			//	var child = __instance.AddChild();
			//	child.popper.textName.text = ModTextManager.Instance.GetText(eTextID.);

				// 「腐敗が進まないアイテムのみ入れる」を追加.
				_AddOptionToggle(__instance, eTextID.Option_OnlyNoRottable, Const.c_FlagIndex_OnlyNoRottable, (v) => {
					if (v) {
						// 腐敗が進む～と連動して動かすためにトグルを動かす.
						if (s_LastShowDistributionWindowSaveData != null) {
							_ForceSyncToggleItem("onlyRottable".lang().ToTitleCase(), false);
						}
					}
				});

				// 【不浄】チェック.
				_AddOptionToggle(__instance, eTextID.Option_NoUndead, Const.c_FlagIndex_NoUndead );
				// 【人肉】チェック.
				_AddOptionToggle(__instance, eTextID.Option_NoHuman, Const.c_FlagIndex_NoHuman);
				// 【猫】チェック.
				_AddOptionToggle(__instance, eTextID.Option_NoCat, Const.c_FlagIndex_NoCat);

				// 無限ループガード解除.
				m_IsGuardRecursiveCall = false;
			}
		}

		/// <summary>
		/// 「腐敗が進むアイテムのみ入れる」のセッターをフック.
		/// </summary>
		[HarmonyPatch(typeof(Window.SaveData), "set_onlyRottable")]
		[HarmonyPostfix]
		public static void Postfix(Window.SaveData __instance, bool value) {
			if (value) {
				// 「腐敗が進まない～」と連動させる.
				_ForceSyncToggleItem(ModTextManager.Instance.GetText(eTextID.Option_OnlyNoRottable), false);
			}
		}

		/// <summary>
		/// トグル連動のための仕掛け.
		/// </summary>
		static void _ForceSyncToggleItem( string toggleTitle, bool isOn ) {
			if (s_LastAddToggleContextMenu == null)
				return;

			// 無理やりコンテキストメニューからGetComponentし、.
			// 設定されてるテキストを調べていく.
			// なぜかGetComponentsInChildren<T>(true)が使えなかったので、愚直にGetChild()して調べる.
			int titleHash = toggleTitle.GetHashCode();
			var tr = s_LastAddToggleContextMenu.transform;
			for ( int i = 0; i < tr.childCount; ++i ) {
				var child = tr.GetChild(i);
				var item = child.GetComponent<UIContextMenuItem>();
				if (item == null)
					continue;
				if (item.toggle == null)
					continue;
				if (item.textName.text.GetHashCode() != titleHash)
					continue;
				item.toggle.isOn = isOn;
				break;
			}
		}




		/// <summary>
		/// アイテムを出した瞬間をフック.
		/// </summary>
		[HarmonyPatch(typeof(ThingContainer), "GetDest")]
		[HarmonyPrefix]
		public static void Prefix(ThingContainer __instance, Thing t, bool tryStack) {
			// 出した瞬間のコンテナを保持.
			s_LastGetDestThing = t;
			s_LastGetDestThingContainer = __instance;
		}

		/// <summary>
		/// アイテムを出した瞬間の関数後片付け.
		/// </summary>
		[HarmonyPatch(typeof(ThingContainer), "GetDest")]
		[HarmonyPostfix]
		public static void Postfix(ThingContainer __instance, Thing t, bool tryStack, ref DestData __result) {
			// static保持の変数群を開放.
			s_LastGetDestThing = null;
			s_LastGetDestThingContainer = null;
		}

		/// <summary>
		/// GetDest()のローカル関数であるTrySearchContainer()をフック.
		/// </summary>
		static void Postfix_TrySearchContainer(Card c) {
			// c -> コンテナ.owner.
			// s_LastGetDestThing -> 出そうとしたアイテム.
			// s_LastGetDestthingContiainer -> 今チェックしているコンテナ.

			if (s_LastGetDestThing == null)
				return;
			var saveData = c?.GetWindowSaveData();
			if (saveData == null)
				return;

			// 不浄.
			if (saveData.b1[Const.c_FlagIndex_NoUndead])
				if (_IsFoodOfUndead(s_LastGetDestThing))
					_RemoveThingContainerThings(s_LastGetDestThingContainer, c);
			// 人肉.
			if (saveData.b1[Const.c_FlagIndex_NoHuman]) 
				if (_IsFoodOfHuman(s_LastGetDestThing))
					_RemoveThingContainerThings(s_LastGetDestThingContainer, c);
			// 腐敗が進まない.
			if (saveData.b1[Const.c_FlagIndex_OnlyNoRottable]) 
				if (s_LastGetDestThing.trait?.Decay != 0)
					_RemoveThingContainerThings(s_LastGetDestThingContainer, c);
			// 猫.
			if (saveData.b1[Const.c_FlagIndex_NoCat])
				if (_IsFoodOfCat(s_LastGetDestThing))
					_RemoveThingContainerThings(s_LastGetDestThingContainer, c);
		}



		/// <summary>
		/// アイテムを自動でしまうタイミングをフック.
		/// </summary>
		[HarmonyPatch(typeof(TaskDump), "ListThingsToPut")]
		[HarmonyPostfix]
		public static void Postfix(TaskDump __instance, Thing c, ref List<Thing> __result ) {
			if (c == null)
				return;
			var saveData = c.GetWindowSaveData();
			if (saveData != null) {
				// 不浄.
				if (saveData.b1[Const.c_FlagIndex_NoUndead]) 
					_RemoveThings(__result, _IsFoodOfUndead);
				// 人肉.
				if (saveData.b1[Const.c_FlagIndex_NoHuman])
					_RemoveThings(__result, _IsFoodOfHuman);
				// 猫.
				if (saveData.b1[Const.c_FlagIndex_NoCat])
					_RemoveThings(__result, _IsFoodOfCat);
				// 腐敗が進まない.
				if (saveData.b1[Const.c_FlagIndex_OnlyNoRottable])
					_RemoveThings(__result, (v) => v.trait?.Decay != 0);
			}
		}


		static bool _IsFoodOfUndead(Thing t) {
			return t.HasElement(FOOD.food_undead);
		}

		static bool _IsFoodOfHuman(Thing t) {
			return t.HasElement(FOOD.food_human);
		}

		static bool _IsFoodOfCat(Thing t) {
			return t.HasElement(FOOD.food_cat);
		}


		/// <summary>
		/// Listからthingを取り除く.
		/// </summary>
		static void _RemoveThings( List<Thing> list, System.Func<Thing, bool> funcCheck ) {
			for ( int i = 0; i < list.Count; ++i ) {
				if (!funcCheck(list[i]))
					continue;
				list.RemoveAt(i--);
			}
		}


		/// <summary>
		/// ThingContainer._listContainersからThingContainerを取り除く.
		/// </summary>
		static void _RemoveThingContainerThings( ThingContainer things, Card c ) {
			var fieldInfo = typeof(ThingContainer).GetField("_listContainers", (BindingFlags)~(0));
			var list = fieldInfo?.GetValue(things) as List<ThingContainer>;
			list?.Remove(c.things);
		}

		/// <summary>
		/// コンテキストメニューにトグルを追加.
		/// </summary>
		static UIContextMenuItem _AddOptionToggle( UIContextMenu menu, eTextID title, int index, System.Action<bool> onToggleEx=null ) {
			return GameUtil.ContextMenu_AddToggle(menu, title, _GetWindowSaveDataFlag(index), (v) => {
				_SetWindowSaveDataFlag(index, v);
				onToggleEx?.Invoke(v);
			});
		}

		/// <summary>
		/// Window.SaveData.b1[]に値をセット.
		/// </summary>
		static void _SetWindowSaveDataFlag( int index, bool v ) {

			if (s_LastShowDistributionWindowSaveData != null) {
				s_LastShowDistributionWindowSaveData.b1[index] = v;
			}
		}

		/// <summary>
		/// Window.SaveData.b1[] の値を取得.
		/// </summary>
		static bool _GetWindowSaveDataFlag( int index) {
			return s_LastShowDistributionWindowSaveData?.b1[index] ?? false;
		}

		
	}
}
