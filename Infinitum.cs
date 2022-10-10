using Terraria.ModLoader;
using Terraria;
using System.IO;
using Infinitum.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Chat;
using Microsoft.Xna.Framework;
using Infinitum.WorldChanges;

namespace Infinitum
{
    public class Infinitum : Mod
    {
        public static Infinitum instance;
        public static Mod myMod = ModLoader.GetMod("Infinitum");
        private AdaptativeDifficulty difficulty;

        internal AdaptativeDifficulty Difficulty{ get => difficulty; set => difficulty = value; }

        public Infinitum() { }

        public enum MessageType : byte
        {
            XP,
            XPMultiplier
        }
        public override void Load()
        {
            instance = this;
            difficulty = new AdaptativeDifficulty(Difficulties.Normal);
            base.Load();                    
        }

        
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {//TODO: Rework with ids and stuff
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

        public void ChatMessage(string text, Color red)
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
        public void GameMessage(string text, Color color)
        {
            if (Main.netMode == NetmodeID.Server || Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), color);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text,color);
            }
        }

    }
}