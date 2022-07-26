using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class GlobalCriticalChance : Skill
    {
        public GlobalCriticalChance(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.GetCritChance(DamageClass.Melee) = player.GetCritChance(DamageClass.Melee) + EffectBuff;
            player.GetCritChance(DamageClass.Magic) = player.GetCritChance(DamageClass.Magic) + EffectBuff;
            player.GetCritChance(DamageClass.Ranged) = player.GetCritChance(DamageClass.Ranged) + EffectBuff;
            //add for mods?
        }

        public override void OnInitialize()
        {

            Name = "Global Critical Chance";
            DisplayName = "Global Critical Chance";
            StatName = "GetCritChance";
            Cost = 250;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 1;
        }
    }

}
