using System;
using System.Collections.Generic;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// Data stored as any integer (maximum 8 numbers long) throughout the game
    /// </summary>
    public struct GameVariable
    {
        int mapID;
        int eventID;
        int var;

        /// <summary>
        /// Special switch for individual events. Switch Default value if 'A'
        /// </summary>
        /// <param Name="map">Map ID of the switch</param>
        /// <param Name="ev">Event ID of the switch</param>
        /// <param Name="sw">Letter of the Switch (as a char)</param>
        public GameVariable(int map, int ev, int sw)
        {
            mapID = map;
            eventID = ev;
            var = sw;
        }
    }

    /// <summary>
    /// This class holds the game variables and has methods for assigning and getting data from variables
    /// </summary>
    public class GameVariables
    {
        #region Variables
        /// <summary>
        /// Game Variables Array
        /// </summary>
        public int[] Arr=new int[Data.System.Variables];
        #endregion

    }

}
