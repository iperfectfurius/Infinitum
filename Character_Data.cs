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
        private Dictionary<string, dynamic> skillCost = new();
        private static string[] skillOrder = {
            "Defense",
            "Melee Damage",
            "Melee Attack Speed",
            "Life Regen",
            "Life Steal",
            "Magic Damage",
            "Maigc Attack Speed",
            "Ranged Damage",
            "Ranged Consume Ammo",
            "Throwing  Damage",
            "Throwing algo?",
            "Summon Damage",
            "Summon Attack Speed",
            "Pickaxe Power",
            "Ranged Attack Speed",
        };
        private float exp = 0.0f;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
        private const int EXPTOLEVEL = 15000;
        private long totalNpcsKilled = 0;
        private bool activate = true;
        private float additionalDefense = 0;
        private float additionalMeleeDamage = 0;
        private float additionalMeleeAttackSpeed = 0;
        private float additionalLifeRegen = 0;
        private float lifeSteal = 0;
        private float stackedLifeSteal = 0;
        private float additionalMagicDamage = 0;
        private float additionalMagicAttackSpeed = 0;
        private float additionalRangedDamage = 0;
        private float additionalRangeAttackSpeed = 0;
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
        public Dictionary<string, object> SkillCost { get => skillCost; set => skillCost = value; }
        //dont need?
        public float AdditionalDefense { get => additionalDefense; set => additionalDefense = value; }
        public float AdditionalMeleeDamage { get => additionalMeleeDamage; set => additionalMeleeDamage = value; }
        public float AdditionalMeleeAttackSpeed { get => additionalMeleeAttackSpeed; set => additionalMeleeAttackSpeed = value; }
        public float AdditionalLifeRegen { get => additionalLifeRegen; set => additionalLifeRegen = value; }
        public float LifeSteal { get => lifeSteal; set => lifeSteal = value; }
        public float AdditionalMagicDamage { get => additionalMagicDamage; set => additionalMagicDamage = value; }
        public float AdditionalRangedDamage { get => additionalRangedDamage; set => additionalRangedDamage = value; }
        public float AdditionalRangeAttackSpeed { get => additionalRangeAttackSpeed; set => additionalRangeAttackSpeed = value; }
        public static string[] SkillOrder { get => skillOrder; set => skillOrder = value; }
        public float AdditionalMagicAttackSpeed { get => additionalMagicAttackSpeed; set => additionalMagicAttackSpeed = value; }
        public float AdditionalthrowingDamage { get => additionalthrowingDamage; set => additionalthrowingDamage = value; }
        public float AdditionalsummonDamage { get => additionalSummonDamage; set => additionalSummonDamage = value; }
        public float AdditionalsummonAttackSpeed { get => additionalSummonDamage; set => additionalSummonDamage = value; }
        public float AdditionalPickingPower { get => additionalPickingPower; set => additionalPickingPower = value; }
        public long TotalNpcsKilled { get => totalNpcsKilled; set => totalNpcsKilled = value; }

        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;
            showDamageText(CombatTextPos["currentLevels"], $"Level {level}", CombatText.DamagedFriendlyCrit);
            InfinitumUI.Instance.stats = this;
        }
        private void showDamageText(int yPos, string text, Color c, bool dramatic = false, bool dot = false)
        {
            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + yPos), 25, 25), c, text, dramatic, dot);
        }
        public override void Load()
        {
            base.Load();
            skillCost.Add("defense", new { baseCost = 10, incrementalCost = .1f });
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
            level = tag.GetInt("Level");
            expMultiplier = tag.GetFloat("ExpMultiplier");
            exp = tag.GetFloat("Exp");
            totalLevel = tag.GetInt("TotalLevel");
            recentChanged = true;
        }

        public override void SaveData(TagCompound tag)
        {
            tag.Add("Level", level);
            tag.Add("ExpMultiplier", expMultiplier);
            tag.Add("Exp", exp);
            tag.Add("TotalLevel", totalLevel);
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
                case "Maigc Attack Speed":
                    additionalMagicAttackSpeed += 1f;//dont work
                    break;
                case "Ranged Damage":
                    additionalRangedDamage += 1f;//dont Work
                    break;
                case "Ranged Consume Ammo":
                    break;
                case "Throwing  Damage":
                    additionalthrowingDamage += 1f;//dont Work
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
            Main.NewText(stat);
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
            player.GetAttackSpeed(DamageClass.Magic) = player.GetAttackSpeed(DamageClass.Magic) + additionalMagicAttackSpeed;
            player.GetDamage(DamageClass.Ranged) = player.GetDamage(DamageClass.Ranged) + additionalRangedDamage;

            //player.GetAttackSpeed(DamageClass.Ranged) = player.GetAttackSpeed(DamageClass.Ranged) + additionalRangeAttackSpeed;
            player.GetDamage(DamageClass.Throwing) = player.GetDamage(DamageClass.Throwing) + additionalthrowingDamage;
            //player.GetAttackSpeed(DamageClass.Throwing) = player.GetAttackSpeed(DamageClass.Throwing) + additionalthrowingAttackSpeed;
            player.GetDamage(DamageClass.Summon) = player.GetDamage(DamageClass.Summon) + additionalSummonDamage;
            player.GetAttackSpeed(DamageClass.Summon) = player.GetAttackSpeed(DamageClass.Summon) + additionalSummonAttackSpeed;
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
                getLifeSteal(damage);
                base.ModifyHitNPC(item, target, ref damage, ref knockback, ref crit);
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            getLifeSteal(damage);

            ChatMessage("");
            base.ModifyHitNPCWithProj(proj, target, ref damage, ref knockback, ref crit, ref hitDirection);

        }
        public override void OnConsumeAmmo(Item weapon, Item ammo)
        {
            base.OnConsumeAmmo(weapon, ammo);

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



    }

}