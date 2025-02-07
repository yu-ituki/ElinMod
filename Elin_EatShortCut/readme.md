## Overview
Adds an "Eat Food" action to the hotbar.  
When used, it automatically consumes food from the backpack.  

## Config  
You can adjust the following settings from the "EatShortcut Settings" in the player's middle-click menu:  

* **Hunger Threshold to Stop Eating**  
  Prevents eating when hunger is below this level.  
  Useful for strategies like taking advantage of hunger-based eating bonuses.  

* **Food Priority**  
  Sets the priority for selecting food.  
  Can be configured based on attributes, nutritional value, or other factors.  
  By default, it consumes the first available food, just like on the world map.  
  Adjust it according to your own or your companions' growth plans.  

* **Prioritize Freshly Cooked Food**  
  Always prioritizes consuming freshly cooked meals.  
  If multiple freshly cooked meals are available, selection follows the food priority settings.  

* **Instant Eating**  
  Allows eating without consuming a turn, similar to eating on the world map.  
  This is slightly overpowered, so it is OFF by default.  

# ■Source Code Explanation
Everything is in `EatShortCut.cs`.  
It postfixes `WidgetHotbar.SetShortcutMenu()` to add a custom `HotAction`.  

The food consumption priority is handled using a `switch` statement.  
Since sorting is a hassle, there's some unnecessary processing, but since it's a one-shot call, it shouldn't be too heavy.  
The main game itself has far more inefficient allocations and processing, so I decided to just leave it as is.  
