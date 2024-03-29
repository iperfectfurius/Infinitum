﻿using Infinitum.Buffs;
using Infinitum.Items.Ores;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class InfinitumPotion : ModItem
    {
        public override void SetStaticDefaults()
        {}
        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.Red;
            Item.consumable = true;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.autoReuse = false;
            Item.useTime = 90;
            Item.useAnimation = 8;
            Item.useTurn = true;
            Item.buffTime = 3600;
            Item.buffType = ModContent.BuffType<InfinitumBuff>();

        }
        public override bool? CanAutoReuseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {         
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff<InfinitumBuff>()) return false;
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<MiniExpStar>(), 1);
            recipe.AddIngredient(ModContent.ItemType<SanjacobosOre>(), 100);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.Register();

            base.AddRecipes();
        }
    }
}
