﻿using Infinitum.Buffs;
using Infinitum.Commands;
using Infinitum.Items.Ores;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace Infinitum.Items
{
    internal class InfinitumTotem : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinitum Totem");
            Tooltip.SetDefault($"Change current difficulty for this map\nCurrently difficulty: {Infinitum.instance.Difficulty.DifficultySetted}");

        }
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.Purple;
            Item.consumable = false;
            Item.UseSound = SoundID.Item3;
            Item.useStyle = ItemUseStyleID.DrinkLiquid;
            Item.autoReuse = false;
            Item.useTime = 120;
            Item.useAnimation = 8;
            Item.useTurn = false;

        }
        public override bool? CanAutoReuseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            //ChatManager.Commands.ProcessIncomingMessage(new ChatMessage(": difficulty"),Main.myPlayer);
            Main.ExecuteCommand("/difficulty",new InifnitumCommandCaller());
            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.Register();

            base.AddRecipes();
        }

        public class InifnitumCommandCaller : CommandCaller
        {
            public CommandType CommandType => CommandType.World;

            public Player Player => Main.clientPlayer;

            public void Reply(string text, Color color = default)
            {
                //throw new NotImplementedException();
            }
        }
    }
}
