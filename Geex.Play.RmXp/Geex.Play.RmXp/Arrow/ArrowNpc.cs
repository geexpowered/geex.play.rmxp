using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Rpg.Arrow
{
    /// <summary>
    /// This arrow cursor is used to choose npcs. This class inherits from the
    /// Arrow_Base class.
    /// </summary>
    public partial class ArrowNpc : ArrowBase
    {
        #region Properties

        /// <summary>
        /// Enemy Indicated by GeexMouse
        /// </summary>
        public GameNpc Npc
        {
            get
            {
                return InGame.Troops.Npcs[index];
            }
        }

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param Name="viewport">sprite viewport</param>
        public ArrowNpc(Viewport viewport)
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
            // Skip if indicating a nonexistant enemy
            for (int i = 0; i < InGame.Troops.Npcs.Count; i++)
            {
                if (this.Npc.IsExist)
                {
                    break;
                }
                index += 1;
                index %= InGame.Troops.Npcs.Count;
            }
            // Left stick right
            if (Input.RMTrigger.Right || Input.RMRepeat.Right)
            {
                Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                for (int i = 0; i < InGame.Troops.Npcs.Count; i++)
                {
                    index += 1;
                    index %= InGame.Troops.Npcs.Count;
                    base.Update();
                    if (this.Npc.IsExist)
                    {
                        break;
                    }
                }
            }
            // Left stick left
            if (Input.RMTrigger.Left || Input.RMRepeat.Left)
            {
                Audio.SoundEffectPlay(Data.System.CursorSoundEffect);
                for (int i = 0; i < InGame.Troops.Npcs.Count; i++)
                {
                    index += InGame.Troops.Npcs.Count - 1;
                    index %= InGame.Troops.Npcs.Count;
                    base.Update();
                    if (this.Npc.IsExist)
                    {
                        break;
                    }
                }
            }
            // Set sprite coordinates
            if (this.Npc != null)
            {
                this.X = this.Npc.ScreenX;
                this.Y = this.Npc.ScreenY;
            }
        }

        /// <summary>
        /// Help Text Update
        /// </summary>
        public override void UpdateHelp()
        {
            // Display npc Name and state in the help window
            HelpWindow.SetNpc(this.Npc);
        }

        #endregion
    }
}
