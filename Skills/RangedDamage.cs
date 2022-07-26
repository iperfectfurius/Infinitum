using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class RangedDamage : Skill
    {
        public RangedDamage(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.GetDamage(DamageClass.Ranged) = player.GetDamage(DamageClass.Ranged) + EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "Ranged Damage";
            DisplayName = "Ranged Damage";
            StatName = "GetDamage";
            Cost = 60;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }
    }
}
