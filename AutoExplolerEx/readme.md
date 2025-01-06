# Readme
* これは https://steamcommunity.com/sharedfiles/filedetails/?id=3365829584 の 拡張です。
* 作者であるYuof氏には深い感謝を申し上げます。
* 純正の物に下記機能を追加しました。
  * 野菜収穫モード -- 野菜/牧草のみを収穫するモードです
  * 鉱石回収モード -- 鉱石/貴石/クリスタルのみを回収するモードです
* Lボタン押下の末尾にモードを追加しました
* コンフィグにも項目を追加しています
* 下記モードはホームでも使用可能です
  * 野菜収穫モード

---

# 使用方法
* 事前に https://steamcommunity.com/sharedfiles/filedetails/?id=3365829584 をサブスクライブしてください。
* ビルド、またはここからダウンロードした Elin_AutoExplore.dll を サブスクライブした純正Modのフォルダに上書きしてください。
* 純正Modはインストールフォルダ次第ですが、通常は 「C:\Program Files (x86)\Steam\steamapps\workshop\content\2135150\3365829584」 などに保存されています。

---


# ソースコード上の拡張説明
* AutoActionFinder.cs に Ex_FindVegetables() と Ex_FindOres() を追加しました
* 関連し、Translation.cs にも項目を追加し、合わせて処理を簡易化しました
* AutoExplolerConfig.cs の SetNextMode() / SetMode() も少しいじりました
* 鉱石/貴石の回収については従来の鉱業モードに統合する形で実装しています

---

# 補足
* Yuof氏さん勝手にいじってすいません。乗っ取る気はありません
* 問題あったら連絡ください
* 可能なら拡張した機能を昇華して純正に取り込んでもらえると嬉しいです