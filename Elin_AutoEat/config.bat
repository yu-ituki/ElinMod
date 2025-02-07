::---------------.
:: 設定項目.
::---------------.
:: Elinのインストールフォルダ.
set ELIN_PATH="C:\Program Files (x86)\Steam\steamapps\common\Elin"
:: Lib元フォルダ.
set LIB_SRC_PATH="%~dp0/../Elin_Libs"
:: Libコピー先フォルダ.
set LIB_DEST_PATH="%~dp0/src/Lib"

::---------------.
:: DLL参照用にシンボリックリンクを貼る.
::---------------.
if not exist %~dp0elin_link\ (
    mklink /D %~dp0elin_link\ %ELIN_PATH% 
)

::---------------.
:: Lib参照用にシンボリックリンクを貼る.
::---------------.
if not exist %LIB_DEST_PATH% (
    mklink /D %LIB_DEST_PATH% %LIB_SRC_PATH% 
)

