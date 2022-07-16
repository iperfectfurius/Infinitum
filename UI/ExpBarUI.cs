using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace Infinitum.UI
{
    internal class ExpBarUI : UIState
    {
        public static ExpBarUI Instance;
        public DragableUIPanel bar;
        public UIPanel ExpBar;
        public UIText level;
        public Character_Data stats = null;
        private const float maxWidth = 245f;
        private const float maxHeigth = 12f;
        private bool Visible = true;


        public ExpBarUI()
        {
            //inicializeUIElements();

        }
        public override void OnInitialize()
        {

            Visible = false;
            Instance = this;
            bar = new DragableUIPanel();
            bar.Height.Set(maxHeigth, 0f);
            bar.Width.Set(maxWidth, 0f);
            bar.BorderColor = new Color(0,0,0,50);
            bar.SetPadding(0);


            bar.Left.Set(Main.screenWidth - maxWidth -50, 0f);
            bar.Top.Set(71f, 0f);

            addUIElementsToPanel();



            Append(bar);
        }

        private void addUIElementsToPanel()
        {
            ExpBar = new UIPanel();
            ExpBar.Width.Set(5, 0f);
            ExpBar.Height.Set(maxHeigth-1, 0f);
            
            ExpBar.SetPadding(0);
            ExpBar.BackgroundColor = Color.Lime;
            ExpBar.BorderColor = new Color(0, 0, 0, 0);

            level = new UIText("0",0.8f);
            level.Left.Set(0, 0);
            level.VAlign = level.HAlign = 0.5f;

            level.SetPadding(0);
            level.TextColor = Color.Coral;


            bar.Append(ExpBar);
            bar.Append(level);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            //this goes on show??
            if (stats.RecentChanged) UpdateAllStats();

            base.Update(gameTime);
        }

        private void UpdateAllStats()
        {
            float percentExp = (float)stats.Exp / stats._EXPTOLEVEL;
            ExpBar.Width.Set((( percentExp * maxWidth) / 100) * 100,0);
            level.SetText($"{stats.Level}  ({(percentExp*100):n1}%)");
            RecalculateChildren();
        }

    }
}
