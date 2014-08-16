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
        /// Scroll Map
        /// </summary>        
        bool Command203()
        {
            // If in battle
            if (InGame.Temp.IsInBattle) return true;
            // If already scrolling
            if (InGame.Map.IsScrolling) return false;
            // Start scroll
            InGame.Map.StartScroll(intParams[0], intParams[1], intParams[2]);
            return true;
        }
    }
}

