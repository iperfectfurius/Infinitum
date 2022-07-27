using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class LifeSteal : Skill
    {
        private bool activate;//temp
        private float stackedLifeSteal = 0;
        public LifeSteal(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer(int damage)
        {
            GetLifeSteal(damage);
        }
        public override void OnInitialize()
        {
            Name = "LifeSteal";
            DisplayName = "Life Steal";
            StatName = "statLife";
            Cost = 125;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.00025f;
            Type = (int)SkillEnums.Type.ModifyHitNPC;
        }
        private void GetLifeSteal(int damage)
        {
            int toHeal = (int)(damage * (float)EffectBuff);
            stackedLifeSteal += (damage * (float)EffectBuff) - (float)Math.Truncate(damage * (float)EffectBuff);

            //rework
            if (stackedLifeSteal > 1)
            {
                stackedLifeSteal -= 1f;

                player.HealEffect(toHeal + 1);
                player.statLife += toHeal + 1;
            }
            else if (toHeal >= 1)
            {
                player.HealEffect(toHeal);
                player.statLife += toHeal;
            }
        }
    }
}
