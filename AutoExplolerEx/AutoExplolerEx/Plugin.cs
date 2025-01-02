using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Core.Logging.Interpolation;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace Elin_AutoExplore
{

	[BepInPlugin("yuof.elin.autoExplore.mod", "Elin AutoExplorer", "1.2.0.0")]
	public class Plugin : BaseUnityPlugin
	{
		private enum State
		{
			Starting,
			Idle,
			Exploring,
			Combat,
			Resting,
			Finished
		}

		private bool isEnable = false;

		private State state = State.Idle;

		private AIActionFinder actionFinder = null;

		private Chara playerCharacter => ELayer.pc;

		private Point currentPos => ((Card)playerCharacter).pos;

		private MapBounds currentBounds => ELayer._map.bounds;

		public AutoExplorerConfig AutoExplorerConfig { get; private set; } = null;

		public static Plugin Instance { get; private set; }

		public IgnoreList IgnoreList { get; private set; } = null;

		private void Awake() {
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			Harmony val = new Harmony("yuof.elin.autoExplore.mod");
			val.PatchAll();
			Instance = this;
			AutoExplorerConfig = new AutoExplorerConfig(((BaseUnityPlugin)this).Config);
			IgnoreList = new IgnoreList(AutoExplorerConfig.GatheringRestrictionList, AutoExplorerConfig.MiningRestrictionList);
			actionFinder = new AIActionFinder();
			ExUtil.SetLogger(Logger);
		}

		private void Unload() {
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			Harmony val = new Harmony("yuof.elin.autoExplore.mod");
			val.UnpatchSelf();
		}

		void _Dump<T>( string saveName, List<T> list, Dictionary<string, T> dic )
			where T : RenderRow
		{
			string tmp = "";
			foreach (var itr in dic) {
				var a = itr.Value;
				tmp += itr.Key;
				tmp += $",{a.name_JP}";
				tmp += $",{a.category}";
				tmp += $",{a.detail_JP}";
				for (int j = 0; j < a.tag.Length; ++j) {
					tmp += $",{a.tag[j]}";
				}

				tmp += "\n";
			}
			foreach (var itr in list) {
				var a = itr;
				tmp += $",{a.name_JP}";
				tmp += $",{a.category}";
				tmp += $",{a.detail_JP}";
				for (int j = 0; j < a.tag.Length; ++j) {
					tmp += $",{a.tag[j]}";
				}

				tmp += "\n";
			}

			string savePath = $"D:\\{saveName}.csv";
			ExUtil.DumpText(savePath, tmp);
		}

		public void Update() {

#if false
			if (Input.GetKeyDown(KeyCode.F10)) {
				
				_Dump("food", EClass.sources.foods.rows, EClass.sources.foods.map);
				_Dump("card", EClass.sources.cards.rows, EClass.sources.cards.map);
			}
#endif


			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Invalid comparison between Unknown and I4
			HandleInput();
			if (!isEnable) {
				return;
			}


			
			if (!EClass.core.IsGameStarted || playerCharacter == null || playerCharacter.isDead) {
				isEnable = false;
				return;
			}
			if ( ((Spatial)ELayer._zone).IsPlayerFaction ) {
				if (!AutoExplorerConfig.HandleVegetables.Value) {
					return;
				}
			}

			if (InventoryIsFull()) {
				Logger.LogWarning((object)"Inventory is full. Stopping autoExplore.");
				isEnable = false;
				Msg.Say("returnOverweight");
				return;
			}
			if (Drowning()) {
				Logger.LogWarning((object)"Drowning. Stopping autoExplore.");
				isEnable = false;
				Msg.Say("regionAbortMove");
				return;
			}
			if ((int)playerCharacter.ai.status == 1 && state != 0) {
				Logger.LogWarning((object)("Current AIAct failed!. " + playerCharacter.ai.Name));
				AIAct ai = playerCharacter.ai;
				if (HookUserInteraction.UserCanceledAiActs.Contains(ai)) {
					Logger.LogWarning((object)"User canceled AIAct. Stopping autoExplore.");
					isEnable = false;
					return;
				}
				if (!ai.IsMoveAI) {
					Logger.LogWarning((object)"Non-move AIAct failed. Stopping autoExplore.");
				}
			}
			if (((Card)playerCharacter).IsDeadOrSleeping) {
				return;
			}
			if (!playerCharacter.ai.IsRunning) {
				state = State.Idle;
			}
			if (playerCharacter.ai.IsIdle) {
				state = State.Idle;
			}
			List<Chara> list = FindVisibleEnemies();

			bool flag = false;
			bool flag2 = false;
			Card val = null;
			
			if (((Spatial)ELayer._zone).IsPlayerFaction) {
				flag = AutoExplorerConfig.UseMeditation.Value && ShouldRest();
				flag2 = AutoExplorerConfig.HandleHunger.Value != 0 && IsHungry();
				val = (AutoExplorerConfig.HandleTraps.Value ? actionFinder.FindTrap() : null);
			}

			switch (state) {
				case State.Exploring:
					if (flag2) {
						HandleFood();
					} else if (flag) {
						HandleResting();
					}
					break;
				case State.Combat:
					break;
				case State.Resting:
					if (list.Any()) {
						HandleCombat(list);
					}
					break;
				case State.Starting:
				case State.Idle: {
						if (list.Any()) {
							HandleCombat(list);
							break;
						}
						if (flag2) {
							HandleFood();
							break;
						}
						if (flag) {
							HandleResting();
							break;
						}
						if (val != null) {
							HandleTrap(val);
							break;
						}
						List<AIAct> list2 = actionFinder.FindPotentialActions();
						if (list2.Any()) {
							HandleActions(list2);
							break;
						}
					Logger.LogMessage((object)"Nothing to do.");
						Msg.Say("noTargetFound");
						isEnable = false;
						break;
					}
			}
		}

		private void HandleInput() {
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			if (EInput.isInputFieldActive) {
				return;
			}
			KeyboardShortcut value = AutoExplorerConfig.ToggleHarvestingAndMiningMode.Value;
			if (((KeyboardShortcut)(value)).IsDown()) {
				Logger.LogInfo((object)"Toggle Harvesting and Mining mode key pressed");
				AutoExplorerConfig.SetNextMode();
				return;
			}
			if (Input.GetKeyDown(AutoExplorerConfig.ActivationKey.Value)) {
				Logger.LogInfo((object)"L key pressed");
				if (isEnable) {
					isEnable = false;
					state = State.Starting;
					Logger.LogInfo((object)"Auto explore disabled.");
				} else {
					isEnable = true;
					state = State.Starting;
					Logger.LogInfo((object)"Auto explore enabled.");
				}
			}
			if (Input.GetKeyDown(AutoExplorerConfig.GoDownKey.Value)) {
				Logger.LogInfo((object)"Go down key pressed");
				AIAct val = actionFinder.FindStairs();
				if (val != null) {
					playerCharacter.SetAIImmediate(val);
					state = State.Exploring;
				}
			}
			if (Input.GetKeyDown(AutoExplorerConfig.GoUpKey.Value)) {
				Logger.LogInfo((object)"Go up key pressed");
				AIAct val2 = actionFinder.FindStairs(down: false);
				if (val2 != null) {
					playerCharacter.SetAIImmediate(val2);
					state = State.Exploring;
				}
			}
		}

		private bool ShouldRest() {
			return !currentPos.cell.CanSuffocate() && (((Card)playerCharacter).hp * 100 < ((Card)playerCharacter).MaxHP * AutoExplorerConfig.MinHP.Value || playerCharacter.mana.value * 100 < playerCharacter.mana.max * AutoExplorerConfig.MinMP.Value || playerCharacter.stamina.value <= 0);
		}

		private bool InventoryIsFull() {
			return ((BaseStats)playerCharacter.burden).GetPhase() >= 3;
		}

		private bool Drowning() {
			if (!((Card)playerCharacter).HasCondition<ConSuffocation>()) {
				return false;
			}
			return ((BaseCondition)playerCharacter.GetCondition<ConSuffocation>()).value >= 100;
		}

		private bool IsHungry() {
			return ((BaseStats)playerCharacter.hunger).GetPhase() >= 3;
		}

		private void HandleFood() {
			if (AutoExplorerConfig.HandleHunger.Value == Elin_AutoExplore.AutoExplorerConfig.HungerMode.StopAutoExplore || ((Card)playerCharacter).things.Find((Func<Thing, bool>)((Thing a) => playerCharacter.CanEat(a, false)), true) == null) {
				Logger.LogWarning((object)"Hungry. Stopping autoExplore.");
				isEnable = false;
				Msg.Say("regionAbortMove");
			} else {
				playerCharacter.InstantEat((Thing)null, true);
			}
		}

		private void HandleCombat(List<Chara> enemies) {
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Expected O, but got Unknown
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Expected O, but got Unknown
			if (!AutoExplorerConfig.HandleFighting.Value) {
				state = State.Finished;
				isEnable = false;
				return;
			}
			Chara val = enemies.OrderBy((Chara enemy) => ((Card)enemy).pos.Distance(currentPos)).First();
			ManualLogSource logger = Logger;
			bool flag = default(bool);
			BepInExMessageLogInterpolatedStringHandler val2 = new BepInExMessageLogInterpolatedStringHandler(17, 1, out flag);
			if (flag) {
				((BepInExLogInterpolatedStringHandler)val2).AppendLiteral("Current enemies: ");
				((BepInExLogInterpolatedStringHandler)val2).AppendFormatted<int>(enemies.Count());
			}
			logger.LogMessage(val2);
			EClass.pc.SetAIImmediate((AIAct)new GoalAutoCombat(val));
			state = State.Combat;
		}

		private List<Chara> FindVisibleEnemies() {
			if (((Spatial)ELayer._zone).IsPlayerFaction)
				return new List<Chara>();
			Map map = ELayer._map;
			List<Point> currentFov = ((Card)playerCharacter).fov.ListPoints();
			List<Chara> source = map.charas.ToList();
			List<Chara> list = source.Where((Chara chara) => currentFov.Contains(((Card)chara).pos)).ToList();
			return list.FindAll((Chara chara) => (int)chara.hostility == 1);
		}

		private void HandleActions(List<AIAct> acts) {
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			AIAct val = acts.First();
			ManualLogSource logger = Logger;
			bool flag = default(bool);
			BepInExInfoLogInterpolatedStringHandler val2 = new BepInExInfoLogInterpolatedStringHandler(31, 3, out flag);
			if (flag) {
				((BepInExLogInterpolatedStringHandler)val2).AppendLiteral("Doing action ");
				((BepInExLogInterpolatedStringHandler)val2).AppendFormatted<string>(val.Name);
				((BepInExLogInterpolatedStringHandler)val2).AppendLiteral("/(move:");
				((BepInExLogInterpolatedStringHandler)val2).AppendFormatted<bool>(val.IsMoveAI);
				((BepInExLogInterpolatedStringHandler)val2).AppendLiteral(") to point ");
				((BepInExLogInterpolatedStringHandler)val2).AppendFormatted<Point>(val.GetDestinationPoint());
			}
			logger.LogInfo(val2);
			playerCharacter.SetAIImmediate(val);
			state = State.Exploring;
		}

		private void HandleTrap(Card trap) {
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			Logger.LogMessage((object)("Handling trap " + trap.Name));
			if (currentPos.Distance(trap.pos) == 1) {
				Trait trait = trap.trait;
				((TraitSwitch)((trait is TraitTrap) ? trait : null)).TryDisarmTrap(playerCharacter);
			} else {
				AI_Goto aIImmediate = new AI_Goto(trap, 1, false, false);
				playerCharacter.SetAIImmediate((AIAct)(object)aIImmediate);
			}
			state = State.Exploring;
		}

		private void HandleResting() {
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			Logger.LogMessage((object)"Resting");
			bool flag = ((Card)playerCharacter).things.Find<TraitBed>() != null;
			if (!(playerCharacter.CanSleep() && flag)) {
				AI_Meditate aIImmediate = new AI_Meditate();
				playerCharacter.SetAIImmediate((AIAct)(object)aIImmediate);
			} else {
				HotItemActionSleep val = new HotItemActionSleep();
				((HotAction)val).Perform();
			}
			state = State.Resting;
		}
	}
}