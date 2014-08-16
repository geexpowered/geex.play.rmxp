using Geex.Edit;
using Geex.Play.Rpg.Scene;
using Geex.Run;
using Microsoft.Xna.Framework;

namespace Geex.Play.Rpg
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application
        /// </summary>
        static void Main(string[] args)
        {
            #region Here you may change Game Settings
            //Change the Game Font and Size
            GeexEdit.DefaultFont = "Arial";
            GeexEdit.DefaultFontSize = 15;
            // Set-up some Collision options
            GeexEdit.IsCollisionMaskOn = false;
            // Change the Content Manager Folder names
            GeexEdit.AnimationContentPath = "Animations/";
            GeexEdit.BattlerContentPath = "Battlers/";
            GeexEdit.BattleBackContentPath = "Battlebacks/";
            GeexEdit.PictureContentPath = "Pictures/";
            GeexEdit.TitleContentPath = "Titles/";
            GeexEdit.CharacterContentPath = "Characters/";
            GeexEdit.PanoramaContentPath = "Panoramas/";
            GeexEdit.FogContentPath = "Fogs/";
            GeexEdit.WindowskinContentPath = "Windowskins/";
            GeexEdit.EffectContentPath = "Effects/";
            GeexEdit.IconContentPath = "Icons/";
            GeexEdit.ChipsetContentPath = "Chipsets/";
            GeexEdit.ParticleContentPath = "Particles/";
            GeexEdit.ChipsetMaskContentPath = "Masks/";
            GeexEdit.VideoContentPath = "Video/";
            GeexEdit.SongContentPath = "Songs/";
            GeexEdit.SoundEffectContentPath = "SoundEffects/";
            GeexEdit.BackgroundEffectContentPath = "SoundEffects/";
            GeexEdit.SongEffectContentPath = "SoundEffects/";
            GeexEdit.FontContentPath = "Fonts/";
            GeexEdit.DataContentPath = "Data/";
            GeexEdit.MapContentPath = "Data/";

            #region Game Window Options
            /// <summary>
            /// Game Resolution Width in pixel
            /// </summary>
            GeexEdit.GameWindowWidth = 640;
            /// <summary>
            /// Game Resolution Height in pixel
            /// </summary>
            GeexEdit.GameWindowHeight = 480;
            /// <summary>
            /// Safe Area (only available for Xbox)
            /// Percentage of inner screen to be displayed (ie 90%=0.9f). Set to 1f for no safe area
            /// </summary>
            GeexEdit.SafeArea = .9f;
            /// <summary>
            /// Bitmap text are displayed with Shadow or not is not indicated
            /// </summary>
            GeexEdit.IsTextShadowedAsStandard = false;
            #endregion

            #region Game Map Options
            /// <summary>
            /// Full Screen option at game start
            /// </summary>
    #if DEBUG
            GeexEdit.IsFullScreen = false;
    #else
            GeexEdit.IsFullScreen = true;
    #endif
            #endregion

            #region Main Options
            /// <summary>
            /// True if Geex Splash screen must be skipped
            /// </summary>
            GeexEdit.IsGeexSplashScreenSkipped = false;

            /// <summary>
            /// Default Font Color for in game windows.
            /// </summary>
            GeexEdit.DefaultFontColor = Color.White;

            /// <summary>
            /// Default Font Shadow Color for in game windows.
            /// </summary>
            GeexEdit.DefaultShadowFontColor = Color.Black;

            /// <summary>
            /// Default Font Shadow Vector
            /// </summary>
            GeexEdit.FontShadow = Vector2.One;
            #endregion

            #region Database Options
            /// <summary>
            /// Max number of Picture on screen
            /// </summary>
            GeexEdit.NumberOfPictures = 51;
            #endregion

            #endregion

            // Start Game
            using (Main game = new Main())
            {
                Geex.Run.Main.StartScene = new SceneTitle();
                game.Run();
            }
        }
    }
}

