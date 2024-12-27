:: Elinのインストールフォルダ.
set ElinDir="C:\Program Files (x86)\Steam\steamapps\common\Elin"
:: 自身のMod制作フォルダ.
set ModPrjPluginDir="D:\my\prj\mydev\elin_mod\ElinMOD-main\Plugins"

xcopy "%ElinDir:"=%\Elin_Data\Managed\Elin.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\Elin_Data\Managed\Plugins.BaseCore.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\Elin_Data\Managed\Plugins.UI.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\Elin_Data\Managed\UnityEngine.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\Elin_Data\Managed\UnityEngine.CoreModule.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\Elin_Data\Managed\Reflex.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i

xcopy "%ElinDir:"=%\BepInEx\core\0Harmony.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\BepInEx\core\BepInEx.Core.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\BepInEx\core\BepInEx.Core.xml" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\BepInEx\core\BepInEx.Unity.dll" "%ModPrjPluginDir:"=%\" /s /e /h /y /i
xcopy "%ElinDir:"=%\BepInEx\core\BepInEx.Unity.xml" "%ModPrjPluginDir:"=%\" /s /e /h /y /i


pause
