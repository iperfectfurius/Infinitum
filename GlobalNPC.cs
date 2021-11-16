using Terraria;
using Terraria.ModLoader;
using Infinitum.Items;
using Terraria.GameContent.ItemDropRules;
namespace Infinitum
{
	public class InfinitumNPCs : GlobalNPC
	{
		public override bool CheckDead(NPC npc)
		{
			//Main.NewText("yes papu",155,145,132);
			
			
			 if(base.CheckDead(npc))
            {
			
				float calcExp = (float)npc.defense + 0.5f * (float)(npc.lifeMax/5);
				Character_Data.AddXp(calcExp);
				
				return true;
            }
			 return false;
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			// if(Main.rand.Next(2)==1){
 			// 	Item.NewItem(npc.getRect(), 22);
				 
			// }
			//npcLoot.Add();
			IItemDropRule expStar =  new CommonDrop(ModContent.ItemType<ExpStar>(),1);			
			npcLoot.Add(expStar);

			base.ModifyNPCLoot(npc, npcLoot);
			
			
		}
	}
}