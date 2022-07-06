using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
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
		private const int exp_TO_LEVEL = 15000;
		

		// public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		// {
		// 	base.OnHitNPC(item, target, damage, knockback, crit);
		// 	if(!target.activWe){
		// 		float calcExp = (float)target.defense + 0.5f * (float)(target.lifeMax / 5);
		// 		AddXp(calcExp);
		// 	}
		// }
		public void AddXp(float xp)
		{

			exp += (float)(xp * expMultiplier);
			UpdateLevel();
			CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 135), 25, 25), CombatText.HealMana, $"+ {((float)(xp * expMultiplier)).ToString("n1")} xp", false, false);		

			//Main.NewText("Gained: " + ((float)(xp * expMultiplier)).ToString("n1") + " total: " + exp.ToString("n1"));

		}
		private void UpdateLevel()
		{
			if (exp > exp_TO_LEVEL)
			{
				int LevelsUp = (int)exp / exp_TO_LEVEL;
				exp -= exp_TO_LEVEL * LevelsUp;
				level += LevelsUp;
				
				CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 195), 25, 25), CombatText.DamagedFriendlyCrit, $"+ {LevelsUp} Levels!", false, false);
			}
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
		}

		public override void LoadData(TagCompound tag)
		{
			//base.LoadData();
			level = tag.GetInt("Level");
			expMultiplier = tag.GetFloat("ExpMultiplier");
			exp = tag.GetFloat("Exp");
		}

		public override void SaveData(TagCompound tag)
		{
			tag.Add("Level", level);
			tag.Add("ExpMultiplier", expMultiplier);
			tag.Add("Exp", exp);

		}
	}

}