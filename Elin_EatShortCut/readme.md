# ■Overview
Adds a "Eat Food" action to the hotbar.  
When used, it automatically consumes food from the backpack.  

# ■Source Code Explanation
Everything is in `EatShortCut.cs`.  
It postfixes `WidgetHotbar.SetShortcutMenu()` to add a custom `HotAction`.  

The food consumption priority is handled using a `switch` statement.  
Since sorting is a hassle, there's some unnecessary processing, but since it's a one-shot call, it shouldn't be too heavy.  
The main game itself has far more inefficient allocations and processing, so I decided to just leave it as is.  
