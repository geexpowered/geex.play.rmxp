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
        /// Erase Event
        /// </summary>
        bool Command116()
        {
            // If event ID is valid
            if (eventId > 0)
            {
                // Erase event
                InGame.Map.Events[eventId].Erase();
            }
            // Advance index
            index += 1;
            return false;
        }
    }
}

