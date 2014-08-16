using System;
using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window class contains cursor movement and scroll functions.
    /// </summary>
    public partial class WindowSelectable : WindowBase
    {
        #region Variables

        /// <summary>
        /// Item max
        /// </summary>
        protected int itemMax;

        /// <summary>
        /// Column number
        /// </summary>
        protected int columnMax;

        #endregion

        #region Properties

        /// <summary>
        /// cursor position
        /// </summary>
        public int Index
        {
            get { return localIndex; }
            set
            {
                localIndex = value;
                // Update Help Text (update_help is defined by the subclasses)
                if (this.IsActive && HelpWindow != null)
                {
                    UpdateHelp();
                }
                // Update cursor rectangle
                UpdateCursorRect();
            }
        }
        int localIndex;

        /// <summary>
        /// help window
        /// </summary>
        public WindowHelp HelpWindow
        {
            get { return localHelpWindow; }
            set
            {
                localHelpWindow = value;
                // Update help text (update_help is defined by the subclasses)
                if (this.IsActive && HelpWindow != null)
                {
                    UpdateHelp();
                }
            }
        }
        WindowHelp localHelpWindow = null;

        /// <summary>
        /// Get Row Count
        /// </summary>
        public int RowMax
        {
            get
            {
                // Compute rows from number of items and columns
                return (itemMax + columnMax - 1) / columnMax;
            }
        }

        /// <summary>
        /// Get & Set Top Row. Value : row shown on top.
        /// </summary>
        public int TopRow
        {
            get
            {
                // Divide y-coordinate of window contents transfer Origin by 1 row
                // height of 32
                return this.Oy >>5;
            }
            set
            {
                // If row is less than 0, change it to 0
                if (value < 0)
                {
                    value = 0;
                }
                // If row exceeds row_max - 1, change it to row_max - 1
                if (value > RowMax - 1)
                {
                    value = RowMax - 1;
                }
                // Multiply 1 row height by 32 for y-coordinate of window contents
                // transfer Origin
                this.Oy = value * 32;
            }
        }

        /// <summary>
        /// Get Number of Rows Displayable on 1 Page
        /// </summary>
        public int PageRowMax
        {
            get
            {
                // Subtract a frame height of 32 from the window height, and divide it by
                // 1 row height of 32
                return (this.Height - 32) >> 5;//(/32)
            }
        }

        /// <summary>
        /// Get Number of Items Displayable on 1 Page
        /// </summary>
        public int PageItemMax
        {
            get
            {
                // Multiply row count (page_row_max) times column count (column_max)
                return PageRowMax * columnMax;
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="x">window x-coordinate</param>
        /// <param Name="y">window y-coordinate</param>
        /// <param Name="width">window width</param>
        /// <param Name="height">window height</param>
        public WindowSelectable(Viewport port, int _x, int _y, int width, int height)
            : base(port, _x, _y, width, height)
        {
            Initialize();
        }
        public WindowSelectable(int _x, int _y, int width, int height)
            : this(Graphics.Foreground, _x, _y, width, height)
        {
        }

        protected virtual void Initialize()
        {
            itemMax = 1;
            columnMax = 1;
            Index = -1;
            this.Contents.Font.Name = GeexEdit.DefaultFont;
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Update GeexMouse Rectangle
        /// </summary>
        public virtual void UpdateCursorRect()
        {
            // If cursor position is less than 0
            if (Index < 0)
            {
                this.CursorRect.Empty();
                return;
            }
            // Get current row
            int _row = Index / columnMax;
            // If current row is before top row
            if (_row < this.TopRow)
            {
                // Scroll so that current row becomes top row
                this.TopRow = _row;
            }
            // If current row is more to back than back row
            if (_row > this.TopRow + (this.PageRowMax - 1))
            {
                // Scroll so that current row becomes back row
                this.TopRow = _row - (this.PageRowMax - 1);
            }
            // Calculate cursor width
            int _cursor_width = this.Width / columnMax - 32;
            // Calculate cursor coordinates
            int _x = Index % columnMax * (_cursor_width + 32);
            int _y = Index / columnMax * 32 - (int)this.Oy;
            // Update cursor rectangle
            this.CursorRect.Set(_x, _y, _cursor_width, 32);
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public override void Update()
        {
            base.Update();
            // If cursor is movable
            if (this.IsActive && itemMax > 0 && Index >= 0)
            {
                // If pressing down on the directional buttons
                if (Input.RMTrigger.Down || Input.RMRepeat.Down)
                {
                    // If column count is 1 and directional button was pressed down with no
                    // IsRepeated, or if cursor position is more to the front than
                    // (item count - column count)
                    if ((columnMax == 1 && Input.RMTrigger.Down) ||
                       Index < itemMax - columnMax)
                    {
                        // Move cursor down
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        Index = (Index + columnMax) % itemMax;
                    }
                }
                // If the up directional button was pressed
                if (Input.RMTrigger.Up || Input.RMRepeat.Up)
                {
                    // If column count is 1 and directional button was pressed up with no
                    // IsRepeated, or if cursor position is more to the back than column count
                    if ((columnMax == 1 && Input.RMTrigger.Up) ||
                       Index >= columnMax)
                    {
                        // Move cursor up
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        Index = (Index - columnMax + itemMax) % itemMax;
                    }
                }
                // If the right directional button was pressed
                if (Input.RMTrigger.Right || Input.RMRepeat.Right)
                {
                    // If column count is 2 or more, and cursor position is closer to front
                    // than (item count -1)
                    if (columnMax >= 2 && Index < itemMax - 1)
                    {
                        // Move cursor right
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        Index += 1;
                    }
                }
                // If the left directional button was pressed
                if (Input.RMTrigger.Left || Input.RMRepeat.Left)
                {
                    // If column count is 2 or more, and cursor position is more back than 0
                    if (columnMax >= 2 && Index > 0)
                    {
                        // Move cursor left
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        Index -= 1;
                    }
                }
                // If R button was pressed
                if (Input.RMRepeat.R)
                {
                    // If bottom row being displayed is more to front than bottom data row
                    if (this.TopRow + (this.PageRowMax - 1) < (this.RowMax - 1))
                    {
                        // Move cursor 1 page back
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        Index = Math.Min(Index + this.PageItemMax, itemMax - 1);
                        this.TopRow += this.PageRowMax;
                    }
                }
                // If L button was pressed
                if (Input.RMRepeat.L)
                {
                    // If top row being displayed is more to back than 0
                    if (this.TopRow > 0)
                    {
                        // Move cursor 1 page forward
                        Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                        Index = Math.Max(Index - this.PageItemMax, 0);
                        this.TopRow -= this.PageRowMax;
                    }
                }
            }
            // Update help text (update_help is defined by the subclasses)
            if (this.IsActive && HelpWindow != null)
            {
                UpdateHelp();
            }
            // Update cursor rectangle
            UpdateCursorRect();
        }
        
        /// <summary>
        /// Help Window Update (overriden)
        /// </summary>
        public virtual void UpdateHelp()
        { 
            // Nothing, should be put abstract ?
        }

        #endregion

    }
}
