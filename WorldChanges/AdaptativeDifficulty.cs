using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Infinitum.WorldChanges
{
    public enum Difficulties : ushort
    {
        Normal,
        Hard,
        T1,
        T2,
        T3,
        T4,
        T5,
        Disabled
    }
    enum EscalationOrder : ushort
    {
        HP,
        Speed,
        Defense,
        Damage
    }
    internal class AdaptativeDifficulty
    {
        private float hp;
        private float speed;
        private float defense;
        private float damage;
        private Difficulties difficulty;
        private List<Boss> bosses;
        private float[,] Escalation = new float[Enum.GetNames(typeof(Difficulties)).Length, Enum.GetNames(typeof(EscalationOrder)).Length];

        public float Hp { get => hp; set => hp = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Defense { get => defense; set => defense = value; }
        public float Damage { get => damage; set => damage = value; }
        public Difficulties Difficulty { get => difficulty; set => difficulty = value; }
        internal List<Boss> Bosses { get => bosses; set => bosses = value; }

        public AdaptativeDifficulty(TagCompound data)
        {
            LoadEscalations();
            SetDifficultyOnLoad(data);
        }
        public AdaptativeDifficulty(Difficulties difficulty)
        {
            LoadEscalations();
            ChangeDifficulty(difficulty);
        }
        private void SetDefaults()
        {
            ChangeDifficulty(Difficulties.Normal);
        }

        private void LoadEscalations()
        {
            Escalation[(int)Difficulties.Normal, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.Normal, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.Normal, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.Normal, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.T1, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.T1, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.T1, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.T1, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.T2, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.T2, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.T2, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.T2, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.T3, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.T3, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.T3, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.T3, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.T4, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.T4, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.T4, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.T4, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.T5, (int)EscalationOrder.HP] = 0.0139f;
            Escalation[(int)Difficulties.T5, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.T5, (int)EscalationOrder.Defense] = 0.0056f;
            Escalation[(int)Difficulties.T5, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.Disabled, (int)EscalationOrder.HP] = 0f;
            Escalation[(int)Difficulties.Disabled, (int)EscalationOrder.Speed] = 0;
            Escalation[(int)Difficulties.Disabled, (int)EscalationOrder.Defense] = 0;
            Escalation[(int)Difficulties.Disabled, (int)EscalationOrder.Damage] = 0f;
        }
        private void SetDifficultyOnLoad(TagCompound data)
        {
            try
            {

            }
            catch
            {
                SetDefaults();
                
            }
        }
        public void ChangeDifficulty(Difficulties difficulty)
        {
            switch (difficulty)
            {
                case Difficulties.Normal:
                    ChangeMonsterStats(0.25f, 0, 0.10f, 0.10f);
                    break;
                case Difficulties.Hard:
                    break;
                case Difficulties.T1:
                    break;
                case Difficulties.T2:
                    break;
                case Difficulties.T3:
                    break;
                case Difficulties.T4:
                    break;
                case Difficulties.T5:
                    break;
                case Difficulties.Disabled:
                    ChangeMonsterStats(1, 1, 1, 1);
                    break;
                default:
                    break;

            }

            Difficulty = difficulty;
        }
        private void ChangeMonsterStats(float hp = 1f, float speed = 1f, float defense = 1f, float damage = 1f)
        {
            Hp = hp;
            Speed = speed;
            Defense = defense;
            Damage = damage;
        }

        public bool CheckBossPlaythrough(string name)
        {
            return true;
        }
    }
}
