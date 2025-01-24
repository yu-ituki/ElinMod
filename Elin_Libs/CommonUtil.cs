using System.IO;
using System.IO.Compression;
using BepInEx;
using HarmonyLib;
using Ionic.Zip;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Elin_Mod
{
	public class ModPatchInfo {
		public System.Type m_TargetType;
		public string m_Regex;
		public MethodInfo m_Prefix;
		public MethodInfo m_Postfix;
		
		public void Patch( Harmony harmony ) {
			var methods = m_TargetType.GetMethods((System.Reflection.BindingFlags)~(0));
			var method = System.Array.Find( methods, v => Regex.IsMatch(v.Name, m_Regex));
		//	DebugUtil.LogError($"{method?.Name} :  {m_Regex} ");
			if ( method == null ) 
				return;
			var prefix = m_Prefix != null ? new HarmonyMethod(m_Prefix) : null;
			var postfix = m_Postfix != null ? new HarmonyMethod(m_Postfix) : null;
			harmony.Patch(method, prefix, postfix, null, null, null);
		}

	}


	public class TmpFile : System.IDisposable
	{
		string m_TmpPath;

		public static TmpFile Create(string basePath) {
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

	public class CommonUtil
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


		public static string GetSaveDataFolderPath() {
			return GameIO.pathCurrentSave;
		}


		public static bool GetKeyDown( UnityEngine.KeyCode keyCode ) {
			return UnityEngine.Input.GetKeyDown(keyCode);
		}

		public static bool GetKeyUp(UnityEngine.KeyCode keyCode) {
			return UnityEngine.Input.GetKeyUp(keyCode);
		}

		public static bool GetKey(UnityEngine.KeyCode keyCode) {
			return UnityEngine.Input.GetKey(keyCode);
		}


		public static string CreateErrorReport() {
			var reportPath = GetResourcePath("error_report_tmp/");
			var reportZipPath = GetResourcePath("error_report.zip");
			if (System.IO.Directory.Exists(reportPath)) {
				System.IO.Directory.Delete(reportPath, true);
			}
			System.IO.Directory.CreateDirectory(reportPath);

			var configPath = CorePath.RootSave + "config.txt";
			System.IO.File.Copy(configPath, reportPath + "config.txt", true);

			var savePath = GetSaveDataFolderPath();
			CopyDirectory(savePath, reportPath + Game.id + "/");

			using ( var zip = new ZipFile() ) {
				zip.AddDirectory(reportPath, "");
				zip.Save(reportZipPath);
			}

			System.IO.Directory.Delete(reportPath, true);

			return reportZipPath;
		}


		public static void CopyDirectory(string sourceDir, string destinationDir) {
			// コピー先のディレクトリが存在しない場合は作成
			if (!Directory.Exists(destinationDir)) {
				Directory.CreateDirectory(destinationDir);
			}

			// ファイルをコピー
			foreach (string file in Directory.GetFiles(sourceDir)) {
				string fileName = Path.GetFileName(file);
				string destFile = Path.Combine(destinationDir, fileName);
				File.Copy(file, destFile, true); // 上書きオプションをtrueに設定
			}

			// サブディレクトリを再帰的にコピー
			foreach (string subDir in Directory.GetDirectories(sourceDir)) {
				string subDirName = Path.GetFileName(subDir);
				string destSubDir = Path.Combine(destinationDir, subDirName);
				CopyDirectory(subDir, destSubDir);
			}
		}



		public static void ApplyHarmonyPatches(Harmony harmony, ModPatchInfo[] infos) {
			foreach (var itr in infos)
				itr.Patch(harmony);
		}


		public static MethodInfo ToMethodInfo( System.Action act ) => act.Method;
		public static MethodInfo ToMethodInfo<T>(System.Action<T> act) => act.Method;
		public static MethodInfo ToMethodInfo<T,T2>(System.Action<T,T2> act) => act.Method;
		public static MethodInfo ToMethodInfo<T,T2,T3>(System.Action<T,T2,T3> act) => act.Method;
		public static MethodInfo ToMethodInfo<T,T2,T3,T4>(System.Action<T,T2,T3,T4> act) => act.Method;
		

	}
}
