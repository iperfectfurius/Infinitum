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
    public enum MessageType : byte
    {
        XP,
        XPMultiplier,
        ChangeDifficulty
    }
    public class Infinitum : Mod
    {
        public static Infinitum instance;
        public static Mod myMod = ModLoader.GetMod("Infinitum");
        private AdaptativeDifficulty difficulty;
        private ModPacket myPacket;

        internal AdaptativeDifficulty Difficulty { get => difficulty; set => difficulty = value; }

        public Infinitum() { }

        public override void Load()
        {
            base.Load();
            instance = this;
            
            difficulty = new AdaptativeDifficulty(Difficulties.Normal);           
        }
        public override void PostSetupContent()
        {
            base.PostSetupContent();
            
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {//TODO: Rework with ids and stuff
            //if (Main.netMode == NetmodeID.Server)
            //{
            //    ModPacket myPacket = myMod.GetPacket();
            //    myPacket.Write(reader.ReadSingle());
            //    myPacket.Send();
            //}
            //else//singlePlayer or client, doesn't matter
            //    AddXPToPlayer(reader.ReadSingle());

            MessageType messageType = (MessageType)reader.ReadByte();

            switch (messageType)
            {
                case MessageType.XP:
                case MessageType.XPMultiplier:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        myPacket = myMod.GetPacket();
                        myPacket.Write((byte)messageType);
                        myPacket.Write(reader.ReadSingle());
                        myPacket.Send();
                    }
                    else
                    {
                        AddXPToPlayer(reader.ReadSingle());
                    }
                    break;
            }

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
                Main.NewText(text, color);
            }
        }

    }
}