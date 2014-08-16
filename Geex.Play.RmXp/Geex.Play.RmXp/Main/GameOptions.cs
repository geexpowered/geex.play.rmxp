using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Geex.Edit
{
    /// <summary>
    /// Manage Game options
    /// </summary>
    public static class GameOptions
    {
        #region Game Option
        /// <summary>
        /// True is Screen Tone is cleaned at each frame - Standard RMXP = true
        /// </summary>
        public const bool IsScreenToneCleanedAtEveryFrame = false;

        /// <summary>
        /// If True change Character sprite shifting value
        /// </summary>
        public const bool IsArpgCharacterOn = false;

        /// <summary>
        /// True if Transition needs to be applied after each transfer - Standard RMXP = false
        /// </summary>
        public const bool IsTransitioningAfterEachTransfer = false;

        /// <summary>
        /// True is Picture must be deleted after transfer - Standard RMXP = false
        /// </summary>
        public const bool IsDeletingPicturesAfterTransfer = false;

        /// <summary>
        /// True is weather must be deleted after transfer - Standard RMXP = false
        /// </summary>
        public const bool IsDeletingWeatherAfterTransfer = false;

		/// <summary>
        /// True if the player moves tile by tile - Standard RMXP = true
        /// </summary>
        public const bool IsTileByTileMoving = true;
		
        /// <summary>
        /// True is Mask collision in on, if false then Tile passability applies - Standard RMXP = false
        /// </summary>
        public const bool IsCollisionMaskOn = false;

        /// <summary>
        /// True if player can slide around small colliding map masks - Standard RMXP = false
        /// </summary>
        public const bool IsSlidingIfColliding = false;

		/// <summary>
        /// True if antilag is on by default - Standard RMXP = false
        /// </summary>
        public const bool IsAntilagOnByDefault = false;
		
        /// <summary>
        /// Width of Game Player in pixel (ie 12)
        /// </summary>
        public static int GamePlayerWidth = 12;
        /// <summary>
        /// Height of Game Player in pixel (ie 12)
        /// </summary>
        public static int GamePlayerHeight = 12;

        /// <summary>
        /// Size of Icon Width and height
        /// </summary>
        public const int IconSize = 24;

        /// <summary>
        /// Map scroll speed diviser - Standard RMXP = 3.0
        /// </summary>
        public const double MapScrollSpeedDiviser = 3.0;
        #endregion

        #region Event Options
        /// <summary>
        /// Max number of branch in conditional events
        /// </summary>
        public const int MaxNumberfOfBranch = 10;
        #endregion

        #region Main Options
        /// <summary>
        /// Max Number of save files
        /// </summary>
        public const short NumberOfSaveFile = 4;

        /// <summary>
        /// Default WindowSkin
        /// </summary>
        public static string WindowskinName = "genese";

        /// <summary>
        /// True is the Tone value of the screen needs to be cleaned at every frame. False allows you to set the tone value once for all through an event of your map
        /// </summary>
        public const bool ResetToneAtEveryFrame = false;

        /// <summary>
        /// Difference between rmxp frame rate (40) and geex frame rate (60)
        /// </summary>
        public static float AdjustFrameRate = 1.5f;
        /// <summary>
        /// Number of move patterns in Character charset
        /// </summary>
        public const short CharacterPatterns = 4;
        /// <summary>
        /// Number of move directions in Character charset
        /// </summary>
        public const short CharacterDirections = 4;
        #endregion

        #region Database Options
        /// <summary>
        /// Max number of Picture on screen
        /// </summary>
        public const int NumberOfPictures = 51;
        #endregion

        #region Windows
        #region Scene Menu
        /// <summary>
        /// The commands list window (upper left)
        /// </summary>
        public const short MenuCommandListX = 0;
        public const short MenuCommandListY = 0;
        public const short MenuCommandListWidth = 160;

        /// <summary>
        /// Play Time window
        /// </summary>
        public const short MenuPlayTimeX = 0;
        public const short MenuPlayTimeY = 224;
        public const short MenuPlayTimeWidth = 160;
        public const short MenuPlayTimeHeight = 96;

        /// <summary>
        /// Step window
        /// </summary>
        public const short MenuStepX = 0;
        public const short MenuStepY = 320;
        public const short MenuStepWidth = 160;
        public const short MenuStepHeight = 96;

        /// <summary>
        /// Gold window
        /// </summary>
        public const short MenuGoldX = 0;
        public const short MenuGoldY = 416;
        public const short MenuGoldWidth = 160;
        public const short MenuGoldHeight = 64;

        /// <summary>
        /// Status window
        /// </summary>
        public const short MenuStatusX = 160;
        public const short MenuStatusY = 0;
        public const short MenuStatusWidth = 480;
        public const short MenuStatusHeight = 480;
        #endregion

        #region Message
        /// <summary>
        /// Message window
        /// </summary>
        public static Rectangle MessageWindowRect = new Rectangle(80, 300, 480, 160);
        /// <summary>
        /// Geex Edit: Message window Size
        /// </summary>
        public const int MessageFontSize = 16;
        /// <summary>
        /// Geex Edit: Message Text Color
        /// </summary>
        public static Color MessageTextColor = Color.White;
        /// <summary>
        /// Geex Edit: Message Gold window
        /// </summary>
        public static Rectangle MessageGold = new Rectangle(0, 582, 304, 90);
        public const short MessageGoldInBattleY = 240;
        #endregion
        #endregion
    }

}
