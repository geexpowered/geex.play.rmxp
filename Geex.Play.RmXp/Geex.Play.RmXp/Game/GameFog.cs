using System;
using System.Collections.Generic;
using Geex.Run;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// This class handles the Game Fogs.
    /// </summary>
    public partial class GameFog
    {
        #region Constants
        #endregion

        #region Variables
        /// <summary>
        /// Id referencing the fog
        /// </summary>
        public int FogId;
        /// <summary>
        /// Fog file name
        /// </summary>
        public string FogFile;
        /// <summary>
        /// Fog opacity
        /// </summary>
        public int FogOpacity;
        /// <summary>
        /// Fog x shift
        /// </summary>
        public int FogOx;
        /// <summary>
        /// Fog y shift
        /// </summary>
        public int FogOy;
        /// <summary>
        /// Fog blend type
        /// </summary>
        public short FogBlend;
        /// <summary>
        /// Fog frames pause in between 2 shifts
        /// </summary>
        public int FogPause;
        /// <summary>
        /// True if fog is refracting
        /// </summary>
        public bool IsFogRefracting;
        #endregion

        #region Properties
        #endregion

        #region Initialize

        /// <summary>
        /// Initializes with the Game Fog
        /// </summary>
        /// <param name="id">Id referencing the fog</param>
        /// <param name="file">Fog file name</param>
        /// <param name="opacity">Fog opacity</param>
        /// <param name="ox">Fog x shift</param>
        /// <param name="oy">Fog y shift</param>
        /// <param name="blend">Fog blend type</param>
        /// <param name="pause">Fog frames pause in between 2 shifts</param>
        /// <param name="isRefracting">True if fog is refracting</param>
        public GameFog(int id, string file, byte opacity, int ox, int oy, short blend,int pause,bool isRefracting)
        {
            FogId = id;
            FogFile = file;
            FogOpacity = opacity;
            FogOx = ox;
            FogOy = oy;
            FogBlend = blend;
            FogPause = pause;
            IsFogRefracting = isRefracting;
        }
        /// <summary>
        /// Empty constructor mandatory for game saving
        /// </summary>
        public GameFog()
        {
        }
        #endregion

        #region Methods
        #endregion

    }
}
