using System.Text;
using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Spriting
{
    /// <summary>
    /// This sprite is used to display the Timer.It observes the GameSystem
    /// class and automatically changes sprite conditions.
    /// </summary>
    public partial class SpriteTimer : Sprite
    {
        #region Variables

        /// <summary>
        /// Elapsed time
        /// </summary>
        int totalSec;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public SpriteTimer()
        {
            this.Bitmap = new Bitmap(88, 48);
            this.Center();
            this.Bitmap.Font.Name = GeexEdit.DefaultFont;
            this.Bitmap.Font.Size = GeexEdit.DefaultFontSize + 10;
            this.X = GeexEdit.GameWindowCenterX;
            this.Y = 32;
            this.Z = 500;
            Update();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            // Set Timer to visible if working
            this.IsVisible = InGame.System.IsTimerWorking;
            // If Timer needs to be redrawn
            if (InGame.System.Timer / Graphics.FrameRate != totalSec)
            {
              // Clear window contents
              this.Bitmap.ClearTexts();
              // Calculate total number of seconds
              totalSec = InGame.System.Timer / Graphics.FrameRate;
              // Make a string for displaying the Timer
              int min = totalSec / 60;
              int sec = totalSec % 60;
              StringBuilder text = new StringBuilder(min.ToString());
              text.Append(":");
              text.Append(sec.ToString());
              // Draw Timer
              this.Bitmap.Font.Color = new Color(255, 255, 255);
              this.Bitmap.DrawText(this.Bitmap.Rect, text.ToString(), 1,true);
            }
        }


        #endregion
    }
}
