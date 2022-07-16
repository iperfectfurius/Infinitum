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
        public UIPanel bar;
        public UIPanel ExpBar;
        public UIText level;
        public Character_Data stats = null;
        private const float maxWidth = 350f;
        private const float maxHeigth = 14f;
        private bool Visible = true;


        public ExpBarUI()
        {
            //inicializeUIElements();

        }
        public override void OnInitialize()
        {

            Visible = false;
            Instance = this;
            bar = new UIPanel();
            bar.Height.Set(maxHeigth, 0f);
            bar.Width.Set(maxWidth, 0f);
            bar.BorderColor = Color.Coral;
            bar.SetPadding(0);


            bar.Left.Set((Main.screenWidth - bar.Width.Pixels) - (Main.screenWidth / 2) + maxWidth / 2, 0f);
            bar.Top.Set(Main.screenHeight - bar.Height.Pixels - (Main.screenHeight / 2) + maxHeigth / 2, 0f);

            addUIElementsToPanel();



            Append(bar);
        }

        private void addUIElementsToPanel()
        {
            ExpBar = new UIPanel();
            ExpBar.Width.Set(5, 0f);
            ExpBar.Height.Set(maxHeigth-1, 0f);
            
            ExpBar.SetPadding(0);
            ExpBar.BackgroundColor = Color.Green;
            ExpBar.BorderColor = new Color(0, 0, 0, 0);

            level = new UIText("0",0.8f);
            level.Left.Set(0, 0);
            level.VAlign = level.HAlign = 0.5f;

            level.SetPadding(0);
            level.TextColor = Color.Red;


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
            ExpBar.Width.Set(((((float)stats.Exp / stats._EXPTOLEVEL) * maxWidth) / 100) * 100,0);
            level.SetText(stats.Level.ToString());
            RecalculateChildren();
        }

    }
}
