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
        private float baseXP = 12f;
        private bool notUnloadedTiles = true;
        private const int CHANCE_BASE = 1000000;
        public HashSet<string> bannedTiles = new HashSet<string>();

        //private Dictionary<int[], int> map = new Dictionary<int[], int>();

        public override bool Drop(int i, int j, int type)
        {
            if (bannedTiles.Contains($"{i}-{j}"))
            {
                Infinitum.instance.ChatMessage("Placed by player!!");
                return base.Drop(i, j, type);
            }


            var item = TileLoader.GetTile(type);
            if (item != null)
            {

                float xp = (item.MinPick / baseXP);

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
                case (int)TileIDEnum.Copper:
                    Tile t = new Tile();

                    break;
                default:

                    break;
            }
            //Infinitum.instance.ChatMessage("Vanilla");
            return base.Drop(i, j, type);

        }
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            base.PlaceInWorld(i, j, type, item);

            //for(int x = 0; x < bannedTiles.Count; x += 2)
            //{

            //}
            if (item.material)
            {
                string pos = $"{i}-{j}";
                if (!bannedTiles.Contains(pos))
                    bannedTiles.Add(pos);
                else
                    Main.NewText("Ya esta dentro");
                Main.NewText(bannedTiles.Count);
            }



            Infinitum.instance.ChatMessage($"{i} : {j}");

        }
        private bool isIn()
        {
            return false;
        }
    }

}

