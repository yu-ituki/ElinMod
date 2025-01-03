using B83.Win32;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Elin_AutoExplore
{

	[NullableContext(1)]
	[Nullable(0)]
	public class AIActionFinder
	{
		private AutoExplorerConfig config = Plugin.Instance.AutoExplorerConfig;

		private Chara playerCharacter => ELayer.pc;

		private Point currentPos => ((Card)playerCharacter).pos;

		private MapBounds currentBounds => ELayer._map.bounds;

		public List<AIAct> FindPotentialActions() {

			if (((Spatial)ELayer._zone).IsPlayerFaction) {
				var vegetables = Ex_FindVegetables();
				if (vegetables.Count > 0) {
					return vegetables;
				}

			} else {

				var vegetables = Ex_FindVegetables();
				if ( vegetables.Count > 0 ) {
					return vegetables;
				}
				var ores = Ex_FindOres();
				if ( ores.Count > 0 ) {
					return ores;
				}

				List<AIAct> unexplored = FindUnexploredPoints();
				List<AIAct> loot = FindLoot();
				List<AIAct> harvestables = FindHarvestables();
				List<AIAct> mineables = FindMineables();
				List<AIAct> shrines = FindShrines();
				return (from p in unexplored.Concat(loot).Concat(harvestables).Concat(mineables)
						.Concat(shrines)
						orderby currentPos.RealDistance(p.GetDestinationPoint())
						select p).ToList();
			}

			return new List<AIAct>();
		}

		public List<AIAct> FindUnexploredPoints() {
			List<AIAct> tasks = new List<AIAct>();
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				if (!point.IsSeen && !point.IsBlocked && IsPointReachable(point)) {
					tasks.Add((AIAct)new AI_Goto(point, 1, false, false));
				}
			});
			return tasks;
		}

		public List<AIAct> FindLoot() {
			List<AIAct> tasks = new List<AIAct>();
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				//IL_004e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0075: Unknown result type (might be due to invalid IL or missing references)
				//IL_007f: Expected O, but got Unknown
				if (!point.IsHidden && !point.IsBlocked && point.HasThing && IsPointReachable(point)) {
					List<Thing> things = point.Things;
					foreach (Thing item in things) {
						if ((int)((Card)item).GetRootCard().placeState == 0 && CanPick(item)) {
							tasks.Add((AIAct)new AI_Goto(point, 0, false, false));
						}
					}
				}
			});
			return tasks;
		}

		public List<AIAct> FindHarvestables() {
			List<AIAct> tasks = new List<AIAct>();
			if (!config.HandleHarvestables.Value) {
				return tasks;
			}
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				if (point.IsInBounds && (point.HasObj || point.HasThing)) {
					TaskHarvest val = TaskHarvest.TryGetAct(ELayer.pc, point);
					if (val != null) {
						string name = (val.IsObj ? ((SourceData.BaseRow)((TaskPoint)val).pos.cell.sourceObj).GetName() : ((Card)((BaseTaskHarvest)val).target).Name);
						if (!Plugin.Instance.IgnoreList.IsIgnoredFromGathering(name) && IsPointReachable(point, 1)) {
							((BaseTaskHarvest)val).SetTarget(playerCharacter, (Thing)null);
							if (!((BaseTaskHarvest)val).IsTooHard) {
								tasks.Add((AIAct)(object)val);
							}
						}
					}
				}
			});
			return tasks;
		}

		public List<AIAct> FindMineables() {
			List<AIAct> tasks = new List<AIAct>();
			if (!config.HandleMineables.Value) {
				return tasks;
			}
			return _FindMinerBase(tasks, null);
		}

		public AIAct FindStairs(bool down = true) {
			AIAct action = null;
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				//IL_0051: Unknown result type (might be due to invalid IL or missing references)
				//IL_0057: Invalid comparison between Unknown and I4
				//IL_009f: Unknown result type (might be due to invalid IL or missing references)
				//IL_00a9: Expected O, but got Unknown
				if (!point.IsHidden && !point.IsBlocked && point.HasThing && IsPointReachable(point)) {
					List<Thing> things = point.Things;
					foreach (Thing item in things) {
						if ((int)((Card)item).GetRootCard().placeState == 2 && ((Card)item).trait is TraitStairs) {
							Trait trait = ((Card)item).trait;
							TraitStairs val = (TraitStairs)(object)((trait is TraitStairs) ? trait : null);
							if (val != null && ((TraitNewZone)val).IsDownstairs == down) {
								action = (AIAct)new AI_Goto(point, 0, false, false);
							}
						}
					}
				}
			});
			return action;
		}

		public List<AIAct> FindShrines() {
			List<AIAct> tasks = new List<AIAct>();
			if (!config.HandleShrines.Value) {
				return tasks;
			}
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				//IL_0054: Unknown result type (might be due to invalid IL or missing references)
				//IL_005a: Invalid comparison between Unknown and I4
				//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
				//IL_0103: Expected O, but got Unknown
				if (!point.IsHidden && !point.IsBlocked && point.HasThing && IsPointReachable(point)) {
					List<Thing> things = point.Things;
					foreach (Thing item in things) {
						if ((int)((Card)item).GetRootCard().placeState == 2) {
							Trait trait = ((Card)item).trait;
							TraitShrine val = (TraitShrine)(object)((trait is TraitShrine) ? trait : null);
							if (val != null && ((Trait)val).CanUse(playerCharacter) && !(val.Shrine.id == "material") && !(val.Shrine.id == "armor")) {
								if (point.Distance(currentPos) > 1) {
									tasks.Add((AIAct)new AI_Goto(point, 1, false, false));
								} else {
									((Trait)val).OnUse(playerCharacter);
								}
							}
						}
					}
				}
			});
			return tasks;
		}

		public Card FindTrap() {
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Invalid comparison between Unknown and I4
			List<Point> currentFov = ((Card)playerCharacter).fov.ListPoints();
			currentFov = currentFov.OrderBy((Point p) => p.Distance(currentPos)).ToList();
			foreach (Point point in currentFov) {
				if (point.IsHidden || point.IsBlocked || !point.HasThing || !IsPointReachable(point)) {
					continue;
				}
				List<Thing> things = point.Things;
				foreach (Thing thing in things) {
					if ((int)((Card)thing).GetRootCard().placeState == 2 && ((Card)thing).trait is TraitTrap) {
						Trait trait = ((Card)thing).trait;
						TraitTrap trait2 = (TraitTrap)(object)((trait is TraitTrap) ? trait : null);
						if (((TraitSwitch)trait2).CanDisarmTrap) {
							return ((Card)thing).GetRootCard();
						}
					}
				}
			}
			return null;
		}

		public bool IsPointReachable(Point point, int distance = 0) {
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Expected O, but got Unknown
			PathProgress path = new PathProgress();
			path.RequestPathImmediate(currentPos, point, distance, false, -1);
			return path.HasPath;
		}

		public bool CanPick(Thing thing) {
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Invalid comparison between Unknown and I4
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Invalid comparison between Unknown and I4
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			if (((Card)thing).isDestroyed) {
				return false;
			}
			if (((Card)thing).GetRootCard().isDestroyed) {
				return false;
			}
			if (((Card)playerCharacter).things.IsFull(thing, true, true)) {
				return false;
			}
			if (((Card)thing).isNPCProperty) {
				return false;
			}
			if (((Card)thing).ignoreAutoPick) {
				return false;
			}
			if (((Card)thing).isThing && (int)((Card)thing).placeState == 0 && !((Card)thing).ignoreAutoPick && EClass.core.config.game.advancedMenu) {
				var dataPick = EClass.player.dataPick;
				ContainerFlag val = ClassExtension.ToEnum<ContainerFlag>(((Card)thing).category.GetRoot().id, true);
				if ((int)val == 0) {
					val = (ContainerFlag)2048;
				}
				if ((!dataPick.noRotten || !((Card)thing).IsDecayed) && (!dataPick.onlyRottable || ((Card)thing).trait.Decay != 0) && (!dataPick.userFilter || (int)dataPick.IsFilterPass(((Card)thing).GetName((NameStyle)1, 1)) == 0)) {
					if (dataPick.advDistribution) {
						{
							foreach (int cat in dataPick.cats) {
								if (((Card)thing).category.uid == cat) {
									return true;
								}
							}
							return false;
						}
					}
					if (!((Enum)dataPick.flag).HasFlag((Enum)(object)val)) {
						return true;
					}
				}
			}
			return true;
		}


		List<AIAct> _FindMinerBase(List<AIAct> tasks, System.Func<TaskMine, bool> checkInclude) {
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				//IL_006a: Unknown result type (might be due to invalid IL or missing references)
				//IL_006f: Unknown result type (might be due to invalid IL or missing references)
				//IL_007c: Expected O, but got Unknown
				if (point.IsInBounds
				&& !Plugin.Instance.IgnoreList.IsIgnoredFromMining(point.cell.GetBlockName())
				&& TaskMine.CanMine(point, (Card)(object)((Card)playerCharacter).Tool)
				&& IsPointReachable(point, 1)
				) {
					TaskMine val = new TaskMine {
						pos = point.Copy()
					};

					if (checkInclude == null || checkInclude(val)) {
						((BaseTaskHarvest)val).SetTarget(playerCharacter, (Thing)null);
						if (!((BaseTaskHarvest)val).IsTooHard) {
							tasks.Add((AIAct)(object)val);
						}
					}
				}
			});
			return tasks;

		}

		struct HarvestData {
			public string name;
			public string rawName;
			public string category;
			public bool isHarvested;
		}

		List<AIAct> _FindHarvestBase(List<AIAct> tasks, System.Func<TaskHarvest, HarvestData, bool> checkInclude) {

			string tmp = "";
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				if (!point.IsInBounds)
					return;
				if (!(point.HasObj || point.HasThing))
					return;

				TaskHarvest val = TaskHarvest.TryGetAct(ELayer.pc, point);
				if (val == null)
					return;
				if (val.wasCrime)
					return;

				HarvestData dat = new HarvestData();
				dat.isHarvested = point.cell.isHarvested;

				if (val.IsObj) {
					var obj = point.cell.sourceObj;
					dat.name = obj.GetName();
					dat.rawName = obj.name;
					dat.category = obj.category;
					if (obj.growth != null) {
						var harvestID = obj.growth.idHarvestThing;
						if (harvestID != null && !harvestID.StartsWith("#")) {
							CardRow s = EClass.sources?.cards?.map?.TryGetValue(harvestID);
							if (s != null) {
								dat.category = s.category;
							}
						}
					}
				} else {
					var obj = val.target;
					dat.name = obj.Name;
					dat.rawName = obj.id;
					dat.category = obj.category.id;
				}

				if (checkInclude != null && !checkInclude(val, dat) )
					return;

				if (!Plugin.Instance.IgnoreList.IsIgnoredFromGathering(dat.name) && IsPointReachable(point, 1)) {
					((BaseTaskHarvest)val).SetTarget(playerCharacter, (Thing)null);
					if (!((BaseTaskHarvest)val).IsTooHard) {
						tasks.Add((AIAct)(object)val);
					}
				}
			});

