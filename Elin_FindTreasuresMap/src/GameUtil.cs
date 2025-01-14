using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using static UnityEngine.UI.GridLayoutGroup;

namespace Elin_Mod
{

	internal class GameUtil
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



		public static Dialog OpenDialog_YesNo( string text, string yesText, string noText, System.Action<bool> onResult )
		{
			Dialog d = Layer.Create<Dialog>();
			d.textDetail.SetText(text + " ");
			d.list.AddButton(null, yesText, ()=> {
				onResult(true);
				d.Close();
			});
			d.list.AddButton(null, noText, ()=>{
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


	}
}
