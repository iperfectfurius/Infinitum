
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;


namespace Infinitum.UI
{
    internal class InfinitumUI : UIState
    {
        public static InfinitumUI Instance;
        public DragableUIPanel InfinitumPanel;
        public bool Visible;
        public Character_Data stats = null;
        private const float maxWidth = 570f;
        private const float maxHeigth = 200f;
        private UIButton reset;
        private UIButton activateStatsButton;
        private UIButton numbers;
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
            float marginTop = 8;
            float marginLeft = 8;
            for (int i = 0; i < statsTexts.Length; i++)
            {
                UIText text = new UIText("Test", .9f);

                text.Top.Set(marginTop, 0f);
                text.Left.Set(marginLeft, 0f);
                statsTexts[i] = text;


                marginTop += 20f;
            }
            //0.30

            reset = new UIButton("Reset Skills", restartProgress);
            reset.Top.Set(marginTop, 0f);
            reset.Left.Set(marginLeft, 0f);
            reset.Height.Set(20f, 0);
            reset.Width.Set(180, 0);
            reset.ChangeColor(new Color(205, 61, 61));

            numbers = new UIButton("Disable Numbers", activateNumbers);
            numbers.Top.Set(marginTop + 22, 0f);
            numbers.Left.Set(marginLeft, 0f);
            numbers.Height.Set(20f, 0);
            numbers.Width.Set(180, 0);
            numbers.ChangeColor(Color.Purple);

            activateStatsButton = new UIButton("Disable Stats", activateStats);
            activateStatsButton.Top.Set(marginTop + 44, 0f);
            activateStatsButton.Left.Set(marginLeft, 0f);
            activateStatsButton.Height.Set(20f, 0);
            activateStatsButton.Width.Set(180, 0);
            activateStatsButton.ChangeColor(Color.Pink);





            marginTop = 3;
            marginLeft = 0;

            UIText costText = new UIText("Cost");
            costText.Top.Set(marginTop - 20, 0f);
            costText.Left.Set(marginLeft + 283, 0f);
            costText.Height.Set(20f, 0);

            skillsElementsPanel.Add(costText);
            //This goes in other panel
            //Unify!
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
                UIButton sumStat = new UIButton("+", addStat);
                UIButton subStat = new UIButton("-", addStat);
                UIText cost = new UIText(Character_Data.SkillCost[i].ToString());


                sumStat.Top.Set(marginTop, 0f);
                sumStat.Left.Set(marginLeft, 0f);
                sumStat.Height.Set(18f, 0);
                sumStat.Width.Set(18f, 0);
                sumStat.OwnStat = Character_Data.SkillOrder[i];
                sumStat.OverflowHidden = false;


                subStat.Top.Set(marginTop, 0f);
                subStat.Left.Set(marginLeft + 25, 0f);
                subStat.Height.Set(18f, 0);
                subStat.Width.Set(18f, 0);
                subStat.OwnStat = Character_Data.SkillOrder[i];
                subStat.OverflowHidden = false;

                cost.Top.Set(marginTop, 0f);
                cost.Left.Set(marginLeft + 35, 0f);
                cost.Height.Set(20f, 0);
                cost.Width.Set(80, 0);
                cost.OverflowHidden = false;

                skillsElementsPanel.Add(sumStat);
                skillsElementsPanel.Add(subStat);
                skillsElementsPanel.Add(cost);

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
            InfinitumPanel.PaddingBottom = 0f;


            InfinitumPanel.Left.Set((Main.screenWidth - InfinitumPanel.Width.Pixels) - (Main.screenWidth / 2) + maxWidth / 2, 0f);
            InfinitumPanel.Top.Set(Main.screenHeight - InfinitumPanel.Height.Pixels - (Main.screenHeight / 2) + maxHeigth / 2, 0f);

            addUIElementsToPanel();



            Append(InfinitumPanel);
        }

