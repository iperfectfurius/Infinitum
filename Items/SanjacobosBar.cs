using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class SanjacobosBar : ModItem
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanjacobo's Bar");

            Tooltip.SetDefault("Really you need more XP?");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99999;
            Item.width = 25;
            Item.height = 25;
            Item.rare = ItemRarityID.Orange;
            Item.material = true;

        }

        public override bool CanUseItem(Player player)
        {
            return false;
        }
        public override bool? UseItem(Player player)
        {
            return false;
        }

    }

}
