
using Infinitum.Items.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Infinitum.WorldBuilding.Tiles
{
    internal class SanjacobosMineralTile : ModTile
    {
        public override void SetStaticDefaults()
        {
            this.MinPick = 35;
            this.MineResist = 3f;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.Ore[Type] = true;
            Main.tileOreFinderPriority[Type] = 100;
            AddMapEntry(new Color(200, 200, 200));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            
        }
        
        
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }


        public override bool Drop(int i, int j)
        {
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<SanjacobosOre>());
            return base.Drop(i, j);
        }
    }
}
