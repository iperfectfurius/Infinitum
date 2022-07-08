using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;
using Infinitum.Items;
using Terraria.GameContent.ItemDropRules;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Infinitum;

namespace Infinitum
{
	public class InfinitumNPCs : GlobalNPC
	{
		private static Mod myMod = ModLoader.GetMod("Infinitum");
		private float GetXpFromNPC(NPC target) => (float)target.defense + 0.5f * (float)(target.lifeMax / 4.5);
		public override void OnKill(NPC npc)
		{

			base.OnKill(npc);
			float xp = GetXpFromNPC(npc);

			if (Main.netMode == NetmodeID.Server)
			{
				ModPacket myPacket = myMod.GetPacket();
				myPacket.Write(xp);
				myPacket.Send();
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
			{
				addXpToPlayer(xp);
			}
		}
		public static void ChatMessage(string text)
		{
			
			if (Main.netMode == NetmodeID.Server)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Red);
			}
			else if (Main.netMode == NetmodeID.SinglePlayer)
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
		private void addXpToPlayer(float xp)
		{
			Main.CurrentPlayer.GetModPlayer<Character_Data>().AddXp(xp);
		}

	}
}