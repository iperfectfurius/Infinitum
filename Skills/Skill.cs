﻿global using Terraria.ModLoader;
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
        private char postText = '%';
        private int level;
        private dynamic effectBuff;
        private float multiplierEffect;
        private int baseCost;
        private int cost;
        private float multiplierCost;
        private int maxLevel;
        private bool automaticMode = false;
        private SkillEnums.Type type;
        private int totalSpend;

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
        public bool AutomaticMode { get => automaticMode; set => automaticMode = value; }
        public int BaseCost { get => baseCost; set => baseCost = value; }
        public int TotalSpend { get => totalSpend; set => totalSpend = value; }
        public SkillEnums.Type Type { get => type; set => type = value; }
        public char PostText { get => postText; set => postText = value; }

        public Skill(int level)
        {
            this.maxLevel = 9999;
            this.level = level;
            this.multiplierCost = 0.05f;
            OnInitialize();
            baseCost = cost;

            Recalculate();
        }
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
            if (skills[skillId].ApplyStat(SkillEnums.Actions.LevelUp, ref levels))
            {
                player.GetModPlayer<Character_Data>().showDamageText(0, $"{skills[skillId].displayName} {skills[skillId].GetStatText()}", Color.Purple, 120, true, false);

                return true;
            }
            return false;

        }
        public static string GetBuffs(Skill[] skills)
        {
            string setBuffs = "";

            foreach (Skill skill in skills)
            {
                if (skill.Level == 0) continue;

                setBuffs += $"{skill.displayName} {skill.GetStatText()}, ";
            }

            return setBuffs.Length > 0 ? setBuffs.Remove(setBuffs.Length - 2) : "";
        }

        public abstract void OnInitialize();
        public bool ApplyStat(SkillEnums.Actions action, ref int Levels)
        {
            bool succes = false;

            switch (action)
            {
                case SkillEnums.Actions.LevelUp:
                    succes = LevelUp(ref Levels);
                    break;
                case SkillEnums.Actions.LevelDown:
                    succes = LevelDown(ref Levels);
                    break;
                case SkillEnums.Actions.LevelUpAll:
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
            TotalPointsSpend();
        }
        public virtual void calculateBuff()
        {
            effectBuff = level * multiplierEffect;
        }
        public virtual bool LevelUp(ref int Levels)
        {
            if (Levels >= cost)
            {
                Levels -= cost;
                level++;
                effectBuff += multiplierEffect;
                totalSpend += cost;
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
                totalSpend -= cost;
                return true;
            }
            return false;
        }
        public virtual bool LevelUpAll(ref int Levels)
        {
            bool canLevelUp = Levels > cost;

            while (LevelUp(ref Levels)) ;

            return canLevelUp;
        }
        public virtual string GetStatText()
        {
            return $"{preText} {(effectBuff * 100):n2}{postText}";
        }
        public virtual void CalcCost()
        {
            cost = (int)(baseCost + (baseCost * (multiplierCost)) * level);
        }
        public virtual void ApplyStatToPlayer() { return; }
        public virtual void ApplyStatToPlayer(dynamic obj) { return; }

        public virtual void ApplyStatToPlayer(out bool arg) { arg = false; }

        private void TotalPointsSpend()
        {
            int cost = level * baseCost;
            float costPerLevel = baseCost * multiplierCost;

            for (int i = 1; i < level; i++) cost += (int)(i * costPerLevel);

            totalSpend = cost;
        }
    }
}
