using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class MovementSpeed : Skill
    {
        public MovementSpeed(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.accRunSpeed = player.accRunSpeed + EffectBuff;
            player.moveSpeed = player.moveSpeed + EffectBuff;
            player.maxRunSpeed = player.maxRunSpeed + EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "Movement Speed";
            DisplayName = "Movement Speed";
            StatName = "moveSpeed";
            Cost = 185;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
        }
    }
}
