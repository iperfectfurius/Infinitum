using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;
using Infinitum.Items;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Microsoft.Xna.Framework;

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
		// }
		public override void OnKill(NPC npc)
		{
			base.OnKill(npc);
			addXpToAllPlayerInRange((float)npc.defense + 0.5f * (float)(npc.lifeMax / 5));
			ChatMessage("xd");
		}
		public static void ChatMessage(string text)
		{
			
			if (Main.netMode == 2)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Red);
				
			}
			else if (Main.netMode == 0)
			{
				Main.NewText(text);
			}
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
				//if(Main.netMode == NetmodeID.Server)
				//Main.NewText(Main.PlayerList.ToString());		

				//NetMessage.SendData(1, -1, -1, NetworkText.FromLiteral("testing"));

				p.GetModPlayer<Character_Data>().AddXp(xp);


			}

		}

	}
}