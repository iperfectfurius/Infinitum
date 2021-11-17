using Terraria;
using Terraria.ModLoader;
using Infinitum.Items;
using Infinitum;
using Terraria.GameContent.ItemDropRules;
namespace Infinitum
{
	public class InfinitumNPCs : GlobalNPC
	{
		public override bool CheckDead(NPC npc)
		{
			if (base.CheckDead(npc))
			{

				float calcExp = (float)npc.defense + 0.5f * (float)(npc.lifeMax / 5);
				Character_Data.AddXp(calcExp);
				return true;
			}
			return false;
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			System.Collections.Generic.List<IItemDropRule> items = new System.Collections.Generic.List<IItemDropRule>();
			items.Add(new CommonDrop(ModContent.ItemType<ExpStar>(), 1500));
			items.Add(new CommonDrop(ModContent.ItemType<MultiplierStar>(), 250));

			items.ForEach(e => npcLoot.Add(e));
			base.ModifyNPCLoot(npc, npcLoot);

		}
	}
}