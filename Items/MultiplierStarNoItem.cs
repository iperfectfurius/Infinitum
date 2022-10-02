using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Infinitum;
using Terraria.Audio;
using Infinitum.Items.Ores;

namespace Infinitum.Items
{

    internal class MultiplierStarNoItem : ModItem
    {
        public const int ChanceFromOres = 175;
        public const int ChanceFromTrees = 250;
        public const int ChanceFromBlocks = ChanceFromOres * 25;
        //count as 4 tiles each??
        public const int ChanceFromHearts = 8;
        public const int ChanceFromPots = 50;
        public const int ChanceFromAltars = 12;
        public const int ChanceFromOrbs = 4;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Star Multiplier EXP(No item)");
            Tooltip.SetDefault("LOL +(2.5%) XP!");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99999;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.Orange;
            Item.consumable = false;
            Item.stack = 1;         
        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            return false;
        }
        public override bool CanPickup(Player player)
        {
            return true;
        }
        public override bool ItemSpace(Player player)
        {
            return true;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange += 35;
            base.GrabRange(player, ref grabRange);
        }
        public override bool OnPickup(Player player)
        {
            if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
            {
                player.GetModPlayer<Character_Data>().AddXpMultiplier(0.025f * Item.stack);
                SoundEngine.PlaySound(SoundID.Item129);
            }

            return false; ;
        }

    }
}


