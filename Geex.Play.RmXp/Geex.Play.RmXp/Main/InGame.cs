using System;
using Geex.Play.Custom;
using Geex.Play.Rpg.Utils;

namespace Geex.Play.Rpg.Game
{
    /// <summary>
    /// Manage Game Global variables
    /// </summary>
    public static partial class InGame
    {
        /// <summary>
        /// In game Random number
        /// </summary>
        public static Random Rnd = new Random();
        /// <summary>
        /// In game actors
        /// </summary>
        public static GameActors Actors;
        /// <summary>
        /// In game Non playing characters
        /// </summary>
        public static GameBattler Npcs;
        /// <summary>
        /// In game party information
        /// </summary>
        public static GameParty Party; 
        /// <summary>
        /// In game player
        /// </summary>
        public static GamePlayer Player;
        /// <summary>
        /// In game screen information and effects
        /// </summary>
        public static GameScreen Screen;
        /// <summary>
        /// In game enemies
        /// </summary>
        public static GameTroop Troops;
        /// <summary>
        /// In game Map
        /// </summary>
        public static GameMap Map;
        /// <summary>
        /// In game system information
        /// </summary>
        public static GameSystem System = new GameSystem();
        /// <summary>
        /// In game system information
        /// </summary>
        public static GameTemp Temp = new GameTemp();
        /// <summary>
        /// In game Tags
        /// </summary>
        public static Tags Tags;
        /// <summary>
        /// In game switches
        /// </summary>
        public static GameSwitches Switches = new GameSwitches();
        /// <summary>
        /// In game variables
        /// </summary>
        public static GameVariables Variables = new GameVariables();
        public static BattlerStateComparer StateComparer = new BattlerStateComparer();
        public static BattlerSpeedComparer SpeedComparer = new BattlerSpeedComparer();
    }

}