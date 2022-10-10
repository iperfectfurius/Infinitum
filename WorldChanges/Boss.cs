using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.WorldChanges
{
    public class Boss
    {
        public enum BossType : ushort
        {
            PreHardMode,
            HardMode,
            PostPlantera,
            PostGolem
        }

        private string name;
        private int id;
        private bool defeated;
        private double totalXP;
        private uint timesDefeated;
        private ulong totalTimeBattle;
        private BossType type;

        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        public bool Defeated { get => defeated; set => defeated = value; }
        public double TotalXP { get => totalXP; set => totalXP = value; }
        public uint TimesDefeated { get => timesDefeated; set => timesDefeated = value; }
        public ulong TotalTimeBattle { get => totalTimeBattle; set => totalTimeBattle = value; }
        public BossType Type { get => type; set => type = value; }
        public Boss(int id, bool defeated = true)
        {
            Id = id;
            Defeated = defeated;
        }
        public Boss(int id, BossType type, bool defeated = true)
        {
            Id = id;
            Defeated = defeated;
            Type = type;
        }
    }


}
