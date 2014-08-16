using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window selects a use target for the actor on item and skill screens.
    /// </summary>
    public partial class WindowTarget : WindowSelectable
    {
        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowTarget()
            : base(0, 0, 336, 480)
        {
            Initialize();
        }

        /// <summary>
        /// Initialization
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.Contents = new Bitmap(Width - 32, Height - 32);
            this.Contents.Font.Size = GeexEdit.DefaultFontSize;
            this.Z += 10;
            itemMax = InGame.Party.Actors.Count;
            this.Index = 0;
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
            for (int i = 0; i < InGame.Party.Actors.Count; i++)
            {
                int _x = 4;
                int _y = i * 116;
                GameActor _actor = InGame.Party.Actors[i];
                DrawActorName(_actor, _x, _y);
                draw_actor_class(_actor, _x + 144, Y);
                DrawActorLevel(_actor, _x + 8, _y + 32);
                DrawActorState(_actor, _x + 8, _y + 64);
                DrawActorHp(_actor, _x + 152, _y + 32);
                DrawActorSp(_actor, _x + 152, _y + 64);
            }
        }

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public override void UpdateCursorRect()
        {
            // GeexMouse position -1 = all choices, -2 or lower = independent choice
            // (meaning the user's own choice)
            if (Index <= -2)
            {
                this.CursorRect.Set(0, (Index + 10) * 116, this.Width - 32, 96);
            }
            else if (Index == -1)
            {
                this.CursorRect.Set(0, 0, this.Width - 32, itemMax * 116 - 20);
            }
            else
            {
                this.CursorRect.Set(0, Index * 116, this.Width - 32, 96);
            }
        }

        #endregion
    }
}
