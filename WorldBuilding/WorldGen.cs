﻿using System;
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
        private const int CHANCE_BASE = 125;
        private int[] blockCountedAsORe = new int[] { 63, 64, 65, 66, 67, 68, 262, 263, 264, 265, 266, 267, };
        public HashSet<string> bannedTiles = new HashSet<string>();

        public override bool Drop(int i, int j, int type)
        {
            
            if (!isOre(type))
            {
                int specificChance = 25;

                switch (type)
                {
                    case (int)TileIDEnum.PineTree:
                    case (int)TileIDEnum.PalmTree:
                    case (int)TileIDEnum.Trees:
                        sendAccumulatedXPFromTrees(1.5f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeAmber:
                        sendAccumulatedXPFromTrees(50.0f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeDiamond:
                        sendAccumulatedXPFromTrees(50.0f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeRuby:
                        sendAccumulatedXPFromTrees(42.5f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeEmerald:
                        sendAccumulatedXPFromTrees(40.0f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeAmethyst:
                        sendAccumulatedXPFromTrees(35.0f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeTopaz:
                        sendAccumulatedXPFromTrees(37.5f);
                        specificChance -= 23;
                        break;
                    case TileID.TreeSapphire:
                        sendAccumulatedXPFromTrees(38.5f);
                        specificChance -= 23;
                        break;

                    default:
                        break;
                }
                

                
                if (Main.rand.NextBool(CHANCE_BASE * specificChance))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.MultiplierStarNoItem>());
                return base.Drop(i, j, type);
            }

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

                xp = (tile.MinPick * baseXP);

                if (tile.GetType().Name == "SanjacobosMineralTile")
                    xp += 35f;


                if (Main.netMode != NetmodeID.Server)
                {
                    Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp);
                }
                else if (Main.netMode == NetmodeID.Server)
                {
                    sendXPToPlayers(xp);

                }
                if (Main.rand.NextBool(CHANCE_BASE))
                    Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.MultiplierStarNoItem>());
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
                Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp);
            }

            else if (Main.netMode == NetmodeID.Server)//too much traffic?
            {
                sendXPToPlayers(xp);
            }

            if (Main.rand.NextBool(CHANCE_BASE))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<Items.MultiplierStarNoItem>());

            //Infinitum.instance.ChatMessage("Vanilla");
            return base.Drop(i, j, type);

        }
        private bool isOre(int type)
        {
            if (TileID.Sets.Ore[type] || blockCountedAsORe.Contains(type))
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
        private void sendXPToPlayers(float xp)
        {
            Task.Run(() =>
            {
                ModPacket myPacket = myMod.GetPacket();
                myPacket.Write(xp);
                myPacket.Send();
            });
        }
        private void sendAccumulatedXPFromTrees(float xp)
        {
            //This method can prevent a lot of traffic
            accumulatedXP += xp;

            if (!haveXPAccumulated)
            {
                haveXPAccumulated = true;
                timer = Task.Delay(100).ContinueWith((e) =>
                {

                    if (Main.netMode != NetmodeID.Server)
                    {
                        Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(accumulatedXP);
                    }

                    else if (Main.netMode == NetmodeID.Server)
                    {
                        ModPacket myPacket = myMod.GetPacket();
                        myPacket.Write(accumulatedXP);
                        myPacket.Send();
                    }
                    haveXPAccumulated = false;
                    accumulatedXP = 0;
                });
            }
        }
    }

}

