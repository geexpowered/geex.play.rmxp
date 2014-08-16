using System;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window is for inputting quantity of items to buy or sell on the
    /// shop screen.
    /// </summary>
    public partial class WindowShopNumber : WindowBase
    {
        #region Variables

        /// <summary>
        /// Item number
        /// </summary>
        public int Number;

        /// <summary>
        /// Item which number is shown
        /// </summary>
        Carriable item;

        /// <summary>
        /// Max item number
        /// </summary>
        int max;

        /// <summary>
        /// Item price
        /// </summary>
        int price;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowShopNumber()
            : base(0, 128, GeexEdit.GameWindowWidth - 272, GeexEdit.GameWindowHeight - 128)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            item = null;
            max = 1;
            price = 0;
            Number = 1;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set Items, Max Quantity, and Price
        /// </summary>
        /// <param Name="item">item</param>
        /// <param Name="max">max quantity</param>
        /// <param Name="price">price</param>
        public void Set(Carriable item, int max, int price)
        {
            this.item = item;
            this.max = max;
            this.price = price;
            this.Number = 1;
            Refresh();
        }

        /// <summary>
        /// Refresh, draw price and currency
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            DrawItemName(item, 4, 96);
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(272, 96, 32, 32, "×");
            this.Contents.DrawText(308, 96, 24, 32, Number.ToString(), 2);
            this.CursorRect.Set(304, 96, 32, 32);
            // Draw total price and currency units;
            string _domination = Data.System.Wordings.Gold;
            int _cx = Contents.TextSize(_domination).Width;
            int _total_price = price * Number;
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(4, 160, 328 - _cx - 2, 32, _total_price.ToString(), 2);
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(332 - _cx, 160, _cx, 32, _domination, 2);
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            if (this.IsActive)
            {
                // GeexMouse right (+1)
                if (Input.RMTrigger.Right && Number < max)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    Number += 1;
                    Refresh();
                }
                // GeexMouse left (-1)
                if (Input.RMTrigger.Left && Number > 1)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    Number -= 1;
                    Refresh();
                }
                // GeexMouse up (+10)
                if (Input.RMTrigger.Up && Number < max)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    Number = Math.Min(Number + 10, max);
                    Refresh();
                }
                // GeexMouse down (-10)
                if (Input.RMTrigger.Down && Number > 1)
                {
                    Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                    Number = Math.Max(Number - 10, 1);
                    Refresh();
                }
            }
        }

        #endregion
    }
}
