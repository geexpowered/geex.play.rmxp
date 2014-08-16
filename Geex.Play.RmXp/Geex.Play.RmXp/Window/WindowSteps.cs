using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays step count on the menu screen.
    /// </summary>
    public partial class WindowSteps : WindowBase
    {
        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowSteps()
            : base(GameOptions.MenuStepX, GameOptions.MenuStepY, GameOptions.MenuStepWidth, GameOptions.MenuStepHeight)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
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
            this.Contents.DrawText(4, 0, 120, 32, "Step Count");
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(4, 32, 120, 32, InGame.Party.Steps.ToString(), 2);
        }

        #endregion

    }
}
