# Readme
* これはElinのMod作成用テンプレートプロジェクトです。
* 基本的な雛形や、基本的なセットアップファイルなどが入っています。
  

# 解説
このテンプレートは下記を有しています。
* 基本となる最小限のプロジェクト、ソース群（エントリポイントや雛形など）
* 配信用の基礎素材データ
* Mod用の簡易的なローカライズ機能（全言語対応用）
* 簡易的にElinインストールフォルダとDLL参照できる仕組み
* Elin本体のPackageフォルダに成果物を自動コピーする仕組み（Mod_Testフォルダ以下に飛ぶ）

## はじめに
* 最初にconfig.bat に自身の環境情報を記載してください

## 基本となる最小限のプロジェクト、ソース群
* Mod構築用の最小限のプロジェクトやソースが入っています。
* DLL参照は「まああっても損はないやろ」的な物が入っています。
* Plugin.cs がエントリポイントです。
* ModConfig.cs、ModConfigUI.cs にMod用コンフィグのサンプルが置いてあります。
* BepInExロガーを使用して、DebugUtil.Log()、LogError() などを用意しました。 Debug.Log() 的に使用できます。
  * BepInExログを閲覧するにはBepInExコンソールを開く必要があります。
  * Elinインストールフォルダ/BepInEx/config/BepInEx.cfg の [Logging.Console] にて、Enabled = true でゲーム起動時に開きます

## 配信用の基礎素材データ
* data/publish 以下にpackage.xml と preview.jpg が置いてます
* 中身を自身Mod用に書き換えてビルドすると自動的に成果物フォルダ（後述）に転送されます

## Mod用の簡易的なローカライズ機能（全言語対応用）
* エクセルを使用した簡易的なローカライズ機能です
* エクセルで定義したIDとC#のenumを同期させる仕組みが入っています
  * 最低限エクセルさえあれば誰でも叩けるということで、VBAという化石で適当に作っています
    * エクセルすらない場合はそもそもテーブル自体いじれないので無視します
  * data/resource/tables/mod_texts.xlsm にテーブル定義＆VBAが入っています
* エクセル上のボタンを叩くとsrc/TextID.cs が出力されます
* src/Lib/ModTextManager.cs で読み込んでいます。
* ModTextManager.Instance.GetText( eTextID ) で、現在の言語コードを自動識別して文字列を返却します。
* 言語を増やす場合はmod_texts.xlsmおよび、Const.cs のeLanguageを増やして、ModText.cs を適当にいじって増やしてください。
* 文中ユーザーデータ埋め込みに対応しています。文中の[0]～[8]と ModTextmanager.Instance.SetUserData() が対応していて、indexに応じて置き換わります。

## 簡易的にElinインストールフォルダとDLL参照できる仕組み
* config.batに書かれたインストールフォルダのDLLを参照しに行きます
* 仕組み的にあconfig.batにてシンボリックリンクを貼り、csprojで参照しているだけです
* DLL参照を増やす場合は直に参照を増やしてもいいですし、csprojを直にいじって、適当にコピペして増やしても大丈夫です

## Elin本体のPackageフォルダに成果物を自動コピーする仕組み
* config.batに書かれたインストールフォルダに下記をコピーします
  * ビルド後のDLL
  * data/resource フォルダごと全部
  * data/publish/ 以下の全ファイル
* これでビルド -> 起動するだけでElin本体で即座に動作確認が可能です
* コピーされたファイル群はそのまま配信データとして使用可能です
* 仕組み的にはシンボリックリンクを辿ってビルド後イベントでxcopyしているだけです

