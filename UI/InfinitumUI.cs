
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;


namespace Infinitum.UI
{
    internal class InfinitumUI : UIState
    {
        public DragableUIPanel InfinitumPanel;
        public bool Visible;

        public override void OnInitialize()
        {
            Visible = true;

            InfinitumPanel = new DragableUIPanel();
            InfinitumPanel.Height.Set(100f, 0f);
            InfinitumPanel.Width.Set(300, 0f);
            InfinitumPanel.Left.Set(Main.screenWidth - InfinitumPanel.Width.Pixels, 0f);
            InfinitumPanel.Top.Set(Main.screenHeight- InfinitumPanel.Height.Pixels, 0f);
            InfinitumPanel.BackgroundColor = new Color(250, 0, 0,100);

            base.Append(InfinitumPanel);
        }


    }
}
