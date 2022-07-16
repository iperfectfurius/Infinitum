using Infinitum.Skills;
using Infinitum.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.Chat;
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
        private bool recentChanged = false;
        private Dictionary<string, int> CombatTextPos = new()
        {
            { "xp", 145},
            { "addedLevels", 180},
            { "currentLevels", 50}
        };

        private static string[] skillOrder = {
            "Defense",
            "Movement Speed",
            "Life Regen",
            "Life Steal",
            "Global Critical Chance",
            "Melee Damage",
            "Melee Attack Speed",
            "Magic Damage",
            "Mana Consumption",
            "Ranged Damage",
            "Ammo Consumption",
            "Summon Damage",
            "Minion Capacity",
            "Pickaxe Power"
        };
        private static int[] skillCost =
        {
            1000,
            750,
            500,
            500,
            1000,
            250,
            250,
            250,
            250,
            250,
            500,
            250,
            5000,
            150,
        };
        private bool notFirstTime = false;
        private string version = "0.43";
        private bool messageReset = false;
        private float exp = 0.0f;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
        private float moreExpMultiplier = 1.0f;
        private const int EXPTOLEVEL = 15000;
        private long totalNpcsKilled = 0;
        private bool activate = true;
        private bool displayNumbers = true;
        private float additionalDefense = 0;
        private float additionalMeleeDamage = 0;
        private float additionalMeleeAttackSpeed = 0;
        private float additionalLifeRegen = 0;
        private float lifeSteal = 0;
        private float stackedLifeSteal = 0;
        private float additionalMagicDamage = 0;
        private float reducedManaConsumption = 0;
        private float additionalRangedDamage = 0;
        private int ammoConsumedReduction = 101;
        private float additionalMovementSpeed = 0;
        private int additionalGlobalCriticalChance = 0;
        private float additionalSummonDamage = 0;
        private int additionalSummonCapacity = 0;
        private float additionalPickingPower = 0;
        private List<Skill> skills = new List<Skill>();



        public float Exp { get => exp; }
        public int Level { get => level; }
        public int TotalLevel { get => totalLevel; }
        public float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }
        public int _EXPTOLEVEL => EXPTOLEVEL;
        public bool RecentChanged { get => recentChanged; set => recentChanged = value; }
        public static int[] SkillCost { get => skillCost; set => skillCost = value; }
        //dont need?
        public float AdditionalDefense { get => additionalDefense; set => additionalDefense = value; }
        public float AdditionalMeleeDamage { get => additionalMeleeDamage; set => additionalMeleeDamage = value; }
        public float AdditionalMeleeAttackSpeed { get => additionalMeleeAttackSpeed; set => additionalMeleeAttackSpeed = value; }
        public float AdditionalLifeRegen { get => additionalLifeRegen; set => additionalLifeRegen = value; }
        public float LifeSteal { get => lifeSteal; set => lifeSteal = value; }
        public float AdditionalMagicDamage { get => additionalMagicDamage; set => additionalMagicDamage = value; }
        public float AdditionalRangedDamage { get => additionalRangedDamage; set => additionalRangedDamage = value; }
        public int AmmoConsumedReduction { get => ammoConsumedReduction; set => ammoConsumedReduction = value; }
        public static string[] SkillOrder { get => skillOrder; set => skillOrder = value; }
        public float ReducedManaConsumption { get => reducedManaConsumption; set => reducedManaConsumption = value; }
        public float AdditionalMovementSpeed { get => additionalMovementSpeed; set => additionalMovementSpeed = value; }
        public float AdditionalsummonDamage { get => additionalSummonDamage; set => additionalSummonDamage = value; }
        public int AdditionalSummonCapacity { get => additionalSummonCapacity; set => additionalSummonCapacity = value; }
        public float AdditionalPickingPower { get => additionalPickingPower; set => additionalPickingPower = value; }
        public long TotalNpcsKilled { get => totalNpcsKilled; set => totalNpcsKilled = value; }
        public bool Activate { get => activate; set => activate = value; }
        public bool DisplayNumbers { get => displayNumbers; set => displayNumbers = value; }
        public float MoreExpMultiplier { get => moreExpMultiplier; set => moreExpMultiplier = value; }
        public int AdditionalGlobalCriticalChance { get => additionalGlobalCriticalChance; set => additionalGlobalCriticalChance = value; }

        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;
            showDamageText(CombatTextPos["currentLevels"], $"Level {totalLevel}", CombatText.DamagedFriendlyCrit, 120, true);
            InfinitumUI.Instance.stats = this;
            ExpBarUI.Instance.stats = this;
            if (messageReset)
                Main.NewText("Skills Reset!");
        }
        private void showDamageText(int yPos, string text, Color c, int duration = 60, bool dramatic = false, bool dot = false)
        {
            if (Main.netMode == NetmodeID.Server || !displayNumbers) return;

            int test = CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + yPos), 25, 25), c, text, dramatic, dot);
            Main.combatText[test].lifeTime = duration;

        }
        public override void Load()
        {
            base.Load();

        }
        public void AddXp(float xp)
        {
            float experienceObtained = xp * (expMultiplier * moreExpMultiplier);
            exp += experienceObtained;
            UpdateLevel();
            showDamageText(CombatTextPos["xp"], $"+ {experienceObtained:n1} XP", CombatText.HealMana);
            totalNpcsKilled++;
            recentChanged = true;

        }
        private void UpdateLevel()
        {
            if (exp < EXPTOLEVEL) return;

            int LevelsUp = (int)exp / EXPTOLEVEL;
            exp -= EXPTOLEVEL * LevelsUp;
            level += LevelsUp;
            totalLevel += LevelsUp;

            showDamageText(CombatTextPos["addedLevels"], $"+ {LevelsUp} Levels!", CombatText.DamagedFriendlyCrit);
            showDamageText(CombatTextPos["currentLevels"], $"Level {level}", CombatText.DamagedFriendlyCrit, 120, true);

        }
        public void AddXpMultiplier(float multiplier)
        {
            expMultiplier += multiplier;
            showDamageText(CombatTextPos["xp"], $"{(expMultiplier * 100f):n2}% Multiplier!", CombatText.DamagedFriendlyCrit);
            recentChanged = true;
        }

        public override void LoadData(TagCompound tag)
        {
            try
            {
                string tempVer;
                tag.TryGet("Level", out level);
                tag.TryGet("ExpMultiplier", out expMultiplier);
                tag.TryGet("Exp", out exp);
                tag.TryGet("TotalLevel", out totalLevel);
                tag.TryGet("TotalNpcsKilled", out totalNpcsKilled);
                tag.TryGet("Activate", out activate);
                tag.TryGet("Defense", out additionalDefense);
                tag.TryGet("MeleeDamage", out additionalMeleeDamage);
                tag.TryGet("MeleeAttackSpeed", out additionalMeleeAttackSpeed);
                tag.TryGet("LifeRegen", out additionalLifeRegen);
                tag.TryGet("LifeSteal", out lifeSteal);
                tag.TryGet("MagicDamage", out additionalMagicDamage);
                tag.TryGet("ManaConsumption", out reducedManaConsumption);
                tag.TryGet("RangedDamage", out additionalRangedDamage);
                tag.TryGet("RangedAmmoConsumption", out ammoConsumedReduction);
                tag.TryGet("SummonDamage", out additionalSummonDamage);
                tag.TryGet("MinionCapacity", out additionalSummonCapacity);
                tag.TryGet("PickaxePower", out additionalPickingPower);
                tag.TryGet("MovementSpeed", out additionalMovementSpeed);
                tag.TryGet("DisplayNumbers", out displayNumbers);
                tag.TryGet("NotFirstTime", out notFirstTime);
                tag.TryGet("Version", out tempVer);
                tag.TryGet("GlobalCriticalChance", out additionalGlobalCriticalChance);

                if (!notFirstTime)
                    displayNumbers = true;

                if (tempVer != version)
                {
                    messageReset = true;
                    resetCurrentSkills();
                }


                recentChanged = true;
            }
            catch
            {
                resetCurrentSkills();
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
            tag.Add("Defense", additionalDefense);
            tag.Add("MeleeDamage", additionalMeleeDamage);
            tag.Add("MeleeAttackSpeed", additionalMeleeAttackSpeed);
            tag.Add("LifeRegen", additionalLifeRegen);
            tag.Add("LifeSteal", LifeSteal);
            tag.Add("MagicDamage", additionalMagicDamage);
            tag.Add("ManaConsumption", reducedManaConsumption);
            tag.Add("RangedDamage", additionalRangedDamage);
            tag.Add("RangedAmmoConsumption", ammoConsumedReduction);
            tag.Add("SummonDamage", additionalSummonDamage);
            tag.Add("MinionCapacity", additionalSummonCapacity);
            tag.Add("PickaxePower", additionalPickingPower);
            tag.Add("DisplayNumbers", displayNumbers);
            tag.Add("MovementSpeed", additionalMovementSpeed);
            tag.Add("GlobalCriticalChance", additionalGlobalCriticalChance);
            tag.Add("NotFirstTime", true);
            //tag.Add("FirstTime", true);
            tag.Add("Version", version);


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
            CombatTextPos = new();
            base.Unload();
        }
        public void ApplyStats(string stat, bool sum)
        {
            //implement skill class
            switch (stat)
            {
                case "Defense":

                    if (level >= skillCost[0] && sum)
                    {
                        level -= skillCost[0];
                        additionalDefense++;
                    }
                    else if (!sum && additionalDefense > 0)
                    {
                        level += skillCost[0];
                        additionalDefense--;
                    }
                    break;
                case "Movement Speed":
                    if (level >= skillCost[1] && sum)
                    {
                        level -= skillCost[1];
                        additionalMovementSpeed += .01f;
                    }
                    else if (!sum && additionalMovementSpeed > 0)
                    {
                        level += skillCost[1];
                        additionalMovementSpeed -= .01f;
                    }
                    break;
                case "Life Regen":
                    if (level >= skillCost[2] && sum)
                    {
                        level -= skillCost[2];
                        AdditionalLifeRegen += 0.25f;
                    }
                    else if (!sum && additionalLifeRegen > 0)
                    {
                        level += skillCost[2];
                        AdditionalLifeRegen -= 0.25f;
                    }
                    break;
                case "Life Steal":
                    if (level >= skillCost[3] && sum)
                    {
                        level -= skillCost[3];
                        LifeSteal += 0.00025f;
                    }
                    else if (!sum && lifeSteal > 0)
                    {
                        level += skillCost[3];
                        LifeSteal -= 0.00025f;
                    }
                    break;
                case "Global Critical Chance":
                    if (level >= skillCost[4] && sum)
                    {
                        level -= skillCost[4];
                        additionalGlobalCriticalChance += 1;
                    }
                    else if (!sum && additionalGlobalCriticalChance > 0)
                    {
                        level += skillCost[4];
                        additionalGlobalCriticalChance--;
                    }
                    break;
                case "Melee Damage":
                    if (level >= skillCost[5] && sum)
                    {
                        level -= skillCost[5];
                        additionalMeleeDamage += .01f;
                    }
                    else if (!sum && additionalMeleeDamage > 0)
                    {
                        level += skillCost[5];
                        additionalMeleeDamage -= 0.01f;
                    }

                    break;
                case "Melee Attack Speed":
                    if (level >= skillCost[6] && sum)
                    {
                        level -= skillCost[6];
                        additionalMeleeAttackSpeed += 0.01f;
                    }
                    else if (!sum && additionalMeleeAttackSpeed > 0)
                    {
                        level += skillCost[6];
                        additionalMeleeAttackSpeed -= 0.01f;
                    }

                    break;

                case "Magic Damage":
                    if (level >= skillCost[7] & sum)
                    {
                        level -= skillCost[7];
                        additionalMagicDamage += .01f;
                    }
                    else if (!sum && additionalMagicDamage > 0)
                    {
                        level += skillCost[7];
                        additionalMagicDamage -= .01f;
                    }
                    break;
                case "Mana Consumption":
                    if (level >= skillCost[8] && sum)
                    {
                        level -= skillCost[8];
                        reducedManaConsumption += 0.01f;
                    }
                    else if (!sum && reducedManaConsumption > 0)
                    {
                        level += skillCost[8];
                        reducedManaConsumption -= 0.01f;
                    }
                    break;
                case "Ranged Damage":
                    if (level >= skillCost[9] && sum)
                    {
                        level -= skillCost[9];
                        additionalRangedDamage += 0.01f;
                    }
                    else if (!sum && additionalRangedDamage > 0)
                    {
                        level += skillCost[9];
                        additionalRangedDamage -= 0.01f;
                    }
                    break;
                case "Ammo Consumption":
                    if (level >= skillCost[10] && sum)
                    {
                        level -= skillCost[10];
                        ammoConsumedReduction -= 1;
                    }
                    else if (!sum && ammoConsumedReduction < 101)
                    {
                        level += skillCost[10];
                        ammoConsumedReduction += 1;
                    }
                    break;

                case "Summon Damage":
                    if (level >= skillCost[11] && sum)
                    {
                        level -= skillCost[11];
                        additionalSummonDamage += 0.01f;
                    }
                    else if (!sum && additionalSummonDamage > 0)
                    {
                        level += skillCost[11];
                        additionalSummonDamage -= 0.01f;
                    }
                    break;
                case "Minion Capacity":
                    if (level >= skillCost[12] && sum)
                    {
                        level -= skillCost[12];
                        additionalSummonCapacity += 1;
                    }
                    else if (!sum && additionalSummonCapacity > 0)
                    {
                        level += skillCost[12];
                        additionalSummonCapacity -= 1;
                    }
                    break;
                case "Pickaxe Power":
                    if (level >= skillCost[13] && sum)
                    {
                        level -= skillCost[13];
                        additionalPickingPower += .025f;
                    }
                    else if (!sum && additionalPickingPower > 0)
                    {
                        level += skillCost[13];
                        additionalPickingPower -= .025f;
                    }
                    break;

                default:
                    break;
            }
            recentChanged = true;
        }
        public override void PostUpdateEquips()
        {
            //consistency...
            if (!activate)
            {
                base.PostUpdateEquips();
                return;
            }
            player.statDefense = player.statDefense + (int)additionalDefense;
            player.GetDamage(DamageClass.Melee) = player.GetDamage(DamageClass.Melee) + additionalMeleeDamage;
            player.GetAttackSpeed(DamageClass.Melee) = player.GetAttackSpeed(DamageClass.Melee) + additionalMeleeAttackSpeed;
            player.lifeRegen = player.lifeRegen + (int)AdditionalLifeRegen;
            player.GetDamage(DamageClass.Magic) = player.GetDamage(DamageClass.Magic) + additionalMagicDamage;
            player.GetAttackSpeed(DamageClass.Magic) = player.GetAttackSpeed(DamageClass.Magic) + reducedManaConsumption;
            player.GetDamage(DamageClass.Ranged) = player.GetDamage(DamageClass.Ranged) + additionalRangedDamage;

            //player.GetAttackSpeed(DamageClass.Ranged) = player.GetAttackSpeed(DamageClass.Ranged) + additionalRangeAttackSpeed;
            player.GetDamage(DamageClass.Throwing) = player.GetDamage(DamageClass.Throwing) + additionalMovementSpeed;
            //player.GetAttackSpeed(DamageClass.Throwing) = player.GetAttackSpeed(DamageClass.Throwing) + additionalthrowingAttackSpeed;
            player.GetDamage(DamageClass.Summon) = player.GetDamage(DamageClass.Summon) + additionalSummonDamage;
            player.maxMinions = player.maxMinions + AdditionalSummonCapacity;
            player.manaCost = player.manaCost - reducedManaConsumption;
            player.pickSpeed = player.pickSpeed - additionalPickingPower;
            player.accRunSpeed = player.accRunSpeed + additionalMovementSpeed;
            player.moveSpeed = player.moveSpeed + additionalMovementSpeed;
            player.maxRunSpeed = player.maxRunSpeed + additionalMovementSpeed;
            player.GetCritChance(DamageClass.Melee) = player.GetCritChance(DamageClass.Melee) + additionalGlobalCriticalChance;
            player.GetCritChance(DamageClass.Magic) = player.GetCritChance(DamageClass.Magic) + additionalGlobalCriticalChance;
            player.GetCritChance(DamageClass.Ranged) = player.GetCritChance(DamageClass.Ranged) + additionalGlobalCriticalChance;
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

        private void getLifeSteal(int damage)
        {
            int toHeal = (int)(damage * lifeSteal);
            stackedLifeSteal += (damage * lifeSteal) - (float)Math.Truncate(damage * lifeSteal);

            if (stackedLifeSteal > 1)
            {
                stackedLifeSteal -= 1f;

                player.HealEffect(toHeal + 1);
                player.statLife += toHeal + 1;
            }
            else if (toHeal >= 1)
            {
                player.HealEffect(toHeal);
                player.statLife += toHeal;
            }
        }
        public override void PreUpdate()
        {
            MoreExpMultiplier = 1f;
            base.PreUpdate();
        }
        public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (activate && target.netID != 488)
                getLifeSteal(damage);
            base.ModifyHitNPC(item, target, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (activate && target.netID != 488)
                getLifeSteal(damage);
            base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);

        }

        public override void ModifyManaCost(Item item, ref float reduce, ref float mult)
        {


            base.ModifyManaCost(item, ref reduce, ref mult);
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {

            if (ammoConsumedReduction < 101 && ammoConsumedReduction > 1)
            {
                return !(Main.rand.Next(100) <= Math.Abs(100 -ammoConsumedReduction));
            }
            return base.CanConsumeAmmo(weapon, ammo);
        }

        public override void PostItemCheck()
        {

            base.PostItemCheck();

        }

        public static void ChatMessage(string text = "")
        {

            if (Main.netMode == NetmodeID.Server)
            {
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text + " Desde Server"), Color.Red);
            }
            else if (Main.netMode == NetmodeID.SinglePlayer)
            {
                Main.NewText(text + " Desde single");
            }
        }

        public void resetCurrentSkills()
        {
            returnLevels();
            level = totalLevel;
            additionalDefense = 0;
            additionalMeleeDamage = 0;
            additionalMeleeAttackSpeed = 0;
            additionalLifeRegen = 0;
            lifeSteal = 0;
            stackedLifeSteal = 0;
            additionalMagicDamage = 0;
            reducedManaConsumption = 0;
            additionalRangedDamage = 0;
            ammoConsumedReduction = 101;
            additionalMovementSpeed = 0;
            additionalGlobalCriticalChance = 0;
            additionalSummonDamage = 0;
            additionalSummonCapacity = 0;
            additionalPickingPower = 0;

            recentChanged = true;
        }

        private void returnLevels()
        {

        }
    }

}