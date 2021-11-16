using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
	internal class ExpStar : ModItem
	{
		//public override string Texture => "Terraria/Item_12";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Exp");
			Tooltip.SetDefault("What you expect?");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 55;
			Item.height = 55;
			Item.rare = 3;
			Item.consumable = true;
			Item.UseSound = SoundID.DD2_BallistaTowerShot;
			Item.useStyle = ItemUseStyleID.EatFood;
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}
		public override bool? UseItem(Player player)
		{
			Character_Data.AddXp(Main.rand.Next(400000));
			return true;
		}
	}
}