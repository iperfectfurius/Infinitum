using Infinitum.Buffs;
using Infinitum.Items.Ores;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class XPPotion : ModItem
    {
        
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("XP Potion");
            Tooltip.SetDefault("MORE XP, PROBLEM?\n[c/ff1493:+50% More XP]");
            
        }
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.Yellow;
            Item.consumable = true;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.autoReuse = false;
            Item.useTime = 60;
            Item.useAnimation = 8;
            Item.useTurn = false;
            Item.buffTime = 28800;
            Item.buffType = ModContent.BuffType<XPBuff>();

        }
        public override bool? CanAutoReuseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<ExpStar>(), 1);
            recipe.Register();

            base.AddRecipes();
        }
    }
}
