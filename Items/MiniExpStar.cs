using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class MiniExpStar : ModItem
    {


        public override void SetStaticDefaults()
        {}

        public override void SetDefaults()
        {
            Item.maxStack = 99999;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.LightRed;
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
                player.GetModPlayer<Character_Data>().AddXp(1250,MessageType.XPFromOtherSources);
            return true;
        }
        public override void GrabRange(Player player, ref int grabRange)
        {
            grabRange += 35;
            base.GrabRange(player, ref grabRange);
        }
    }
}
