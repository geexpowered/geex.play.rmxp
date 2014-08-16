using Geex.Play.Rpg.Game;
using Geex.Play.Rpg.Window;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Arrow
{
    /// <summary>
    /// This sprite is used as an arrow cursor for the battle screen. This class
    /// is used as a superclass for the Arrow_Enemy and Arrow_Actor classes.
    /// </summary>
    public partial class ArrowBase : Sprite
    {
        #region Variables

        /// <summary>
        /// The blink counter for the cursor, between 0 and 7.
        /// If under 4, display the first arrow of the Windowskin,
        /// else display the second arrow.
        /// </summary>
        int blinkCount;

        #endregion

        #region Properties

        /// <summary>
        /// cursor position
        /// </summary>
        public int index
        {
            get { return localIndex; }
            set
            {
                localIndex = value;
            }

        }
        int localIndex;

        /// <summary>
        /// Set Help Window
        /// </summary>
        public WindowHelp HelpWindow
        {
            get { return localHelpWindow; }
            set
            {
                localHelpWindow = value;
                // Update help text (update_help is defined by the subclasses)
                if (localHelpWindow != null)
                {
                    UpdateHelp();
                }
            }
        }
        WindowHelp localHelpWindow;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="viewport">viewport for the arrow cursor</param>
        public ArrowBase(Viewport viewport)
            : base(viewport)
        {
            this.Bitmap = Cache.Windowskin(InGame.System.WindowskinName);
            this.Ox = 16;
            this.Oy = 64;
            this.Z = 2500;
            blinkCount = 0;
            index = 0;
            HelpWindow = null;
            Update();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public new void Update()
        {
            // Update blink count
            blinkCount = (blinkCount + 1) % 8;
            // Set forwarding Origin rectangle
            if (blinkCount < 4)
            {
                Rectangle _new_src_rect = new Rectangle(128, 96, 32, 32);
                this.SourceRect = _new_src_rect;
            }
            else
            {
                Rectangle _new_src_rect = new Rectangle(160, 96, 32, 32);
                this.SourceRect = _new_src_rect;
            }
            // Update help text (update_help is defined by the subclasses)
            if (HelpWindow != null)
            {
                UpdateHelp();
            }
        }

        /// <summary>
        /// Help Text Update
        /// </summary>
        public virtual void UpdateHelp()
        {

        }

        #endregion
    }
}
