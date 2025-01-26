# Readme
## ■Overview
Add a variety of gun and bow modification parts to the game.  
Currently, the following parts will be added.
More parts will be added in the future.  
* Barrel -- Increases or decreases the appropriate distance. It also reduces the shooting distance penalty slightly depending on the level of the gun.    
* Attributes -- Adds various attribute effects such as fire and cold to guns and bows.  

This mod can be introduced as a stand-alone item, though, 
We recommend to use it together with the mod “Elin_GunSmith”.  
(Elin_GunSmith is available here -> https://steamcommunity.com/sharedfiles/filedetails/?id=3407191321)  

## ■Configuration
The balance of each part type can be adjusted in the configurator.  
You can jump to various settings in “ExGunMods Settings” from the player middle click.   
You can also adjust the settings by editing the following files directly.  
(Elin installation folder)\BepInEx\config\yu-ituki.elin.ex-gun-mods.cfg 

## ■About Mod erase and uninstall commands
In order to add the table data of items and attributes, the save data after installing the mod is 
If this mod is turned off, it may not work properly.  
Therefore, we have prepared an “uninstall” function for the mod.   
By selecting “Uninstall” in the “ExGunMods Settings” from the player middle click   
The effect of all modification parts of this mod saved in the game will be removed,   
The game will automatically save and return to the title screen.  
After returning to the title screen, please turn off this mod from the in-game mod viewer,   
Then, exit the game once,  
You can continue to play with the saved data without this mod. 

## ■About ID
This mod uses the following ID bands in the element table.  
4000000 - 4199999  
Please note that if other mods use this bandwidth in the element table, they will interfere with it.  
If there is interference, you will be asked to delete one of the mods.   
If you find that you are interfering with a well-known mod, or if you find that you are interfering with a variety of mods,  
We may be able to create an ID converter for your save data.  
In that case, please contact us at  
https://github.com/yu-ituki/ElinMod/issues  

## ■Notes/Disclaimer 
This mod may interfere with other mods and future versions of Elin itself.  
Please make sure to back up your save data before installing this mod in case of unexpected accidents.  
In the worst case scenario, your saved data may be corrupted.  
We are not responsible for any problems that may occur with this mod. 


## ■Explanation of source code
### Outline
The following source code is the main body of the code.      
* NewRangedModManager -- Class for managing additional parts, including ID management and initialization of each part class.
    * Uninstallation process is also written here.
* NewRangedModBase -- Base class for additional parts.
* NewRangedMod_Barrel -- Class for the part “barrel”. Necessary definitions and HarmonyPatch required for implementation are placed here.
* NewRangedMod_Elements -- Class for the part “Attribute Assignment”. Necessary definitions and HarmonyPatches for implementation are placed here.
  * The process itself bypasses the main Card.DamageHP() and just flows to the main attribute ID.

Elin attribute called Elements? This is accomplished by adding a table called Elements.    
You can add modified parts by adding data here with the tag “modRanged”.  

### ▼ Parts “gun barrel
We just use alias to determine the attachment and hook the get property of the optimum distance to calculate the add/subtract distance for each part.   

### ▼ ▼ Part “Attribute assignment
The original attribute attack process is somewhat special, because it is hard-coded with alias, id, etc. in various places,   
I'm trying to deal with it by just bypassing the Card.DamageHP() ele, persisting the processing change until the very last minute.   
(This is the only way to do it, since the main body processing is all done by direct ID reference, including effects and state error calculations.)  
All the rest of the processing is done by having the original attribute attack ride on its back.    
Therefore, if the processing of the main body is changed, it will probably break immediately.  

### Uninstallation process
If you turn off the mod after the item has been added, the error message “ID does not exist” will appear.  
If you turn off the mod after the item is added, an error will occur saying that the ID does not exist.  
This may destroy your save data.    
But you don't want your saved data to be corrupted after thousands of hours of play.    
So I created an uninstall process.    
The process is as follows    
* Search the list of all saved cards in the game.
* Search for all the “things” (objects held by cards) in the list.
  * Delete all the things in a particular ID band.
  * Remove all enchantments of a specific ID band if any are attached.
* Do the same for all saved maps (zone/map)
If you turn off the mod in this state, you will theoretically be back to a clean environment for now.    
The only problem is that the master ID in the Element table is numeric, so no matter how strange you make the ID, there is a possibility that it will be shared by other mods.    
And if they do, the uninstall process described above will also blow away the enchantments of the other mods that have been covered by the master ID.  
There is probably no way to avoid this, and if the enchantment is covered by another mod, the data will be overwritten by the ID due to Elin's system, so you will be stuck when the enchantment is covered by the other mod.    
So, we can only hope that the data will not be shared with other mods.   
I thought about using alias, search, converter, and so on, but it is impossible unless Elin supports source bandwidth for each mod.   

### ▼ Regarding the timing of importing additional element tables.
For some reason, it seems to be incompatible with mods that support Korean, and importExcel() in OnStartCore overwrites the translated data.  
So, I use a method called Game.OnBeforeInstantiate() hooked with HarmonyPatch.   

### ▼ Translation of elements
For various reasons, translation is done independently.  
The text for translation is inserted in column 61 and after of the element addition data (element sheet in data/resource/tables/add_data.xlsx),  
Prepare a SourceElementNew class that inherits from SourceElement only thinly,  
CreateRow() and Reset() are hijacked, and SourceData.GetString() is used to overwrite the row name and textPhase for each language used.  
Initialize() in NewRangedModManager.cs.
  
Reason ↓  
* Import is run after the original Mod translation mechanism because of the timing of reading, and the original mechanism cannot be used.
* The original mechanism is vulnerable to change due to the increase of folders and the increase of many excel files with the same structure.
* I don't want to mess with multiple excel files just for translation to add one element data.
