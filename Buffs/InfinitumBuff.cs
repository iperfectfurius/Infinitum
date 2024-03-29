﻿using Microsoft.Xna.Framework;
using Terraria.ID;

namespace Infinitum.Buffs
{
    internal class InfinitumBuff : ModBuff
    {
        private float xpMultiplier = 0.0f;
        
        public float XPMultiplier { get => xpMultiplier; set => xpMultiplier = value; }
        public override void SetStaticDefaults()
        {
            Main.debuff[Type] = false;
        }
        public override void ModifyBuffText(ref string buffName, ref string tip, ref int rare)
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

            int addedTime = 15;
            if(Main.netMode == NetmodeID.MultiplayerClient)
                addedTime -= 3;

            player.buffTime[player.FindBuffIndex(this.Type)] += addedTime;
            xpMultiplier += 0.0005f;

            CombatText.NewText(new Rectangle((int)player.position.X, ((int)player.position.Y + 50), 25, 25),Color.Green,$"+{(float)(addedTime/60f):n2}s");         
        }
    }
}
