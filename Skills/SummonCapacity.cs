using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class SummonCapacity : Skill
    {
        public SummonCapacity(int level) : base(level)
        {
        }

        public override void OnInitialize()
        {
            Name = "MinionCapacity";
            DisplayName = "Summon Minions";
            StatName = "maxMinions";
            Cost = 1250;
            EffectBuff = 0;
            MultiplierEffect = 1;
            MultiplierCost = 1f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }
        public override void ApplyStatToPlayer()
        {
            player.maxMinions = player.maxMinions + (int)EffectBuff;
        }

        public override string GetStatText()
        {
            return $"{PreText} {EffectBuff}";
        }
    }
}
