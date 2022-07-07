using Terraria.ModLoader;
using Terraria;
using System.Threading.Tasks;
using System.IO;
using Infinitum.UI;
using Terraria.UI;
using Terraria.DataStructures;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Infinitum
{
    public class Infinitum : Mod
    {
        public static Infinitum instance;
        public Infinitum() { }
        public override void Load()
        {
            base.Load();
            instance = this;
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            base.HandlePacket(reader, whoAmI);
            AddXPToPlayer(reader.ReadSingle());

        }
        private void AddXPToPlayer(float xp)
        {//test myplayer
            Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(xp);
        }

    }
}