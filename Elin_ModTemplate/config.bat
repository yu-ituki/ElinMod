::---------------.
:: 設定項目.
::---------------.
:: Elinのインストールフォルダ.
set ELIN_PATH="C:\Program Files (x86)\Steam\steamapps\common\Elin"
:: 自身のMod名.
set MY_MOD_NAME=Elin_Template

::---------------.
:: DLL参照用にシンボリックリンクを貼る.
::---------------.
if not exist %~dp0elin_link\ (
    mklink /D %~dp0elin_link\ %ELIN_PATH% 
)

