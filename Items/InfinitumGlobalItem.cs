using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class InfinitumGlobalItem : GlobalItem
    {

        
        public override bool? UseItem(Item item, Player player)
        {
            //player.GetModPlayer<Character_Data>().MoreExpMultiplier += 0.25f;
            return base.UseItem(item, player);
        }
        
        
    }
}
