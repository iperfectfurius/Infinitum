﻿using System;
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
            player.GetDamage(DamageClass.Magic) = player.GetDamage(DamageClass.Magic) + EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "Magic Damage";
            DisplayName = "Magic Damage";
            StatName = "GetDamage";
            Cost = 60;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }
    }
}
