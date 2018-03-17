using Terraria.ModLoader;

namespace InventoryHotkeys
{
	public class InventoryHotkeys : Mod
	{
        public static InventoryHotkeys instance;
        public static ModHotKey[] QuickHotKeys = new ModHotKey[10];
        public InventoryHotkeys()
		{
			Properties = new ModProperties
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

        public override void Load()
        {
            instance = this;
            for(int i = 0; i < 10; i++)
            {
                QuickHotKeys[i] = RegisterHotKey("Quick Use Item " + (i + 11), "");
            }
        }

        public override void Unload()
        {
            QuickHotKeys = new ModHotKey[10];
			instance = null;
        }
    }
}
