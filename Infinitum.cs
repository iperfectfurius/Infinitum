using Terraria.ModLoader;
using Terraria;
using System.IO;
using Infinitum.UI;
using Terraria.ID;
using Terraria.Localization;
using Terraria.Chat;
using Microsoft.Xna.Framework;
using Infinitum.WorldChanges;
using log4net.Util;

namespace Infinitum
{
    public enum MessageType : byte
    {
        XPFromNPCs,
        XPFromOtherSources,
        XPMultiplier,
        ChangeDifficulty,
        GetDifficultySettings
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
        {
            MessageType messageType = (MessageType)reader.ReadByte();
            myPacket = myMod.GetPacket();
            switch (messageType)
            {
                case MessageType.XPFromNPCs:
                case MessageType.XPMultiplier:
                    if (Main.netMode == NetmodeID.Server)
                    {
                        myPacket.Write((byte)messageType);
                        myPacket.Write(reader.ReadSingle());
                        myPacket.Send();
                    }
                    else
                    {
                        AddXPToPlayer(reader.ReadSingle());
                    }
                    break;
                case MessageType.ChangeDifficulty:
                    if (Main.netMode == NetmodeID.Server) return;

                    Difficulty.DifficultySetted = (Difficulties)reader.ReadByte();
                    Difficulty.Hp = reader.ReadSingle();
                    Difficulty.Speed = reader.ReadSingle();
                    Difficulty.Defense = reader.ReadSingle();
                    Difficulty.Damage = reader.ReadSingle();
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