﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class Defense : Skill
    {
        public Defense(int level) : base(level)
        {
        }
     
        public override void OnInitialize()
        {
            Name = "Defense";
            DisplayName = "Defense";
            StatName = "statDefense";
            Cost = 250;
            EffectBuff = 0;
            MultiplierEffect = 1;
            Type = SkillEnums.Type.PostUpdateEquips;
        }
        public override void ApplyStatToPlayer()
        {
            player.statDefense = player.statDefense + (int)EffectBuff;
        }
        public override string GetStatText()
        {
            return $"{PreText} {EffectBuff}";
        }

    }
}
