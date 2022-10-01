
using IL.Terraria.Chat;
using Infinitum.Skills;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;
using Terraria.Chat;


namespace Infinitum.UI
{
    internal class InfinitumUI : UIState
    {
        public static InfinitumUI Instance;
        public DragableUIPanel InfinitumPanel;
        public bool Visible;
        public Character_Data stats = null;
        private const float maxWidth = 625f;
        private const float maxHeigth = 222f;
        private UIButton reset;
        private UIButton activateStatsButton;
        private UIButton numbers;
        private UIButton[] SetsButtons = new UIButton[Enum.GetNames(typeof(UIElementsEnum.ButtonsSets)).Length];
        UIText[] statsTexts = new UIText[6];
        UIList skillsElementsPanel = new();

        private enum statsOrder : ushort
        {
            Level,
            Exp,
            ExpMultiplier,
            TotalLevel,
            TotalKills,
            AverageXP
        }
        public InfinitumUI()
        {

            inicializeUIElements();
        }
        public override void OnInitialize()
        {
            Visible = false;
            Instance = this;
            InfinitumPanel = new DragableUIPanel();
            InfinitumPanel.Height.Set(maxHeigth, 0f);
            InfinitumPanel.Width.Set(maxWidth, 0f);
            InfinitumPanel.PaddingBottom = 0f;

            skillsElementsPanel.ListPadding = 0;
            skillsElementsPanel.Width.Set(maxWidth, 0f);

            InfinitumPanel.Left.Set((Main.screenWidth - InfinitumPanel.Width.Pixels) - (Main.screenWidth / 2) + maxWidth / 2, 0f);
            InfinitumPanel.Top.Set(Main.screenHeight - InfinitumPanel.Height.Pixels - (Main.screenHeight / 2) + maxHeigth / 2, 0f);

            addUIElementsToPanel();
            Append(InfinitumPanel);
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

            UIButton buttonChangeSet = new UIButton("Change Set (0)", ApplySet);
            buttonChangeSet.Top.Set(marginTop, 0f);
            buttonChangeSet.Left.Set(marginLeft, 0f);
            buttonChangeSet.Height.Set(20f, 0);
            buttonChangeSet.Width.Set(135, 0);
            buttonChangeSet.ChangeColor(Color.Green);
            buttonChangeSet.OwnStat = (int)UIElementsEnum.SetsActions.ChangeSet;
            buttonChangeSet.changeOnMouse = false;

            UIButton buttonAddSet = new UIButton("+", ApplySet);

            buttonAddSet.Top.Set(marginTop, 0f);
            buttonAddSet.Left.Set(buttonChangeSet.Width.Pixels + 12f, 0);
            buttonAddSet.Height.Set(18f, 0);
            buttonAddSet.Width.Set(18f, 0);
            buttonAddSet.ChangeColor(Color.Green);
            buttonAddSet.OwnStat = (int)UIElementsEnum.SetsActions.AddSet;
            buttonAddSet.changeOnMouse = false;

            UIButton buttonDelSet = new UIButton("-", ApplySet);

            buttonDelSet.Top.Set(marginTop, 0f);
            buttonDelSet.Left.Set(buttonChangeSet.Width.Pixels + 34f, 0);
            buttonDelSet.Height.Set(18f, 0);
            buttonDelSet.Width.Set(18f, 0);
            buttonDelSet.ChangeColor(Color.Red);
            buttonDelSet.OwnStat = (int)UIElementsEnum.SetsActions.DeleteSet;
            buttonDelSet.changeOnMouse = false;

            SetsButtons[(int)UIElementsEnum.ButtonsSets.ButtonChangeSet] = buttonChangeSet;
            SetsButtons[(int)UIElementsEnum.ButtonsSets.ButtonAddSet] = buttonAddSet;
            SetsButtons[(int)UIElementsEnum.ButtonsSets.ButtonDeleteSet] = buttonDelSet;

            marginTop += 22f;
            //refactor with 1 method?
            reset = new UIButton("Reset Skills", restartProgress);
            reset.Top.Set(marginTop, 0f);
            reset.Left.Set(marginLeft, 0f);
            reset.Height.Set(20f, 0);
            reset.Width.Set(180, 0);
            reset.ChangeColor(new Color(205, 61, 61));
            reset.changeOnMouse = false;

            numbers = new UIButton("Disable Numbers", activateNumbers);
            numbers.Top.Set(marginTop + 22, 0f);
            numbers.Left.Set(marginLeft, 0f);
            numbers.Height.Set(20f, 0);
            numbers.Width.Set(180, 0);
            numbers.ChangeColor(Color.Purple);
            numbers.changeOnMouse = false;

            activateStatsButton = new UIButton("Disable Stats", activateStats);
            activateStatsButton.Top.Set(marginTop + 44, 0f);
            activateStatsButton.Left.Set(marginLeft, 0f);
            activateStatsButton.Height.Set(20f, 0);
            activateStatsButton.Width.Set(180, 0);
            activateStatsButton.ChangeColor(Color.Pink);
            activateStatsButton.changeOnMouse = false;

            marginTop = 3;
            marginLeft = 0;

            skillsElementsPanel.Width.Set(maxWidth - 25, 0);
            skillsElementsPanel.Height.Set(maxHeigth, 0);

            UIText costText = new UIText("Cost");
            costText.Top.Set(marginTop - 20, 0f);
            costText.Left.Set(marginLeft + 334, 0f);
            costText.Height.Set(20f, 0);

            skillsElementsPanel.Add(costText);

            marginTop = 0;
            marginLeft = 0;

            for (int i = 0; i < SkillEnums.GetNumberOfSkills; i++)
            {
                UIList skill = new();

                UITextInfinitum text = new("test" + ": 0", .9f);
                UIButton sumStat = new UIButton("+", ModifyStat);
                UIButton subStat = new UIButton("-", ModifyStat);
                UIButton allStat = new UIButton("All", ModifyStat);
                UIButton automatic = new("×", ModifyStat);

                UIText cost = new UIText("test");

                skill.Height.Set(20f, 0);
                skill.Width.Set(maxWidth - 15, 0);

                text.Left.Set(0, 0f);
                text.Height.Set(20f, 0);

                marginLeft = 225;

                sumStat.Left.Set(marginLeft, 0f);
                sumStat.Height.Set(18f, 0);
                sumStat.Width.Set(18f, 0);
                sumStat.MaxHeight.Set(18f, 0);
                sumStat.MaxWidth.Set(18f, 0);
                sumStat.OwnStat = i;
                sumStat.OverflowHidden = false;

                subStat.Left.Set(marginLeft + 25, 0f);
                subStat.Height.Set(18f, 0);
                subStat.Width.Set(18f, 0);
                subStat.MaxHeight.Set(18f, 0);
                subStat.MaxWidth.Set(18f, 0);
                subStat.OwnStat = i;
                subStat.OverflowHidden = false;

                allStat.Left.Set(marginLeft + 48, 0f);
                allStat.Height.Set(18f, 0);
                allStat.Width.Set(30f, 0);
                allStat.MaxHeight.Set(18f, 0);
                allStat.MaxWidth.Set(30f, 0);
                allStat.OwnStat = i;
                allStat.OverflowHidden = false;
                allStat.hoverText = "Spend All Levels.";

                cost.Left.Set(marginLeft + 110, 0f);
                cost.Height.Set(20f, 0);
                cost.Width.Set(35, 0);
                cost.MaxHeight.Set(20f, 0);
                cost.MaxWidth.Set(35f, 0);
                cost.OverflowHidden = false;
            
                automatic.Left.Set(marginLeft + 82, 0f);
                automatic.Height.Set(18f, 0);
                automatic.Width.Set(18f, 0);
                automatic.OwnStat = i;
                automatic.OverflowHidden = false;
                automatic.ChangeColor(new Color(229, 38, 0) * 0.7f);
                automatic.changeOnMouse = false;
                automatic.hoverText = "Enable/Disable Automatic leveling";

                skill.Append(text);
                skill.Append(sumStat);
                skill.Append(subStat);
                skill.Append(allStat);
                skill.Append(cost);
                skill.Append(automatic);

                skillsElementsPanel.Add(skill);

                marginTop += 20f;
            }           
        }

