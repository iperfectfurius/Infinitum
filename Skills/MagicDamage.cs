using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class MagicDamage : Skill
    {
        public MagicDamage(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.GetDamage(DamageClass.Magic) = player.GetDamage(DamageClass.Magic) + (float)EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "MagicDamage";
            DisplayName = "Magic Damage";
            StatName = "GetDamage";
            Cost = 60;
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }
    }
}
