
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
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
        private Dictionary <string,float> map = new Dictionary<string,float>();

        public InfinitumUI()
        {
            map["Level"] = 1;
        }
        public override void OnInitialize()
        {
           
            Visible = true;

            InfinitumPanel = new DragableUIPanel();
            InfinitumPanel.Height.Set(100f, 0f);
            InfinitumPanel.Width.Set(300, 0f);
            InfinitumPanel.Left.Set(Main.screenWidth - InfinitumPanel.Width.Pixels, 0f);
            InfinitumPanel.Top.Set(Main.screenHeight- InfinitumPanel.Height.Pixels, 0f);
            InfinitumPanel.OnClick += new MouseEvent(test2);
            //InfinitumPanel.BackgroundColor = new Color(250, 0, 0,100);
            Append(InfinitumPanel);

            UIText text = new($"Total Level: {map["Level"]}");

            UIPanel button = new UIPanel();
            button.Width.Set(100, 0);
            button.Height.Set(50, 0);
            button.HAlign = 0.5f;
            button.Top.Set(25, 0);
            button.OnClick += new MouseEvent(test2);
            
            button.Append(text);
            InfinitumPanel.Append(button);
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void test2(UIMouseEvent evt, UIElement listeningElement)
        {
            map["Level"]+=1;
            Main.NewText(map["Level"]);
        }


    }
}
