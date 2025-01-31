# ■Overview
This mod adds a simple system to increase the usage of old documents.  
It creates new uses for used and no longer needed old documents.  
  
Adds an “archivist” near the Lumiest fountain.  
The archivist has the following characteristics.  
* Buy old documents (read/unread).  
* Sells technical books.  
* Uses “exotic currency” for purchases and sales, respectively.  

# ■Formulas and configurations  
This mod uses “foreign currency” as currency.  
Each “foreign currency” has its own value, just like real currency.  
All settings, including value and price, can be adjusted in the configurator.  
  
Configurations can be edited by clicking in the player or in the following file.  
<Elin installation folder>\BepInEx\config\yu-ituki.elin.ex-ancient-book.cfg

## ◯Currency prices
The config.
Worth_GachaCoin_~” in the config file allows you to set the price of each currency.  
The default price per coin is as follows  
* Copper: 1
* Silver: 10
* Gold Coin: 50
* Platinum coin: 200
  

## ◯ Price of old documents
The sale price of ancient documents can be adjusted in the config.  
The sale price of ancient documents can be adjusted in “Worth_AncientBook”.  
The price is calculated as follows  
　Sale price = (Document type ID + 1) * Worth_AncientBook
The rarer the document type ID, the higher the price.  
(Default: 5.0)

## ◯Price of technical book
Config.  
The purchase price of technical books can be adjusted in “Worth_SkillBook”.  
The price is calculated as follows  
　Purchase price = (Oren's equivalent price of the technical book) * Worth_SkillBook
(Default: 0.1)


## ◯Display of technical books
The display of technical books is performed as follows.  
* Draw lots to determine the number of books to be displayed.  
* Drawing lots for the display rarity of each book.   
* The number of books displayed is determined by the number of books in the configurations.

The number of books displayed is determined by the “SalesNum_Skill_Level” in the configuration.  
SalesNum_SkillBook_Base” (the minimum number of technical books to be sold)  
SalesNum_SkillBook_Add” (the number of additional technical books for sale by lottery)  
SalesNum_SkillBook_Add” (additional number of technical books to be sold by drawing lots)  
Number of books = SalesNum_SkillBook_Base + rnd(SalesNum_SkillBook_Add);.
  
The display rarity is determined by drawing lots for “High,” “Medium,” and “Low.  
The probability is set by “Config_SalesLvLotRate_SkillBook_~” in the configuration.  
The higher the number, the higher the probability of being drawn.  
  
The technical books that will actually be displayed are  
The level of the technical book that will actually be displayed is drawn based on the store's standard level based on the investment + the offset value based on the display rarity.  
The base value of the lottery is based on the table data of Elin itself.  
(Simply, the higher the level of the lottery, the better the item will be. (Simply, the higher the level of the lottery, the better the result will be.)  
The offset value according to rarity is set in the configuration  
SalesLv_SkillBook_~” in the configuration.  
  
# ■How to uninstall
This mod can be uninstalled by simply turning it off.  



# ■ Source code description
TraitMerchantEx_AncientResearcher.cs is almost the core of the processing.  
Also, the process of handling GachaCoin as currency is in WalletGachaCoin.cs.  

## ◯WalletGachaCoin 
This class handles buying, selling, and change calculation for multiple currencies.  
It is not particularly interesting as it contains popular-like processing.  

## ◯TraitMerchantEx_AncientResearcher
This class has been modified in various areas.  
I also wrote a lot of processing around the store,  
I also modified various parts around the process of handling gacha coins as currency.  
I've tried to write comments as much as possible, but the following is an excerpt of the process.  

### Handling of Currency
The following is touched in HarmonyPatch to handle new currencies.  
* Lang._currency() -- displaying “◯Yen” in the log
* Card.GetCurrency() -- acquiring the amount of currency held
* Card.ModCurrency() -- process of changing currency
* Card.GetPrice() -- getting price of individual items
* UICurrency.Build() -- UI for displaying currency holdings  
 
The above functions are defined in the CurrencyType and are used to distribute the processing.  
The above functions are hooked with Prefix and Postfix to determine the currency type and then perform their own processing.  

### ▼ Shop handling process
In adding new store types, the following are handled by HarmonyPatch.  
* OnBarter() -- when the store menu is opened  
* OnEndTransaction() -- when the shop is closed  

At the timing of opening, we display dedicated products and at the same time set the “currently open flag”.  
This flag is used for culling in various places so that the processing speed of normal play is not affected as much as possible.  
The various lottery processes are as described above.  


