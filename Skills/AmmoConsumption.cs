using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal class AmmoConsumption : Skill
    {
        public static string saveName = "RangedAmmoConsumption";
        public AmmoConsumption(int level) : base(level)
        {
        }

        public override void OnInitialize()
        {
            Name = saveName;
            DisplayName = "Ammo Consumption";
            StatName = "CanConsumeAmmo";
            Cost = 125;//after 1.0v
            EffectBuff = 101;
            MultiplierEffect = 1;
            MaxLevel = 99;

            Type = (int)SkillEnums.Type.CanConsumeAmmo;
            PreText = '-';
        }
        public override void calculateBuff()
        {
            EffectBuff -= Level;
        }
        // TODO: Refactor all ApplyStat (Ammo Consumption)
        public override bool LevelUp(ref int Levels)
        {
            if (Levels > Cost)
            {
                Levels -= Cost;
                Level++;
                TotalSpend += Cost;
                CalcCost();
                EffectBuff -= MultiplierEffect;
                return true;
            }
            return false;
        }
        public override bool LevelDown(ref int Levels)
        {
            if (Level > 0)
            {
                Level--;
                CalcCost();
                Levels += Cost;         
                EffectBuff += MultiplierEffect;
                TotalSpend -= Cost;
                return true;
            }
            return false;
        }
        public override bool LevelUpAll(ref int Levels)
        {
            bool canLevelUp = Levels > Cost;

            while (LevelUp(ref Levels)) ;

            return canLevelUp;
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
        public override string GetStatText()
        {
            return $"{(int)EffectBuff-101}%";
        }
    }
}