//			ExUtil.DumpText("D:\\tmp.csv", tmp);

			return tasks;
		}


		public List<AIAct> Ex_FindOres() {
			List<AIAct> tasks = new List<AIAct>();
			if (!config.HandleMineOreOnly.Value) {
				return tasks;
			}
			_FindHarvestBase(tasks, (task, dat) => {

				switch (dat.rawName) {
					case "crystal":     //< ÉNÉäÉXÉ^Éã.
					case "sulfur rock": //< ó∞â©.
					case "ore": //< çzñ¨.
					case "gem ore": //< ãMêŒ.
						return true;
					default:
						return false;
				}
			});
			return tasks;
		}

		public List<AIAct> Ex_FindVegetables() {
			List<AIAct> tasks = new List<AIAct>();
			if (!config.HandleVegetables.Value) {
				return tasks;
			}

			_FindHarvestBase(tasks, (task, dat) => {

				if (dat.isHarvested)
					return false;

				// ñqëêópâÒî.
				bool isPasture = (dat.rawName == "pasture" || dat.rawName == "silver grass");
				if (isPasture)
					return true;

				switch (dat.category) {
					case "fruit":
					case "vegi":
					case "nuts":
						return true;
					default:
						return false;
				}
			});

			return tasks;
		}


	}
}
