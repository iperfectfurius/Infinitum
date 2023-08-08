using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using System.Linq;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Infinitum.Items;
using System.Security.Cryptography.X509Certificates;

namespace Infinitum.WorldBuilding
{
    internal class WorldGen : GlobalTile
    {
        private static Mod myMod = ModLoader.GetMod("Infinitum");
        private float baseXP = 0.5f;
        private float accumulatedXP = 0;
        private bool haveXPAccumulated = false;
        private Task timer;
        private bool notUnloadedTiles = true;
        private readonly int[] blockCountedAsORe = new int[] { 63, 64, 65, 66, 67, 68, 262, 263, 264, 265, 266, 267, 408 };
        public HashSet<string> bannedTiles = new HashSet<string>();
        private ModPacket myPacket;

        public override void Drop(int i, int j, int type)
        {

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
                //provisional, calamity dont register ores to main.
                if (!TileID.Sets.Ore[type] && !Main.tileSpelunker[type])
                {
                    if (Main.rand.NextBool(MultiplierStarNoItem.ChanceFromBlocks))
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<MultiplierStarNoItem>());
                    if (Main.rand.NextBool(ExpStar.ChanceFromBlocks))
                        Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<ExpStar>());

                    return base.Drop(i, j, type);
                }


                xp = (tile.MinPick * baseXP);

                //test
                if (xp == 0)
                    xp = 1;

                if (tile.GetType().Name == "SanjacobosMineralTile")
                    xp += 35f;

