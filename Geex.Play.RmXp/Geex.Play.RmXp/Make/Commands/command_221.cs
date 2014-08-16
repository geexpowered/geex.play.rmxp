using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Run;

namespace Geex.Play.Make
{
    /// <summary>
    /// This interpreter runs event commands. This class is used within the
    //  GameSystem class and the GameEvent class
    /// </summary>
    public sealed partial class Interpreter
    {
        /// <summary>
        /// Prepare for Transition
        /// </summary>	    
		bool Command221()
        {
			// If showing message window
			if (InGame.Temp.IsMessageWindowShowing) return false;
			// Prepare for Transition
			Graphics.Freeze();
			// Continue
            return true;
        }
    }
}

