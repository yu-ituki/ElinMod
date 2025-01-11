using System.IO;
using BepInEx.Logging;

namespace Elin_ModTemplate
{

	class DebugUtil
	{
		private static ManualLogSource s_Logger;

		public static void Initialize( ManualLogSource body )
		{
			s_Logger = body;
		}

		public static void Log( string message )
		{
			if (s_Logger != null)
			{
				s_Logger.LogInfo( (object)message );
			}
		}

		public static void LogError( string message )
		{
			if (s_Logger != null)
			{
				s_Logger.LogError( (object)message );
			}
		}

		public static void LogWarning( string message )
		{
			if (s_Logger != null)
			{
				s_Logger.LogWarning( (object)message );
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
