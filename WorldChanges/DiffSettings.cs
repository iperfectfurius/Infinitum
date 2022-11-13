using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.WorldChanges
{
    enum EscalationStep : ushort
    {
        PreHardMode = 4,
        HardMode = 9,
        PostPlantera = 15,
        PostGolem = 22
    }
    internal class DiffSettings
    {
        //TODO: Use diffSettings
        private float hp;
        private float speed;
        private float defense;
        private float damage;
        private EscalationStep step;
        private Boss.BossType bestBossTypeBeated;

        public float Hp { get => hp * (int)step; set => hp = value; }
        public float Speed { get => speed * (int)step; set => speed = value; }
        public float Defense { get => defense * (int)step; set => defense = value; }
        public float Damage { get => damage * (int)step; set => damage = value; }
        public EscalationStep Step { get => step; set => step = value; }
        public Boss.BossType BestBossTypeBeated { get => bestBossTypeBeated; set => bestBossTypeBeated = value; }

        public readonly float GetMultiplierXP;
        public DiffSettings(float hp, float speed, float defense, float damage, float multiplierXP, EscalationStep step = EscalationStep.PreHardMode)
        {
            this.hp = hp;
            this.speed = speed;
            this.defense = defense;
            this.damage = damage;
            this.step = step;
            GetMultiplierXP = multiplierXP;
        }

    }
}
