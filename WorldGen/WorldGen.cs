using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
namespace Infinitum.WorldGen
{
    internal class WorldGen : GlobalTile
    {
        private static Mod myMod = ModLoader.GetMod("Infinitum");
        private float baseXP = 2f;
        private bool notUnloadedTiles = true;
        private const int CHANCE_BASE = 1000000;
        public HashSet<string> bannedTiles = new HashSet<string>();

        public override bool Drop(int i, int j, int type)
        {
            if (!isOre(type)) return base.Drop(i, j, type);

            float xp = 0;
            string pos = $"{i}-{j}";
            if (bannedTiles.Contains(pos))
            {
                Task.Run(() => bannedTiles.Remove(pos));
                return base.Drop(i, j, type);
            }


            var tile = TileLoader.GetTile(type);
            if (tile != null)
            {

                xp = (tile.MinPick / baseXP);

                if (Main.netMode != NetmodeID.Server)
                {
                    Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp);
                }

                else if (Main.netMode == NetmodeID.Server)
                {
                    ModPacket myPacket = myMod.GetPacket();
                    myPacket.Write(xp);
                    myPacket.Send();
                }

                //Infinitum.instance.ChatMessage("Modded");
                return base.Drop(i, j, type);

            }
            xp = 0;
            //Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp();

            switch (type)
            {
                case (int)TileIDEnum.Copper:
                    xp = 12.5f;
                    break;
                case (int)TileIDEnum.Tin:
                    xp = 12.5f;
                    break;
                case (int)TileIDEnum.Iron:
                    xp = 15f;
                    break;
                case (int)TileIDEnum.Silver:
                    xp = 17.5f;
                    break;
                case (int)TileIDEnum.Tungsten:
                    xp = 20f;
                    break;
                case (int)TileIDEnum.Gold:
                    xp = 22.5f;
                    break;
                case (int)TileIDEnum.Platinum:
                    xp = 25f;
                    break;
                case (int)TileIDEnum.Meteorite:
                    xp = 27.5f;
                    break;
                case (int)TileIDEnum.Demonite:
                    xp = 30f;
                    break;
                case (int)TileIDEnum.Crimtane:
                    xp = 32.5f;
                    break;
                case (int)TileIDEnum.Hellstone:
                    xp = 35f;
                    break;
                case (int)TileIDEnum.Cobalt:
                    xp = 37.5f;
                    break;
                case (int)TileIDEnum.Palladium:
                    xp = 40f;
                    break;
                case (int)TileIDEnum.Mythril:
                    xp = 42.5f;
                    break;
                case (int)TileIDEnum.Orichalcum:
                    xp = 45f;
                    break;
                case (int)TileIDEnum.Adamantite:
                    xp = 47.5f;
                    break;
                case (int)TileIDEnum.Titanium:
                    xp = 50f;
                    break;
                case (int)TileIDEnum.Chlorophyte:
                    xp = 52.5f;
                    break;
                case (int)TileIDEnum.Amethyst:
                    xp = 100f;
                    break;
                case (int)TileIDEnum.Topaz:
                    xp = 105f;
                    break;
                case (int)TileIDEnum.Sapphire:
                    xp = 110f;
                    break;
                case (int)TileIDEnum.Emerald:
                    xp = 115f;
                    break;
                case (int)TileIDEnum.Ruby:
                    xp = 120;
                    break;
                case (int)TileIDEnum.Diamond:
                    xp = 130f;
                    break;
                default:
                    break;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp);
            }

            else if (Main.netMode == NetmodeID.Server)//too much traffic?
            {
                ModPacket myPacket = myMod.GetPacket();
                myPacket.Write(xp);
                myPacket.Send();
            }

            //Infinitum.instance.ChatMessage("Vanilla");
            return base.Drop(i, j, type);

        }
        private bool isOre(int type)
        {
            if (TileID.Sets.Ore[type] || type == 67 || type == 66 || type == 63 || type == 65 || type == 64 || type == 68)
                return true;
            return false;
        }
        private bool isOre(ModTile tile, int type)
        {
            return true;
        }

        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            base.PlaceInWorld(i, j, type, item);

            if (isOre(item.createTile))
            {
                string pos = $"{i}-{j}";
                bannedTiles.Add(pos);
            }

        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);

        }

        public override void Unload()
        {
            //save actual banned tiles?
            bannedTiles.Clear();
            base.Unload();
        }
    }

}

