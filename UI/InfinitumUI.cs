
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
        //public UIMoneyDisplay MoneyDisplay;
        //public UIHoverImageButton ExampleButton;
        public bool Visible;
        public override void OnInitialize()
        {
            Visible = true;

            UIPanel parent = new UIPanel();
            parent.Height.Set(100f, 0f);
            parent.Width.Set(300, 0f);
            parent.Left.Set(Main.screenWidth - parent.Width.Pixels, 0f);
            parent.Top.Set(Main.screenHeight-100f, 0f);
            parent.BackgroundColor = new Color(255, 255, 255);

            base.Append(parent);
        }


    }
}
