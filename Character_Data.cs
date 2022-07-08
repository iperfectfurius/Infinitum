using Infinitum.UI;
using Microsoft.Xna.Framework;
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

        public void AddXp(float xp)
        {
            exp += (float)(xp * expMultiplier);
            UpdateLevel();
            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.HealMana, $"+ {((float)(xp * expMultiplier)).ToString("n1")} xp", false, false);



        }
        private void UpdateLevel()
        {
            if (exp < EXPTOLEVEL) return;

            int LevelsUp = (int)exp / EXPTOLEVEL;
            exp -= EXPTOLEVEL * LevelsUp;
            level += LevelsUp;
            totalLevel += LevelsUp;

            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 195), 25, 25), CombatText.DamagedFriendlyCrit, $"+ {LevelsUp} Levels!", false, false);
            Task.Delay(2000).ContinueWith(_ => { showDamageText($"Level {level}", Color.Red); });


        }
        public void AddXpMultiplier(float xp)
        {
            expMultiplier += xp;

            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.DamagedFriendlyCrit, $"{(expMultiplier * 100f).ToString("n2")}% Multiplier!", false, false);
        }
        public override void OnEnterWorld(Player currentPLayer)
        {
            player = currentPLayer;

            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.DamagedFriendlyCrit, $"Level {level}", false, false);
            InfinitumUI.Instance.stats = this;


        }

        public override void LoadData(TagCompound tag)
        {
            level = tag.GetInt("Level");
            expMultiplier = tag.GetFloat("ExpMultiplier");
            exp = tag.GetFloat("Exp");
            totalLevel = tag.GetInt("TotalLevel");
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

        private void showDamageText(string text, Color c)
        {
            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 195), 25, 25), CombatText.LifeRegen, text, false, false);
        }
        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            if (InfinitumModSystem.UIKey.JustPressed)
            {
                InfinitumUI.Instance.Visible = !InfinitumUI.Instance.Visible;
                showDamageText("hotKey", Color.Red);
            }
                
           

            base.ProcessTriggers(triggersSet);

        }
    }

}