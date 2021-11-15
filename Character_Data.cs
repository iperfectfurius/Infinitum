using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace Infinitum
{
	public class Character_Data : ModPlayer
	{
		private static double exp = 0;
		private int level = 0;

		public static double Exp { get => exp; set => exp = value; }
        public int Level { get => level; set => level = value; }
		
		public static void AddXp(int xp)
        {
			Exp += xp;
			Main.NewText(Exp);
        }

		public override void OnEnterWorld(Player player){
			Main.NewText("pepipi", 150, 250, 150);
		}
		
		
		 
	}
}