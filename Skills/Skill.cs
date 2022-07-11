using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal abstract class Skill
    {
        private string name;
        private string statName;
        private string displayName;
        private int level;
        private float effectBuff;
        private int cost;
        private float multiplierCost;
        private int maxLevel;

        public string Name { get => name; set => name = value; }
        public string StatName { get => statName; set => statName = value; }
        public int Level { get => level; set => level = value; }
        public float EffectBuff { get => effectBuff; set => effectBuff = value; }
        public int Cost { get => cost; set => cost = value; }
        public float MultiplierCost { get => multiplierCost; set => multiplierCost = value; }
        public int MaxLevel { get => maxLevel; set => maxLevel = value; }
        public string DisplayName { get => displayName; set => displayName = value; }

        public Skill(string name,string statName)
        {
            this.name = name;
            this.statName = statName;
            OnInitialize();
            
        }
        public abstract void OnInitialize();

        
    }
}
