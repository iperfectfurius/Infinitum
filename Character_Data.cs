using Infinitum.Skills;
using Infinitum.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Infinitum
{
    public class Character_Data : ModPlayer
    {
        private Player player = Main.CurrentPlayer;
        private static Mod myMod = ModLoader.GetMod("Infinitum");
        private bool recentChanged = false;
        private string lastHeldItem;
        private List<float> avgXP = new List<float>() { 0 };
        public float getAvgXP() => (float)Queryable.Average(avgXP.AsQueryable());
        public enum CombatTextPos : int
        {
            Xp = 155,
            AddedLevels = 190,
            CurrentLevels = 50
        };
        private string version = "0.75";//Only used in case need for all players in next update.
        private bool messageReset = false;
        private float exp = 0.0f;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
        private float moreExpMultiplier = 1.0f;
        private const int EXPTOLEVEL = 60000;
        private long totalNpcsKilled = 0;
        private bool activate = true;
        private bool displayNumbers = true;
        //to do
        private Skill[] skills = new Skill[0];

        public float Exp { get => exp; }
        public int Level { get => level; }
        public int TotalLevel { get => totalLevel; }
        public float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }
        public int _EXPTOLEVEL => EXPTOLEVEL;
        public bool RecentChanged { get => recentChanged; set => recentChanged = value; }
        //dont need?
        public long TotalNpcsKilled { get => totalNpcsKilled; set => totalNpcsKilled = value; }
        public bool Activate { get => activate; set => activate = value; }
        public bool DisplayNumbers { get => displayNumbers; set => displayNumbers = value; }
        public float MoreExpMultiplier { get => moreExpMultiplier; set => moreExpMultiplier = value; }
        internal Skill[] Skills { get => skills; set => skills = value; }

        public override void Initialize()
        {
            base.Initialize();

        }
        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;

            showDamageText((int)CombatTextPos.CurrentLevels, $"Level {totalLevel}", CombatText.DamagedFriendlyCrit, 120, true);

            InfinitumUI.Instance.stats = this;
            ExpBarUI.Instance.stats = this;
            Skill.player = currentPLayer;

            if (messageReset)
                showDamageText((int)CombatTextPos.CurrentLevels + 50, "Skills Reset!", Color.Red, 180, true);

        }
        public void showDamageText(int yPos, string text, Color c, int duration = 60, bool dramatic = false, bool dot = false)
        {
            if (Main.netMode == NetmodeID.Server || !displayNumbers) return;

            int i = CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + yPos), 25, 25), c, text, dramatic, dot);
            if (i < 100)//haha meme (out of index???)
                Main.combatText[i].lifeTime = duration;

        }
        public override void Load()
        {
            base.Load();

        }
        public void AddXp(float xp)
        {
            if (Main.gameMenu) return;//This can be triggered by calamity first time in the world?
            try
            {
                float experienceObtained = xp * (expMultiplier * moreExpMultiplier);
                exp += experienceObtained;
                UpdateLevel();
                showDamageText((int)CombatTextPos.Xp, $"+ {experienceObtained:n1} XP", CombatText.HealMana);
                totalNpcsKilled++;

                if (avgXP.Count > 100)
                    avgXP.RemoveRange(0, 50);
                avgXP.Add(experienceObtained);
                recentChanged = true;
            }
            catch (IndexOutOfRangeException)
            {
                Infinitum.instance.ChatMessage("Error addXp");

            }

        }
        private void UpdateLevel()
        {
            if (exp < EXPTOLEVEL) return;

            int LevelsUp = (int)exp / EXPTOLEVEL;
            exp -= EXPTOLEVEL * LevelsUp;
            level += LevelsUp;
            totalLevel += LevelsUp;

            showDamageText((int)CombatTextPos.AddedLevels, $"+ {LevelsUp} Levels!", CombatText.DamagedFriendlyCrit);
            showDamageText((int)CombatTextPos.CurrentLevels, $"Level {level}", CombatText.DamagedFriendlyCrit, 120, true);

            Skill.AutoLevelUpSkills(ref skills, ref level);


            SoundEngine.PlaySound(SoundID.Chat);

        }

        public void AddXpMultiplier(float multiplier)
        {
            expMultiplier += multiplier;
            showDamageText((int)CombatTextPos.Xp, $"{(expMultiplier * 100f):n2}% Multiplier!", CombatText.DamagedFriendlyCrit);
            recentChanged = true;
        }

        public override void LoadData(TagCompound tag)
        {

            try
            {
                //probably save all character_data is more efficient?
                string tempVer;
                tag.TryGet("Version", out tempVer);
                tag.TryGet("DisplayNumbers", out displayNumbers);
                tag.TryGet("Level", out level);
                tag.TryGet("ExpMultiplier", out expMultiplier);
                tag.TryGet("Exp", out exp);
                tag.TryGet("TotalLevel", out totalLevel);
                tag.TryGet("TotalNpcsKilled", out totalNpcsKilled);
                tag.TryGet("Activate", out activate);

                if (tempVer != version)
                {
                    messageReset = true;
                    resetCurrentSkills();
                    return;
                }
                loadSkills(tag);
                //save in dictionaries for future Sets            
                recentChanged = true;

            }
            catch
            {
                resetCurrentSkills();
                recentChanged = true;
            }

        }

        private void loadSkills(TagCompound tag)
        {

            TagCompound savedSkills = tag.GetCompound("Skills");
            TagCompound skill;
            skills = new Skill[SkillEnums.GetNumberOfSkills];

            skill = savedSkills.GetCompound(typeof(Defense).ToString());
            Skills[(int)SkillEnums.SkillOrder.Defense] = new Defense(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.Defense].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(LifeRegen).ToString());
            Skills[(int)SkillEnums.SkillOrder.LifeRegen] = new LifeRegen(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.LifeRegen].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(MeleeDamage).ToString());
            Skills[(int)SkillEnums.SkillOrder.MeleeDamage] = new MeleeDamage(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.MeleeDamage].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(MeleeAttackSpeed).ToString());
            Skills[(int)SkillEnums.SkillOrder.MeleeAttackSpeed] = new MeleeAttackSpeed(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.MeleeAttackSpeed].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(MagicDamage).ToString());
            Skills[(int)SkillEnums.SkillOrder.MagicDamage] = new MagicDamage(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.MagicDamage].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(ReducedManaConsumption).ToString());
            Skills[(int)SkillEnums.SkillOrder.ManaConsumption] = new ReducedManaConsumption(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.ManaConsumption].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(RangedDamage).ToString());
            Skills[(int)SkillEnums.SkillOrder.RangedDamage] = new RangedDamage(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.RangedDamage].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(SummonDamage).ToString());
            Skills[(int)SkillEnums.SkillOrder.SummonDamage] = new SummonDamage(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.SummonDamage].AutomaticMode = skill.GetBool("automaticMode");


            skill = savedSkills.GetCompound(typeof(SummonCapacity).ToString());
            Skills[(int)SkillEnums.SkillOrder.MinionCapacity] = new SummonCapacity(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.MinionCapacity].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(PickaxeSpeed).ToString());
            Skills[(int)SkillEnums.SkillOrder.PickaxeSpeed] = new PickaxeSpeed(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.PickaxeSpeed].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(MovementSpeed).ToString());
            Skills[(int)SkillEnums.SkillOrder.MovementSpeed] = new MovementSpeed(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.MovementSpeed].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(GlobalCriticalChance).ToString());
            Skills[(int)SkillEnums.SkillOrder.GlobalCriticalChance] = new GlobalCriticalChance(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.GlobalCriticalChance].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(LifeSteal).ToString());
            Skills[(int)SkillEnums.SkillOrder.LifeSteal] = new LifeSteal(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.LifeSteal].AutomaticMode = skill.GetBool("automaticMode");

            skill = savedSkills.GetCompound(typeof(AmmoConsumption).ToString());
            Skills[(int)SkillEnums.SkillOrder.AmmoConsumption] = new AmmoConsumption(skill.GetInt("level"));
            Skills[(int)SkillEnums.SkillOrder.AmmoConsumption].AutomaticMode = skill.GetBool("automaticMode");
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("Level", level);
            tag.Add("ExpMultiplier", expMultiplier);
            tag.Add("Exp", exp);
            tag.Add("TotalLevel", totalLevel);
            tag.Add("TotalNpcsKilled", totalNpcsKilled);
            tag.Add("Activate", activate);
            tag.Add("DisplayNumbers", displayNumbers);
            tag.Add("Version", version);

            TagCompound dataSkill = new();

            foreach (Skill s in Skills)
            {
                TagCompound skill = new TagCompound();
                skill.Add("level", s.Level);
                skill.Add("automaticMode", s.AutomaticMode);
                dataSkill.Add(s.GetType().ToString(), skill);
            }

            tag.Add("Skills",dataSkill);

        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (InfinitumModSystem.UIKey.JustPressed)
            {
                recentChanged = true;
                InfinitumUI.Instance.Visible = !InfinitumUI.Instance.Visible;
            }
            else if (InfinitumModSystem.NumbersDisplay.JustPressed)
            {
                displayNumbers = !displayNumbers;
                CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 50), 25, 25), Color.Red, displayNumbers ? "Numbers Activated!" : "Numbers Disabled!", true, false);
            }

            base.ProcessTriggers(triggersSet);

        }
        public override void Unload()
        {
            base.Unload();
        }
        public bool ApplyStats(int stat, int apply)
        {
            //implement skill class     
            if (skills[stat].ApplyStat(apply, ref level))
            {
                recentChanged = true;
                return true;
            }
            return false;
        }
        public override void PostUpdateEquips()
        {
            if (player.HeldItem.Name != lastHeldItem)
            {
                lastHeldItem = player.HeldItem.Name;
                recentChanged = true;
            }
            //consistency...
            if (!activate)
            {
                base.PostUpdateEquips();
                return;
            }

            foreach (Skill s in skills)
            {
                if (s.Type == (int)SkillEnums.Type.PostUpdateEquips)
                    s.ApplyStatToPlayer();
            }

            getAdditionalsExp();
            base.PostUpdateEquips();

        }

        private void getAdditionalsExp()
        {

            ModPrefix prefix = PrefixLoader.GetPrefix(player.HeldItem.prefix);
            if (prefix != null)
                switch (prefix.Name)
                {
                    case "UnrealPlus":
                    case "MythicalPlus":
                    case "LegendaryPlus":
                        moreExpMultiplier += .25f;
                        break;
                    default:
                        break;
                }
        }
        public override void PreUpdate()
        {
            MoreExpMultiplier = 1f;
            base.PreUpdate();
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            int damage2 = damage;
            if (activate && target.netID != 488)
                skills[(int)SkillEnums.SkillOrder.LifeSteal].ApplyStatToPlayer(damage2);

            base.ModifyHitNPC(item, target, ref damage, ref knockback, ref crit);

        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            int damage2 = damage;
            if (activate && target.netID != 488)
                skills[(int)SkillEnums.SkillOrder.LifeSteal].ApplyStatToPlayer(damage2);
            base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);

        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {

            skills[(int)SkillEnums.SkillOrder.AmmoConsumption].ApplyStatToPlayer(out bool canConsumeAmmo);

            return canConsumeAmmo;

        }

        public static void ChatMessage(string text = "")
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text + " Desde Server"), Color.Red);
            }
            else
            {
                Main.NewText(text + " Desde single");
            }
        }

        public void resetCurrentSkills()
        {

            level = totalLevel;

            skills = new Skill[SkillEnums.GetNumberOfSkills];

            Skills[(int)SkillEnums.SkillOrder.Defense] = new Defense(0);
            Skills[(int)SkillEnums.SkillOrder.LifeRegen] = new LifeRegen(0);
            Skills[(int)SkillEnums.SkillOrder.MeleeDamage] = new MeleeDamage(0);
            Skills[(int)SkillEnums.SkillOrder.MeleeAttackSpeed] = new MeleeAttackSpeed(0);
            Skills[(int)SkillEnums.SkillOrder.MagicDamage] = new MagicDamage(0);
            Skills[(int)SkillEnums.SkillOrder.ManaConsumption] = new ReducedManaConsumption(0);
            Skills[(int)SkillEnums.SkillOrder.RangedDamage] = new RangedDamage(0);
            Skills[(int)SkillEnums.SkillOrder.SummonDamage] = new SummonDamage(0);
            Skills[(int)SkillEnums.SkillOrder.MinionCapacity] = new SummonCapacity(0);
            Skills[(int)SkillEnums.SkillOrder.PickaxeSpeed] = new PickaxeSpeed(0);
            Skills[(int)SkillEnums.SkillOrder.MovementSpeed] = new MovementSpeed(0);
            Skills[(int)SkillEnums.SkillOrder.GlobalCriticalChance] = new GlobalCriticalChance(0);


            Skills[(int)SkillEnums.SkillOrder.LifeSteal] = new LifeSteal(0);
            Skills[(int)SkillEnums.SkillOrder.AmmoConsumption] = new AmmoConsumption(0);

            recentChanged = true;

        }
        public override void ModifyCaughtFish(Item fish)
        {
            float xp = (((fish.rare * 5) + 1) * 3.5f + (fish.value / 500)) * fish.stack;
            
            if (Main.netMode == NetmodeID.SinglePlayer)
                AddXp(xp);
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //mirar
                Task.Run(() =>
                {
                    ModPacket myPacket = Infinitum.instance.GetPacket();
                    myPacket.Write(xp);
                    myPacket.Send();
                });
            }


            base.ModifyCaughtFish(fish);
        }
    }

}