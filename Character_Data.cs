using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace Infinitum
{
	public class Character_Data : ModPlayer
	{
		private static float exp = 0.0f;
		private int level = 0;
		private static float expMultiplier = 1.0f;
		private const int expToLevel = 15000; 

		public static float Exp { get => exp; set => exp = value; }
        public int Level { get => level; set => level = value; }
		public static float ExpMultiplier { get => expMultiplier; set => expMultiplier = value; }

		public static void AddXp(float xp)
        {
			Player player = Main.player[Main.myPlayer];
			Exp +=  (float)(xp * ExpMultiplier);
			Main.NewText("Gained: "+ xp +" total: " +  Exp);
			
			CombatText.NewText(new Rectangle((int)player.position.X,((int)player.position.Y + 135),50,50),CombatText.HealMana,$"+ {xp} xp",false,false);
			
			//CombatText.UpdateCombatText();
			
        }

		public override void OnEnterWorld(Player player){
			//Main.NewText("pepipi", 150, 250, 150);
		}
		
		
		 
	}
}