        private void addUIElementsToPanel()
        {
            UIImageButton close = new(ModContent.Request<Texture2D>("Infinitum/UI/Textures/close"));
            close.Top.Set(0f, 0f);
            close.Left.Set(maxWidth - 25, 0f);
            close.Height.Set(22, 0);
            close.Width.Set(22, 0);
            close.OnClick += (e, i) => Visible = false;

            foreach (UIButton button in SetsButtons)
                InfinitumPanel.Append(button);

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

            UIScrollbar skillScrollbar = new();
            skillScrollbar.Top.Set(5, 0f);
            skillScrollbar.Height.Set(skillsPanel.Height.Pixels - 40, 0f);
            skillScrollbar.Width.Set(22f, 0f);
            skillScrollbar.Left.Set(skillsPanel.Width.Pixels - 35f, 0f);

            skillsElementsPanel.Append(skillScrollbar);
            skillsElementsPanel.SetScrollbar(skillScrollbar);

            skillsPanel.Append(skillsElementsPanel);

            InfinitumPanel.Append(skillsPanel);
            InfinitumPanel.Append(close);
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

            if (stats.RecentChanged)
            {
                try
                {
                    UpdateAllStats();
                }
                catch
                {
                    Infinitum.instance.ChatMessage("[Infinitum] Error on UI", Color.Red);
                    
                }
                stats.RecentChanged = false;
            }

            base.Update(gameTime);
        }
        private void ModifyStat(UIMouseEvent evt, UIElement listeningElement)
        {
            UIButton me = (UIButton)listeningElement.Parent;
            switch (me.Text)
            {
                case "+":
                    if (stats.ApplyStats(me.OwnStat, SkillEnums.Actions.LevelUp))
                    {
                        SoundEngine.PlaySound(SoundID.AchievementComplete);
                        //((UIButton)listeningElement.Parent).hoverText = $"Level: {stats.Skills[me.OwnStat].Level}";
                    }
                    break;
                case "-":
                    if (stats.ApplyStats(me.OwnStat, SkillEnums.Actions.LevelDown)) SoundEngine.PlaySound(SoundID.AchievementComplete);
                    break;
                case "All":
                    if (stats.ApplyStats(me.OwnStat, SkillEnums.Actions.LevelUpAll)) SoundEngine.PlaySound(SoundID.AchievementComplete);
                    break;
                case "×":
                    stats.Skills[me.OwnStat].AutomaticMode = true;
                    stats.RecentChanged = true;
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    break;
                case "✓":
                    stats.Skills[me.OwnStat].AutomaticMode = false;
                    stats.RecentChanged = true;
                    SoundEngine.PlaySound(SoundID.AchievementComplete);
                    break;
                default:
                    break;
            }
        }
        private void restartProgress(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.ResetCurrentSkills();
            SoundEngine.PlaySound(SoundID.Camera);
        }

