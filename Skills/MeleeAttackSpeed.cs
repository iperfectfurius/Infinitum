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
            player.GetAttackSpeed(DamageClass.Melee) = player.GetAttackSpeed(DamageClass.Melee) + (float)EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "MeleeAttackSpeed";
            DisplayName = "Melee Speed";
            StatName = "GetAttackSpeed";
            Cost = 60;
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }
    }
}
