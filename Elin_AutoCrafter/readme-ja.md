# ■概要
制作設備で L-Shift または R-Shift を押しながら制作を開始すると、  
最後に作った物を自動で作り続けます。  
大量の肉や魚の調理に毎回「制作」ボタンを押す必要が無くなります。  
自動生成はなにかのキーを押すことで解除されます。  
また、疲労や空腹の際にも自動制作を解除します。  

# ■ソースコード解説
AutoCrafter.cs に全部書いてます。  
制作施設メニューの制作ボタンコールバックであるOnClickCraftをPostfixして、  
L-ShiftとR-Shiftを判定してます。  
更新だけPlugin.csのUpdateで回してます。  


