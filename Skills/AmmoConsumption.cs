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
        public override bool LevelUp(ref int Levels)
        {
            if (Levels > Cost)
            {
                Levels -= Cost;
                Level++;
                EffectBuff -= MultiplierEffect;
                return true;
            }
            return false;
        }
        public override bool LevelDown(ref int Levels)
        {
            if (Level > 0)
            {
                Levels += Cost;
                Level--;
                EffectBuff += MultiplierEffect;
                return true;
            }
            return false;
        }
        public override bool LevelUpAll(ref int Levels)
        {
            if (Levels > Cost)
            {
                int maxLevels = Levels / Cost;
                Levels -= Cost * maxLevels;
                Level += maxLevels;
                EffectBuff -= maxLevels * MultiplierEffect;
                return true;
            }
            return false;
        }
        public override void ApplyStatToPlayer(out bool arg)
        {
            if (EffectBuff < 101 && EffectBuff > 1)
            {
                arg = !(Main.rand.Next(100) <= Math.Abs(100 - (int)EffectBuff));
                Main.NewText(arg);
                return;
            }
            Main.NewText("true");
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
