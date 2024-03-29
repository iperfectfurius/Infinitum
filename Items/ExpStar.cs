using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;


namespace Infinitum.Items
{
	internal class ExpStar : ModItem
	{
		public const int NormalChanceFromNPCs = 1500;
        public const int ExpertChanceFromNPCs = 1250;
        public const int ChanceFromOres = 2500;
		public const int ChanceFromBlocks = 25000;
		public const int ChanceFromTrees = 3500;
		public const int ChanceFromFishing = 175;
        public const int ChanceFromHearts = 100;
        public const int ChanceFromPots = 500;
        public const int ChanceFromAltars = 400;
        public const int ChanceFromOrbs = 250;
        public override void SetStaticDefaults()
		{}

        public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.width = 25;
			Item.height = 25;
			Item.rare = ItemRarityID.Lime;
			Item.consumable = true;
			Item.UseSound = SoundID.Item2;
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
			if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
				player.GetModPlayer<Character_Data>().AddXp(Main.rand.Next(30000),MessageType.XPFromOtherSources);
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange += 35;
			base.GrabRange(player, ref grabRange);
		}
	}
}