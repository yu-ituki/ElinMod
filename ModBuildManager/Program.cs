using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;

public class ModBuildManager : Form
{
	static readonly string[] c_ExcludeMods = new string[]
	{
		""
		, "Elin_Libs"
		, "AutoExplorerEx"
		, "ModBuildManager"
		, "Elin_ModTemplate"
	};

	private TextBox m_outputTextBox;
	private ListBox m_modListBox;
	private Button m_buildAllButton;
	private Button m_editVersionButton;
	private Button m_buildButton;
	private TextBox m_versionTextBox;
	private string m_basePath;
	private string m_exeDirPath;

	public ModBuildManager()
	{
		Text = "Mod Build Manager";
		Size = new System.Drawing.Size( 600, 400 );

		m_modListBox = new ListBox { Left = 10, Top = 10, Width = 300, Height = 300 };
		Controls.Add( m_modListBox );

		m_editVersionButton = new Button { Text = "Edit Version", Left = 320, Top = 10, Width = 100 };
		m_editVersionButton.Click += EditVersion!;
		Controls.Add( m_editVersionButton );

		m_versionTextBox = new TextBox { Left = 320, Top = 50, Width = 100 };
		Controls.Add( m_versionTextBox );

		m_buildButton = new Button { Text = "Build", Left = 320, Top = 90, Width = 100 };
		m_buildButton.Click += BuildMod!;
		Controls.Add( m_buildButton );

		m_buildAllButton = new Button { Text = "Build All", Left = 320, Top = 130, Width = 100 };
		m_buildAllButton.Click += BuildAllMods!;
		Controls.Add( m_buildAllButton );

		m_outputTextBox = new TextBox { Left = 10, Top = 320, Width = 560, Height = 400, Multiline = true, ScrollBars = ScrollBars.Vertical };
		Controls.Add( m_outputTextBox );

		string basePath = AppDomain.CurrentDomain.BaseDirectory; // 実行ファイルのあるディレクトリ
		m_exeDirPath = basePath;
		string? parentPath = Directory.GetParent( basePath )?.Parent?.Parent?.FullName; // 2つ上のディレクトリ
		m_basePath = parentPath!;

		LoadMods();
	}


	private void LoadMods()
	{
		m_modListBox.Items.Clear();
		if (m_basePath != null)
		{
			string[] modDirectories = Directory.GetDirectories( m_basePath );
			foreach (var dir in modDirectories)
			{
				string modName = Path.GetFileName( dir );
				if (System.Array.Exists( c_ExcludeMods, v => v == modName ))
					continue;
				string version = GetModVersion( dir );
				m_modListBox.Items.Add( $"{modName} - {version}" );
			}
		}
	}

	private string GetModVersion( string modPath )
	{
		string modInfoPath = Path.Combine( modPath, "src", "ModInfo.cs" );
		if (File.Exists( modInfoPath ))
		{
			string content = File.ReadAllText( modInfoPath );
			Match match = Regex.Match( content, "c_ModVersion = \"(.*?)\"" );
			if (match.Success) return match.Groups[ 1 ].Value;
		}
		return "Unknown";
	}

	private void EditVersion( object sender, EventArgs e )
	{
		if (m_modListBox.SelectedItem == null)
			return;
		if (m_modListBox == null)
			return;
		string selectedMod = m_modListBox?.SelectedItem?.ToString()?.Split( '-' )[ 0 ]?.Trim()!;
		string newVersion = m_versionTextBox.Text;
		string modPath = Path.Combine( m_basePath, selectedMod );
		UpdateModVersion( modPath, newVersion );
		LoadMods();
	}

	private void UpdateModVersion( string modPath, string newVersion )
	{
		string modInfoPath = Path.Combine( modPath, "src", "ModInfo.cs" );
		string packagePath = Path.Combine( modPath, "data", "publish", "package.xml" );

		if (File.Exists( modInfoPath ))
		{
			string content = File.ReadAllText( modInfoPath );
			content = Regex.Replace( content, "c_ModVersion = \"(.*?)\"", $"c_ModVersion = \"{newVersion}\"" );
			File.WriteAllText( modInfoPath, content );
		}

		if (File.Exists( packagePath ))
		{
			string content = File.ReadAllText( packagePath );
			content = Regex.Replace( content, "<version>(.*?)</version>", $"<version>{newVersion}</version>" );
			File.WriteAllText( packagePath, content );
		}
	}

	private void BuildMod( object sender, EventArgs e )
	{
		if (m_modListBox.SelectedItem == null)
			return;
		string selectedMod = m_modListBox.SelectedItem.ToString()!.Split( '-' )[ 0 ].Trim();
		BuildModBase( selectedMod );
	}

	private void BuildAllMods( object sender, EventArgs e )
	{
		foreach (var item in m_modListBox.Items)
		{
			string modName = item.ToString()!.Split( '-' )[ 0 ].Trim();
			BuildModBase( modName );
		}
	}
	
	private void BuildModBase( string modName )
	{
		string modPath = Path.Combine( m_basePath, modName );

		RunCommand( $"{m_exeDirPath}\\__build.bat", modName, modPath );
	}

	private void RunCommand( string command, string args, string workingDir )
	{
		ProcessStartInfo psi = new ProcessStartInfo( command, args )
		{
			WorkingDirectory = workingDir,
			RedirectStandardOutput = true,
			RedirectStandardError = true,
			UseShellExecute = false,
			CreateNoWindow = false
		};
		Process process = new Process { StartInfo = psi };
		process.OutputDataReceived += ( sender, e ) => AppendOutput( e.Data! );
		process.ErrorDataReceived += ( sender, e ) => AppendOutput( e.Data! );
		process.Start();
		process.BeginOutputReadLine();
		process.BeginErrorReadLine();
	}

	private void AppendOutput( string text )
	{
		if (InvokeRequired)
		{
			Invoke( new Action<string>( AppendOutput ), text );
			return;
		}
		if (!string.IsNullOrEmpty( text ))
		{
#if true
			EncodingProvider provider = System.Text.CodePagesEncodingProvider.Instance;
			var sjis = provider.GetEncoding( "shift-jis" );
			var sjisBytes = sjis!.GetBytes( text );
			var utf8Text = Encoding.UTF8.GetString( sjisBytes );
			m_outputTextBox.AppendText( utf8Text + Environment.NewLine );
#else
			m_outputTextBox.AppendText( text + Environment.NewLine );
#endif
		}
	}

	[STAThread]
	public static void Main()
	{
		Application.EnableVisualStyles();
		Application.Run( new ModBuildManager() );
	}
}
