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
                        tooltips.Insert(6, new TooltipLine(Mod, "Inifnitum", "[c/FF0000:+ 25% Global XP!]"));
                        
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
