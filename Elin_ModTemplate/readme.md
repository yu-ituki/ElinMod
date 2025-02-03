# Readme
* This is a template project for creating Mods for Elin.
* It includes basic templates and setup files.

# Description
This template contains the following:
* A minimal base project and source files (entry points, templates, etc.).
* Basic materials for distribution.
* A simple localization system for Mods (supports all languages).
* A mechanism for easily referencing the Elin installation folder and DLLs.
* An automatic process for copying build artifacts to the Elin Package folder (placed under the Mod_Test folder).
* Basic lifecycle management during gameplay.
* A simplified method for dynamic HarmonyPatch registration.
* A function to dump in-game table data as TSV files.

## How to Use
1. First, copy this folder.
2. Next, copy the `Elin_Lib` folder from this repository.
   1. Library-related source code is organized into a separate folder (`./../Elin_Lib`).
3. Use a batch find-and-replace tool to replace all instances of `ModTemplate` and `mod-template` with your Mod's name.
   * You can use VSCode, Hidemaru, Sakura Editor, or any other text editor.
4. Rename `Elin_ModTemplate.sln` to match your Mod's name.
5. Edit `config.bat` to include your environment details.
6. Setup is complete. You should now be able to build the project.

## Environment Requirements
* Runs on .NET Framework 4.8.
* Install Visual Studio 2022 and .NET Framework 4.8 to get started.
* Unity is not required for this project. However, if you need to create prefabs, ScriptableObjects, or AssetBundles, Unity will be necessary.
  * If you only use C#, you might be able to avoid Unity by using `new GameObject()`, `AddComponent()`, or `Texture2D.LoadRawTextureData()`.
  * However, if you're adding new resources, having Unity is recommended.
* Finally, buy and install Elin to complete your environment. Get around 10 copies for evangelism purposes.

## About Elin_Lib
* A set of source files designed for shared use across multiple Mods.
* The folder is located one level above this project.
* Running `config.bat` creates symbolic links between `Elin_Lib` and the `src/Lib` directory of your project.
* If you download `Elin_Lib` and place it in the same directory as your project folder, you should be able to build without issues.

## Minimal Base Project and Source Files
* Includes essential files for constructing a Mod.
* DLL references include useful dependencies.
* The entry point is `Plugin.cs`.
* `ModConfig.cs` and `ModConfigUI.cs` contain sample configuration files.
* The `DebugUtil.Log()` and `LogError()` functions are available for debugging, similar to `Debug.Log()`.
  * To view BepInEx logs, enable the console in `BepInEx.cfg` (`Elin Installation Folder/BepInEx/config/BepInEx.cfg` under `[Logging.Console]`, set `Enabled = true`).

## Basic Lifecycle Management in the Game
* `MyModManager.cs` handles initialization and basic callbacks for in-game events.
* `Plugin.cs` handles initialization and callback registration.
* `RegisterOnLoadTableAction()` provides a callback for when tables are loaded.
  * This runs just before the game loads its tables.
  * At this stage, most game data is not yet loaded.
  * You can import custom table data here to ensure proper integration into in-game tables like the Card table.
* `RegisterOnStartGameAction()` provides a callback just before the game starts.
  * This triggers right before a new game or a load begins.
  * At this point, almost all game data is fully loaded.
  * Use this callback for initializing data that needs access to various in-game elements.

## Basic Materials for Distribution
* The `data/publish` folder contains `package.xml` and `preview.jpg`.
* Modify these for your Mod, and they will be automatically transferred to the output folder upon building.

## Simple Localization System for Mods (Full Language Support)
* Uses an Excel-based localization system.
* Syncs defined IDs in Excel with a C# enum.
  * Built using VBA for ease of use.
  * If Excel is unavailable, table editing won't be possible, so it can be ignored.
  * `data/resource/tables/mod_texts.xlsm` contains table definitions and VBA macros.
* Pressing a button in the Excel file generates `src/TextID.cs`.
* `ModTextManager.cs` loads this data.
* `ModTextManager.Instance.GetText(eTextID)` automatically retrieves text based on the current language setting.
* To add more languages, modify `mod_texts.xlsm`, update `eLanguage` in `Const.cs`, and adjust `ModText.cs`.
* Supports embedding user data into text. `[0]` to `[8]` placeholders are replaced using `ModTextManager.Instance.SetUserData()`.

## Mechanism for Easy Reference to the Elin Installation Folder and DLLs
* References DLLs based on the installation folder specified in `config.bat`.
* Uses symbolic links for referencing DLLs.
* To add more DLL references, modify the `.csproj` file or manually add references.

## Automatic Copying of Build Artifacts to the Elin Package Folder
* `config.bat` defines the installation folder and automatically copies the following:
  * Built DLLs
  * Entire `data/resource` folder
  * All files under `data/publish/`
* This allows immediate testing in Elin after building.
* The copied files are ready for distribution.
* Uses symbolic links and `xcopy` in a post-build event.

## Simplified Dynamic HarmonyPatch Registration
* Allows for registering patches dynamically, useful for local functions or performance optimization.
* Example usage:
```csharp
// Example patch for Game.Init
static void _OnInitGame() { }

var info = new ModPatchInfo() {
  m_TargetType = typeof(Game),
  m_Regix = "Init",
  m_Prefix = CommonUtil.ToMethodInfo(_OnInitGame)
};

// Apply the patch
MyModManager.Instance.AddPatch(info);

// Remove the patch
MyModManager.Instance.RemovePatch(info);
```

## In-Game Table Dumping as TSV Files
* Provides functionality to dump in-game tables as TSV files.
* Use the `Debug_AnalyzeElin` class and its `Dump_~` methods to generate dumps.
* Specify the full output path when calling the function.

