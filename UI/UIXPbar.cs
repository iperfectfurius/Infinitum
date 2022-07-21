using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace Infinitum.UI
{
    class UIXPBar : UIElement
    {

        private int _cornerSize = 2;

        private int _barSize = 0;

        private Asset<Texture2D> _borderTexture;

        private Asset<Texture2D> _backgroundTexture;

        public Color BorderColor = Color.Black;

        public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;


        public UIXPBar()
        {
            if (_backgroundTexture == null)
                _backgroundTexture = Main.Assets.Request<Texture2D>("Images/UI/PanelBackground");
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height +8);
            spriteBatch.Draw((Texture2D)_backgroundTexture, new Rectangle(point1.X, point1.Y, width, height), BackgroundColor);

        }

    }
}
