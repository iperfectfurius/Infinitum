using Terraria.ModLoader;
using Terraria;
using System.Threading.Tasks;
using System.IO;

namespace Infinitum
{
	public class Infinitum : Mod
	{

		public Infinitum()
		{

		}
		public override void Load()
		{
			base.Load();
			
		}
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            base.HandlePacket(reader, whoAmI);
			AddXPToPlayer(reader.ReadSingle());
			//Main.NewText(reader.ReadString() + whoAmI + "desde main");
		}
		private void AddXPToPlayer(float xp)
        {
			Main.player[Main.myPlayer].GetModPlayer<Character_Data>().AddXp(xp);
        }
    }
}