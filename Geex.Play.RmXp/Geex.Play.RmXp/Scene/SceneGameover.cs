using Geex.Play.Rpg.Game;
using Geex.Run;
using Geex.Edit;

namespace Geex.Play.Rpg.Scene
{
    /// <summary>
    /// This class performs game over screen processing.
    /// </summary>
    class SceneGameover : SceneBase
    {
        #region Variables
        /// <summary>
        /// GameOver sprite
        /// </summary>
        Sprite sprite;

        #endregion

        #region Initialize
        /// <summary>
        /// Initialize
        /// </summary>
        public override void LoadSceneContent()
        {
            InitializeSprite();
            InitializeAudio();
            InitializeTransition();
        }

        /// <summary>
        /// Initialize Processing : Sprite Initialization
        /// </summary>
        void InitializeSprite()
        {
            // Make game over graphic
            sprite = new Geex.Run.Sprite();
            sprite.Bitmap = Cache.Picture(Data.System.GameoverName);
        }

        /// <summary>
        /// Initialize Processing : Audio Initialization
        /// </summary>
        void InitializeAudio()
        {
            // Stop BGM and BGS
            InGame.System.SongPlay(null);
            InGame.System.BackgroundSoundPlay(null);
            // Play game over ME
            InGame.System.SongEffectPlay(Data.System.GameoverMusicEffect);
        }

        /// <summary>
        /// Initialize Processing : Transition
        /// </summary>
        void InitializeTransition()
        {
            Graphics.Transition(120);
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose()
        {
            // Dispose of game over graphic
            sprite.Bitmap.Dispose();
            sprite.Dispose();
        }


        #endregion

        #region Methods

        /// <summary>
        /// Frame update
        /// </summary>
        public override void Update()
        {
            // If C button was pressed
            if (Input.RMTrigger.C)
            {
                // Switch to title screen
                Main.Scene = new SceneTitle();
            }
        }

        #endregion
    }
}
