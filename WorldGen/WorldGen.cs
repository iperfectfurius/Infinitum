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
        private float baseXP = 5f;
        private bool notUnloadedTiles = true;
        private const int CHANCE_BASE = 1000000;
        public HashSet<string> bannedTiles = new HashSet<string>();

        //private Dictionary<int[], int> map = new Dictionary<int[], int>();

        public override bool Drop(int i, int j, int type)
        {
            string pos = $"{i}-{j}";
            if (bannedTiles.Contains(pos))
            {
                Task.Run(() => bannedTiles.Remove(pos));
                return base.Drop(i, j, type);
            }


            var tile = TileLoader.GetTile(type);
            if (tile != null && isOre(type))
            {

                float xp = (tile.MinPick / baseXP);

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

            switch (type)
            {
                //case (int)TileIDEnum.Copper:
                    

                //    break;
                default:
                    isOre(type);
                    break;
            }
            //Infinitum.instance.ChatMessage("Vanilla");
            return base.Drop(i, j, type);

        }
        private bool isOre(int type)
        {
            bool isOre = TileID.Sets.Ore[type];
            return isOre;
        }
        private bool isOre(ModTile tile,int type)
        {

            return true;
        }

        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            base.PlaceInWorld(i, j, type, item);

            if (isOre(item.createTile))
            {
                string pos = $"{i}-{j}";
                if (!bannedTiles.Contains(pos))
                    bannedTiles.Add(pos);
            }




            //Infinitum.instance.ChatMessage($"{i} : {j}");

        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {          
            base.KillTile(i, j, type, ref fail, ref effectOnly, ref noItem);
            if (!fail) Infinitum.instance.ChatMessage("Tile killed");

        }

        public override void Unload()
        {
            //save actual banned tiles?
            bannedTiles.Clear();
            base.Unload();
        }
    }

}

