using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Infinitum.WorldGen
{
    internal class WorldGen : GlobalTile
    {
        private const int CHANCE_BASE = 1000000;
        public override bool Drop(int i, int j, int type)
        {
            //ModContent.TileType<Tile>
            //Main.NewText(fail);

            //if (type == (int)TileIDEnum.Dirt)
            //    Item.NewItem(new EntitySource_TileBreak(i ,j, ModContent.ItemType<Items.ExpStar>);
            
            
            //Main.NewText(Main.netMode);   
            return base.Drop(i, j, type);

        }
    }
}
