﻿using Microsoft.Xna.Framework;
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
        public static Vector2 DefaultPos = new Vector2(Main.screenWidth - maxWidth - 50, 71);
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
            bar.BackgroundColor = new Color(63, 82, 151) * 0.5f;
            bar.BorderColor = new Color(0, 0, 0, 50);
            bar.SetPadding(0);
            bar.OnMouseOver += Bar_OnMouseOver;
            bar.OnMiddleClick += openStatsTab;
            //bar.OnMouseUp += setPos;
            bar.continueStuff = true;

            addUIElementsToPanel();
            //not consistent
            bar.Left.Set(DefaultPos.X, 0f);
            bar.Top.Set(DefaultPos.Y, 0f);

            Append(bar);
        }

        private void setPos(UIMouseEvent evt, UIElement listeningElement)
        {
        }

        private void Bar_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            //show exp
        }

        private void openStatsTab(UIMouseEvent evt, UIElement listeningElement)
        {
            InfinitumUI.Instance.Visible = !InfinitumUI.Instance.Visible;
        }

        private void addUIElementsToPanel()
        {
            ExpBar = new UIPanel();
            ExpBar.Width.Set(5, 0f);
            ExpBar.Height.Set(maxHeigth - 1, 0f);

            ExpBar.SetPadding(0);
            ExpBar.BackgroundColor = Color.Lime * 0.6f;
            ExpBar.BorderColor = new Color(0, 0, 0, 0);

            level = new UIText("0", 0.8f);
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
            if (stats == null) {
                base.Update(gameTime);
                return;
            }

            //this goes on show??
            if (stats.RecentChanged) UpdateAllStats();

            base.Update(gameTime);

        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            if (bar.IsMouseHovering)
                Main.hoverItemName = $"{stats.Exp:n1}/{stats.ExpToLevel} Exp";
        }
        private void UpdateAllStats()
        {
            float percentExp = (float)stats.Exp / stats.ExpToLevel;

            ExpBar.Width.Set(((percentExp * maxWidth) / 100) * 100, 0);
            level.SetText($"LV {stats.Level}  ({(percentExp * 100):n1}%)");
            RecalculateChildren();
        }

        public void ChangeOwnPos(Vector2 pos)
        {
            bar.Left.Pixels = pos.X;
            bar.Top.Pixels = pos.Y;
        }
        public Vector2 GetCurrentPos()
        {
            return new Vector2(bar.Left.Pixels,bar.Top.Pixels);
        }
    }
}