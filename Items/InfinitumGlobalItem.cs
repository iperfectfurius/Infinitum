using Infinitum.Skills;
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
        public static InfinitumGlobalItem instance;
        public static Character_Data playerDataHook;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();
            instance = this;
        }
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            ModPrefix prefix = PrefixLoader.GetPrefix(item.prefix);
            if (prefix != null)
            {
                switch (prefix.Name)
                {
                    case "UnrealPlus":
                    case "MythicalPlus":
                    case "LegendaryPlus":
                        tooltips.Insert(tooltips.Count, new TooltipLine(Mod, "Inifnitum", "[c/ff1493:+25% More XP]"));

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

        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (playerDataHook.Skills[(int)SkillEnums.SkillOrder.GrabRange].Level == 0) return;

            grabRange += (int)playerDataHook.Skills[(int)SkillEnums.SkillOrder.GrabRange].EffectBuff;

        }
    }
}
