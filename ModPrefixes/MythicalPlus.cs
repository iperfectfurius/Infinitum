using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;

namespace Infinitum.ModPrefixes
{
    internal class MythicalPlus : ModPrefix
    {
        private readonly byte _power;

        public override float RollChance(Item item)
            => 3.5f;

        public override bool CanRoll(Item item)
            => true;

        public override PrefixCategory Category
            => PrefixCategory.Magic;

        public MythicalPlus()
        {

        }

        public MythicalPlus(byte power)
        {
            _power = power;
        }


        public override void Apply(Item item)
        {


        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            damageMult += (float)(damageMult * 0.15f);
            knockbackMult += (float)(knockbackMult * 0.15f);
            manaMult -= (float)(manaMult * 0.10f);
            useTimeMult -= (float)(useTimeMult * 0.10f);
            critBonus += 5;



            base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
        }
        public override void ModifyValue(ref float valueMult)
        {

            float multiplier = 1f + 0.05f * _power;
            valueMult *= multiplier;
        }
    }
}
