using System;
using System.Collections.Generic;
using Geex.Play.Rpg;
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
        /// Change Gold
        /// </summary>
        bool Command125()
        {
            // Get value to operate
            int value = OperateValue(intParams[0], intParams[1], intParams[2]);
            // Increase / decrease amount of gold
            InGame.Party.GainGold(value);
            return true;
        }
    }
}