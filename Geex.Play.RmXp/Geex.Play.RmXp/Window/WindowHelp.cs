using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;  

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window shows skill and item explanations along with actor status.
    /// </summary>
    public partial class WindowHelp : WindowBase
    {
        #region Variables

        /// <summary>
        /// Displayed text
        /// </summary>
        string text;

        /// <summary>
        /// Text alignment (0..flush left, 1..center, 2..flush right).
        /// </summary>
        int align;

        /// <summary>
        /// Displayed actor
        /// </summary>
        GameActor actor;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor, default window parameters
        /// </summary>
        public WindowHelp() : base(0, 0, GeexEdit.GameWindowWidth, 64)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Set the text string displayed in window
        /// </summary>
        /// <param Name="text">text string displayed in window</param>
        /// <param Name="align">alignment (0..flush left, 1..center, 2..flush right)</param>
        public void SetText(string text, int align)
        {
            // If at least one part of text and alignment differ from last time
            if (text != this.text || align != this.align)
            {
                // Redraw text
                this.Contents.Clear();
                this.Contents.Font.Color = NormalColor;
                this.Contents.DrawText(4, 0, this.Width - 40, 32, text, align);
                this.text = text;
                this.align = align;
                this.actor = null;
            }
            this.IsVisible = true;
        }

        /// <summary>
        /// Set the text string displayed in window (default: align = flush left)
        /// </summary>
        /// <param Name="text">text string displayed in window</param>
        public void SetText(string text)
        {
            SetText(text, 0);
        }

        /// <summary>
        /// Set actor displayed in window
        /// </summary>
        /// <param Name="actor">status displaying actor</param>
        public void SetActor(GameActor actor)
        {
            if (actor != this.actor)
            {
                this.Contents.Clear();
                DrawActorName(actor, 4, 0);
                DrawActorState(actor, 140, 0);
                DrawActorHp(actor, 284, 0);
                DrawActorSp(actor, 460, 0);
                this.actor = actor;
                this.text = null;
                this.IsVisible = true;
            }
        }

        /// <summary>
        /// Set npc displayed in window
        /// </summary>
        /// <param Name="npc">Name and status displaying npc</param>
        public void SetNpc(GameNpc npc)
        {
            string _text = npc.Name;
            string _state_text = MakeBattlerStateText(npc, 112, false);
            if (_state_text != "")
            {
                _text += "  " + _state_text;
            }
            SetText(_text, 1);
        }

        #endregion

    }
}
