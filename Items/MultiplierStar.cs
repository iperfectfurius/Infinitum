using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Infinitum;
namespace Infinitum.Items
{
	internal class MultiplierStar : ModItem
	{
		//private Infinitum infinitumMod = (Infinitum)ModLoader.GetMod("Infinitum");
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
			//Infinitum.PlayerModded.AddXpMultiplier(0.025f);
			//Character_Data.AddXpMultiplier(0.025f);
			//Character_Data.ModPlayer(player);
			Character_Data modPlayer = Character_Data.Get(player);
			
			modPlayer.AddXpMultiplier(0.025f);
			
			

			return true;
		}
	}
}