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
        public enum BossesIds : ushort
        {
            KingSlime = 50,
            EyeOfCthulhu = 4,
            EaterOfWorlds = 33,
            BrainOfCthulhu = 266,
            QueenBee = 222,
            Skeletron = 35,
            Deerclops = 668,
            WallOfFlesh = 113,
            QueenSlime = 657,
            Retinazer = 125,
            Spazmatism = 126,
            DestroyerHead = 134,
            DestroyerBody = 135,
            DestroyerTail = 136,
            SkeletronPrime = 127,
            Plantera = 262,
            Golem = 245,
            DukeFishrom = 370,
            EmpressOfLight = 636,
            MoonLord = 396
        }
        private string name;
        private int id;
        private bool defeated;
        private double totalXP;
        private uint timesDefeated;
        private ulong totalTimeBattle;
        private BossType type;
        public static readonly ushort[] ValidBossesIds = {50,4,33,266,222,35,668,113};

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
