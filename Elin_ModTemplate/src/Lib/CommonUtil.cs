using System.IO;
using BepInEx;

namespace Elin_ModTemplate
{

	public class CommonUtil
	{
		private static string m_ResourcePathBase;

		public class TmpFile : System.IDisposable {
			string m_TmpPath;

			public static TmpFile Create( string basePath ) {
				if (!System.IO.File.Exists(basePath))
					throw new System.Exception($"[TmpFile] file not found : {basePath}");
				TmpFile ret = new TmpFile();
				
				var ext = System.IO.Path.GetExtension(basePath);
				ret.m_TmpPath = basePath.Replace($".{ext}", "");
				ret.m_TmpPath = $"{ret.m_TmpPath}_tmp.{ext}";

				if (System.IO.File.Exists(ret.m_TmpPath))
					System.IO.File.Delete(ret.m_TmpPath);
				System.IO.File.Copy(basePath, ret.m_TmpPath);

				return ret;
			}

			public void Delete() {
				if (System.IO.File.Exists(m_TmpPath))
					System.IO.File.Delete(m_TmpPath);
			}

			public void Dispose() {
				Delete();
			}

			public string GetPath() {
				return m_TmpPath;
			}

		}


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
