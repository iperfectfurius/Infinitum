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
    internal class AdaptativeDifficulty
    {
        enum EscalationOrder : ushort
        {
            HP,
            Speed,
            Defense,
            Damage
        }     
        private float hp;
        private float speed;
        private float defense;
        private float damage;
        private Difficulties difficulty;
        private Boss.BossType currentDifficulty = Boss.BossType.PreHardMode;
        private List<Boss> bosses = new();
        private float[,] Escalation = new float[Enum.GetNames(typeof(Difficulties)).Length, Enum.GetNames(typeof(EscalationOrder)).Length];
        private readonly float[] DifficultyXP = { 1.0f, 1.25f, 1.40f, 1.3f, 1.4f, 1.5f, 1.6f, 1.0f };
        public float Hp { get => hp; set => hp = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Defense { get => defense; set => defense = value; }
        public float Damage { get => damage; set => damage = value; }
        public Difficulties Difficulty { get => difficulty; set => difficulty = value; }
        internal List<Boss> Bosses { get => bosses; set => bosses = value; }
        public float GetXPFromDifficulty => DifficultyXP[(int)difficulty];

        public bool IsBossDefeated(int id) => bosses.Exists(e => e.Id == id && e.Defeated);

        public AdaptativeDifficulty(TagCompound data)
        {
            LoadEscalations();
            //LoadBreakPointsBosses();
            SetDifficultyOnLoad(data);
        }
        public AdaptativeDifficulty(Difficulties difficulty)
        {
            LoadEscalations();
            //LoadBreakPointsBosses();
            ChangeDifficulty(difficulty);
        }
        private void SetDefaults()
        {
            ChangeDifficulty(Difficulties.Normal);
        }

        private void LoadBreakPointsBosses()
        {
            Boss WallOfFlesh = new(113,false);
            Boss Plantera = new(262, false);

            WallOfFlesh.Type = Boss.BossType.HardMode;
            Plantera.Type = Boss.BossType.PostPlantera;

            bosses.Add(WallOfFlesh);
            bosses.Add(Plantera);
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
                    ChangeMonsterStats(0.05f, 0, 0.0f, 0.02f);
                    break;
                case Difficulties.Hard:
                    ChangeMonsterStats(0.50f, 0, 0.15f, 0.15f);
                    break;
                case Difficulties.T1:
                    ChangeMonsterStats(0.75f, 0, 0.20f, 0.20f);
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
                    ChangeMonsterStats();
                    break;
                default:
                    break;

            }

            Difficulty = difficulty;
        }
        private void ChangeMonsterStats(float hp = 0f, float speed = 0f, float defense = 0f, float damage = 0f)
        {
            Hp = hp;
            Speed = speed;
            Defense = defense;
            Damage = damage;
        }
        public bool CheckBossPlaythrough(NPC npc)
        {
            if (!npc.boss || IsBossDefeated(npc.type)) return false;

            switch (npc.type)
            {
                case 113:
                    AddNewBossDefeated(npc, Boss.BossType.PreHardMode);
                    ChangeMonsterStats(0.125f,0,0.035f,0.035f);
                    //AdjustDifficulty();
                    break;
                    
                default:
                    AddNewBossDefeated(npc);
                    break;
            }
            //AddNewBossDefeated(npc);
            return true;
        }
        
        private void AddNewBossDefeated(NPC boss,Boss.BossType type = Boss.BossType.PreHardMode)
        {
            Boss newBoss = new(boss.type,type);
            bosses.Add(newBoss);
            Main.NewText("Added a new Boss");
        }
        private void AdjustDifficulty()
        {
            
        }
        public string GetCurrentStatsFromMonsters()
        {
            return $"HP:{hp:n1}%, Damage:{damage:n1}%, Defense:{defense:n1}%";
        }
    }
}
