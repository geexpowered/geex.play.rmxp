using Geex.Edit;
using Geex.Play.Rpg.Game;
using Geex.Run;


namespace Geex.Play.Rpg.Window
{
    /// <summary>
    /// This window displays party member status on the menu screen.
    /// </summary>
    public partial class WindowMenuStatus : WindowSelectable
    {
        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        public WindowMenuStatus()
            : base(GameOptions.MenuStatusX, GameOptions.MenuStatusY, GameOptions.MenuStatusWidth, GameOptions.MenuStatusHeight)
        {
            this.Contents = new Bitmap(Width - 32, Height - 32);
            base.Initialize();
            Refresh();
            this.IsActive = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Refresh window content
        /// </summary>
        public void Refresh()
        {
            this.Contents.Clear();
            itemMax = InGame.Party.Actors.Count;
            for(int i=0 ; i<InGame.Party.Actors.Count ; i++)
            {
                int _x = 64;
                int _y = i * 116;
                GameActor _actor = InGame.Party.Actors[i];
                DrawActorGraphic(_actor, _x - 40, _y + 80);
                DrawActorName(_actor, _x, _y);
                draw_actor_class(_actor, _x + 144, _y);
                DrawActorLevel(_actor, _x, _y + 32);
                DrawActorState(_actor, _x + 90, _y + 32);
                DrawActorExp(_actor, _x, _y + 64);
                DrawActorHp(_actor, _x + 236, _y + 32);
                DrawActorSp(_actor, _x + 236, _y + 64);
            }
        }

        /// <summary>
        /// GeexMouse Rectangle Update
        /// </summary>
        public override void UpdateCursorRect()
        {
            if (Index < 0)
            {
                this.CursorRect.Empty();
            }
            else
            {
                this.CursorRect.Set(0, Index * 116, this.Width - 32, 96);
            }
        }

        #endregion
    }
}
