using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class MeleeDamage : Skill
    {
        public MeleeDamage(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.GetDamage(DamageClass.Melee) = player.GetDamage(DamageClass.Melee) + EffectBuff;
        }

        public override void OnInitialize()
        {

            Name = "Melee Damage";
            DisplayName = "Melee Damage";
            StatName = "GetDamage";
            Cost = 60;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }
    }
}
