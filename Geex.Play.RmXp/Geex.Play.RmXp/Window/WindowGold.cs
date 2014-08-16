using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays amount of gold.
    /// </summary>
    public partial class WindowGold : WindowBase
    {
        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowGold()
            : base(GameOptions.MenuGoldX, GameOptions.MenuGoldY, GameOptions.MenuGoldWidth, GameOptions.MenuGoldHeight)
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
            int _cx = Contents.TextSize(Data.System.Wordings.Gold).Width;
            this.Contents.Font.Color = NormalColor;
            this.Contents.DrawText(4, 0, 120-_cx-2, 32, InGame.Party.Gold.ToString(), 2);
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(124 - _cx, 0, _cx, 32, Data.System.Wordings.Gold, 2);
        }

        #endregion
    }
}
