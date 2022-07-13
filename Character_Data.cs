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
            { "currentLevels", 65}
        };

        private static string[] skillOrder = {
            "Defense",
            "Melee Damage",
            "Melee Attack Speed",
            "Life Regen",
            "Life Steal",
            "Magic Damage",
            "Mana Consumption",
            "Ranged Damage",
            "Ammo Consumption",
            "Throwing  Damage",
            "Throwing algo?",
            "Summon Damage",
            "Summon Attack Speed",
            "Pickaxe Power",
            "Ranged Attack Speed",
        };
        private static int[] skillCost =
        {
            1000,
            250,
            250,
            100,
            500,
            250,
            250,
            250,
            250,
            0,
            0,
            250,
            0,
            150,
            0
        };
        private float exp = 0.0f;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
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
        private float additionalthrowingDamage = 0;
        private float additionalthrowingAttackSpeed = 0;
        private float additionalSummonDamage = 0;
        private float additionalSummonAttackSpeed = 0;
        private float additionalPickingPower = 0;
        private List<Skill> skills = new List<Skill>();


        public float Exp { get => exp; }
        public int Level { get => level; }
        public int TotalLevel { get => totalLevel; }
        public float ExpMultiplier { get => expMultiplier; }
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
        public float AdditionalthrowingDamage { get => additionalthrowingDamage; set => additionalthrowingDamage = value; }
        public float AdditionalsummonDamage { get => additionalSummonDamage; set => additionalSummonDamage = value; }
        public float AdditionalsummonAttackSpeed { get => additionalSummonDamage; set => additionalSummonDamage = value; }
        public float AdditionalPickingPower { get => additionalPickingPower; set => additionalPickingPower = value; }
        public long TotalNpcsKilled { get => totalNpcsKilled; set => totalNpcsKilled = value; }
        public bool Activate { get => activate; set => activate = value; }
        public bool DisplayNumbers { get => displayNumbers; set => displayNumbers = value; }


        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;
            showDamageText(CombatTextPos["currentLevels"], $"Level {level}", CombatText.DamagedFriendlyCrit);
            InfinitumUI.Instance.stats = this;
        }
        private void showDamageText(int yPos, string text, Color c, int duration = 1, bool dramatic = false, bool dot = false)
        {
            if (displayNumbers)
                CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + yPos), 25, 25), c, text, dramatic, dot);
        }
        public override void Load()
        {
            base.Load();
           
        }
        public void AddXp(float xp)
        {
            exp += (float)(xp * expMultiplier);
            UpdateLevel();
            showDamageText(CombatTextPos["xp"], $"+ {((float)(xp * expMultiplier)):n1} XP", CombatText.HealMana);
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

            showDamageText(CombatTextPos["addedLevels"], $"+Level {LevelsUp}!", CombatText.DamagedFriendlyCrit);
            showDamageText(CombatTextPos["currentLevels"], $"Level {level}", CombatText.DamagedFriendlyCrit);

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
                tag.TryGet("PickaxePower", out additionalPickingPower);
                tag.TryGet("DisplayNumbers", out displayNumbers);//better this...


                recentChanged = true;
            }
            catch
            {

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
            //tag.Add("ThrowingDamage", additionalthrowingDamage);
            //tag.Add("ThrowingAttackSpeed", additionalthrowingAttackSpeed);
            tag.Add("SummonDamage", additionalSummonDamage);
            //tag.Add("SummonAttackSpeed", additionalSummonAttackSpeed);
            tag.Add("PickaxePower", additionalPickingPower);
            tag.Add("DisplayNumbers", displayNumbers);


        }
        public Character_Data GetStats()
        {
            return this;
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
        public void ApplyStats(string stat)
        {//sw probablemente
            //implement skill class
            switch (stat)
            {
                case "Defense":
                    additionalDefense++;
                    break;
                case "Melee Damage":
                    additionalMeleeDamage += .01f;
                    break;
                case "Melee Attack Speed":
                    additionalMeleeAttackSpeed += 0.01f;
                    break;
                case "Life Regen":
                    AdditionalLifeRegen += 0.25f;
                    break;
                case "Life Steal":
                    LifeSteal += 0.00025f;
                    break;
                case "Magic Damage":
                    additionalMagicDamage += .01f;
                    break;
                case "Mana Consumption":
                    reducedManaConsumption += 0.01f;//dont work
                    break;
                case "Ranged Damage":
                    additionalRangedDamage += 0.01f;
                    break;
                case "Ammo Consumption":
                    ammoConsumedReduction -= 1;
                    break;
                case "Throwing  Damage":
                    //additionalthrowingDamage += 1f;//dont Work
                    break;
                case "Throwing algo?":
                    break;
                case "Summon Damage":
                    additionalSummonDamage += 0.01f;
                    break;
                case "Summon Attack Speed":
                    additionalSummonAttackSpeed += 1f;//dont Work
                    break;
                case "Pickaxe Power":
                    additionalPickingPower += .025f;
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
            player.GetDamage(DamageClass.Throwing) = player.GetDamage(DamageClass.Throwing) + additionalthrowingDamage;
            //player.GetAttackSpeed(DamageClass.Throwing) = player.GetAttackSpeed(DamageClass.Throwing) + additionalthrowingAttackSpeed;
            player.GetDamage(DamageClass.Summon) = player.GetDamage(DamageClass.Summon) + additionalSummonDamage;
            player.GetAttackSpeed(DamageClass.Summon) = player.GetAttackSpeed(DamageClass.Summon) + additionalSummonAttackSpeed;
            player.manaCost = player.manaCost - reducedManaConsumption;
            player.pickSpeed = player.pickSpeed - additionalPickingPower;



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

            //mult -= .3f;
            base.ModifyManaCost(item, ref reduce, ref mult);
        }
        public override bool CanConsumeAmmo(Item weapon, Item ammo)
        {

            if (ammoConsumedReduction < 101 && ammoConsumedReduction > 1)
            {
                return !(Main.rand.Next(ammoConsumedReduction) <= Math.Abs(ammoConsumedReduction - 100));
            }
            return base.CanConsumeAmmo(weapon, ammo);
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
            additionalthrowingDamage = 0;
            additionalthrowingAttackSpeed = 0;
            additionalSummonDamage = 0;
            additionalSummonAttackSpeed = 0;
            additionalPickingPower = 0;

            recentChanged = true;
        }

        private void returnLevels()
        {

        }
    }

}