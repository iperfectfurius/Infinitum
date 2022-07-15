using Terraria;
using Terraria.ModLoader;

namespace Infinitum.ModPrefixes
{
    internal class LegendaryPlus : ModPrefix
    {
        private readonly byte _power;
        private readonly float _more_exp = 1.25f;
        // see documentation for vanilla weights and more information
        // note: a weight of 0f can still be rolled. see CanRoll to exclude prefixes.
        // note: if you use PrefixCategory.Custom, actually use ChoosePrefix instead, see ExampleInstancedGlobalItem
        public override float RollChance(Item item)
            => 5f;

        // determines if it can roll at all.
        // use this to control if a prefixes can be rolled or not
        public override bool CanRoll(Item item)
            => true;

        // change your category this way, defaults to Custom
        public override PrefixCategory Category
            => PrefixCategory.Melee;

        public LegendaryPlus()
        {
            
        }

        public LegendaryPlus(byte power)
        {
            _power = power;
        }


        public override void Apply(Item item)
        {
          
            
        }

        public override void SetStats(ref float damageMult, ref float knockbackMult, ref float useTimeMult, ref float scaleMult, ref float shootSpeedMult, ref float manaMult, ref int critBonus)
        {
            critBonus += 15;
            damageMult += (float)(damageMult * 0.15f);
            knockbackMult += (float)(knockbackMult * 0.15f);
            //Main.CurrentPlayer.GetModPlayer<Character_Data>().ExpMultiplier *= _more_exp;
            //Main.NewText("t");

            base.SetStats(ref damageMult, ref knockbackMult, ref useTimeMult, ref scaleMult, ref shootSpeedMult, ref manaMult, ref critBonus);
        }
        public override void ModifyValue(ref float valueMult)
        {
            
            float multiplier = 1f + 0.05f * _power;
            valueMult *= multiplier;
        }
    }
}
