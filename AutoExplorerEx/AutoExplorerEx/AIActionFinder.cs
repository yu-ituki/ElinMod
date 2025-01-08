using B83.Win32;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

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

		List<AIAct> m_TmpFindQuestHarvestTasks;
		List<TmpQuestHarvestData> m_TmpFindQuestHarvestTasks2;
		List<(int, int)> m_TmpFindQuestHarvestTaskDist;

		class TmpQuestHarvestData
		{
			public AIAct m_Act;
			public int m_X;
			public int m_Z;
			public int m_Weight;
			public int m_Dist;
			public int m_PriorityScore;

			public static TmpQuestHarvestData Create(TaskHarvest task, Point currentPos, int weight) {
				var ret =  new TmpQuestHarvestData() {
					m_Act = task,
					m_X = task.pos.x,
					m_Z = task.pos.z,
					m_Weight = weight,
					m_Dist = currentPos.Distance(task.pos)
				};

				// 適当に距離で重み付けする.
				const int c_DistScore = 100;
				ret.m_PriorityScore = ret.m_Weight - (ret.m_Dist * c_DistScore);

				return ret;
			}
		}




		public void FindPotentialActions( ref List<AIAct> list ) {

			// ホームのみ.
			if (((Spatial)ELayer._zone).IsPlayerFaction) {
				Ex_FindVegetables(list);

			} else {
				// ホーム外の処理.
				Ex_FindQuest_Harvest(list);
				if (list.Count > 0)
					return;	//< 収穫クエストはなによりも優先とする.

				Ex_FindVegetables(list);
				FindUnexploredPoints(list);
				FindLoot(list);
				FindHarvestables(list);
				FindMineables(list);
				FindShrines(list);
			}

			list.Sort((a, b) => {
				var bDist = currentPos.RealDistance(b.GetDestinationPoint());
				var aDist = currentPos.RealDistance(a.GetDestinationPoint());
				if (aDist > bDist)
					return 1;
				if (bDist > aDist)
					return -1;
				return 0;
			});
		}

	


		public void OnEndxplore() {
		}



		public List<AIAct> Ex_FindQuest_Harvest(List<AIAct> tasks) {
			if (!config.HandleQuest_Harvest.Value) {
				return tasks;
			}
			if (!ExUtil.IsPlayingQuest_Harvest())
				return tasks;

			if (m_TmpFindQuestHarvestTasks==null)
				m_TmpFindQuestHarvestTasks = new List<AIAct>();
			if (m_TmpFindQuestHarvestTasks2 == null)
				m_TmpFindQuestHarvestTasks2 = new List<TmpQuestHarvestData>();
			if (m_TmpFindQuestHarvestTaskDist == null)
				m_TmpFindQuestHarvestTaskDist = new List<(int, int)>();

			m_TmpFindQuestHarvestTasks.Clear();
			m_TmpFindQuestHarvestTasks2.Clear();
			m_TmpFindQuestHarvestTaskDist.Clear();
			Ex_FindVegetables(m_TmpFindQuestHarvestTasks);
			var playerPos = playerCharacter.pos;

#if false
			// 一度チェック済み状態だったら.
			if ( m_LastQuestHarvestData != null ) {
				// 座標の近いものを探してみる.

				for ( int i = 0; i < m_TmpFindQuestHarvestTasks.Count; ++i ) {
					var tmpTask = m_TmpFindQuestHarvestTasks[i] as TaskHarvest;
					int dist = Fov.Distance(
						m_LastQuestHarvestData.m_X,
						m_LastQuestHarvestData.m_Z,
						tmpTask.pos.x,
						tmpTask.pos.z
					);
					m_TmpFindQuestHarvestTaskDist.Add((i,dist));
				}
				m_TmpFindQuestHarvestTaskDist.Sort( (a,b) => {
					return a.Item2 - b.Item2;
				});

				m_LastQuestHarvestData = null;
				const int c_MaxCheckDist = 5;
				for ( int i = 1; i < c_MaxCheckDist; ++i ) {
					(int,int)? item = m_TmpFindQuestHarvestTaskDist.Find(v => ( v.Item2 <= i ) );
					if ( item != null ) {
						var tmpTask = m_TmpFindQuestHarvestTasks[item.Value.Item1] as TaskHarvest;
						m_LastQuestHarvestData = TmpQuestHarvestData.Create(tmpTask, currentPos, 0);
						break;
					}
				}
			} 

			// まだチェック済みではない or 近いのが見つからなかったら.
			if ( m_LastQuestHarvestData == null ) 
#endif
			{ 
			
				// 全ての重さ期待値を計算.
				int sumWeight = 0;
				for (int i = 0; i < m_TmpFindQuestHarvestTasks.Count; ++i) {

					var tmpTask = m_TmpFindQuestHarvestTasks[i] as TaskHarvest;
					int currentWeight = _CalcHarvestObjWeight(tmpTask);
					var tmpData = TmpQuestHarvestData.Create(tmpTask, currentPos, currentWeight);
					sumWeight += currentWeight;
					m_TmpFindQuestHarvestTasks2.Add(tmpData);
				}
				m_TmpFindQuestHarvestTasks.Clear();



				// 重さ期待値&距離でソート.
				m_TmpFindQuestHarvestTasks2.Sort((a, b) => {
					if (a.m_PriorityScore > b.m_PriorityScore)
						return -1;
					if (b.m_PriorityScore > a.m_PriorityScore)
						return 1;
					return 0;
				});

				// 詰めていく.
				//	foreach (var itr in m_TmpFindQuestHarvestTasks2) {
				//		tasks.Add(itr.m_Act);
				//	}

				//一番重くて近いいものだけ選択.
				//		m_LastQuestHarvestData = m_TmpFindQuestHarvestTasks2[0];

				tasks.Add(m_TmpFindQuestHarvestTasks2[0].m_Act);
			}

			// なにか居たらとりあえずそれを追加.
		//	if (m_LastQuestHarvestData != null)
		//		tasks.Add(m_LastQuestHarvestData.m_Act);

			m_TmpFindQuestHarvestTasks2.Clear();

			// おわり.
			return tasks;
		}

		int _CalcHarvestObjWeight(TaskHarvest task ) {
			var plantData = EClass._map.TryGetPlant(task.pos.cell);
			if (plantData == null)
				return 0;

			var growth = task.pos.growth;
			int num = growth.source._growth[4].ToInt();
			CardRow s = EClass.sources?.cards?.map?.TryGetValue(growth.idHarvestThing);
			int baseWeight = 1;
			if (s != null)
				baseWeight = s.model.SelfWeight;

			return plantData.size * num * baseWeight;
		}

		public List<AIAct> FindUnexploredPoints(List<AIAct> tasks) {
			currentBounds.ForeachPoint((Action<Point>)delegate (Point point) {
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				//IL_003a: Expected O, but got Unknown
				if (!point.IsSeen && !point.IsBlocked && IsPointReachable(point)) {
					tasks.Add((AIAct)new AI_Goto(point, 1, false, false));
				}
			});
			return tasks;
		}

		public List<AIAct> FindLoot(List<AIAct> tasks) {
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

		public List<AIAct> FindHarvestables(List<AIAct> tasks) {
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

		public List<AIAct> FindMineables(List<AIAct> tasks) {
			if (config.HandleMineables.Value == AutoExplorerConfig.eMineMode.Ignore) {
				return tasks;
			}

			switch (config.HandleMineables.Value) {
				case AutoExplorerConfig.eMineMode.All:
					_FindMinerBase(tasks, null);
					_Ex_FindOres(tasks);
					break;
				case AutoExplorerConfig.eMineMode.OreOnly:
					_Ex_FindOres(tasks);
					break;
			}

			return tasks;
		}

		List<AIAct> _Ex_FindOres(List<AIAct> tasks) {
			_FindHarvestBase(tasks, (task, dat) => {

				switch (dat.rawName) {
					case "crystal":     //< クリスタル.
					case "sulfur rock": //< 硫黄.
					case "ore": //< 鉱脈.
					case "gem ore": //< 貴石.
						return true;
					default:
						return false;
				}
			});
			return tasks;
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

		public List<AIAct> FindShrines(List<AIAct> tasks) {
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
			public bool isCanReapSeed;
		}

		List<AIAct> _FindHarvestBase(List<AIAct> tasks, System.Func<TaskHarvest, HarvestData, bool> checkInclude) {

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
						dat.isCanReapSeed = point.cell.CanReapSeed();
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


			return tasks;
		}




		public List<AIAct> Ex_FindVegetables(List<AIAct> tasks) {
			if (!config.HandleVegetables.Value) {
				return tasks;
			}

			_FindHarvestBase(tasks, (task, dat) => {

				if (dat.isHarvested)	//< 収穫状態にあるか.
					return false;

				if (!dat.isCanReapSeed)	//< 種収穫可能か.
					return false;

				// 牧草用回避.
				bool isPasture = (dat.rawName == "pasture" || dat.rawName == "silver grass");
				if (isPasture)
					return true;

				switch (dat.category) {
					case "fruit":
					case "vegi":
					case "nuts":
					case "foodstuff_raw":
						return true;
					default:
						return false;
				}
			});

			return tasks;
		}


	}
}
