using System.Collections.Generic;

namespace Elin_ModTemplate
{

	internal class GameUtil
	{
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
	}
}
