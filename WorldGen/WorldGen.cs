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
        private const int CHANCE_BASE = 1000000;
        public override bool Drop(int i, int j, int type)
        {
            var item = TileLoader.GetTile(type);
            if (item != null)
            {
                
                float xp = (item.MinPick / 12);
                
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

                Main.NewText("modded");
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
            Main.NewText("Vanilla");
            //ModContent.TileType<Tile>


            //if (type == (int)TileIDEnum.Dirt)
            //    Item.NewItem(new EntitySource_TileBreak(i ,j, ModContent.ItemType<Items.ExpStar>);


            //Main.NewText(Main.netMode);   
            return base.Drop(i, j, type);

        }
        public override void PlaceInWorld(int i, int j, int type, Item item)
        {
            
            base.PlaceInWorld(i, j, type, item);
        }
    }

}

