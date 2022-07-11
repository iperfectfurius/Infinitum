
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
        private const float maxWidth = 500f;
        private const float maxHeigth = 200f;

        UIText[] statsTexts = new UIText[5];

        UIList skillsElementsPanel = new();
        private enum statsOrder : ushort
        {
            Level = 0,
            Exp = 1,
            ExpMultiplier = 2,
            TotalLevel = 3,
            TotalKills = 4
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
            for (int i = 0; i < 14; i++)
            {
                UIText text = new(Character_Data.SkillOrder[i] + ": 0", .9f);

                text.Top.Set(marginTop, 0f);
                text.Left.Set(marginLeft, 0f);
                text.Height.Set(20f, 0);

                skillsElementsPanel.Add(text);

                marginTop += 20f;
            }

            marginTop = 0;
            marginLeft = 225;

            for (int i = 0; i < 14; i++)
            {
                UIButton button = new UIButton("+", addStat);

                button.Top.Set(marginTop, 0f);
                button.Left.Set(marginLeft, 0f);
                button.Height.Set(20f, 0);
                button.Width.Set(20, 0);
                button.OwnStat = Character_Data.SkillOrder[i];
                button.OverflowHidden = false;

                skillsElementsPanel.Add(button);

                marginTop += 20f;
            }

            skillsElementsPanel.SetPadding(0);
        }

        public override void OnInitialize()
        {

            Visible = false;
            Instance = this;
            InfinitumPanel = new DragableUIPanel();
            InfinitumPanel.Height.Set(maxHeigth, 0f);
            InfinitumPanel.Width.Set(maxWidth, 0f);
            InfinitumPanel.Left.Set((Main.screenWidth - InfinitumPanel.Width.Pixels) - (Main.screenWidth/2) + maxWidth/2, 0f);
            InfinitumPanel.Top.Set(Main.screenHeight - InfinitumPanel.Height.Pixels - (Main.screenHeight / 2) + maxHeigth/2, 0f);

            addUIElementsToPanel();



            Append(InfinitumPanel);
        }

        private void addUIElementsToPanel()
        {
            foreach (UIText text in statsTexts)
                InfinitumPanel.Append(text);

            UIPanel skillsPanel = new();
            skillsPanel.Top.Set(-12f, 0f);
            skillsPanel.Left.Set(190, 0f);
            skillsPanel.Height.Set(200, 0f);
            skillsPanel.Width.Set(maxWidth - 203, 0f);
            skillsPanel.OverflowHidden = true;
            skillsPanel.OnScrollWheel += ScrollWheelSkill;


            UIScrollbar skillScrollbar = new();
            skillScrollbar.Top.Set(5, 0f);
            skillScrollbar.Height.Set(skillsPanel.Height.Pixels - 40, 0f);
            skillScrollbar.Width.Set(22f, 0f);
            skillScrollbar.Left.Set(skillsPanel.Width.Pixels - 45f, 0f);
            //skillScrollbar.OnMouseDown += test2;

            skillsElementsPanel.SetScrollbar(skillScrollbar);

            foreach (UIElement el in skillsElementsPanel)
                skillsPanel.Append(el);

            skillsPanel.Append(skillScrollbar);
            //skillsTexts.SetScrollbar(skillScrollbar);

            InfinitumPanel.Append(skillsPanel);
        }

        private void ScrollWheelSkill(UIScrollWheelEvent evt, UIElement listeningElement)
        {

            //provisional
            foreach (UIElement uiel in skillsElementsPanel)
                uiel.Top.Set(uiel.Top.Pixels + (evt.ScrollWheelValue < 0 ? -40 : 40), 0);

            Recalculate();

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

            if (stats.RecentChanged) UpdateAllStats();

            base.Update(gameTime);
        }
        private void addStat(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.ApplyStats(((UIButton)listeningElement.Parent).OwnStat);

        }

        private void UpdateAllStats()
        {
            statsTexts[(int)statsOrder.Level].SetText("Level: " + stats.Level);
            statsTexts[(int)statsOrder.Exp].SetText($"Exp: {stats.Exp.ToString("n0")}/15000 ({((float)stats.Exp / stats._EXPTOLEVEL) * 100:n1}%)");
            statsTexts[(int)statsOrder.ExpMultiplier].SetText($"XP Multiplier: {stats.ExpMultiplier * 100:n1}%");
            statsTexts[(int)statsOrder.TotalLevel].SetText($"Total Level: {stats.TotalLevel}");
            statsTexts[(int)statsOrder.TotalKills].SetText("Total Kills: " + stats.TotalNpcsKilled);
            //skillsTexts[0].SetText($"Additional defense: {stats.AdditionalDefense} { (dynamic)stats.SkillCost["defense"].GetType().GetProperty("baseCost").ToString()}");
            //(UIText)skillsTexts.GetEnumerator().Current.
            //skillsTexts[0].SetText($"Additional defense: {stats.AdditionalDefense}");

            UIElement[] test = skillsElementsPanel._items.ToArray();
            for (int i = 0; i < test.Length; i++)
            {
                if (test[i].GetType() != typeof(UIText)) continue;

                int uniqueID = test[i].UniqueId;
                string message = ((UIText)test[i]).Text.Split(':')[0];
                switch (message)
                {
                    case "Defense":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[0]}: +{stats.AdditionalDefense}");
                        break;
                    case "Melee Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[1]}: {stats.AdditionalMeleeDamage*100:n2}%");               
                        break;
                    case "Melee Attack Speed":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[2]}: {stats.AdditionalMeleeAttackSpeed*100:n2}%");
                        break;
                    case "Life Regen":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[3]}: {stats.AdditionalLifeRegen*100:n2}%");
                        break;
                    case "Life Steal":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[4]}: {stats.LifeSteal*100:n2}%");
                        break;
                    case "Magic Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[5]}: {stats.AdditionalMagicDamage*100:n2}%");
                        break;
                    case "Magic Attack Speed":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[6]}: {stats.AdditionalMagicAttackSpeed*100:n2}%");
                        break;
                    case "Ranged Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[7]}: {stats.AdditionalRangedDamage*100:n2}%");
                        break;
                    case "Ranged Consume Ammo":
                        //((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[i]}: {stats.AdditionalLifeRegen:n2}");
                        break;
                    case "Throwing  Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[9]}: {stats.AdditionalthrowingDamage*100:n2}%");
                        break;
                    case "Throwing algo?":
                        //((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[i]}: {stats.AdditionalLifeRegen:n2}");
                        break;
                    case "Summon Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[11]}: {stats.AdditionalsummonDamage*100:n2}%");
                        break;
                    case "Summon Attack Speed":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[12]}: {stats.AdditionalsummonAttackSpeed*100:n2}%");
                        break;
                    case "Pickaxe Power":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{Character_Data.SkillOrder[13]}: {stats.AdditionalPickingPower * 100:n2}%");
                        break;
                    default:
                        break;
                }
               // Main.NewText(uniqueID);

            }

        RecalculateChildren();
            stats.RecentChanged = false;

            //recalculate here
        }

    }
}
