using Terraria;
using Terraria.ModLoader;
using Infinitum.Items;
using Terraria.GameContent.ItemDropRules;

namespace Infinitum
{
	public class InfinitumNPCs : GlobalNPC
	{
		// public override void OnKill(NPC npc)
		// {
		// 	base.OnKill(npc);
		// 	if (!CheckKill(Main.myPlayer)) return;

		// 	float calcExp = (float)npc.defense + 0.5f * (float)(npc.lifeMax / 5);

		// 	Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(calcExp);
		// }W
		public override void OnKill(NPC npc)
		{
			base.OnKill(npc);
			addXpToAllPlayerInRange((float)npc.defense + 0.5f * (float)(npc.lifeMax / 5));
		}
		public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
		{
			System.Collections.Generic.List<IItemDropRule> items = new System.Collections.Generic.List<IItemDropRule>();
			items.Add(new CommonDrop(ModContent.ItemType<ExpStar>(), 1500));
			items.Add(new CommonDrop(ModContent.ItemType<MultiplierStar>(), 250, 1, 3));

			items.ForEach(e => npcLoot.Add(e));
			base.ModifyNPCLoot(npc, npcLoot);

		}
		private void addXpToAllPlayerInRange(float xp)
		{
			foreach (Player p in Main.player)
			{
				if (!p.active) return;

				Main.NewText(Main.PlayerList.ToString());			
				p.GetModPlayer<Character_Data>().AddXp(xp);

			}
		}

	}
}