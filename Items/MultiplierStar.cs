using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Infinitum;
using Terraria.Audio;
using Infinitum.Items.Ores;

namespace Infinitum.Items
{
	internal class MultiplierStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Multiplier EXP");
			Tooltip.SetDefault("LOL");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 99999;
			Item.width = 25;
			Item.height = 25;
			Item.rare = ItemRarityID.Orange;
			Item.consumable = true;
			Item.UseSound = SoundID.DD2_BallistaTowerShot;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = true;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.reuseDelay = 0;
			Item.useTurn = true;
			

		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}
		public override bool? UseItem(Player player)
		{
			if(Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
				player.GetModPlayer<Character_Data>().AddXpMultiplier(0.025f);
			
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange += 35;
			base.GrabRange(player, ref grabRange);
		}
        public override void AddRecipes()
        {
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<SanjacobosOre>(), 25);
			recipe.Register();

			base.AddRecipes();
        }
    }
}