using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Arrow
{
    /// <summary>
    /// This arrow cursor is used to choose an actor. This class inherits from the
    /// Arrow_Base class.
    /// </summary>
    public partial class ArrowActor : ArrowBase
    {
        #region Properties

        /// <summary>
        /// Actor Indicated by GeexMouse
        /// </summary>
        GameActor Actor
        {
            get
            {
                return InGame.Party.Actors[index];
            }
        }

        #endregion

        #region Initialize
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="viewport">sprite viewport</param>
        public ArrowActor(Viewport viewport)
            : base(viewport)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Frame Update
        /// </summary>
        public void Update()
        {
            base.Update();
            // GeexMouse right
            if (Input.RMTrigger.Right || Input.RMRepeat.Right)
            {
                Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                index += 1;
                index %= InGame.Party.Actors.Count;
                base.Update();
            }
            // GeexMouse left
            if (Input.RMTrigger.Left || Input.RMRepeat.Left)
            {
                Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                index += InGame.Party.Actors.Count - 1;
                index %= InGame.Party.Actors.Count;
                base.Update();
            }
            // Set sprite coordinates
            if (this.Actor != null)
            {
                this.X = this.Actor.ScreenX;
                this.Y = this.Actor.ScreenY;
            }
        }

        /// <summary>
        /// Help Text Update 
        /// </summary>
        public override void UpdateHelp()
        {
            // Display actor status in help window
            HelpWindow.SetActor(this.Actor);
        }

        #endregion
    }
}
