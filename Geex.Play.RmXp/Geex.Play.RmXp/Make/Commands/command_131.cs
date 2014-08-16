﻿using System;
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
        /// Change Windowskin
        /// </summary>
        bool Command131()
        {
            // Change Windowskin file Name
            InGame.System.WindowskinName = stringParams[0];
            return true;
        }
    }
}