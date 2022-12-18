using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            tip = "More XP Really?";
        }
        public override bool RightClick(int buffIndex)
        {
            return true;
        }
        public void UpdateFromKill(Player player)
        {
            player.buffTime[player.FindBuffIndex(this.Type)] += 15;
            xpMultiplier += 0.1f;
        }
    }
}
