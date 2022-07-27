using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
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
			Append(_uiPanel);

			_uiText = new UIText("");
			_uiText.VAlign = _uiText.HAlign = 0.5f;
			_uiPanel.Append(_uiText);

			_uiPanel.OnClick += _clickAction;
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
