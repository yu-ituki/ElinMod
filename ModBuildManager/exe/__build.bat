@echo off
setlocal enabledelayedexpansion

set MSBUILD_PATH="C:\Program Files\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\"

:: 環境変数 PATH に追加
set PATH=!MSBUILD_PATH!;%PATH%
echo MSBuild のパスを追加しました: !MSBUILD_PATH!

:: MSBuild のバージョン確認
MsBuild.exe -version

::-----------------------------------.

cd /d %1
call cleanup.bat
call config.bat
call MsBuild.exe /p:Configuration=Release

