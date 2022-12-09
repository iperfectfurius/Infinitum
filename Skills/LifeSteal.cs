namespace Infinitum.Skills
{
    internal class LifeSteal : Skill
    {
        private bool activate;//temp
        private float stackedLifeSteal = 0;
        private float difficulty = 0;
        public LifeSteal(int level) : base(level)
        {
        }

        public override void ApplyStatToPlayer(dynamic obj)
        {          
            if(Level==0) return;
            if(difficulty == 0)
            {
                switch (Main.GameMode)
                {
                    case 0:
                        difficulty = 0.5f;
                        break;
                    case 1:
                        difficulty = 0.75f;
                        break;
                    case 2:
                        difficulty = 1f;
                        break;
                    default:
                        difficulty = 0.5f;
                        break;
                }
            }
            int damageAfterArmor = (int)(obj.damage - (obj.defense * difficulty));
            
            GetLifeSteal(damageAfterArmor);
        }
        public override void OnInitialize()
        {
            Name = "LifeSteal";
            DisplayName = "Life Steal";
            StatName = "statLife";
            Cost = 125;
            MultiplierCost = 0.1f;
            EffectBuff = 0;
            MultiplierEffect = 0.00025f;
            Type = SkillEnums.Type.ModifyHitNPC;
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
