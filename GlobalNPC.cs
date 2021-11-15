using Terraria;
using Terraria.ModLoader;

namespace Infinitum
{
	public class InfinitumNPCs : GlobalNPC
	{
		public override bool CheckDead(NPC npc)
		{
			Main.NewText("yes papu",155,145,132);
			Character_Data.AddXp(5);
			
			return base.CheckDead(npc);
		}
	}
}