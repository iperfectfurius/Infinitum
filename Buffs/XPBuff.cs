using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infinitum.Buffs
{
    internal class XPBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("XP Buffed!");
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
       
    }
}
