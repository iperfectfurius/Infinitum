using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class GrabRange : Skill
    {
        public GrabRange(int level) : base(level)
        {
        }

        public override void OnInitialize()
        {
            Name = "GrabRange";
            DisplayName = "Grab Range";
            StatName = "GrabRange";
            Cost = 10;
            MultiplierCost = 0.15f;
            EffectBuff = 0;
            MultiplierEffect = 10f;
            Type = SkillEnums.Type.Other;
        }

        public override string GetStatText()
        {
            return $"+ {EffectBuff:n2}";
        }
    }
}
