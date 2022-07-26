using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class ReducedManaConsumption : Skill
    {
        public ReducedManaConsumption(int level) : base(level)
        {
            
        }

        public override void ApplyStatToPlayer()
        {
            player.manaCost = player.manaCost - EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "Mana Consumption";
            DisplayName = "Mana Consumption";
            StatName = "manaCost";
            Cost = 60;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }
    }
}
