using Microsoft.Xna.Framework;
using Terraria.ID;
using static System.Net.Mime.MediaTypeNames;

namespace Infinitum.Buffs
{
    internal class InfinitumBuff : ModBuff
    {
        private float xpMultiplier = 0.0f;
        
        public float XPMultiplier { get => xpMultiplier; set => xpMultiplier = value; }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infinitum Buffed!");
            Description.SetDefault("More Global XP!");
            Main.debuff[Type] = false;
        }
        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
            tip = $"You collected {(xpMultiplier * 100):n1}% more Exp Multiplier!";
        }
        public override bool RightClick(int buffIndex)
        {
            return true;
        }
        public void UpdateFromKill(Player player)
        {
            if (Main.netMode == NetmodeID.Server) return;

            if(Main.netMode == NetmodeID.MultiplayerClient)
                player.buffTime[player.FindBuffIndex(this.Type)] += 12;
            else
                player.buffTime[player.FindBuffIndex(this.Type)] += 15;
            xpMultiplier += 0.0005f;

            int i = CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 50), 25, 25),Color.Green,"+0.25s");

            
        }
    }
}
