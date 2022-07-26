namespace Infinitum.Skills
{
    internal class LifeRegen : Skill
    {
        public LifeRegen(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            player.lifeRegen = player.lifeRegen + (int)EffectBuff;
        }

        public override void OnInitialize()
        {
            Name = "Life Regen";
            DisplayName = "Life Regen";
            StatName = "lifeRegen";
            Cost = 125;
            MultiplierCost = 0;//after 1.0v
            EffectBuff = 0;
            MultiplierEffect = 0.25f;
        }
    }
}
