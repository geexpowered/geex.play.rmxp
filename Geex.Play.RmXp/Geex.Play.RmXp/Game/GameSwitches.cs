using System;
using System.Collections.Generic;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// Special switch for game switch
    /// </summary>
    public struct GameSwitch
    {
        /// <summary>
        /// Mad Id where the event's self switch stands
        /// </summary>
        public int MapID;
        /// <summary>
        /// Event Id where the event's self switch stands
        /// </summary>
        public int EventID;
        /// <summary>
        /// Switch Letter of self switch
        /// </summary>
        public string Switch;

        /// <summary>
        /// Special switch for individual events. Switch Default value if 'A'
        /// </summary>
        /// <param Name="map">Map ID of the switch</param>
        /// <param Name="ev">Event ID of the switch</param>
        /// <param Name="sw">Letter of the Switch (as a string)</param>
        public GameSwitch(int map, int ev, string sw)
        {
            MapID = map;
            EventID = ev;
            Switch = sw;
        }
        /// <summary>
        /// Special switch for individual events. Switch Default value if 'A'
        /// </summary>
        /// <param Name="map">Map ID of the switch</param>
        /// <param Name="ev">Event ID of the switch</param>
        public GameSwitch(int map, int ev)
        {
            MapID = map;
            EventID = ev;
            Switch = "A";
        }
    }

    /// <summary>
    /// This class holds the game switches and has methods for assigning and getting data from switches
    /// </summary>
    public class GameSwitches
    {
        #region Variables
        /// <summary>
        /// Game Switches Array
        /// </summary>
        public bool[] Arr = new bool[Data.System.Switches];
        #endregion
    }
}

