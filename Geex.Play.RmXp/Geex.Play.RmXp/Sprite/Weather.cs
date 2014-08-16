using System;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// This class manages weather sprites.
    /// </summary>
    public partial class Weather
    {
        #region Variables
        /// <summary>
        /// Sprites x-coordinates gap
        /// </summary>
        public int Ox;

        /// <summary>
        /// Sprites y-coordinates gap
        /// </summary>
        public int Oy;
        /// <summary>
        /// Rain bitmap
        /// </summary>
        Bitmap rainBitmap;

        /// <summary>
        /// Storm bitmap
        /// </summary>
        Bitmap stormBitmap;

        /// <summary>
        /// Snow bitmap
        /// </summary>
        Bitmap snowBitmap;

        /// <summary>
        /// Weather sprites
        /// </summary>
        Sprite[] sprites;

        #endregion

        #region Properties

        /// <summary>
        /// Weather type
        /// </summary>
        public int Type
        {
            get { return localType; }
            set
            {
                if (localType == value)
                {
                    return;
                }
                localType = value;

                // Sprite bitmap is function of weather type
                Bitmap _bitmap = new Bitmap();
                switch (localType)
                {
                    case 1:
                        _bitmap = rainBitmap;
                        break;
                    case 2:
                        _bitmap = stormBitmap;
                        break;
                    case 3:
                        _bitmap = snowBitmap;
                        break;
                    default:
                        _bitmap = null;
                        break;
                }
                //Set sprite visible if max number of sprite is not reached
                for (int i = 1; i < 40; i++)
                {
                    Geex.Run.Sprite _sprite = sprites[i];
                    if (_sprite != null)
                    {
                        _sprite.IsVisible = (i <= Max);
                        _sprite.Bitmap = _bitmap;
                    }
                }
            }
        }
        int localType;

        /// <summary>
        /// Max sprites number
        /// </summary>
        public int Max
        {
            get { return localMax; }
            set
            {
                if (localMax == value)
                { return; }

                localMax = Math.Min(Math.Max(value, 0), 40);

                for (int i = 1; i < 40; i++)
                {
                    Geex.Run.Sprite _sprite = sprites[i];
                    if (_sprite != null)
                    {
                        _sprite.IsVisible = (i <= localMax);
                    }
                }
            }
        }
        int localMax;
        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="viewport">weather sprites' viewport</param>
        public Weather(Viewport viewport)
        {
            Type = 0;
            Max = 0;
            Ox = 0;
            Oy = 0;
            Color _color1 = new Color(255, 255, 255, 255);
            Color _color2 = new Color(255, 255, 255, 128);
            rainBitmap = new Bitmap(7, 56);

            for (int i = 0; i < 6; i++)
            {
                rainBitmap.FillRect(6 - i, i * 8, 1, 8, _color1);
            }

            stormBitmap = new Bitmap(34, 64);

            for (int i = 0; i < 31; i++)
            {
                stormBitmap.FillRect(33 - i, i * 2, 1, 2, _color2);
                stormBitmap.FillRect(32 - i, i * 2, 1, 2, _color1);
                stormBitmap.FillRect(31 - i, i * 2, 1, 2, _color2);
            }

            snowBitmap = new Bitmap(6, 6);
            snowBitmap.FillRect(0, 1, 6, 4, _color2);
            snowBitmap.FillRect(1, 0, 4, 6, _color2);
            snowBitmap.FillRect(1, 2, 4, 2, _color1);
            snowBitmap.FillRect(2, 1, 2, 4, _color1);
            sprites = new Geex.Run.Sprite[40];

            for (int i = 0; i < 40; i++)
            {
                sprites[i] = new Geex.Run.Sprite(viewport);
                sprites[i].Z = 1000;
                sprites[i].IsVisible = false;
                sprites[i].Opacity = 0;
            }
        }

        /// <summary>
        /// Constructor (viewport = null)
        /// </summary>
        public Weather()
            : this(Graphics.Background)
        {

        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            foreach (Sprite sprite in sprites)
            {
                sprite.Dispose();
            }

            rainBitmap.Dispose();
            stormBitmap.Dispose();
            snowBitmap.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            if (Type == 0)
            {
                return;
            }

            for (int i = 0; i < Max; i++)
            {
                Geex.Run.Sprite _sprite = sprites[i];

                if (_sprite == null)
                {
                    break;
                }

                if (Type == 1)
                {
                    _sprite.X -= 2;
                    _sprite.Y += 16;
                    _sprite.Opacity -= 8;
                }

                if (Type == 2)
                {
                    _sprite.X -= 8;
                    _sprite.Y += 16;
                    _sprite.Opacity -= 12;
                }

                if (Type == 3)
                {
                    _sprite.X -= 2;
                    _sprite.Y += 8;
                    _sprite.Opacity -= 8;
                }

                int x = _sprite.X;
                int y = _sprite.Y;
                if (_sprite.Opacity < 64 || x < 0 || x > GeexEdit.GameWindowWidth || y < 0 || y > GeexEdit.GameWindowHeight)
                {
                    _sprite.X = InGame.Rnd.Next(GeexEdit.GameWindowWidth);
                    _sprite.Y = InGame.Rnd.Next(GeexEdit.GameWindowHeight) - 200;
                    _sprite.Opacity = 255;
                }
            }
        }

        #endregion
    }
}
