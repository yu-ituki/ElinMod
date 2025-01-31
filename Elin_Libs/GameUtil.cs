﻿using BepInEx.Configuration;

using System;
using System.Collections.Generic;
using System.Security.Policy;

using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UI.GridLayoutGroup;

namespace Elin_Mod
{

	public class GameUtil
	{

		/// <summary>
		/// Trait生成.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static T CreateTraitCrafter<T>( string ownerToolID ) where T : TraitCrafter, new() {
			var dmyOwner = ThingGen.Create(ownerToolID);
			var ret = new T();
			dmyOwner.trait = ret;
			ret.SetOwner(dmyOwner);

			return ret;
		}

		/// <summary>
		/// 渡されたTrailCrafterを強制使用.
		/// </summary>
		/// <param name="trait"></param>
		public static void UseForceTraitCrafter( TraitCrafter trait ) {
			var actPlan = new ActPlan();

			actPlan.TrySetAct(trait.CrafterTitle, delegate {
				LayerDragGrid.CreateCraft(trait);
				return false;
			}, trait.owner);

			if (actPlan.list.Count > 0) {
				var act = actPlan.list[0].act;
				EClass.pc.SetAIImmediate(
					new DynamicAIAct(act.GetText(), () => act.Perform())
				) ;
			}
		}


		/// <summary>
		/// キャラクター新規生成.
		/// </summary>
		/// <typeparam name="TraitType">付与するTraitのタイプ.</typeparam>
		/// <param name="charaID"></param>
		/// <param name="zoneID"></param>
		/// <param name="x"></param>
		/// <param name="z"></param>
		/// <returns></returns>
		public static Chara CreateModChara<TraitType>( string charaID, string zoneID, int x, int z )
			where TraitType : TraitChara, new()
		{
			var zone = EClass.game.spatials.Find(zoneID);
			// キャラが既に登録されているかチェック.
			var globalCharas = EClass.game.cards.globalCharas;
			var chara = globalCharas.Find(charaID);
			if (chara == null) {
				// いなければ生成.
				chara = CharaGen.Create(charaID);
			}
			chara.SetGlobal(zone, x, z);
			if (!(chara.trait is TraitType)) {
				chara.trait = new TraitType();
				chara.trait.SetOwner(chara);
			}

			return chara;
		}


		/// <summary>
		/// ゲームのプレイ中かどうか.
		/// </summary>
		/// <returns></returns>
		public static bool IsPlayingGame()
		{
			if (!EClass.core.IsGameStarted)
			{
				return false;
			}
			if (ELayer.pc == null)
			{
				return false;
			}
			if (ELayer.pc.isDead)
			{
				return false;
			}
			return true;
		}

		public static bool IsPlayingQuest_War()
		{
			return EClass._zone?.events?.GetEvent<ZoneEventDefenseGame>() != null;
		}

		public static bool IsPlayingQuest_Harvest()
		{
			return EClass._zone?.events?.GetEvent<ZoneEventHarvest>() != null;
		}

		public static List<ZoneEvent> GetZoneEvents()
		{
			return (EClass._zone?.events)?.list;
		}

		public static string GetZoneName()
		{
			return EClass._zone?.Name;
		}

		public static bool IsZonePlayerFaction()
		{
			return ELayer._zone.IsPlayerFaction;
		}





		public static Dialog OpenDialog_1Button(eTextID text, eTextID yesText, System.Action onResult) {
			var textMng = ModTextManager.Instance;
			return OpenDialog_1Button(textMng.GetText(text), textMng.GetText(yesText), onResult);
		}

		public static Dialog OpenDialog_YesNo(eTextID text, eTextID yesText, eTextID noText, System.Action<bool> onResult) {
			var textMng = ModTextManager.Instance;
			return OpenDialog_YesNo(textMng.GetText(text), textMng.GetText(yesText), textMng.GetText(noText), onResult);
		}


		public static Dialog OpenDialog_3Button(eTextID text, eTextID text1, eTextID text2, eTextID text3, System.Action<int> onResult) {
			var textMng = ModTextManager.Instance;
			return OpenDialog_3Button(textMng.GetText(text), textMng.GetText(text1), textMng.GetText(text2), textMng.GetText(text3), onResult);
		}





		public static Dialog OpenDialog_1Button(string text, string yesText, System.Action onResult) {
			Dialog d = Layer.Create<Dialog>();
			d.textDetail.SetText(text + " ");
			d.list.AddButton(null, yesText, () => {
				onResult();
				d.Close();
			});
			ELayer.ui.AddLayer(d);
			return d;
		}

		public static Dialog OpenDialog_YesNo(string text, string yesText, string noText, System.Action<bool> onResult) {
			Dialog d = Layer.Create<Dialog>();
			d.textDetail.SetText(text + " ");
			d.list.AddButton(null, yesText, () => {
				onResult(true);
				d.Close();
			});
			d.list.AddButton(null, noText, () => {
				onResult(false);
				d.Close();
			});
			ELayer.ui.AddLayer(d);
			return d;
		}
	

		public static Dialog OpenDialog_3Button(string text, string text1, string text2, string text3, System.Action<int> onResult) {
			Dialog d = Layer.Create<Dialog>();
			d.textDetail.SetText(text + " ");
			d.list.AddButton(null, text1, () => {
				onResult(0);
				d.Close();
			});
			d.list.AddButton(null, text2, () => {
				onResult(1);
				d.Close();
			});
			d.list.AddButton(null, text3, () => {
				onResult(2);
				d.Close();
			});
			ELayer.ui.AddLayer(d);
			return d;
		}





		public static UIContextMenu CreateContextMenu() {
			return EClass.ui.CreateContextMenu("ContextMenu");
		}

		public static void ContextMenu_AddButton(UIContextMenu menu, eTextID textID, System.Action act) {
			var textMng = ModTextManager.Instance;
			menu.AddButton(() => textMng.GetText(textID), () => {
				act();
			});
		}

		public static void ContextMenu_AddSlider(UIContextMenu menu, eTextID textID, BepInEx.Configuration.ConfigEntry<float> entry, float min, float max, bool isInt) {
			var textMng = ModTextManager.Instance;
			menu.AddSlider(
					textMng.GetText(textID),
					(v) => v.ToString(),
					entry.Value,
					(v) => {
						entry.Value = v;
					},
				min, max, isInt, false, false);
		}

		public static UIContextMenuItem ContextMenu_AddToggle(UIContextMenu menu, eTextID textID, bool isDefault, System.Action<bool> act ) {
			var textMng = ModTextManager.Instance;
			return menu.AddToggle(
					textMng.GetText(textID),
					isDefault,
					(v) => {
						act(v);
					}
				);
		}


		public static int GetRand( int max ) {
			return EClass.rnd(max);
		}

		public static float GetRand( float max ) {
			return EClass.rndf(max);
		}


		public static Zone GetZone( string id ) {
			return EClass.game.spatials.Find(id);
		}


		public static void Cheat_AllItemJIdentify() {
			foreach (Thing item in EClass.pc.things) {
				item.Identify(true, IDTSource.SuperiorIdentify);
			}
		}
	}
}
