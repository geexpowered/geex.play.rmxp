using System.Collections.Generic;
using Geex.Edit;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window deals with general menu commands.
    /// </summary>
    public partial class WindowCommand : WindowSelectable
    {
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
                if (localCommands == value)
                {
                    return;
                }
                // Reset Commands
                localCommands = value;
                // Resets Item Max
                int item_max_temp = this.itemMax;
                this.itemMax = localCommands.Count;
                // If Item Max Changes
                if(item_max_temp != itemMax)
                {
                    // Deletes Existing Contents (If Exist)
                    if (!this.Contents.IsNull)
                    {
                        this.Contents.Dispose();
                        this.Contents = null;
                    }
                    // Recreates Contents
                    this.Contents = new Bitmap(Width - 32, itemMax * 32);
                }
                // Refresh Window
                Refresh();
            }
        }
        List<string> localCommands = new List<string>();

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor, set the menu Font, draw the commands.
        /// </summary>
        /// <param Name="width">command menu's window width</param>
        /// <param Name="commands">command text string array</param>
        public WindowCommand(int width, List<string> commands)
            : base(GameOptions.MenuCommandListX, GameOptions.MenuCommandListY, width, commands.Count * 32 + 32)
        {
            Initialize(width, commands);
        }

        protected void Initialize(int width, List<string> commands)
        {
            base.Initialize();
            // Compute window height from command quantity
            itemMax = commands.Count;
            this.Commands = commands;
            this.Contents = new Bitmap(width - 32, itemMax * 32);
            this.Contents.Font.Name = GeexEdit.DefaultFont;
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
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
        /// Return specified command (default : index = window current index)
        /// </summary>
        /// <returns>Command string</returns>
        public string Command()
        {
            return Commands[this.Index];
        }


        /// <summary>
        /// Refresh : draw every item
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            for (int i=0; i<itemMax ; i++)
            {
                DrawItem(i, NormalColor);
            }
        }

        /// <summary>
        /// Draw the selected item
        /// </summary>
        /// <param Name="index">item index</param>
        /// <param Name="Color">text Color</param>
        public void DrawItem(int index, Color color)
        {
            this.Contents.Font.Color = color;
            Rectangle _rect = new Rectangle(4, 32 * index, this.Contents.Width - 8, 32);
            // Cleaning is not needed
            // this.contents.FillRect(_rect, new Color(0, 0, 0, 0));
            this.Contents.DrawText(_rect, Commands[index]);
        }

        /// <summary>
        /// Disable the selected item
        /// </summary>
        /// <param Name="index">item index</param>
        public void DisableItem(int index)
        {
            DrawItem(index, DisabledColor);
        }

        #endregion
    }
}
