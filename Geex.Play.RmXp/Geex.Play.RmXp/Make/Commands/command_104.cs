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
        /// Change Text Options
        /// </summary>
        bool Command104()
        {
            // If message is showing
            if (InGame.Temp.IsMessageWindowShowing) return false;
            // Change each option
            InGame.System.MessagePosition = intParams[0];
            InGame.System.MessageFrame = intParams[1];
            // Continue
            return true;
        }
    }
}

