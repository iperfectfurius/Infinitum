using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Skills
{
    internal abstract class Skill
    {
        public enum action : ushort
        {
            LevelUp = 0,
            LevelDown = 1,
            LevelUpAll = 2
        }
        private string name;
        private string statName;
        private string displayName;
        private int level;
        private dynamic effectBuff;
        private float multiplierEffect;
        private int cost;
        private float multiplierCost;
        private int maxLevel;

        public string Name { get => name; set => name = value; }
        public string StatName { get => statName; set => statName = value; }
        public int Level { get => level; set => level = value; }
        public dynamic EffectBuff { get => effectBuff; set => effectBuff = value; }
        public int Cost { get => cost; set => cost = value; }
        public float MultiplierCost { get => multiplierCost; set => multiplierCost = value; }
        public int MaxLevel { get => maxLevel; set => maxLevel = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public float MultiplierEffect { get => multiplierEffect; set => multiplierEffect = value; }

        public Skill(int level)
        {
            this.maxLevel = 9999;
            this.level = level;           
            OnInitialize();
            calculateBuff();

        }
        public virtual void ApplyStat(int action,ref int Levels)
        {
            switch (action)
            {
                case 0:
                    LevelUp(ref Levels);
                    break;
                    case 1:
                    LevelDown(ref Levels);
                    break;
                case 2:
                    LevelUpAll(ref Levels);
                    break;
                default:
                    break;
            }
        }
        private void calculateBuff()
        {
            effectBuff =  level * multiplierEffect;
        }
        public abstract void OnInitialize();

        public virtual bool LevelUp(ref int Levels)
        {
            if (Levels > cost)
            {
                Levels -= cost;
                level++;
                effectBuff += multiplierEffect;
                return true;
            }
            return false;
        }
        public virtual bool LevelDown(ref int Levels)
        {
            if (level > 0)
            {
                Levels += cost;
                level--;
                effectBuff -= multiplierEffect;
                return true;
            }
            return false;
        }
        public virtual bool LevelUpAll(ref int Levels)
        {
            if (Levels > cost)
            {
                int maxLevels = level / cost;
                Levels -= cost * maxLevels;
                level+= maxLevels;
                effectBuff += maxLevels * multiplierEffect;
                return true;
            }
            return false;
        }

    }
}
