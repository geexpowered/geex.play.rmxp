using System;
using Geex.Edit;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays play time on the menu screen.
    /// </summary>
    public partial class WindowPlayTime : WindowBase
    {
        #region Variables

        /// <summary>
        /// Number of seconds since the game start
        /// </summary>
        int totalSec;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowPlayTime()
            : base(GameOptions.MenuPlayTimeX, GameOptions.MenuPlayTimeY, GameOptions.MenuPlayTimeWidth, GameOptions.MenuPlayTimeHeight)
        {
            this.Contents = new Bitmap(Width-32, Height-32);
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh window content
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(4, 0, 120, 32, "Play Time");
            // Calculate playtime
            totalSec = Graphics.FrameCount / Graphics.FrameRate;
            int hour = totalSec / 60 / 60;
            int min = totalSec / 60 % 60;
            int sec = totalSec % 60;
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(4, 32, 120, 32, String.Format("{0}:{1}:{2}", hour, min, sec), 2);
        }

        #endregion

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            base.Update();
            if (Graphics.FrameCount / Graphics.FrameRate != totalSec)
            {
                Refresh();
            }
        }
    }
}
