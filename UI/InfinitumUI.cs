﻿
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace Infinitum.UI
{
    internal class InfinitumUI : UIState
    {
        public static InfinitumUI Instance;
        public DragableUIPanel InfinitumPanel;
        public bool Visible;
        public Character_Data stats = null;
        private const float maxWidth = 550f;
        private const float maxHeigth = 180f;

        UIText[] statsTexts = new UIText[5];

        UIPanel[] StatsButtons =
        {

        };

        UIText[] skillsTexts = new UIText[12];
        private enum statsOrder : ushort
        {
            Level = 0,
            Exp=1,
            ExpMultiplier = 2,
            TotalLevel = 3           
        }

        public InfinitumUI()
        {         
            inicializeUIElements();
            
        }

        private void inicializeUIElements()
        {
            //need improve...
            float marginTop = 0;
            float marginLeft = 0;
            for (int i = 0; i < statsTexts.Length; i++)
            {
                UIText text = new UIText("Test", .9f);

                text.Top.Set(marginTop, 0f);
                text.Left.Set(marginLeft, 0f);
                statsTexts[i] = text;

                marginTop += 20f;
            }
            //0.26

            
                
            marginTop = 3;
            marginLeft = 0;


            //This goes in other panel
            for (int i = 0; i < skillsTexts.Length; i++)
            {
                UIText text = new($"Skill: ", .9f);

                text.Top.Set(marginTop, 0f);
                text.Left.Set(marginLeft, 0f);
                text.Height.Set(20f, 0);
                skillsTexts[i] = text;

                marginTop += 20f;
            }
        }

        public override void OnInitialize()
        {

            Visible = false;
            Instance = this;
            InfinitumPanel = new DragableUIPanel();
            InfinitumPanel.Height.Set(maxHeigth, 0f);
            InfinitumPanel.Width.Set(maxWidth, 0f);
            InfinitumPanel.Left.Set(Main.screenWidth - InfinitumPanel.Width.Pixels, 0f);
            InfinitumPanel.Top.Set(Main.screenHeight - InfinitumPanel.Height.Pixels, 0f);

            addUIElementsToPanel();
            
            

            Append(InfinitumPanel);
        }

        private void addUIElementsToPanel()
        {
            foreach (UIText text in statsTexts)
                InfinitumPanel.Append(text);

            UIPanel skillsPanel = new();
            skillsPanel.Top.Set(0, 0f);
            skillsPanel.Left.Set(190, 0f);
            skillsPanel.Height.Set(maxHeigth - 20, 0f);
            skillsPanel.Width.Set(maxWidth - 203, 0f);
            skillsPanel.OverflowHidden = true;

            UIScrollbar skillScrollbar = new();
            skillScrollbar.Top.Set(5, 0f);
            skillScrollbar.Height.Set(skillsPanel.Height.Pixels -40, 0f);
            skillScrollbar.Width.Set(22f, 0f);
            skillScrollbar.Left.Set(skillsPanel.Width.Pixels - 45f, 0f);

            skillsPanel.Append(skillScrollbar);

            foreach (UIText text in skillsTexts)
                skillsPanel.Append(text);

            UIButton defenseButtonAdd = new UIButton("+", addStat,"defense");
            defenseButtonAdd.Width.Set(22, 0);
            defenseButtonAdd.Height.Set(22, 0);
            defenseButtonAdd.Left.Set(160 + 5,0);
            defenseButtonAdd.Top.Set(-1, 0);
            defenseButtonAdd.OverflowHidden = false;
            skillsPanel.Append(defenseButtonAdd);


            InfinitumPanel.Append(skillsPanel);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {         
            base.Draw(spriteBatch);
        }
        public override void Update(GameTime gameTime)
        {
            //this goes on show??
            if (!Visible)
            {
                base.Update(gameTime);
                return;
            }

             if(stats.RecentChanged) UpdateAllStats();

            base.Update(gameTime);
        }
        private void test2(UIMouseEvent evt, UIElement listeningElement)
        {
            
        }
        private void addStat (UIMouseEvent evt, UIElement listeningElement)
        {
            //verfifyLevels();
            stats.ApplyStats("");
        }

        private void UpdateAllStats()
        {
            statsTexts[(int)statsOrder.Level].SetText("Level: " + stats.Level);
            statsTexts[(int)statsOrder.Exp].SetText($"Exp: {stats.Exp.ToString("n0")}/15000 ({((float)stats.Exp / stats._EXPTOLEVEL) * 100:n1}%)");
            statsTexts[(int)statsOrder.ExpMultiplier].SetText($"XP Multiplier: {stats.ExpMultiplier * 100:n1}%");
            statsTexts[(int)statsOrder.TotalLevel].SetText($"Total Level: {stats.TotalLevel}");
            //statsTexts[(int)statsOrder.Level].SetText("Level: " + stats.Level);
            //skillsTexts[0].SetText($"Additional defense: {stats.AdditionalDefense} { (dynamic)stats.SkillCost["defense"].GetType().GetProperty("baseCost").ToString()}");
            skillsTexts[0].SetText($"Additional defense: {stats.AdditionalDefense}");
            RecalculateChildren();
            stats.RecentChanged = false;
            //recalculate here
        }

    }
}
