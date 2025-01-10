using System.IO;
using BepInEx;

namespace Elin_ModTemplate
{

	internal class CommonUtil
	{
		private static string m_ResourcePathBase;

		public static void Initialize( PluginInfo info )
		{
			m_ResourcePathBase = Path.GetDirectoryName( info.Location ) + "/resource/";
		}

		public static string GetResourcePath( string resName )
		{
			return m_ResourcePathBase + resName;
		}
	}
}