                //if tile is more big than 1 tile better to sendAccumulated XP for less traffic
                if (Main.netMode != NetmodeID.Server)
                {
                    Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp,MessageType.XPFromOtherSources);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    sendXPToPlayers(xp);

                }
                if (Main.rand.NextBool(MultiplierStarNoItem.ChanceFromOres))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<MultiplierStarNoItem>());
                if (Main.rand.NextBool(ExpStar.ChanceFromOres))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<ExpStar>());
                return base.Drop(i, j, type);

            }         

            if (!isOre(type))
            {
                int multiplierStarChance = MultiplierStarNoItem.ChanceFromBlocks;
                int expStarChance = ExpStar.ChanceFromBlocks;

                //Special and global Tiles.
                switch (type)
                {

                    case (int)TileIDEnum.PineTree:
                    case (int)TileIDEnum.PalmTree:
                    case (int)TileIDEnum.Trees:
                        sendAccumulatedXPFromTile(1.5f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeAmethyst:
                        sendAccumulatedXPFromTile(35.0f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeTopaz:
                        sendAccumulatedXPFromTile(37.5f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeSapphire:
                        sendAccumulatedXPFromTile(38.5f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeEmerald:
                        sendAccumulatedXPFromTile(40.0f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeRuby:
                        sendAccumulatedXPFromTile(42.5f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeAmber:
                        sendAccumulatedXPFromTile(50.0f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.TreeDiamond:
                        sendAccumulatedXPFromTile(50.0f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                        expStarChance = ExpStar.ChanceFromTrees;
                        break;
                    case TileID.Heart:
                    case TileID.LifeFruit:
                        sendAccumulatedXPFromTile(500f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromHearts;
                        expStarChance = ExpStar.ChanceFromHearts;
                        break;
                    case TileID.DemonAltar:
                        sendAccumulatedXPFromTile(500f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromAltars;
                        expStarChance = ExpStar.ChanceFromAltars;
                        break;
                    case TileID.ShadowOrbs:
                        sendAccumulatedXPFromTile(250f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromOrbs;
                        expStarChance = ExpStar.ChanceFromOrbs;
                        break;
                    case TileID.Pots:
                        sendAccumulatedXPFromTile(7.5f);
                        multiplierStarChance = MultiplierStarNoItem.ChanceFromPots;
                        expStarChance = ExpStar.ChanceFromPots;
                        break;
                    default:
                        if (TileID.Sets.IsATreeTrunk[type])
                        {
                            sendAccumulatedXPFromTile(0.5f);
                            multiplierStarChance = MultiplierStarNoItem.ChanceFromTrees;
                            expStarChance = ExpStar.ChanceFromTrees;
                        }
                        break;
                }

                if (Main.rand.NextBool(multiplierStarChance))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<MultiplierStarNoItem>());
                if (Main.rand.NextBool(expStarChance))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<ExpStar>());
                return base.Drop(i, j, type);

            }

            switch (type)
            {
                case (int)TileIDEnum.Copper:
                    xp = 12.5f;
                    break;
                case (int)TileIDEnum.Tin:
                    xp = 12.5f;
                    break;
                case (int)TileIDEnum.Lead:
                    xp = 15f;
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
                    xp = 37.5f;
                    break;
                case (int)TileIDEnum.Mythril:
                    xp = 42.5f;
                    break;
                case (int)TileIDEnum.Orichalcum:
                    xp = 42.5f;
                    break;
                case (int)TileIDEnum.Adamantite:
                    xp = 47.5f;
                    break;
                case (int)TileIDEnum.Titanium:
                    xp = 47.5f;
                    break;
                case (int)TileIDEnum.Chlorophyte:
                    xp = 52.5f;
                    break;
                case (int)TileID.LunarOre:
                    xp = 55.0f;
                    break;
                case (int)TileIDEnum.AmethystGemspark:
                case (int)TileIDEnum.Amethyst:
                    xp = 100f;
                    break;
                case (int)TileIDEnum.TopazGemspark:
                case (int)TileIDEnum.Topaz:
                    xp = 105f;
                    break;
                case (int)TileIDEnum.SapphireGemspark:
                case (int)TileIDEnum.Sapphire:
                    xp = 110f;
                    break;
                case (int)TileIDEnum.EmeraldGemspark:
                case (int)TileIDEnum.Emerald:
                    xp = 115f;
                    break;
                case (int)TileIDEnum.RubyGemspark:
                case (int)TileIDEnum.Ruby:
                    xp = 120;
                    break;
                case (int)TileIDEnum.DiamondGemspark:
                case (int)TileIDEnum.Diamond:
                    xp = 130f;
                    break;
                default:
                    break;
            }

            if (Main.netMode != NetmodeID.Server)
            {
                Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp,MessageType.XPFromOtherSources);
            }

            else if (Main.netMode == NetmodeID.Server)//too much traffic?
            {
                sendXPToPlayers(xp);
            }

            if (Main.rand.NextBool(MultiplierStarNoItem.ChanceFromOres))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<MultiplierStarNoItem>());
            if (Main.rand.NextBool(ExpStar.ChanceFromOres))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<ExpStar>());

            return base.Drop(i, j, type);

        }
        private bool isOre(int type)
        {
            if (TileID.Sets.Ore[type] || blockCountedAsORe.Contains(type) || TileLoader.GetTile(type) != null)
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
            //TODO Check for multitile blocks
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
        private void sendXPToPlayers(float xp)
        {
            myPacket = myMod.GetPacket();
            myPacket.Write((byte)MessageType.XPFromOtherSources);
            myPacket.Write(xp);
            myPacket.Send();
        }
        private void sendAccumulatedXPFromTile(float xp)
        {
            //This method can prevent a lot of traffic
            accumulatedXP += xp;

            if (!haveXPAccumulated)
            {
                haveXPAccumulated = true;
                timer = Task.Delay(100).ContinueWith((e) =>
                {

                    if (Main.netMode == NetmodeID.SinglePlayer)
                    {
                        Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(accumulatedXP,MessageType.XPFromOtherSources);
                    }

                    else if (Main.netMode == NetmodeID.Server)
                    {
                        sendXPToPlayers(accumulatedXP);
                    }
                    haveXPAccumulated = false;
                    accumulatedXP = 0;
                });
            }
        }


    }

}

