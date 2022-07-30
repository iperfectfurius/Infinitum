using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class SummonDamage : Skill
    {
        public SummonDamage(int level) : base(level)
        {
        }

        public override void OnInitialize()
        {
            Name = "SummonDamage";
            DisplayName = "Summon Damage";
            StatName = "GetDamage";
            Cost = 60;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }
        public override void ApplyStatToPlayer()
        {
            player.GetDamage(DamageClass.Summon) = player.GetDamage(DamageClass.Summon) + (float)EffectBuff;
        }
    }
}
