﻿using Infinitum.WorldGen.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items.Ores
{
	internal class SanjacobosOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sanjacobo's Ore");
			Tooltip.SetDefault("Good Shit");
		}

		public override void SetDefaults()
		{
			Item.maxStack = 999;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 20;
			Item.height = 20;
			Item.rare = ItemRarityID.LightRed;
			Item.consumable = true;
			Item.autoReuse = true;
			Item.useTime = 10;
			Item.useAnimation = 10;
			Item.reuseDelay = 0;
			Item.useTurn = true;
			Item.createTile = ModContent.TileType<SanjacobosMineralTile>();


		}
		public override bool OnPickup(Player player)
		{
			//if (Main.netMode != NetmodeID.Server && player.whoAmI == Main.myPlayer)
			//{
			//	player.GetModPlayer<Character_Data>().AddXpMultiplier(0.075f);
			//	SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot);
			//}

			//return false; ç
			return true;
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange += 25;
			base.GrabRange(player, ref grabRange);
		}
		
	}
}