using Terraria.ModLoader;
using Terraria;
using System.IO;
using Infinitum.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Chat;
using Microsoft.Xna.Framework;

namespace Infinitum
{
    public class Infinitum : Mod
    {
        public static Infinitum instance;
        public static Mod myMod = ModLoader.GetMod("Infinitum");

        public Infinitum() { }
        public override void Load()
        {
            base.Load();
            instance = this;
            
        }
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {//Rework with ids and stuff
            if (Main.netMode == NetmodeID.Server)
            {
                ModPacket myPacket = myMod.GetPacket();
                myPacket.Write(reader.ReadSingle());
                myPacket.Send();
            }             
            else//singlePlayer or client, doesn't matter
                AddXPToPlayer(reader.ReadSingle());

            base.HandlePacket(reader, whoAmI);
        

        }
        public void AddXPToPlayer(float xp)
        {//test myplayer
            Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(xp);
        }

        public void ChatMessage(string text)
        {

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text + " Desde Server"), Color.Red);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text + " Desde single");
            }
        }
    }
}