using Infinitum.Buffs;
using Infinitum.Items;
using Infinitum.Skills;
using Infinitum.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Terraria;
using Terraria.Audio;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader.Exceptions;
using Terraria.ModLoader.IO;
using Terraria.WorldBuilding;

namespace Infinitum
{
    //TODO: Use name for sets not numbers
    //TODO: Check automatic skills when change set
    public class Character_Data : ModPlayer
    {
        private Player player = Main.CurrentPlayer;
        private static Mod myMod = ModLoader.GetMod("Infinitum");
        private ModPacket myPacket;
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
        private string version = "0.84";//Use for resetting skills when new added.
        private bool messageReset = false;
        private double exp = 0;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
        private float moreExpMultiplier = 1.0f;
        private const int BASE_EXP = 30000;
        private ulong expToLevel = BASE_EXP;
        private const float EXPPERLEVEL = 0.0005f;
        private long totalNpcsKilled = 0;
        private bool activate = true;
        private bool displayNumbers = true;

        private Dictionary<string, Skill[]> skillsSets = new Dictionary<string, Skill[]>();
        private string setSelected = "0";
        public double Exp { get => exp; }
        public int Level { get => level; }
        public int TotalLevel { get => totalLevel; }
        public float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }
        public ulong ExpToLevel => expToLevel;
        public bool RecentChanged { get => recentChanged; set => recentChanged = value; }
        //dont need?
        public long TotalNpcsKilled { get => totalNpcsKilled; set => totalNpcsKilled = value; }
        public bool Activate { get => activate; set => activate = value; }
        public bool DisplayNumbers { get => displayNumbers; set => displayNumbers = value; }
        public float MoreExpMultiplier { get => moreExpMultiplier; set => moreExpMultiplier = value; }

        internal Skill[]? Skills { get => skillsSets[setSelected]; set => skillsSets[setSelected] = value; }
        public string SetSelected { get => setSelected; set => setSelected = value; }
        public int SetCount { get => skillsSets.Count; }      
        public float? GetXpFromDifficulty => Infinitum.instance.Difficulty.GetXPFromDifficulty;
        public Tuple<float, float> GetTotalXpMultiplier => Tuple.Create(expMultiplier * moreExpMultiplier, (float)((expMultiplier * moreExpMultiplier) * GetXpFromDifficulty) - (expMultiplier * moreExpMultiplier));

        public override void Initialize()
        {
            base.Initialize();
            CalcXPPerLevel();
        }

        private void CalcXPPerLevel()
        {
            expToLevel = BASE_EXP + (ulong)((BASE_EXP * EXPPERLEVEL) * totalLevel);
        }

