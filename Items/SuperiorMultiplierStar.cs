﻿
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class SuperiorMultiplierStar : ModItem
    {
		public const int ChanceFromNPCS = 500;
		public override void SetStaticDefaults()
		{}			

		public override void SetDefaults()
		{
			Item.maxStack = 99999;
			Item.width = 25;
			Item.height = 25;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.autoReuse = true;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.reuseDelay = 0;
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
		public override bool OnPickup(Player player)
        {
			if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
			{
				player.GetModPlayer<Character_Data>().AddXpMultiplier(0.075f * Item.stack);
				SoundEngine.PlaySound(SoundID.Item113);
			}
				
			return false; ;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
			grabRange += 25;			
			base.GrabRange(player, ref grabRange);
        }
    }
}
