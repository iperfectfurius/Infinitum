using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace Infinitum
{
    internal class KeyBindSystem : ModSystem
    {
        public static ModKeybind UI;



        public override void Load()
        {

            //UI = KeybindLoader.RegisterKeybind(Infinitum, "Show UI", "L");
            base.Load();

        }
        public override void Unload()
        {
            base.Unload();
            //UI = null;
        }
    }
}
