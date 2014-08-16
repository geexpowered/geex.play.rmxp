using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Abort Battle
        /// </summary>
		bool Command340()
        {
			// Set battle abort flag
			InGame.Temp.BattleAbort = true;
			index += 1;
            return false;
        }
    }
}

