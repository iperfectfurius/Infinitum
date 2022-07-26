using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class MeleeAttackSpeed : Skill
    {
        public MeleeAttackSpeed(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.GetAttackSpeed(DamageClass.Melee) = player.GetAttackSpeed(DamageClass.Melee) + EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "Melee Speed";
            DisplayName = "Melee Speed";
            StatName = "GetAttackSpeed";
            Cost = 60;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }
    }
}
