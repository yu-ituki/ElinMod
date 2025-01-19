using System.IO;
using System.IO.Compression;

using BepInEx;

using Ionic.Zip;

namespace Elin_Mod
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
	}
}
