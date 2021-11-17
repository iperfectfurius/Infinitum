using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace Infinitum
{
	public class Character_Data : ModPlayer
	{
		private static Player player = Main.player[Main.myPlayer];
		private static float exp = 0.0f;
		private static int level = 0;
		private static float expMultiplier = 1.0f;
		private const int EXPTOLEVEL = 15000;

		public static float Exp { get => exp; set => exp = value; }
		public int Level { get => level; set => level = value; }
		public static float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }
		public static void AddXp(float xp)
		{
			Exp += (float)(xp * ExpMultiplier);
			CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.HealMana, $"+ {((float)(xp * ExpMultiplier)).ToString("n1")} xp", false, false);
			
			UpdateLevel();
			Main.NewText("Gained: " + ((float)(xp * ExpMultiplier)).ToString("n1") + " total: " + Exp);
		}
		private static void UpdateLevel()
		{
			if (Exp > EXPTOLEVEL)
			{
				int LevelsUp = (int)Exp / EXPTOLEVEL;
				Exp -= EXPTOLEVEL * LevelsUp;
				level += LevelsUp;
				CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 195), 25, 25), CombatText.DamagedFriendlyCrit, $"+ {LevelsUp} Levels!", false, false);
			}
		}
		public static void AddXpMultiplier(float xp)
		{
			ExpMultiplier += xp;
			CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.DamagedFriendlyCrit, $"{ExpMultiplier.ToString("n2")}% Multiplier!", false, false);
		}
		public override void OnEnterWorld(Player player)
		{
			CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.DamagedFriendlyCrit, $"{Level} Level!", false, false);
		}

		public override void LoadData(TagCompound tag)
		{

		}

		public static void SaveData()
		{

		}
	}
}