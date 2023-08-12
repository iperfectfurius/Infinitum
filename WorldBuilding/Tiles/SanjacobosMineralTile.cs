
using Infinitum.Items;
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
        private const int multiplierStarChance = 100;
        private const int expStarChance = 2000;
        public const string TileName = "SanjacobosMineralTile";

        
        public override void SetStaticDefaults()
        {
            this.MinPick = 35;
            this.MineResist = 1f;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileLavaDeath[Type] = false;
            TileID.Sets.Ore[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileOreFinderPriority[Type] = 100;
            AddMapEntry(new Color(200, 200, 200),this.GetLocalization("SanjacobosTile"));
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.addTile(Type);
            HitSound = SoundID.Tink;
            RegisterItemDrop(ModContent.ItemType<SanjacobosOre>(), -1);
        }       
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            base.KillMultiTile(i, j, frameX, frameY);
        }
        public override bool CanReplace(int i, int j, int tileTypeBeingPlaced)//TODO prevent replace
        {
            return base.CanReplace(i, j, tileTypeBeingPlaced);
        }
        public static void GetDrops(int i, int j)
        {
            if (Main.rand.NextBool(multiplierStarChance))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<MultiplierStarNoItem>());
            if (Main.rand.NextBool(expStarChance))
                Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 32, 16, ModContent.ItemType<ExpStar>());
        }
        //public override bool Drop(int i, int j)
        //{       

        //    return base.Drop(i, j);
        //}
    }
}
