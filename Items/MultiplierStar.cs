using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
	internal class MultiplierStar : ModItem
	{
		//public override string Texture => "Terraria/Item_12";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Multiplier EXP");
			Tooltip.SetDefault("LOL");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 25;
			Item.height = 25;
			Item.rare = 3;
			Item.consumable = true;
			Item.UseSound = SoundID.DD2_BallistaTowerShot;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.autoReuse = true;
			Item.useTime = 15;
			Item.useAnimation = 15;
			Item.reuseDelay = 0;
			Item.useTurn = true;

		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}
		public override bool? UseItem(Player player)
		{
			
			Character_Data.AddXpMultiplier(0.025f);
			return true;
		}
	}
}