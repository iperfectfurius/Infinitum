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
        UpdateStats,
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
        private void CreateAndSendPacket(int to = -1)
        {
            //TODO create a method for send any packet
        }

        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            MessageType messageType = (MessageType)reader.ReadByte();
            
            switch (messageType)
            {
                case MessageType.XPFromNPCs:
                case MessageType.XPFromOtherSources:
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
                        AddXPToPlayer(reader.ReadSingle(),messageType == MessageType.XPFromNPCs);
                    }
                    break;
                case MessageType.UpdateStats://only Client
                    if (Main.netMode == NetmodeID.Server) return;

                    //TODO implement in class
                    Difficulty.DifficultySetted = (Difficulties)reader.ReadByte();
                    Difficulty.Hp = reader.ReadSingle();
                    Difficulty.Speed = reader.ReadSingle();
                    Difficulty.Defense = reader.ReadSingle();
                    Difficulty.Damage = reader.ReadSingle();                   
                    break;
                case MessageType.GetDifficultySettings:
                    if(Main.netMode == NetmodeID.Server)
                    {
                        myPacket = myMod.GetPacket();
                        myPacket.Write((byte)MessageType.UpdateStats);
                        myPacket.Write((byte)Difficulty.DifficultySetted);
                        myPacket.Write(Difficulty.Hp);
                        myPacket.Write(Difficulty.Speed);
                        myPacket.Write(Difficulty.Defense);
                        myPacket.Write(Difficulty.Damage);
                        myPacket.Send(whoAmI);
                    }
                    break;
            }

            base.HandlePacket(reader, whoAmI);
        }

        public void AddXPToPlayer(float xp,bool XPMultiplierApplicable = true)
        {//test myplayer
            Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(xp, XPMultiplierApplicable);
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