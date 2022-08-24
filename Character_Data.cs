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
using Terraria.GameContent.Creative;

using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Infinitum
{
    //TODO: Use name for sets not numbers
    //TODO: Report what set changed have spended
    //TODO: Check automatic skills when change set
    public class Character_Data : ModPlayer
    {
        private Player player = Main.CurrentPlayer;
        private static Mod myMod = ModLoader.GetMod("Infinitum");
        private UISettings playerSettings = new();
        private bool recentChanged = false;
        private string lastHeldItem;
        private List<float> avgXP = new List<float>() { 0 };// TODO: avg XP base on time
        public float getAvgXP() => (float)Queryable.Average(avgXP.AsQueryable());
        public enum CombatTextPos : int
        {
            Xp = 155,
            AddedLevels = 190,
            CurrentLevels = 50
        };
        private string version = "0.78";// TODO: search for assembly version?
        private bool messageReset = false;
        private float exp = 0.0f;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
        private float moreExpMultiplier = 1.0f;
        private const int BASE_EXP = 60000;
        private int expToLevel = BASE_EXP;
        private const float EXPPERLEVEL = 0.0001f;
        private long totalNpcsKilled = 0;
        private bool activate = true;
        private bool displayNumbers = true;

        private Dictionary<string, Skill[]> skillsSets = new Dictionary<string, Skill[]>(); 
        private string setSelected = "0";
        public float Exp { get => exp; }
        public int Level { get => level; }
        public int TotalLevel { get => totalLevel; }
        public float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }
        public int ExpToLevel => expToLevel;
        public bool RecentChanged { get => recentChanged; set => recentChanged = value; }
        //dont need?
        public long TotalNpcsKilled { get => totalNpcsKilled; set => totalNpcsKilled = value; }
        public bool Activate { get => activate; set => activate = value; }
        public bool DisplayNumbers { get => displayNumbers; set => displayNumbers = value; }
        public float MoreExpMultiplier { get => moreExpMultiplier; set => moreExpMultiplier = value; }


        internal Skill[]? Skills { get => skillsSets[setSelected]; set => skillsSets[setSelected] = value; }
        public string SetSelected { get => setSelected; set => setSelected = value; }
        public int SetCount { get => skillsSets.Count; }

        public override void Initialize()
        {
            base.Initialize();
            CalcXPPerLevel();

        }

        private void CalcXPPerLevel()
        {
            expToLevel = BASE_EXP + (int)((BASE_EXP * EXPPERLEVEL) * totalLevel);
        }

        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;

            playerSettings.SetSettings();
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
        private void UpdateLevel()
        {
            if (exp < ExpToLevel) return;

            int levelsUp = 0;
            while (CanLevelUp())
            {
                levelsUp++;
            }

            showDamageText((int)CombatTextPos.AddedLevels, $"+ {levelsUp} Levels!", CombatText.DamagedFriendlyCrit);
            showDamageText((int)CombatTextPos.CurrentLevels, $"Level {level}", CombatText.DamagedFriendlyCrit, 120, true);

            Skill.AutoLevelUpSkills(Skills, ref level);

            SoundEngine.PlaySound(SoundID.Chat);

        }
        private bool CanLevelUp()
        {
            if (exp < ExpToLevel) return false;
            level++;
            totalLevel++;
            exp -= ExpToLevel;

            CalcXPPerLevel();
            return true;

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

                playerSettings.loadMyData(tag.Get<TagCompound>("UI"));

                if (tempVer != version)
                {
                    messageReset = true;
                    ResetAllSkills(tag.GetCompound("SkillData").Count);
                    return;
                }
                CalcXPPerLevel();
                loadSkills(tag);

                string? lastSet = tag.GetString("CurrentSet");
                setSelected = string.IsNullOrEmpty(lastSet) ? "0" : lastSet;
                recentChanged = true;

            }
            catch
            {
                ResetAllSkills(tag.GetCompound("SkillData").Count);
                recentChanged = true;
            }

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

            TagCompound skillData = new();

            foreach (KeyValuePair<string, Skill[]> entry in skillsSets)
            {
                TagCompound set = new TagCompound();

                foreach (Skill skill in entry.Value)
                {
                    TagCompound dataSkill = new TagCompound();
                    dataSkill.Add("level", skill.Level);
                    dataSkill.Add("automaticMode", skill.AutomaticMode);
                    set.Add(skill.GetType().ToString(), dataSkill);
                }
                skillData.Add(entry.Key, set);
            }
            tag.Add("CurrentSet", setSelected);
            tag.Add("SkillData", skillData);

            tag.Add("UI", playerSettings.SaveMyData());


        }
        private void loadSkills(TagCompound tag)
        {
            //get names
            for (int i = 0; i < tag.GetCompound("SkillData").Count; i++)
            {
                setSelected = i.ToString();
                TagCompound skillSet = (TagCompound)tag.GetCompound("SkillData")[i.ToString()];

                TagCompound skill;

                skillsSets.Add(setSelected, new Skill[SkillEnums.GetNumberOfSkills]);

                skill = skillSet.GetCompound(typeof(Defense).ToString());
                Skills[(int)SkillEnums.SkillOrder.Defense] = new Defense(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.Defense].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(LifeRegen).ToString());
                Skills[(int)SkillEnums.SkillOrder.LifeRegen] = new LifeRegen(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.LifeRegen].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(MeleeDamage).ToString());
                Skills[(int)SkillEnums.SkillOrder.MeleeDamage] = new MeleeDamage(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.MeleeDamage].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(MeleeAttackSpeed).ToString());
                Skills[(int)SkillEnums.SkillOrder.MeleeAttackSpeed] = new MeleeAttackSpeed(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.MeleeAttackSpeed].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(MagicDamage).ToString());
                Skills[(int)SkillEnums.SkillOrder.MagicDamage] = new MagicDamage(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.MagicDamage].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(ReducedManaConsumption).ToString());
                Skills[(int)SkillEnums.SkillOrder.ManaConsumption] = new ReducedManaConsumption(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.ManaConsumption].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(RangedDamage).ToString());
                Skills[(int)SkillEnums.SkillOrder.RangedDamage] = new RangedDamage(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.RangedDamage].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(SummonDamage).ToString());
                Skills[(int)SkillEnums.SkillOrder.SummonDamage] = new SummonDamage(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.SummonDamage].AutomaticMode = skill.GetBool("automaticMode");


                skill = skillSet.GetCompound(typeof(SummonCapacity).ToString());
                Skills[(int)SkillEnums.SkillOrder.MinionCapacity] = new SummonCapacity(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.MinionCapacity].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(PickaxeSpeed).ToString());
                Skills[(int)SkillEnums.SkillOrder.PickaxeSpeed] = new PickaxeSpeed(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.PickaxeSpeed].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(MovementSpeed).ToString());
                Skills[(int)SkillEnums.SkillOrder.MovementSpeed] = new MovementSpeed(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.MovementSpeed].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(GlobalCriticalChance).ToString());
                Skills[(int)SkillEnums.SkillOrder.GlobalCriticalChance] = new GlobalCriticalChance(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.GlobalCriticalChance].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(LifeSteal).ToString());
                Skills[(int)SkillEnums.SkillOrder.LifeSteal] = new LifeSteal(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.LifeSteal].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(AmmoConsumption).ToString());
                Skills[(int)SkillEnums.SkillOrder.AmmoConsumption] = new AmmoConsumption(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.AmmoConsumption].AutomaticMode = skill.GetBool("automaticMode");
            }
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
            else if (InfinitumModSystem.ChangeSet.JustPressed)
            {
                if (SetActions((int)UIElementsEnum.SetsActions.ChangeSet)) CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 50), 25, 25), Color.Red, $"Set {setSelected}", true, false);

            }

            base.ProcessTriggers(triggersSet);

        }
        public override void Unload()
        {
            base.Unload();
        }

        public bool ApplyStats(int skill, int apply)
        {
            if (Skills[skill].ApplyStat(apply, ref level))
            {
                recentChanged = true;
                return true;
            }
            return false;
        }
        public override void PostUpdateEquips()
        {
            if(Main.netMode == NetmodeID.Server)
            {             
                base.PostUpdateEquips();
                return;
            }
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

            //sets
            foreach (Skill s in Skills)
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
                Skills[(int)SkillEnums.SkillOrder.LifeSteal].ApplyStatToPlayer(damage2);

            base.ModifyHitNPC(item, target, ref damage, ref knockback, ref crit);

        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            int damage2 = damage;
            if (activate && target.netID != 488)
                Skills[(int)SkillEnums.SkillOrder.LifeSteal].ApplyStatToPlayer(damage2);
            base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);

        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {
            Skills[(int)SkillEnums.SkillOrder.AmmoConsumption].ApplyStatToPlayer(out bool canConsumeAmmo);

            return canConsumeAmmo;
        }
        public void ChatMessage(string text, Color c)
        {
            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), c);
            }
            else
            {
                Main.NewText(text, c);
            }
        }

        public void ResetCurrentSkills()
        {

            level = totalLevel;
            CalcXPPerLevel();

            InitializeSkillsOfCurrentSet();

        }
        private void InitializeSkillsOfCurrentSet()
        {
            if (!skillsSets.ContainsKey(setSelected))
                skillsSets.Add(setSelected, new Skill[SkillEnums.GetNumberOfSkills]);

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
        public void ResetAllSkills(int currentSets)
        {
            level = totalLevel;
            CalcXPPerLevel();
            currentSets = currentSets == 0 ? 1 : currentSets;//minimum 1 set
            for (int i = 0; i < currentSets; i++)
            {
                setSelected = i.ToString();
                InitializeSkillsOfCurrentSet();
            }
            setSelected = "0";
        }

        public bool SetActions(int action)
        {
            switch (action)
            {
                case (int)UIElementsEnum.SetsActions.ChangeSet:
                    //rework
                    if (skillsSets.Count == 1)
                    {
                        ChatMessage("Infinitum: Only one set is created!", Color.Red);
                        return false;
                    }

                    if (int.Parse(setSelected) + 1 == skillsSets.Count)
                        setSelected = "0";
                    else
                        setSelected = (int.Parse(setSelected) + 1).ToString();

                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    ChatMessage($"Infinitum: Set {setSelected}", Color.Green);
                    break;


                case (int)UIElementsEnum.SetsActions.AddSet:
                    int setsCount = skillsSets.Count;
                    skillsSets.Add(setsCount.ToString(), new Skill[SkillEnums.GetNumberOfSkills]);
                    setSelected = setsCount.ToString();
                    InitializeSkillsOfCurrentSet();
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    ChatMessage($"Infinitum: New Set {setSelected}", Color.Green);
                    break;
                case (int)UIElementsEnum.SetsActions.DeleteSet:

                    int currentSet = int.Parse(setSelected);
                    Dictionary<string, Skill[]> newSets = new Dictionary<string, Skill[]>();
                    if (currentSet == 0)
                    {
                        ChatMessage("Infinitum: Set 0 cannot be deleted!", Color.Red);
                        return false;
                    }

                    setSelected = (currentSet - 1).ToString();
                    skillsSets.Remove(currentSet.ToString());

                    ChatMessage($"Infinitum: Set {setSelected} replaced with {int.Parse(setSelected) + 1}", Color.Green);

                    int iterator = 0;
                    foreach (KeyValuePair<string, Skill[]> entry in skillsSets)
                    {
                        newSets.Add(iterator.ToString(), entry.Value);
                        iterator++;
                    }
                    skillsSets = newSets;
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    break;
            }
            RecalcLevel();
            recentChanged = true;
            return true;
        }

        private void RecalcLevel()
        {
            level = totalLevel;
            foreach (Skill skill in Skills) level -= skill.TotalSpend;
        }

        public override void ModifyCaughtFish(Item fish)
        {
            // TODO: Add stars to pool fishing
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