# Readme
* This is a template project for creating Elin mods.
* It contains a basic template, basic setup files, etc.

# Description
This template has the following.
* A minimal basic project, a set of sources (entry points, templates, etc.)
* Basic material data for distribution
* Simple localization functionality for Mods (for all languages)
* Simple Elin installation folder and DLL reference mechanism
* Automatic copying of deliverables to Elin's Package folder (jump to Mod_Test folder)

## How to use
1. First, copy this folder. 
2. Next, copy the Elin_Lib folder that exists in this repository. 
   1. copy the library-like source code to another folder (. /... /Elin_Lib). 
3. Next, replace “ExAncientBook” and “ex-ancient-book” with your own mod name by using a tool such as a batch replacement tool under the folder of strings.
    * VSCode, Hidemaru, Sakura Editor, or any other replacement tool will do. 
4. After that, rename “Elin_ExAncientBook.sln” to your own mod name.
5. Write your environment information in config.bat.
6. preparation is complete. You should now be able to build.

## System Requirements
* Net Framework 4.8.    
* If you have Visual Studio 2022 and .Net Framework 4.8, it will work anyway.  
* Unity is not required for this project, but it is necessary if you want to generate prefabs, ScriptableObjects, AssetBundle, etc.  
  * Even if you only use C#, if you just new GameObject() and AddComponent, or use Texture2D.LoadRawTextureData() or something like that, it might work.   
    You may not need Unity, but if you want to add a new resource, it is better to have it.   
* Then, you just need to buy and install Elin and you have a working environment. Please buy about 10 copies for proselytizing.  

## About Elin_Lib
* This is a group of sources that can be used by all mods. 
* There is a folder on top of this folder. 
* By hitting config.bat, a symbolic link is created between Elin_Lib and the project's src/Lib directory. 
* If you drop Elin_Lib together and put it in the same directory as the project folder,  
  If you drop Elin_Lib with your project and place it in the same directory as your project folder, you can copy and paste the template and build it anyway.  

## Minimum basic projects and sources
* Minimum projects and sources for building a Mod are included.
* DLL references are included in the “it wouldn't hurt to have them” type.
* Plugin.cs is the entry point.
* ModConfig.cs and ModConfigUI.cs contain sample configurations for mods.
* DebugUtil.Log(), LogError(), etc. using the BepInEx logger. Debug.Log()-like.
  * To view BepInEx logs, you must open the BepInEx console.
  * ［Console］ in the Elin installation folder/BepInEx/config/BepInEx.cfg with Enabled = true.

## Basic material data for distribution
* package.xml and preview.jpg are placed under data/publish
* Rewrite the contents for your own mod and build it, and it will be automatically transferred to the deliverables folder (see below).

## Simple localization function for mods (for all languages)
* Simple localization function using Excel.
* Includes a mechanism to synchronize IDs defined in Excel with C# enums
  * It is made with fossilized VBA so that anyone can use it as long as they have at least Excel.
    * If you don't even have Excel, you can't even mess with the tables themselves.
  * data/resource/tables/mod_texts.xlsm contains table definitions and VBA.
* When you hit the button on excel, src/TextID.cs will be output.
* Read in src/Lib/ModTextManager.cs
* ModTextManager.Instance.GetText( eTextID ) automatically identifies the current language code and returns a string.
* If you want to increase the number of languages, increase mod_texts.xlsm and eLanguage in Const.cs, and increase ModText.cs by modifying it appropriately.
* Corresponds to in-sentence user data embedding. Corresponds to [0]-[8] in the statement and ModTextmanager.Instance.SetUserData() and is replaced according to the index.

## Simple mechanism to refer to the Elin installation folder and DLLs
* Go to the DLL of the installation folder written in config.bat.
* Symbolic link in config.bat and reference in csproj.
* You can add DLL references directly, or you can add them by directly modifying csproj and copying and pasting them in as you see fit.

## Mechanism for automatically copying artifacts to the Package folder of Elin's main unit
* Copy the following into the installation folder written in config.bat
  * DLL after build
  * The entire data/resource folder
  * All files under data/publish/
* Now you can check the operation of Elin immediately by simply building -> launching it.
* The copied files can be used as delivery data as they are.
* The files copied can be used as delivery data.

