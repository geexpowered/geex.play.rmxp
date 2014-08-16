using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays the skill user's status on the skill screen.
    /// </summary>
    public partial class WindowSkillStatus : WindowBase
    {
        #region Variables

        /// <summary>
        /// Current actor
        /// </summary>
        GameActor actor;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="actor">window actor</param>
        public WindowSkillStatus(GameActor actor)
            : base(0, 64, GeexEdit.GameWindowWidth, 64)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
            this.actor = actor;
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
            DrawActorName(actor, 4, 0);
            DrawActorState(actor, 140 * GeexEdit.GameWindowWidth / 640, 0);
            DrawActorHp(actor, 284 * GeexEdit.GameWindowWidth / 640, 0);
            DrawActorSp(actor, 460 * GeexEdit.GameWindowWidth / 640, 0);
        }

        #endregion
    }
}