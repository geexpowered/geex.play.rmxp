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
        /// Change Save Access
        /// </summary>
        bool Command134()
        {
            // Change save access flag
            InGame.System.IsSaveDisabled = (intParams[0] == 0);
            return true;
        }
    }
}