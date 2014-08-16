using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays amount of gold and EXP acquired at the end of a battle.
    /// </summary>
    public partial class WindowBattleResult : WindowBase
    {
        #region Variables

        /// <summary>
        /// Gold acquired at the end of a battle
        /// </summary>
        int gold;

        /// <summary>
        /// EXP acquired at the end of a battle
        /// </summary>
        int exp;

        /// <summary>
        /// List of items acquired at the end of a battle
        /// </summary>
        List<Carriable> treasures = new List<Carriable>();

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="exp">EXP</param>
        /// <param Name="gold">amount of gold</param>
        /// <param Name="treasures">treasures</param>
        public WindowBattleResult(int _exp, int _gold, List<Carriable> _treasures)
            : base(GeexEdit.GameWindowWidth/2 - 160, 0, 320, _treasures.Count * 32 + 64)
        {
            exp = _exp;
            gold = _gold;
            treasures = _treasures;
            Contents = new Bitmap(Width - 32, Height - 32);
            Y = GeexEdit.GameWindowHeight / 3 - Height / 2;
            BackOpacity = 160;
            IsVisible = false;
            Refresh();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            int _x = 4;
            this.Contents.Font.Color = NormalColor;
            int cx = Contents.TextSize(exp.ToString()).Width;
            this.Contents.DrawText(_x, 0, cx, 32, exp.ToString());
            _x += cx + 4;
            this.Contents.Font.Color = SystemColor;
            cx = Contents.TextSize("EXP").Width;
            this.Contents.DrawText(_x, 0, 64, 32, "EXP");
            _x += cx + 16;
            this.Contents.Font.Color = NormalColor;
            cx = Contents.TextSize(gold.ToString()).Width;
            this.Contents.DrawText(_x, 0, cx, 32, gold.ToString());
            _x += cx + 4;
            this.Contents.Font.Color = SystemColor;
            this.Contents.DrawText(_x, 0, 128, 32, Data.System.Wordings.Gold);
            int _y = 32;
            foreach (Carriable item in treasures)
            {
                DrawItemName(item, 4, _y);
                _y += 32;
            }
        }

        #endregion
    }
}
