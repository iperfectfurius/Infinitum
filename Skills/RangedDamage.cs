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
            player.GetDamage(DamageClass.Ranged) = player.GetDamage(DamageClass.Ranged) + (float)EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "RangedDamage";
            DisplayName = "Ranged Damage";
            StatName = "GetDamage";
            Cost = 60;
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            Type = SkillEnums.Type.PostUpdateEquips;
        }
    }
}
