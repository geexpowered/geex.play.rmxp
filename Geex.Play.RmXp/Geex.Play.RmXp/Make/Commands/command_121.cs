using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Control Switches
        /// </summary>
        bool Command121()
        {
            // Loop for group control
            for (int i=intParams[0];i<=intParams[1];i++)
            {
                // Change switch
                InGame.Switches.Arr[i] = (intParams[2] == 0);
            }
            // Refresh map
            InGame.Map.IsNeedRefresh = true;
            return true;
        }
    }
}