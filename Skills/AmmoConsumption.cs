using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class AmmoConsumption : Skill
    {
        public AmmoConsumption(int level) : base(level)
        {
        }

        public override void calculateBuff()
        {
            EffectBuff -= Level;
        }
        public override void ApplyStatToPlayer(out bool arg)
        {
            if (EffectBuff < 101 && EffectBuff > 1)
            {
                arg = !(Main.rand.Next(100) <= Math.Abs(100 - (int)EffectBuff));
                return;
            }
            arg = true;
        }


        public override void OnInitialize()
        {
            Name = "RangedAmmoConsumption";
            DisplayName = "Ammo Consumption";
            StatName = "CanConsumeAmmo";
            Cost = 125;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 101;
            MultiplierEffect = 1;
            Type = (int)SkillEnums.Type.CanConsumeAmmo;
            PreText = '-';
        }

        public override string GetStatText()
        {
            return $"{(int)EffectBuff-101}%";
        }
    }
}
