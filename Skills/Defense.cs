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
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 1;
        }
    }
}
