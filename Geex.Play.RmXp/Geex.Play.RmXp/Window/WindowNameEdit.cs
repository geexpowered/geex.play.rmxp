using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window is used to edit your Name on the input Name screen.
    /// </summary>
    public partial class WindowNameEdit : WindowBase
    {
        #region Variables

        /// <summary>
        /// Name
        /// </summary>
        public string Name;

        /// <summary>
        /// cursor position
        /// </summary>
        public int Index;

        /// <summary>
        /// Actor whom Name is editing
        /// </summary>
        GameActor actor;

        /// <summary>
        /// Default Name
        /// </summary>
        string defaultName;

        /// <summary>
        /// Max number of letter for the Name
        /// </summary>
        int maxChar;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="actor">actor whom Name is edited</param>
        /// <param Name="max_char">maximum number of characters</param>
        public WindowNameEdit(GameActor actor, int max_char)
            : base(0, 0, 640, 128)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.actor = actor;
            Name = actor.Name;
            this.maxChar = max_char;
            // Fit Name within maximum number of characters
            char[] name_array = Name.ToCharArray();
            Name = "";
            for (int i = 0; i < name_array.Length; i++)
            {
                Name += name_array[i];
            }
            defaultName = Name;
            Index = name_array.Length;
            Refresh();
            update_cursor_rect();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Return to Default localName
        /// </summary>
        public void RestoreDefault()
        {
            Name = defaultName;
            Index = Name.ToCharArray().Length;
            Refresh();
            update_cursor_rect();
        }

        /// <summary>
        /// Add Character
        /// </summary>
        /// <param Name="Character">text Character to be added</param>
        public void Add(string character)
        {
            if (Index < maxChar && character != "")
            {
                Name += character;
                Index += 1;
                Refresh();
                update_cursor_rect();
            }
        }

        /// <summary>
        /// Delete Character
        /// </summary>
        public void Back()
        {
            if (Index > 0)
            {
                // Delete 1 text Character
                char[] name_array = Name.ToCharArray();
                Name = "";
                for (int i = 0 ; i < name_array.Length-1 ; i++)
                {
                    Name += name_array[i];
                }
                Index -= 1;
                Refresh();
                update_cursor_rect();
            }
        }


        public void Refresh()
        {
            this.Contents.Clear();
            // Draw Name
            char[] _name_array = Name.ToCharArray();
            for (int i=0 ; i<maxChar ; i++)
            {
                char c = _name_array[i];
                // If c is null, draw a long bar
                /*if (c == null)
                {
                    c = (("＿").ToCharArray())[0];
                }*/
                X = 320 - maxChar * 14 + i * 28;
                this.Contents.DrawText(X, 32, 28, 32, c.ToString(), 1);
            }
            // Draw graphic
            DrawActorGraphic(actor, 320 - maxChar * 14 - 40, 80);
        }

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public void update_cursor_rect()
        {
            X = 320 - maxChar * 14 + Index * 28;
            this.CursorRect.Set(X, 32, 28, 32);
        }

        /// <summary>
        /// Frame Update
        /// </summary>
        public new void update()
        {
            base.Update();
            update_cursor_rect();
        }

        #endregion
    }
}
