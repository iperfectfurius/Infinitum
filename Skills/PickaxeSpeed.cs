using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class PickaxeSpeed : Skill
    {
        public PickaxeSpeed(int level) : base(level)
        {
        }

        public override void OnInitialize()
        {
            Name = "PickaxeSpeed";
            DisplayName = "Picking Speed";
            StatName = "pickSpeed";
            Cost = 10;
            MultiplierCost = 0.02f;
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }

        public override void ApplyStatToPlayer()
        {
            player.pickSpeed = player.pickSpeed - (float)EffectBuff;
        }
    }
}
