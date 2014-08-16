using System.Collections.Generic;
using Geex.Run;
using Microsoft.Xna.Framework;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window deals with general command choices. (Horizontal)
    /// </summary>
    public partial class WindowHorizCommand : WindowSelectable
    {
        #region Variables

        /// <summary>
        /// Command spacing
        /// </summary>
        public int cSpacing;

        /// <summary>
        /// Commands alignment
        /// </summary>
        public int Alignment;

        #endregion

        #region Properties

        /// <summary>
        /// Menu commands list
        /// </summary>
        protected List<string> Commands
        {
            get { return localCommands; }
            set
            { 
                // Return if Commands Are Same
                if (this.localCommands == value)
                {
                    return; 
                }
                // Reset Commands
                this.localCommands = value;
                // Resets Item Max
                int _item_max_temp = this.itemMax;
                this.itemMax = localCommands.Count;
                columnMax = this.itemMax;
                // If Item Max Changes
                if(_item_max_temp != this.itemMax)
                {
                    // Deletes Existing Contents (If Exist)
                    if(!this.Contents.IsNull)
                    {
                        this.Contents.Dispose();
                        this.Contents = null;
                    }
                // Recreates Contents
                this.Contents = new Bitmap(this.itemMax * cSpacing, Height - 32);
                }
                // Refresh Window
                Refresh();
            }
        }
        List<string> localCommands = new List<string>();

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="width">window width</param>
        /// <param Name="commands">commands list</param>
        /// <param Name="c_spacing">command spacing</param>
        public WindowHorizCommand(int width, List<string> commands, int c_spacing)
            : base(0, 0, width, 64)
        {
            Initialize(width, commands, c_spacing);
        }

        /// <summary>
        /// Constructor (Default : c_spacing = (width - 32) / commands.Count)
        /// </summary>
        /// <param Name="width">window width</param>
        /// <param Name="commands">commands list</param>
        public WindowHorizCommand(int width, List<string> commands)
            : base(0, 0, width, 64)
        {
            // Initialize
            Initialize(width, commands, ((width - 32) / commands.Count));
        }

        /// <summary>
        /// Constructor for WindowShopCommand
        /// </summary>
        /// <param name="width">Window width</param>
        public WindowHorizCommand(int width)
            : base(0, 0, width, 64)
        {

        }

        /// <summary>
        /// Initialzing method
        /// </summary>
        /// <param Name="width">window width</param>
        /// <param Name="commands">commands list</param>
        /// <param Name="c_spacing">command spacing</param>
        public void Initialize(int width, List<string> commands, int c_spacing)
        {
            // Compute window height from command quantity
            this.itemMax = commands.Count;
            this.columnMax = this.itemMax;
            this.cSpacing = c_spacing;
            this.Alignment = 1;
            this.Contents = new Bitmap(this.itemMax * this.cSpacing, Height - 32);
            Refresh();
            this.Index = 0;
        }

        /// <summary>
        /// Initialzing method
        /// </summary>
        /// <param Name="width">window width</param>
        /// <param Name="c_spacing">command spacing</param>
        public void Initialize(int width, int c_spacing)
        {
            // Compute window height from command quantity
            this.itemMax = Commands.Count;
            this.columnMax = this.itemMax;
            this.cSpacing = c_spacing;
            this.Alignment = 0;
            this.Contents = new Bitmap(this.itemMax * this.cSpacing, Height - 32);
            Refresh();
            this.Index = 0;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return specified command
        /// </summary>
        /// <param Name="index">command index</param>
        /// <returns>command string</returns>
        public string Command(int index)
        {
            return Commands[index];
        }

        /// <summary>
        /// Refresh window content
        /// </summary>
        public virtual void Refresh()
        {
            this.Contents.Clear();
            for (int i = 0; i < itemMax; i++)
            {
                DrawItem(i, NormalColor);
            }
        }

        /// <summary>
        /// Draw item with the selected Color
        /// </summary>
        /// <param Name="index">item index</param>
        /// <param Name="Color">text Color</param>
        public virtual void DrawItem(int index, Color color)
        {
            string _command = Commands[index];
            int textX = cSpacing + index * cSpacing + 4;
            this.Contents.Font.Color = color;
            this.Contents.DrawText(textX, 0, cSpacing - 8, 32, _command, Alignment);
        }

        /// <summary>
        /// Disable selected item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DisableItem(int index)
        {
            DrawItem(index, DisabledColor);
        }

        /// <summary>
        /// Update GeexMouse Rectangle
        /// </summary>
        public override void UpdateCursorRect()
        {
            if (Index < 0)
            {
                this.CursorRect.Empty();
            }
            else
            {
                if(Alignment == 0)
                    this.CursorRect.Set(Index * cSpacing, 0, cSpacing, 32);
                else
                    this.CursorRect.Set(cSpacing + Index * cSpacing, 0, cSpacing, 32);
            }
        }

        #endregion
    }
}
