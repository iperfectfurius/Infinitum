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
        // TODO: LifeSteal based on damage done not raw.
        public override void ApplyStatToPlayer(NPC target,int damage)
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
            int finalDamage = (int)(damage - (target.defense * difficulty));
            
            GetLifeSteal(finalDamage);
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
            Type = (int)SkillEnums.Type.ModifyHitNPC;
        }
        private void GetLifeSteal(int damage)
        {
            int toHeal = (int)(damage * (float)EffectBuff);
            stackedLifeSteal += (damage * (float)EffectBuff) - (float)Math.Truncate(damage * (float)EffectBuff);
            float temp = (damage * (float)EffectBuff);
            //Infinitum.instance.GameMessage(temp.ToString());
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
