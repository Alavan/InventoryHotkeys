using Terraria.ModLoader;
using Terraria.GameInput;
using Terraria;

namespace InventoryHotkeys
{
    public class PlayerModded : ModPlayer
    {
        private int? _PreviousItemBeforeQuickUseKeyIndex;
        private int _QuickItemIndex;
        private bool _HoldingTheKey;
        private bool _StopAction;
        private ModHotKey _CurrentKey;

        // In MP, other clients need accurate information about your player or else bugs happen.
        // clientClone, SyncPlayer, and SendClientChanges, ensure that information is correct.
        // We only need to do this for data that is changed by code not executed by all clients, 
        // or data that needs to be shared while joining a world.
        // For example, examplePet doesn't need to be synced because all clients know that the player is wearing the ExamplePet item in an equipment slot. 
        // The examplePet bool is set for that player on every clients computer independently (via the Buff.Update), keeping that data in sync.
        // ExampleLifeFruits, however might be out of sync. For example, when joining a server, we need to share the exampleLifeFruits variable with all other clients.
        public override void clientClone(ModPlayer clientClone)
        {
            PlayerModded clone = clientClone as PlayerModded;
            // Here we would make a backup clone of values that are only correct on the local players Player instance.
            // Some examples would be RPG stats from a GUI, Hotkey states, and Extra Item Slots
            // clone.someLocalVariable = someLocalVariable;
        }

        /// <summary>
        /// Checks for any of the quick hotkeys.
        /// </summary>
        /// <param name="triggersSet">The triggers set.</param>
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
			if (InventoryHotkeys.QuickHotKeys != null) 
			{
				int itemIndex = 0;
				ModHotKey keyPressed = null;
				for (int i=0; i < InventoryHotkeys.QuickHotKeys.Length; i++)
				{
					if (InventoryHotkeys.QuickHotKeys[i] != null) 
					{
						if (InventoryHotkeys.QuickHotKeys[i].JustPressed)
						{
							itemIndex = i + 10;
							keyPressed = InventoryHotkeys.QuickHotKeys[i];
						}
					}
				}
				if (keyPressed != null && player != null)
				{
					_PreviousItemBeforeQuickUseKeyIndex = _PreviousItemBeforeQuickUseKeyIndex ?? player.selectedItem;
					_QuickItemIndex = itemIndex;
					_HoldingTheKey = true;
					_CurrentKey = keyPressed;
				}
				if (_CurrentKey != null && _CurrentKey.JustReleased)
				{
					_HoldingTheKey = false;
				}
			}
			base.ProcessTriggers(triggersSet);
        }

        /// <summary>
        /// Uses the item if we are holding the key
        /// </summary>
        /// <returns></returns>
        public override bool PreItemCheck()
        {
            if (_HoldingTheKey)
            {
                player.selectedItem = _QuickItemIndex;
                player.controlUseItem = true;
            }
            if (!_HoldingTheKey && _CurrentKey != null)
            { 
                _StopAction = true;
            }
            if (player.itemAnimation == 0 && _StopAction)
            {
                player.delayUseItem = true;
                player.selectedItem = _PreviousItemBeforeQuickUseKeyIndex.GetValueOrDefault();

                _PreviousItemBeforeQuickUseKeyIndex = null;
                _QuickItemIndex = 0;
                _CurrentKey = null;
                _StopAction = false;
            }
            return true;
        }
    }

}
