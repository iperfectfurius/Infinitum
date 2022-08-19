namespace Infinitum.Skills
{
    internal class LifeRegen : Skill
    {
        private float StacketLifeRegen = 0f;
        public LifeRegen(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer()
        {
            //StacketLifeRegen += (float)EffectBuff - (int)EffectBuff;
            //if((int)StacketLifeRegen %15 == 0)
            //    Main.NewText(StacketLifeRegen);
            player.lifeRegen = player.lifeRegen + (int)EffectBuff;

            
        }

        public override void OnInitialize()
        {
            Name = "LifeRegen";
            DisplayName = "Life Regen";
            StatName = "lifeRegen";
            Cost = 125;
            EffectBuff = 0;
            MultiplierEffect = 0.25f;
            Type = (int)SkillEnums.Type.PostUpdateEquips;
        }

        public override string GetStatText()
        {
            //Formula is equals to + 4 lifeRegen = 2 L/s
            return $"{PreText} {EffectBuff * 0.5f:n1} L/s";
        }
    }
}
