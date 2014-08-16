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
        /// Change Battle BGM
        /// </summary>
        bool Command132()
        {
            // Change battle BGM
            InGame.System.BattleSong = new AudioFile(stringParams[0], intParams[0], intParams[1]);
            return true;
        }
    }
}