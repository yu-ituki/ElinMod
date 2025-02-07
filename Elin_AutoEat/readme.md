## Overview  
Automates player eating and allows consumption via a single button from the hotbar.  
This mod adds the following features:  
* Adds "Eat Food" to the hotbar.  
* Automatically consumes food when hunger falls below a certain threshold.  
  * The priority for selecting food can be configured to some extent.  
  
## Config  
From the player's middle-click "AutoEat Settings," you can adjust the following settings:  
* **Enable Auto Eating**  
  * Toggles automatic eating ON/OFF.  
  * For players who want to eat only via hotbar shortcuts.  
* **Hunger Threshold for Auto Eating**  
  * Automatically consumes food when hunger falls below this threshold.  
  * Useful for taking advantage of hunger bonuses.  
* **Food Priority**  
  * Determines the priority for selecting food.  
  * Options include prioritizing nutritional value, specific stats, etc.  
  * "Standard" mode consumes the first available food item, similar to the world map behavior.  
  * Can be customized based on personal or companion growth plans.  
* **Prioritize Freshly Cooked Meals**  
  * Prefers freshly cooked meals above all else.  
  * If multiple freshly cooked meals are available, selection follows food priority settings.  
* **Instant Consumption**  
  * Allows eating without consuming turns, similar to the world map.  
  * Recommended ON for those using auto-eating.  
  * Recommended OFF for those who prefer a more immersive or balanced gameplay experience.  
  

# ■Source Code Explanation
Everything is in `AutoEat.cs`.  
It postfixes `WidgetHotbar.SetShortcutMenu()` to add a custom `HotAction`.  

The food consumption priority is handled using a `switch` statement.  
Since sorting is a hassle, there's some unnecessary processing, but since it's a one-shot call, it shouldn't be too heavy.  
The main game itself has far more inefficient allocations and processing, so I decided to just leave it as is.  
