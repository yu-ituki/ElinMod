# ■ Overview  
When you start crafting at a crafting station while holding L-Shift or R-Shift,  
it will automatically continue crafting the last item you made.  
This eliminates the need to press the "Craft" button repeatedly when cooking large quantities of meat or fish.  
Auto-crafting stops when any key is pressed.  
Additionally, it will also stop automatically when the character becomes fatigued or hungry.  
  
# ■ Source Code Explanation  
Everything is written in AutoCrafter.cs.  
The OnClickCraft callback, which handles the crafting button in the crafting station menu, is patched using Postfix,  
where it checks for L-Shift and R-Shift.  
The update process is handled within Plugin.cs using the Update method.  