using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class SanjacobosBar : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanjacobo's Bar");

            Tooltip.SetDefault("Really you need more XP?");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99999;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.Orange;
            Item.material = true;

        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            return false;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CopperBar, 6);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TinBar, 6);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.IronBar, 6);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LeadBar, 6);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.SilverBar, 5);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TungstenBar, 5);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.GoldBar, 4);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PlatinumBar, 4);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MeteoriteBar, 4);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 15);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 8);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Obsidian, 50);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CobaltBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.PalladiumBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.MythrilBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.OrichalcumBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.AdamantiteBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.TitaniumBar, 3);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.ChlorophyteBar, 4);
            recipe.Register();

            recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.LunarBar, 4);
            recipe.Register();

            base.AddRecipes();
        }
    }

}
