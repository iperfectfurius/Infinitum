global using Terraria.ModLoader;
global using Terraria;
global using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

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
        private int baseCost;
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
        public bool AutomaticMode { get => automaticMode; set => automaticMode = value; }
        public int BaseCost { get => baseCost; set => baseCost = value; }
        public int TotalSpend { get => baseCost * Level + ((int)(multiplierCost * baseCost) * level);}

        public Skill(int level)
        {
            this.maxLevel = 9999;
            this.level = level;
            this.multiplierCost = 0.05f;
            OnInitialize();
            baseCost = cost;

            Recalculate();
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
            return succes;
        }
        public virtual void Recalculate()
        {
            calculateBuff();
            CalcCost();
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
                CalcCost();
                return true;
            }
            return false;
        }
        public virtual bool LevelDown(ref int Levels)
        {
            if (level > 0)
            {
                level--;
                CalcCost();
                Levels += cost;             
                effectBuff -= multiplierEffect;
                return true;
            }
            return false;
        }
        public virtual bool LevelUpAll(ref int Levels)
        {
            bool canLevelUp = Levels > cost;

            while (LevelUp(ref Levels));

            return canLevelUp;
        }
        public virtual string GetStatText()
        {
            return $"{preText} {(effectBuff * 100):n2}%";
        }
        public virtual void CalcCost()
        {
            cost = (int)(baseCost + (baseCost * (multiplierCost)) * level);
        }
        public virtual void ApplyStatToPlayer() { return; }
        public virtual void ApplyStatToPlayer(int arg) { return; }

        public virtual void ApplyStatToPlayer(out bool arg) { arg = false; }

        public static bool AutoLevelUpSkills(Skill[] skills, ref int levels)
        {
            //this need to wokr in recursive
            int minimumLevel = int.MaxValue;
            int skillId = skills.Length + 1;

            for (int i = 0; i < skills.Length; i++)
            {
                if (!skills[i].automaticMode) continue;
                if (skills[i].level < minimumLevel)
                {
                    minimumLevel = skills[i].level;
                    skillId = i;
                }
            }

            if (skillId > skills.Length) return false;

            //while here?
            if (skills[skillId].ApplyStat((int)SkillEnums.Actions.LevelUp, ref levels))
            {
                player.GetModPlayer<Character_Data>().showDamageText(0, $"{skills[skillId].displayName} {skills[skillId].GetStatText()}", Color.Purple, 120, true, false);

                return true;
            }
            return false;

        }
    }
}
