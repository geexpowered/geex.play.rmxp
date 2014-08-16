using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Microsoft.Xna.Framework;
using Geex.Edit; 

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window is used to select whether to fight or escape on the battle
    /// screen.
    /// </summary>
    public partial class WindowPartyCommand : WindowHorizCommand
    {
        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowPartyCommand() : base(GeexEdit.GameWindowWidth)
        {

        }

        /// <summary>
        /// Initialize
        /// </summary>
        protected override void Initialize()
        {
            Commands.Clear();
            Commands.Add("Fight");
            Commands.Add("Escape");
            this.itemMax = 2;
            this.BackOpacity = 160;
            if (!InGame.Temp.IsBattleCanEscape)
            {
                DisableItem(1);
            }
            this.IsActive = false;
            this.IsVisible = false;
            base.Initialize(GeexEdit.GameWindowWidth, GeexEdit.GameWindowWidth / (this.itemMax * 2));//(GeexEdit.GameWindowWidth - 32) / Commands.Count);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draw Item
        /// </summary>
        /// <param Name="index">item index</param>
        /// <param Name="Color">text Character Color</param>
        public void DrawItem(int index, Color Color)
        {
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
            this.Contents.Font.Color = Color;
            Rectangle Rect = new Rectangle(160 + index * 160 + 4, 0, 128 - 10, 32);
            this.Contents.FillRect(Rect, new Color(0, 0, 0, 0));
            Contents.DrawText(Rect, Commands[index], 1,true);
        }

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public override void UpdateCursorRect()
        {
            this.CursorRect.Set(cSpacing - cSpacing / 5 + Index * cSpacing, 0, 128, 32);
        }

        #endregion
    }
}
