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
        /// Change Menu Access
        /// </summary>
        bool Command135()
        {
            // Change menu access flag
            InGame.System.IsMenuDisabled = (intParams[0] == 0);
            return true;
        }
    }
}