global using Terraria.ModLoader;
global using Terraria;
using System;
using System.Collections.Generic;
using System.Text;


namespace Infinitum.Skills
{
    internal abstract class Skill
    {

        public static Player player;
        private string name;
        private string statName;
        private string displayName;
        private char preText = '+';
        private int level;
        private dynamic effectBuff;
        private float multiplierEffect;
        private int cost;
        private float multiplierCost;
        private int maxLevel;
        private bool automaticMode = false;
        private int type;

        public string Name { get => name; set => name = value; }
        public string StatName { get => statName; set => statName = value; }
        public int Level { get => level; set => level = value; }
        public dynamic EffectBuff { get => effectBuff; set => effectBuff = value; }
        public int Cost { get => cost; set => cost = value; }
        public float MultiplierCost { get => multiplierCost; set => multiplierCost = value; }
        public int MaxLevel { get => maxLevel; set => maxLevel = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public float MultiplierEffect { get => multiplierEffect; set => multiplierEffect = value; }
        public char PreText { get => preText; set => preText = value; }
        public int Type { get => type; set => type = value; }

        public Skill(int level)
        {
            this.maxLevel = 9999;
            this.level = level;
            OnInitialize();
            calculateBuff();
            //calcCost();

        }
        public abstract void OnInitialize();
        public bool ApplyStat(int action, ref int Levels)
        {
            bool succes = false;

            switch (action)
            {
                case (int)SkillEnums.Actions.LevelUp:
                    succes = LevelUp(ref Levels);
                    break;
                case (int)SkillEnums.Actions.LevelDown:
                    succes = LevelDown(ref Levels);
                    break;
                case (int)SkillEnums.Actions.LevelUpAll:
                    succes = LevelUpAll(ref Levels);
                    break;
                default:
                    break;
            }
            //CalcCost();
            return succes;
        }
        public virtual void calculateBuff()
        {
            effectBuff = level * multiplierEffect;
        }
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
                int maxLevels = Levels / cost;
                Levels -= cost * maxLevels;
                level += maxLevels;
                effectBuff += maxLevels * multiplierEffect;
                return true;
            }
            return false;
        }
        public virtual string GetStatText()
        {
            return $"{preText} {(effectBuff * 100):n2}%";
        }
        private void CalcCost()
        {
            cost += (int)(cost * multiplierCost);
        }
        public virtual void ApplyStatToPlayer() { return; }
        public virtual void ApplyStatToPlayer(int arg) { return; }

        public virtual void ApplyStatToPlayer(out bool arg) { arg = false; }

    }
}
