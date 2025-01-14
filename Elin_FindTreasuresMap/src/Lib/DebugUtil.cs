using System.Collections.Generic;
using System.IO;
using BepInEx.Logging;

using UnityEngine;

namespace Elin_Mod
{

	class DebugUtil
	{
		private static ManualLogSource s_Logger;

		public static void Initialize( ManualLogSource body )
		{
			s_Logger = body;
		}

		public static void Log( object message )
		{
			if (s_Logger != null)
			{
				s_Logger.LogInfo( message );
			}
		}

		public static void LogError( object message )
		{
			if (s_Logger != null)
			{
				s_Logger.LogError( message );
			}
		}

		public static void LogWarning( object message )
		{
			if (s_Logger != null)
			{
				s_Logger.LogWarning( message );
			}
		}

		public static void DumpText( string path, string text )
		{
			if (File.Exists( path ))
			{
				File.Delete( path );
			}
			File.WriteAllText( path, text );
		}

	}


}
