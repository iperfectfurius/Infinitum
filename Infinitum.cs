using Terraria.ModLoader;
using Terraria;
using System.IO;
using Infinitum.UI;

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
        {//revisar
            base.HandlePacket(reader, whoAmI);
            AddXPToPlayer(reader.ReadSingle());

        }
        private void AddXPToPlayer(float xp)
        {//test myplayer
            Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(xp);
        }
        
    }
}