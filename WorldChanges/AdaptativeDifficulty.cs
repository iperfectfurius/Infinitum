using IL.Terraria.DataStructures;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Security.Permissions;
using Terraria.ID;
using Terraria.ModLoader;
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
        private Difficulties difficultySetted;
        private List<Boss> bosses = new();
        public Boss.BossType BestBossTypeBeated = Boss.BossType.PreHardMode;
        private float[,] Escalation = new float[Enum.GetNames(typeof(Difficulties)).Length, Enum.GetNames(typeof(EscalationOrder)).Length];
        private readonly float[] DifficultyXP = { 1.0f, 1.05f, 1.10f, 1.15f, 0f, 0f, 0f, 0.85f };
        private DiffSettings[] diffsSettings = new DiffSettings[Enum.GetNames(typeof(Difficulties)).Length];
        public const string version = "0.82.4";
        public float Hp { get => hp; set => hp = value; }
        public float Speed { get => speed; set => speed = value; }
        public float Defense { get => defense; set => defense = value; }
        public float Damage { get => damage; set => damage = value; }
        public Difficulties DifficultySetted { get => difficultySetted; set => difficultySetted = value; }
        private DiffSettings GetDiffSettings { get => diffsSettings[(int)difficultySetted]; }
        public EscalationStep GetStep { get => GetDiffSettings.Step; }
        internal List<Boss> Bosses { get => bosses; set => bosses = value; }
        public float GetXPFromDifficulty => DifficultyXP[(int)difficultySetted];


        public bool IsBossDefeated(int id) => bosses.Exists(e => e.Id == id && e.Defeated);

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
        public void SetDefaults()
        {
            BestBossTypeBeated = Boss.BossType.PreHardMode;
            ChangeDifficulty(Difficulties.Normal);
            bosses = new List<Boss>();
        }

        [Obsolete]
        private void LoadBreakPointsBosses()
        {
            Boss WallOfFlesh = new(113, false);
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
            Escalation[(int)Difficulties.Normal, (int)EscalationOrder.Defense] = 0.0055f;
            Escalation[(int)Difficulties.Normal, (int)EscalationOrder.Damage] = 0.0056f;

            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.HP] = 0.0231f;
            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.Defense] = 0.008f;
            Escalation[(int)Difficulties.Hard, (int)EscalationOrder.Damage] = 0.0069f;

            Escalation[(int)Difficulties.T1, (int)EscalationOrder.HP] = 0.0355f;
            Escalation[(int)Difficulties.T1, (int)EscalationOrder.Speed] = 0.0f;
            Escalation[(int)Difficulties.T1, (int)EscalationOrder.Defense] = 0.0114f;
            Escalation[(int)Difficulties.T1, (int)EscalationOrder.Damage] = 0.0085f;

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

            //TODO: Diffsettings part2

            diffsSettings[(int)Difficulties.Normal] = new DiffSettings(.0139f, 0f, .0055f, .0056f, 1.0f);
            diffsSettings[(int)Difficulties.Hard] = new DiffSettings(.0139f, 0f, .0055f, .0056f, 1.0f);
            diffsSettings[(int)Difficulties.T1] = new DiffSettings(.0139f, 0f, .0055f, .0056f, 1.0f);
            diffsSettings[(int)Difficulties.Disabled] = new DiffSettings(.0139f, 0f, .0055f, .0056f, 1.0f);
        }
        private void SetDifficultyOnLoad(TagCompound tag)
        {
            try
            {
                ChangeStepAndDifficulty((Difficulties)tag.GetByte("Difficulty"), (Boss.BossType)tag.GetByte("LastStepBossBeated"));
            }
            catch
            {
                SetDefaults();
            }
        }
        public void ChangeDifficulty(Difficulties difficulty)
        {
            //TODO: Get scalation step saved.
            DifficultySetted = difficulty;

            switch (BestBossTypeBeated)
            {
                case Boss.BossType.PreHardMode:
                    ChangeMonsterStats(EscalationStep.PreHardMode);
                    break;
                case Boss.BossType.HardMode:
                    ChangeMonsterStats(EscalationStep.HardMode);
                    break;
                case Boss.BossType.PostPlantera:
                    ChangeMonsterStats(EscalationStep.PostPlantera);
                    break;
                case Boss.BossType.PostGolem:
                    ChangeMonsterStats(EscalationStep.PostGolem);
                    break;
            }
            SendNewStatsToAllPlayers();
        }
        public void ChangeStepAndDifficulty(Difficulties difficulty, Boss.BossType progress)
        {
            BestBossTypeBeated = progress;
            ChangeDifficulty(difficulty);
        }
        private void ChangeMonsterStats(EscalationStep step)
        {
            //TODO diffsettings part 3
            //diffsSettings[(int)difficultySetted].Step = step;
            //hp = GetDiffSettings.Hp;
            //(hp,speed,defense,damage) = (GetDiffSettings.Hp,GetDiffSettings.Hp,GetDiffSettings.Hp,GetDiffSettings.Hp);
            hp = Escalation[(int)difficultySetted, (int)EscalationOrder.HP] * (int)step;
            speed = 0f;
            defense = Escalation[(int)difficultySetted, (int)EscalationOrder.Defense] * (int)step;
            damage = Escalation[(int)difficultySetted, (int)EscalationOrder.Damage] * (int)step;
            //GetDiffSettings.Step = step;
        }
        public bool CheckBossPlaythrough(NPC npc)
        {
            //TODO: Save stats from bosses.
            if (!npc.boss || IsBossDefeated(npc.type)) return false;


            switch (npc.type)
            {
                case (int)Boss.BossesIds.WallOfFlesh:
                    AddNewBossDefeated(npc, Boss.BossType.HardMode);

                    if (BestBossTypeBeated >= Boss.BossType.HardMode) break;

                    BestBossTypeBeated = Boss.BossType.HardMode;
                    ChangeMonsterStats(EscalationStep.HardMode);

                    SendNewStatsToAllPlayers();
                    break;
                case (int)Boss.BossesIds.Plantera:
                    AddNewBossDefeated(npc, Boss.BossType.PostPlantera);

                    if (BestBossTypeBeated >= Boss.BossType.PostPlantera) break;

                    BestBossTypeBeated = Boss.BossType.PostPlantera;
                    ChangeMonsterStats(EscalationStep.PostPlantera);

                    SendNewStatsToAllPlayers();
                    break;
                case (int)Boss.BossesIds.Golem:
                    AddNewBossDefeated(npc, Boss.BossType.PostGolem);

                    if (BestBossTypeBeated >= Boss.BossType.PostGolem) break;

                    BestBossTypeBeated = Boss.BossType.PostGolem;
                    ChangeMonsterStats(EscalationStep.PostGolem);

                    SendNewStatsToAllPlayers();
                    break;
                default:
                    AddNewBossDefeated(npc);
                    break;
            }
            //AddNewBossDefeated(npc);
            return true;
        }

        private void SendNewStatsToAllPlayers()
        {
            if (Main.netMode != NetmodeID.Server || Main.gameMenu) return;

            ModPacket myPacket = ModLoader.GetMod("Infinitum").GetPacket();

            myPacket.Write((byte)MessageType.UpdateStats);
            myPacket.Write((byte)DifficultySetted);
            myPacket.Write(Hp);
            myPacket.Write(Speed);
            myPacket.Write(Defense);
            myPacket.Write(Damage);
            myPacket.Send();
        }

        private void AddNewBossDefeated(NPC boss, Boss.BossType type = Boss.BossType.PreHardMode)
        {
            Boss newBoss = new(boss.type, type);
            bosses.Add(newBoss);
            Infinitum.instance.GameMessage("Added new Boss", Color.White);
        }
        private void AdjustDifficulty()
        {

        }
        public string GetCurrentStatsFromMonsters()
        {
            return $"HP:{hp:n1}%, Damage:{damage:n1}%, Defense:{defense:n1}%. {GetStep}";
        }
    }
}
