using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace Infinitum.UI
{
    internal class UIButton : UIElement
    {
        private object _text;
        private UIElement.MouseEvent _clickAction;
        private UIPanel _uiPanel;
        private UIText _uiText;
		private int ownStat;
		private Color color;
		public bool changeOnMouse = true;
        public string Text
        {
            get => _uiText?.Text ?? string.Empty;
            set => _text = value;
        }
        public int OwnStat { get => ownStat; set => ownStat = value; }

        public UIButton(object text, UIElement.MouseEvent clickAction) : base()
		{ 
			_text = text?.ToString() ?? string.Empty;
			_clickAction = clickAction;
		
		}
		
		
		public override void OnInitialize()
		{
			_uiPanel = new UIPanel(); 
			_uiPanel.Width = StyleDimension.Fill; 
			_uiPanel.Height = StyleDimension.Fill;
			_uiPanel.BackgroundColor = color;
            _uiPanel.OnMouseOver += _uiPanel_OnMouseOver;
            _uiPanel.OnMouseOut += _uiPanel_OnMouseOut;
			Append(_uiPanel);

			_uiText = new UIText("");
			_uiText.VAlign = _uiText.HAlign = 0.5f;
			_uiPanel.Append(_uiText);

			_uiPanel.OnClick += _clickAction;
		}

        private void _uiPanel_OnMouseOut(UIMouseEvent evt, UIElement listeningElement)
        {
			if(changeOnMouse)
				_uiPanel.BackgroundColor = new Color(63, 82, 151) * 0.7f;		
		}

        private void _uiPanel_OnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
			if (changeOnMouse)
				_uiPanel.BackgroundColor = new Color(133,151,219) * 0.8f;

			SoundEngine.PlaySound(SoundID.MenuTick);
		}


        public override void Update(GameTime gameTime)
		{
			if (_text != null)
			{ 
				_uiText.SetText(_text.ToString());
				_text = null;
				Recalculate(); 
				base.MinWidth = _uiText.MinWidth; 
				base.MinHeight = _uiText.MinHeight; 
			}
		}
		public void ChangeColor(Color c)
        {
			this.color = c;
        }
		
       
    }
}
