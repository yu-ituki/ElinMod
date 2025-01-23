# Readme
* Add the following to the container sorting settings.
  * Do not include [impure].
  * Do not include [human flesh
  * Only include items that do not decay.

# Source code explanation

It is quite a tricky thing to do in many ways.  
The body of the process is AddFoodDistribution.cs.  

* Adding a dedicated toggle to the container sorting menu.
  * I didn't have a suitable hook for the processing, so I hooked the ContextMenu's AddToggle already.
  * I've already hooked the AddToggle in the ContextMenu, so if a specific string comes down in the AddToggle, I'll add my own.
  * Sorry if this is too heavy. I assume it will only run when the menu is displayed, so it should be acceptable. 
* Functions and static variables
  * I couldn't find any suitable hooks, so I followed the nesting of each function, and at the beginning of the nesting, I put a temporary value in a static variable.
  * I tried to follow the process, but I will die if there is an unexpected call order, etc. I apologize if there is. Sorry if there is.
* Dealing with local functions
  * C# allows you to create a function within a function, and it is called a local function.
  * The compiler usually gives a suitable method name statically, so if you call it in HarmonyPatch, it will be vulnerable to change.
  * In this case, I really needed to hook a local function called ThingContainer.GetDest().TrySearchContiner().
  * Fortunately, Harmony has a mechanism to call System.Action etc. as a patch directly by inserting MethodInfo.
  Action, etc. * I used it this time.
  * ApplyPatch() is the process of applying the patch.
* Saved data storage location.
  * The b1 flag group in Window.SaveData (Card.GetWindowSaveData()) in each container Card is diverted.
  * I'm using index values that didn't seem to be used yet & don't seem to be covered much.
    * Unholy -- 25
    * Cannibalism -- 26
    * Not corruptible -- 27
  * If in the future the original or other mods start using this index of b1, the mod will no longer function.