        private void addUIElementsToPanel()
        {
            foreach (UIText text in statsTexts)
                InfinitumPanel.Append(text);

            InfinitumPanel.Append(reset);
            InfinitumPanel.Append(activateStatsButton);
            InfinitumPanel.Append(numbers);

            UIPanel skillsPanel = new();
            skillsPanel.Top.Set(0f, 0f);
            skillsPanel.Left.Set(210f, 0f);
            skillsPanel.Height.Set(maxHeigth, 0f);
            skillsPanel.Width.Set(maxWidth - 209, 0f);
            skillsPanel.PaddingRight = 0f;
            skillsPanel.OverflowHidden = true;
            skillsPanel.OnScrollWheel += ScrollWheelSkill;


            UIScrollbar skillScrollbar = new();
            skillScrollbar.Top.Set(5, 0f);
            skillScrollbar.Height.Set(skillsPanel.Height.Pixels - 40, 0f);
            skillScrollbar.Width.Set(22f, 0f);
            skillScrollbar.Left.Set(skillsPanel.Width.Pixels - 35f, 0f);

            skillsElementsPanel.SetScrollbar(skillScrollbar);

            foreach (UIElement el in skillsElementsPanel)
                skillsPanel.Append(el);

            skillsPanel.Append(skillScrollbar);


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
            UIButton me = (UIButton)listeningElement.Parent;
            stats.ApplyStats(me.OwnStat, me.Text == "+" ? true : false);
            SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot);
        }
        private void restartProgress(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.resetCurrentSkills();
            SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot);
        }
        private void activateStats(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.Activate = !stats.Activate;
            stats.RecentChanged = true;
            SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot);
        }
        private void activateNumbers(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.DisplayNumbers = !stats.DisplayNumbers;
            stats.RecentChanged = true;
            SoundEngine.PlaySound(SoundID.DD2_BallistaTowerShot);

        }
        private void UpdateAllStats()
        {
            statsTexts[(int)statsOrder.Level].SetText("Level: " + stats.Level);
            statsTexts[(int)statsOrder.Exp].SetText($"Exp: {stats.Exp.ToString("n0")}/15000 ({((float)stats.Exp / stats._EXPTOLEVEL) * 100:n1}%)");
            statsTexts[(int)statsOrder.ExpMultiplier].SetText($"XP Multiplier: {(stats.ExpMultiplier * stats.MoreExpMultiplier) * 100:n1}%");
            statsTexts[(int)statsOrder.TotalLevel].SetText($"Total Level: {stats.TotalLevel}");
            statsTexts[(int)statsOrder.TotalKills].SetText("Total Kills: " + stats.TotalNpcsKilled);

            activateStatsButton.Text = stats.Activate ? "Disable Stats" : "Enable Stats";
            numbers.Text = stats.DisplayNumbers ? "Disable Numbers" : "Enable Numbers";

            UIElement[] test = skillsElementsPanel._items.ToArray();

            for (int i = 0; i < test.Length; i++)
            {
                if (test[i].GetType() != typeof(UIText)) continue;

                int uniqueID = test[i].UniqueId;
                string message = ((UIText)test[i]).Text.Split(':')[0];
                switch (message)
                {
                    case "Defense":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: +{stats.AdditionalDefense}");
                        break;
                    case "Melee Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalMeleeDamage * 100:n2}%");
                        break;
                    case "Melee Attack Speed":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalMeleeAttackSpeed * 100:n2}%");
                        break;
                    case "Life Regen":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalLifeRegen * 100:n2}%");
                        break;
                    case "Life Steal":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.LifeSteal * 100:n2}%");
                        break;
                    case "Magic Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalMagicDamage * 100:n2}%");
                        break;
                    case "Mana Consumption":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: -{stats.ReducedManaConsumption * 100:n2}%");
                        break;
                    case "Ranged Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalRangedDamage * 100:n2}%");
                        break;
                    case "Ammo Consumption":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AmmoConsumedReduction - 101}%");
                        break;
                    case "Movement Speed":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {(stats.AdditionalMovementSpeed * 100):n2}%");
                        break;
                    case "Global Critical Chance":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: +{stats.AdditionalGlobalCriticalChance}%");
                        break;
                    case "Summon Damage":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalsummonDamage * 100:n2}%");
                        break;
                    case "Minion Capacity":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalSummonCapacity}");
                        break;
                    case "Pickaxe Power":
                        ((UIText)skillsElementsPanel._items.Find(x => x.UniqueId == uniqueID)).SetText($"{message}: {stats.AdditionalPickingPower * 100:n2}%");
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
