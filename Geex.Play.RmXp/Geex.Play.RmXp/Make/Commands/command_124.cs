using System;
using System.Collections.Generic;
using Geex.Run;
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
        /// Control Timer
        /// </summary>
        bool Command124()
        {
            // If started
            if (intParams[0] == 0)
            {
                InGame.System.Timer = intParams[1] * Graphics.FrameRate;
                InGame.System.IsTimerWorking = true;
            }
            // If stopped
            if (intParams[0] == 1)
            {
                InGame.System.IsTimerWorking = false;
            }
            // Continue
            return true;
        }
    }
}