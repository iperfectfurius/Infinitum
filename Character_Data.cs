using Infinitum.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
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
        private float exp = 0.0f;
        private int level = 0;
        private int totalLevel = 0;
        private float expMultiplier = 1.0f;
        private const int EXPTOLEVEL = 15000;
        private float additionalDefense = 0;
        private float additionalMeleeDamage = 0;
        private float additionalMeleeAttackSpeed = 0;
        private float additionalLifeRegen = 0;
        private float lifeSteal = 0;
        private float additionalMagicDamage = 0;
        private float additionalRangedDamage = 0;
        private float additionalRangeAttackSpeed = 0;

        public float Exp { get => exp; }
        public int Level { get => level; }
        public int TotalLevel { get => totalLevel; }
        public float ExpMultiplier { get => expMultiplier; }
        public int _EXPTOLEVEL => EXPTOLEVEL;
        public bool RecentChanged { get => recentChanged; set => recentChanged = value; }


        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;
            showDamageText(CombatTextPos["currentLevels"], $"Level {level}", CombatText.DamagedFriendlyCrit);
            InfinitumUI.Instance.stats = this;
        }
        private void showDamageText(int yPos, string text, Color c,bool dramatic = false,bool dot = false)
        {
            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + yPos), 25, 25), c, text, dramatic, dot);
        }
        public void AddXp(float xp)
        {
            exp += (float)(xp * expMultiplier);
            UpdateLevel();

            showDamageText(CombatTextPos["xp"], $"+ {((float)(xp * expMultiplier)):n1} XP", CombatText.HealMana);
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
                InfinitumUI.Instance.Visible = !InfinitumUI.Instance.Visible;
            }
            base.ProcessTriggers(triggersSet);

        }
        public override void Unload()
        {
            CombatTextPos = new();
            base.Unload();
        }
        private void ApplyStats()
        {

        }
    }

}