using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.ItemDropRules;
using Terraria.ModLoader;

namespace Infinitum.Items
{
    internal class InfinitumGlobalItem : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            ModPrefix prefix = PrefixLoader.GetPrefix(item.prefix);
            if(prefix != null)
            {
                switch (prefix.Name)
                {
                    case "UnrealPlus":
                    case "MythicalPlus":
                    case "LegendaryPlus":
                        tooltips.Insert(tooltips.Count,new TooltipLine(Mod, "Inifnitum", "[c/ff1493:+25% More XP]"));
                        
                        break;
                    default:
                        break;
                }
            }

        }
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<MiniExpStar>(), 1));
            base.ModifyItemLoot(item, itemLoot);
        }
    }
}
