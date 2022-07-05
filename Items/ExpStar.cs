using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Infinitum;
namespace Infinitum.Items
{
	internal class ExpStar : ModItem
	{
		//public override string Texture => "Terraria/Item_12";
		//private Infinitum infinitumMod = (Infinitum)ModLoader.GetMod("Infinitum");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Exp");
			Tooltip.SetDefault("What you expect?");
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
			//Infinitum.PlayerModded.AddXp(Main.rand.Next(300000));
			//Infinitum.CD.AddXp(Main.rand.Next(300000));
			//Character_Data modPlayer = player.GetModPlayer<Character_Data>();
			player.GetModPlayer<Character_Data>().AddXp(Main.rand.Next(300000));
			return true;
		}
	}
}