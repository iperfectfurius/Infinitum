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
            Name = "Picking Speed";
            DisplayName = "Picking Speed";
            StatName = "pickSpeed";
            Cost = 10;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }

        public override void ApplyStatToPlayer()
        {
            player.pickSpeed = player.pickSpeed - EffectBuff;
        }
    }
}