        public override void OnEnterWorld()
        {
            player = this.Player;

            playerSettings.SetSettings();
            showDamageText((int)CombatTextPos.CurrentLevels, $"Level {totalLevel}", CombatText.DamagedFriendlyCrit, 120, true);

            InfinitumUI.Instance.stats = this;
            ExpBarUI.Instance.stats = this;
            Skill.player = this.Player;
            InfinitumGlobalItem.playerDataHook = this;

            if (messageReset)
            {
                showDamageText((int)CombatTextPos.CurrentLevels + 50, "Skills Reset!", Color.Red, 180, true);
                ChatMessage($"New Skills Version Detected. Check Your skills has beed reset.[Infinitum v{version}]", Color.Red);
            }

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
        public void AddXp(float xp, MessageType type)
          {
            if (Main.gameMenu) return;//This can be triggered by calamity first time in the world?

            double experienceObtained = (double)xp * ((double)expMultiplier * moreExpMultiplier);
            if(type == MessageType.XPFromNPCs)
            {
                experienceObtained *= Infinitum.instance.Difficulty.GetXPFromDifficulty;
                CheckForBuffs();
            }
                

            exp += experienceObtained > 0.0 ? experienceObtained : 0;
            UpdateLevel();
            showDamageText((int)CombatTextPos.Xp, $"+ {experienceObtained:n1} XP", CombatText.HealMana);
            totalNpcsKilled++;

            if (avgXP.Count > 100)
                avgXP.RemoveRange(0, 50);
            avgXP.Add((float)experienceObtained);
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
                tag.TryGet("Version", out string tempVer);
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

            if (!skillsSets.ContainsKey(setSelected)) InitializeSkillsOfCurrentSet();//for new players

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
            if (tag.GetCompound("SkillData").Count == 0)
            {
                InitializeSkillsOfCurrentSet();
                return;
            }

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

                skill = skillSet.GetCompound(typeof(ArmorPenetration).ToString());
                Skills[(int)SkillEnums.SkillOrder.ArmorPenetration] = new ArmorPenetration(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.ArmorPenetration].AutomaticMode = skill.GetBool("automaticMode");

                skill = skillSet.GetCompound(typeof(GrabRange).ToString());
                Skills[(int)SkillEnums.SkillOrder.GrabRange] = new GrabRange(skill.GetInt("level"));
                Skills[(int)SkillEnums.SkillOrder.GrabRange].AutomaticMode = skill.GetBool("automaticMode");
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
                if (SetActions(UIElementsEnum.SetsActions.ChangeSet)) CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 50), 25, 25), Color.Red, $"Set {setSelected}", true, false);

            }

            base.ProcessTriggers(triggersSet);
        }
        public override void Unload()
        {
            base.Unload();
        }

        public bool ApplyStats(int skill, SkillEnums.Actions action)
        {
            if (Skills[skill].ApplyStat(action, ref level))
            {
                recentChanged = true;
                return true;
            }
            return false;
        }
        public override void PostUpdateEquips()
        {
            // TODO: When in multiplayer a default character_data with no loads call this???

            if (Main.netMode == NetmodeID.Server || SetCount == 0)
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
                if (s.Type == SkillEnums.Type.PostUpdateEquips)
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

            if (player.HasBuff<XPBuff>()) moreExpMultiplier += .5f;
            if (player.HasBuff<InfinitumBuff>())
            {
                moreExpMultiplier += ((InfinitumBuff)BuffLoader.GetBuff(ModContent.BuffType<InfinitumBuff>())).XPMultiplier;
            }
            recentChanged = true;
        }
        public override void PreUpdate()
        {
            MoreExpMultiplier = 1f;
            base.PreUpdate();
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            dynamic playerHit = new { damage = damageDone, defense = target.defense };

            if (activate && target.netID != 488)
                Skills[(int)SkillEnums.SkillOrder.LifeSteal].ApplyStatToPlayer(playerHit);

            base.OnHitNPC(target, hit, damageDone);
        }
        public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
        {
            base.OnHitNPCWithProj(proj, target, hit, damageDone);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
        {
             dynamic hit = new { damage = proj.damage, defense = target.defense };

            if (activate && target.netID != 488)
                Skills[(int)SkillEnums.SkillOrder.LifeSteal].ApplyStatToPlayer(hit);
            //base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);
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
        public void ResetAllCharacterData()
        {
            ResetAllSkills(skillsSets.Count);
            exp = 0;
            level = 0;
            totalLevel = 0;
            expMultiplier = 1.0f;
            expToLevel = BASE_EXP;
            totalNpcsKilled = 0;

            showDamageText((int)CombatTextPos.CurrentLevels + 50, "Skills Reset!", Color.Red, 250, true);
            showDamageText((int)CombatTextPos.CurrentLevels + 150, "Character Data Reset", Color.Red, 250, true);
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
            Skills[(int)SkillEnums.SkillOrder.ArmorPenetration] = new ArmorPenetration(0);
            Skills[(int)SkillEnums.SkillOrder.LifeSteal] = new LifeSteal(0);
            Skills[(int)SkillEnums.SkillOrder.AmmoConsumption] = new AmmoConsumption(0);
            Skills[(int)SkillEnums.SkillOrder.GrabRange] = new GrabRange(0);
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

        public bool SetActions(UIElementsEnum.SetsActions action)
        {
            switch (action)
            {
                case UIElementsEnum.SetsActions.ChangeSet:
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
                    ChatMessage($"Infinitum: Set {setSelected}({Skill.GetBuffs(Skills)})", Color.Green);
                    break;

                case UIElementsEnum.SetsActions.AddSet:
                    int setsCount = skillsSets.Count;
                    skillsSets.Add(setsCount.ToString(), new Skill[SkillEnums.GetNumberOfSkills]);
                    setSelected = setsCount.ToString();
                    InitializeSkillsOfCurrentSet();
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    ChatMessage($"Infinitum: New Set {setSelected}", Color.Green);
                    break;
                case UIElementsEnum.SetsActions.DeleteSet:

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
            if (Main.rand.NextBool(MultiplierStar.ChanceFromFishing))
            {

            }

            int rarity = fish.rare >= ItemRarityID.White ? fish.rare : 1;
            float xp = (((rarity * 5) + 1) * 3.5f + (fish.value / 250)) * fish.stack;

            if (Main.netMode == NetmodeID.SinglePlayer)
                AddXp(xp,MessageType.XPFromOtherSources);
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //mirar
                Task.Run(() =>
                {
                    myPacket = myMod.GetPacket();
                    myPacket.Write((byte)MessageType.XPFromNPCs);
                    myPacket.Write(xp);
                    myPacket.Send();
                });
            }
            base.ModifyCaughtFish(fish);
        }
        public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
        {
            if (Main.rand.NextBool(ExpStar.ChanceFromFishing))
                itemDrop = ModContent.ItemType<ExpStar>();
            else if (Main.rand.NextBool(MultiplierStar.ChanceFromFishing))
                itemDrop = ModContent.ItemType<MultiplierStar>();

            base.CatchFish(attempt, ref itemDrop, ref npcSpawn, ref sonar, ref sonarPosition);
        }
        private void CheckForBuffs()
        {
            if (!player.HasBuff<InfinitumBuff>()) return;


            ((InfinitumBuff)BuffLoader.GetBuff(ModContent.BuffType<InfinitumBuff>())).UpdateFromKill(player);          
            
        }
    }

}