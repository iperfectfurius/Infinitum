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
            player.manaCost = player.manaCost - (float)EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "ManaConsumption";
            DisplayName = "Mana Consumption";
            StatName = "manaCost";
            Cost = 60;
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            MaxLevel = 99;
            Type = SkillEnums.Type.PostUpdateEquips;
            PreText = '-';
        }
    }
}
