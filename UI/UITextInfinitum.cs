using Terraria.GameContent.UI.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Localization;
using Microsoft.Xna.Framework.Graphics;

namespace Infinitum.UI
{
    internal class UITextInfinitum : UIText
    {
        public string hoverText;
        public UITextInfinitum(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
        }

        public UITextInfinitum(LocalizedText text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            if(base.IsMouseHovering && !string.IsNullOrEmpty(hoverText))
                Main.hoverItemName = hoverText;
            base.Draw(spriteBatch);
        }
    }
}
