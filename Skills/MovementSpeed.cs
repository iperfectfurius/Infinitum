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
            player.accRunSpeed = player.accRunSpeed + (float)EffectBuff;
            player.moveSpeed = player.moveSpeed + (float)EffectBuff;
            player.maxRunSpeed = player.maxRunSpeed + (float)EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "MovementSpeed";
            DisplayName = "Movement Speed";
            StatName = "moveSpeed";
            Cost = 185;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.01f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }
    }
}
