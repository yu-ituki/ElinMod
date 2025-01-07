# ElinModメモ置き場
ElinのModあれこれ用

--- 

# ■作り方情報
ここ  
https://ylvapedia.wiki/wiki/User:Discode/Modding_Getting_Started    
インストールしたゲーム本体から使いたいdllをプロジェクトで参照して、    
VisualStudioで.Netクラスライブラリとして出力するだけ。    
  
自身のElinインストールフォルダのdllをそのまま参照してやると   
自分だけで作業する分には問題ない。  
多人数で本格的にいじる場合はベースのパスを統一するか、環境変数などを通して参照するのが良さそう。  
↑のページに書いてある「コピーして持ってこよう！」は正直オススメしない。  
実行環境用にどのみちElinをインストールする必要があるし、だったら直参照したほうが早い。  

---

# ■必須 or 頻出（かもしれない）dll
たぶん全部参照しても問題ないので面倒であれば全部参照しておくと良い気がする。  
ベースのインストールパス： <Steamのインストールパス>/steamapps/common/Elin  

* ベースパス/Elin_Data/Managed/Elin.dll
  * Elin本体。こいつがなければ始まらない
* ベースパス/Elin_Data/Managed/Plugins.dll
* ベースパス/Elin_Data/Managed/Plugins.BaseCore.dll
* ベースパス/Elin_Data/Managed/Plugins.UI.dll
* ベースパス/Elin_Data/Managed/Plugins.Util.dll
  * noa猫様によるありがたいMod用ライブラリ郡とのこと。使わせていただこう。
* ベースパス/Elin_Data/Managed/UnityEngine.dll
* ベースパス/Elin_Data/Managed/UnityEngine.CoreModule.dll
* ベースパス/Elin_Data/Managed/UnityEngine.UI.dll
  * UnityEngine本体。とりあえず必須。
*  ベースパス/BepInEx/core/0Harmony.dll
*  ベースパス/BepInEx/core/BepInEx.Core.dll
*  ベースパス/BepInEx/core/BepInEx.Unity.dll
*  ベースパス/BepInEx/core/BepInEx.Preloader.Core.dll
*  ベースパス/BepInEx/core/BepInEx.Preloader.Unity.dll
  * BepInEx系。必須。
  
以下任意系
* ベースパス/Elin_Data/Managed/UnityEngine.InputLegacyModule.dll
  * UnityEngine.Input系のクラスが使える。キーやマウス駆動のModを作るなら

* ベースパス/Elin_Data/Managed/Plugins.Dungeon.dll
  * ダンジョンとかマップとかの生成機能が入ってる？不明
 
# ■dllの展開方法
ILSpyの左ペインにDLLを放り込んで、DLL名を右クリック -> [Save Code] で .csproj が吐ける。
ILSpy↓  
https://github.com/icsharpcode/ILSpy/releases  
あとはVisualStudioで空の.Netクラスライブラリプロジェクトを作り、そこにcsprojを紐づけるだけ。  
既存のModのdllも、Elin本体のソースもこれで開ける。  
あとはそのプロジェクトの参照として、必要なdllを紐付ければビルド可能。   
  
# ■Mod開発にあたって
## ◯デバッグメモ
printfデバッグ的なものしか分からない…break置きたい……。  
とりあえずSystem.IO.File.WriteAllText() でひたすらダンプする方法しか現状分からない。  
BepInExになんか便利な機能がある？分からん。  

## ◯ソース解析情報


### ▼Card
Elinのキャラや落ちている物体などは概ねardクラスを継承している……と思う。多分。  
Cardは基本的なアイテムなどの情報が全て入っている。  

全カードの情報を知りたければとりあえずcsvにでも適当にダンプしてやろう。  
下記は適当なダンプ用のサンプルコード。  
```
void Update() {
  if (Input.GetKeyDown(KeyCode.F10)) {
  	_Dump("card", EClass.sources.cards.rows, EClass.sources.cards.map);
  }
}

void _Dump<T>( string saveName, List<T> list, Dictionary<string, T> dic )
	where T : RenderRow
{
	string tmp = "";
	foreach (var itr in dic) {
		var a = itr.Value;
		tmp += itr.Key;
		tmp += $",{a.name_JP}";
		tmp += $",{a.category}";
		tmp += $",{a.detail_JP}";
		for (int j = 0; j < a.tag.Length; ++j) {
			tmp += $",{a.tag[j]}";
		}

		tmp += "\n";
	}
	foreach (var itr in list) {
		var a = itr;
		tmp += $",{a.name_JP}";
		tmp += $",{a.category}";
		tmp += $",{a.detail_JP}";
		for (int j = 0; j < a.tag.Length; ++j) {
			tmp += $",{a.tag[j]}";
		}

		tmp += "\n";
	}

	string savePath = $"D:\\{saveName}.csv";
	if (System.IO.File.Exists(savePath))
  	System.IO.File.Delete(savePath);
  System.IO.File.WriteAllText(savePath, tmp);
}



```


### ▼テーブルデータ群
起動時に下記に全て読み込まれる仕組みになっているようだ。  
これらは全てstaticになっており、いつでもアクセス可能となっている。  
```
EClass.sources
```
  
Modで新規にエクセルを追加する場合は[公式情報](https://docs.google.com/document/d/e/2PACX-1vR3GPx71Xnjfme6PtdqNnS5GnxlOFr2A8KdzH8bYTEwEOCgeVYROi3YaMQ2_h4qsySU_BORHKXPUi9i/pub) を参考にすると良い。  
公式ではOnStartCore() で ModUtil.ImportExcel() を使用してエクセルをインポートしている。  
```
[BepInPlugin("lafrontier.example_plugins", "ImportTest Example", "1.0.0.0")]
public class ImportTest : BaseUnityPlugin {
        public void OnStartCore() {
                var dir = Path.GetDirectoryName(Info.Location);
                var excel = dir + "/SourceCard.xlsx";
                var sources = Core.Instance.sources;
                ModUtil.ImportExcel(excel, "Chara", sources.charas);
                ModUtil.ImportExcel(excel, "CharaText", sources.charaText);
                ModUtil.ImportExcel(excel, "Thing", sources.things);
        }
}
```
Core.Instance.sources と EClass.sources は同一の物と思われる。  
追加された、または既存のデータにアクセスするには下記のようにする。  
```
// カードIDからCardを検索取得してくる.
// mapは内部Dictionary.
CardRow s = EClass.sources.cards.map.TryGetValue("カードID");
```


### ▼プレイヤー
下記に入っている。  
```
ELayer.pc
```

下記のように使える。  
```
// プレイヤー座標取得.
Point pos = ELayer.pc.pos;

```

### ▼マップ情報
下記に大体入っている。  
```
ELayer._map
ELayer._zone
```
下記のように使える。  
```
// プレイヤーに属する拠点か否か？.
if ( ELayer._zone.IsPlayerFaction ) {
}
```




### ▼テキストボックスへのメッセージ表示
```
Msg.Say("returnOverweight");
```



### ▼感想
正直ソース読みづらい、というかひどい。  
カプセル化もアレだし、staticとハードコードばっかだし。	
他の人のModを開いて解析していくとまだ楽に解析できるかもしれない。