        private void activateStats(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.Activate = !stats.Activate;
            stats.RecentChanged = true;
            SoundEngine.PlaySound(SoundID.ChesterOpen);
        }
        private void activateNumbers(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.DisplayNumbers = !stats.DisplayNumbers;
            stats.RecentChanged = true;
            SoundEngine.PlaySound(SoundID.AchievementComplete);
        }

        private void ApplySet(UIMouseEvent evt, UIElement listeningElement)
        {
            stats.SetActions((UIElementsEnum.SetsActions)Enum.Parse(typeof(UIElementsEnum.SetsActions), ((UIButton)listeningElement.Parent).OwnStat.ToString()));
            SoundEngine.PlaySound(SoundID.ChesterOpen);
        }

        private void UpdateAllStats()
        {
            List<UIElement> skills = skillsElementsPanel._items;

            statsTexts[(int)statsOrder.Level].SetText("Level: " + stats.Level);
            statsTexts[(int)statsOrder.Exp].SetText($"Exp: {stats.Exp.ToString("n0")}/{stats.ExpToLevel} ({((float)stats.Exp / stats.ExpToLevel) * 100:n1}%)");
            statsTexts[(int)statsOrder.ExpMultiplier].SetText($"XP Multiplier: {(stats.ExpMultiplier * stats.MoreExpMultiplier) * 100:n1}%");
            statsTexts[(int)statsOrder.TotalLevel].SetText($"Total Level: {stats.TotalLevel}");
            statsTexts[(int)statsOrder.TotalKills].SetText("Total Kills: " + stats.TotalNpcsKilled);
            statsTexts[(int)statsOrder.AverageXP].SetText($"Average XP: {stats.getAvgXP():n2}");
            activateStatsButton.Text = stats.Activate ? "Disable Stats" : "Enable Stats";
            numbers.Text = stats.DisplayNumbers ? "Disable Numbers" : "Enable Numbers";

            SetsButtons[(int)UIElementsEnum.ButtonsSets.ButtonChangeSet].Text = $"Change Set ({stats.SetSelected})";
            SetsButtons[(int)UIElementsEnum.ButtonsSets.ButtonChangeSet].hoverText = $"Set {stats.SetSelected} of {stats.SetCount - 1}";

            for (int i = 1; i < SkillEnums.GetNumberOfSkills + 1; i++)
            {
                int skillNumber = i - 1;
                UIList skill = (UIList)skills.ElementAt(i);

                UITextInfinitum skillText = (UITextInfinitum)skill.Children.ElementAt(1);
                UIText costText = (UIText)skill.Children.ElementAt(5);
                UIButton automaticButton = (UIButton)skill.Children.ElementAt(6);

                skillText.SetText($"{stats.Skills[skillNumber].DisplayName}: {stats.Skills[skillNumber].GetStatText()}");
                skillText.hoverText = $"Level: {stats.Skills[skillNumber].Level}";

                costText.SetText($"{stats.Skills[skillNumber].Cost}");

                automaticButton.ChangeBackgroundFromValue(stats.Skills[skillNumber].AutomaticMode);
            }          
        }
    }
}
