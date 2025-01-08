using BepInEx;
using BepInEx.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace Elin_AutoExplore
{



	class ExUtil
	{
		static ManualLogSource s_Logger;
		public static void SetLogger(ManualLogSource body) {
			s_Logger = body;
		}



		public static void DumpText(string path, string text) {
			if (System.IO.File.Exists(path))
				System.IO.File.Delete(path);

			//System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
			System.IO.File.WriteAllText(path, text);
		//	if (s_Logger != null)
		//		s_Logger.Log(LogLevel.Info, text);
		}



		public static bool IsPlayingQuest_War() {
			return EClass._zone?.events?.GetEvent<ZoneEventDefenseGame>() != null;
		}

		public static bool IsPlayingQuest_Harvest() {
			return EClass._zone?.events?.GetEvent<ZoneEventHarvest>() != null;
		}

		public static List<ZoneEvent> GetZoneEvents() {
			var events = EClass._zone?.events;
			if (events == null)
				return null;
			return events.list;
		}

		public static string GetZoneName() {
			return EClass._zone?.Name;
		}


	}
}
