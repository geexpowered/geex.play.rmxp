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
        /// Change Battle End ME
        /// </summary>
        bool Command133()
        {
            // Change battle } ME
            InGame.System.BattleEndSongEffect = new AudioFile(stringParams[0], intParams[0], intParams[1]);
            return true;
  }
    }
}