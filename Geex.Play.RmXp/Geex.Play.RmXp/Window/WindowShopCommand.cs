using System.Collections.Generic;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window is used to choose your business on the shop screen.
    /// </summary>
    public partial class WindowShopCommand : WindowHorizCommand
    {
        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowShopCommand()
            : base(480 * GeexEdit.GameWindowWidth/640)
        {
            this.Initialize();
        }

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            Commands.Clear();
            Commands.Add("Buy");
            Commands.Add("Sell");
            Commands.Add("Exit");
            base.Initialize(480 * GeexEdit.GameWindowWidth / 640, (480 - 32) / Commands.Count);
            //Refresh();
            this.Y = 64;
            Alignment = 0;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Refresh window content
        /// </summary>
        public override void Refresh()
        {
            this.Contents.Clear();
            for (int i = 0; i < itemMax; i++)
            {
                DrawItem(i);
            }
        }

        /// <summary>
        /// Draw a command item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DrawItem(int index)
        {
            int _x = 4 + (index * 160) * GeexEdit.GameWindowWidth / 640;
            this.Contents.DrawText(_x, 0, 128, 32, Commands[index]);
        }

        #endregion
    }
}
