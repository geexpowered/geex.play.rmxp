using System;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window is for inputting numbers, and is used within the
    /// message window.
    /// </summary>
    public partial class WindowInputNumber : WindowBase
    {
        #region Variables

        /// <summary>
        /// digit count
        /// </summary>
        int digitsMax;

        /// <summary>
        /// Index
        /// </summary>
        int index;

        /// <summary>
        /// Dummy bitmap, necessary to calculate cursor_width
        /// </summary>
        static Bitmap dummyBitmap = new Bitmap(32, 32);

        /// <summary>
        /// GeexMouse width
        /// Calculate cursor width from number width (0-9 equal width and postulate)
        /// </summary>
        static int cursorWidth = dummyBitmap.TextSize("0").Width + 8;

        #endregion

        #region Properties

        /// <summary>
        /// Number
        /// </summary>
        public int Number
        {
            get { return localNumber; }
            set
            {
                localNumber = value;//Math.Min(Math.Max(value, 0), (int)Math.Pow(10, digitsMax - 1));
                Refresh();
            }
        }
        int localNumber;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="digits_max">digit count</param>
        public WindowInputNumber(int digits_max)
            : base(0, 0, cursorWidth * digits_max + 32, 64)
        {
            this.digitsMax = digits_max;
            Number = 0;
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Z += 100;// 9999;
            this.Opacity = 0;
            index = 0;
            Refresh();
            UpdateCursorRect();
            //Dispose dummy_bitmap, statically initialized
            dummyBitmap.Dispose();
        }

        #endregion

        #region Methods

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public void UpdateCursorRect()
        {
            this.CursorRect.Set(index * cursorWidth, 0, cursorWidth, 32);
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public new void Update()
        {
            base.Update();
            // If up or down directional button was pressed
            if (Input.RMTrigger.Up || Input.RMTrigger.Down)
            {
                Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                // Get current place number and change it to 0
                int place = (int)Math.Pow(10, (digitsMax - 1 - index));
                int n = Number / place % 10;
                Number -= n * place;
                // If up add 1, if down substract 1
                if (Input.RMTrigger.Up)
                {
                    n = (n + 1) % 10;
                }
                if (Input.RMTrigger.Down)
                {
                    n = (n + 9) % 10;
                }
                // Reset current place number
                Number += n * place;
            }
            // GeexMouse right
            if (Input.RMTrigger.Right)
            {
                if (digitsMax >= 2)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    index = (index + 1) % digitsMax;
                }
            }
            // GeexMouse left
            if (Input.RMTrigger.Left)
            {
                if (digitsMax >= 2)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    index = (index + digitsMax - 1) % digitsMax;
                }
            }
            UpdateCursorRect();
        }

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            this.Contents.Font.Color = NormalColor;
            char[] s = new char[digitsMax]; 
            char[] val = Number.ToString().ToCharArray();
            short l = 0;
            // If value has not enough digits, add zeros before
            if (val.Length < digitsMax)
            {
                for (int k = 0; k < digitsMax - val.Length; k++)
                {
                    s[k] = '0';
                    l++;
                }
            }
            // Fill remaining chars with value's numbers
            for (int j = l; j < digitsMax; j++)
            {
                s[j] = val[j - l];
                
            }
            // Draw
            for (short i = 0; i < digitsMax; i++)
            {
                this.Contents.DrawText(i * cursorWidth + 4, 0, 32, 32, s[i].ToString());
            }
        }

        #endregion
    }
}
