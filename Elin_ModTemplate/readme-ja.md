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
* ゲーム中の基本的なライフサイクルの管理
* 動的HarmonyPatch登録の簡易化機能
* コンフィグメニュー機能
* ゲーム中各テーブルのtsvダンプ機能

## 使い方
1. 最初にこのフォルダをコピーしてください。
2. 次にこのリポジトリに存在するElin_Libフォルダもコピーしてください。
   1. ライブラリ的なソースコードを別フォルダ（./../Elin_Lib）に括りだしています。 
3.  次に文字列のフォルダ以下一括置換ツールなどで「ModTemplate」および「mod-template」を自身のMod名に全文字列置換してください。
    * 置換はVSCodeや秀丸、さくらエディタ、その他なんでも大丈夫です。  
4. そのあと「Elin_ModTemplate.sln」を自身のMod名にリネームしてください。
5. config.bat に自身の環境情報を記載してください
6. 準備完了です。これでビルドできるようになっているはずです。

## 動作環境
* .Net Framework 4.8で動作しています。    
* Visual Studio 2022 と .Net Framework 4.8を入れてもらえればとりあえず動くと思います。  
* このプロジェクトにはUnityは必要ありませんが、プレハブやScriptableObject、AssetBundleなどを生成する場合は必要です。  
  * C#のみでも、とりあえず new GameObject() して AddComponentしたり、 Texture2D.LoadRawTextureData() なりを活用すれば   
    もしかしたらUnityが必要ないかもしれませんが、リソースを新規追加するんだったら有ったほうが良いです。   
* あとはElinを買ってインストールすれば動作環境が完成です。布教用に10本くらい買ってください。  

## Elin_Libについて
* 各Modで完全に共通使用出来そうなソース群です。 
* このフォルダの1つ上にフォルダが存在しています。 
* config.batを叩くことで、Elin_Libとプロジェクトのsrc/Lib以下にシンボリックリンクが張られ参照されます。 
* Elin_Libを一緒に落としてきて頂き、プロジェクトフォルダと同列のディレクトリに置いて頂ければ、  
  テンプレートをコピペしてもとりあえずビルドできるようになっています。  

## 基本となる最小限のプロジェクト、ソース群
* Mod構築用の最小限のプロジェクトやソースが入っています。
* DLL参照は「まああっても損はないやろ」的な物が入っています。
* Plugin.cs がエントリポイントです。
* ModConfig.cs にMod用コンフィグのサンプルが置いてあります。
* BepInExロガーを使用して、DebugUtil.Log()、LogError() などを用意しました。 Debug.Log() 的に使用できます。
  * BepInExログを閲覧するにはBepInExコンソールを開く必要があります。
  * Elinインストールフォルダ/BepInEx/config/BepInEx.cfg の [Logging.Console] にて、Enabled = true でゲーム起動時に開きます

## ゲーム中の基本的なライフサイクルの管理
* MyModManager.cs がライブラリ群の初期化やゲーム内の基本的なタイミングのコールバックを管理しています。  
* Plugin.cs で初期化＆コールバック登録をしています。  
* RegisterOnLoadTableAction() でテーブル読み込みタイミングのコールバックを返します。
  * ゲーム内テーブルが読み込まれる直前のコールバックです。
  * まだゲーム内の殆どのデータが読み込まれていない状態です。
  * ここで自前のテーブルデータをインポートすることで、Cardテーブルなどに問題なくデータが読み込まれるようになります。
* RegisterOnStartGameAction() でゲーム開始直前タイミングのコールバックを返します。
  * NewGameまたはLoadでゲームがセットアップされ、開始される直前のコールバックです。
  * ほぼすべてのデータが揃った状態のコールバックになります。
  * ゲーム中の様々なデータにアクセスしたい初期化処理はここで書いてください。

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

## 動的HarmonyPatch登録の簡易化機能
* ローカルファンクションの登録や、パフォーマンスの観点から特定タイミングでのみ登録しておきたいなど、時には動的にPatchを当てたい時もあると思います
* そんな際になるたけ簡易的にPatch関数を登録できるよう、サポート機能を用意しました。
* 下記のように書くと、登録、登録解除が比較的かんたんに出来ます。
```
// GameのInitにパッチを当てる例.
satic void _OnInitGame() {

}

var info = new ModPatchInfo() {
  m_TargetType = typeof(Game),
  m_Regix = "Init",
  m_Prefix = CommonUtil.ToMethodInfo( _OnInitGame )
};

// 当てる.
MyModManager.Instance.AddPatch(info);

// 外す.
MyModManager.Instance.RemovePatch(info);

```

## コンフィグメニュー機能
* プレイヤー中クリックから親子階層からなるコンフィグメニューを作ることが出来ます。
* ModConfigMenu.Instance.AddMenu() でメニューが追加できます。
  * ModConfig.cs にサンプルが置いてあります。
  * これを通して追加すると勝手に各Modで親子関係が構築され、階層メニューが生成されます。
* 下記はプログラム内部の仕組みです。
  * ModConfigMenu.cs が本体で、ここにUIContextMenuを様々操作する処理が書かれています。
  * 各Modとの同期はまあもうどうでもええやろの精神でGameObjectのSendMessage()を使ってます。
    * ModConfigMenuにて"_ModConfigMenu_OnAddCallback"のメッセージを飛ばしてます。
    * Plugin.csでそれを受取、ModConfigMenuにパスしてます。
  * スライダーにはキー入力が実装されています。
    * 「W」「←」「D」「→」で操作できます。
    * 「L-Shift」「R-Shift」を押しっぱなしにすると10倍量移動します。
    * UISliderKeyMover.cs コンポーネントでスライダーの監視とキーハンドリングをしてます。
      * まあもうどうでもええやろの精神でアロケーションやスパイクなどは無視してます。
      * GameUtil._AddSliderKeyMover() で各SliderにAddComponentしてます。
    * ModInput.cs がキー操作の根幹を担っています。リピート入力用に作りました。
      * ここまで作る予定はなかったです。これもう業務だろ。

## ゲーム中各テーブルのtsvダンプ機能
* ゲーム中のテーブルをtsvダンプする機能を用意しました。
* Debug_AnalyzeElinクラス内の各Dump_～関数でダンプ可能です。
* 呼び出しの際は出力パスをフルパスで指定してください。