using Geex.Play.Rpg;
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
        /// Call Common Event
        /// </summary>
        bool Command117()
        {
            // Get common event
            CommonEvent common_event = Data.CommonEvents[intParams[0]];
            // If common event is valid
            if (common_event != null)
            {
                // Make child interpreter
                childInterpreter = new Interpreter(depth + 1);
                childInterpreter.Setup(common_event.List, eventId);
            }
            // Continue
            return true;
        }
    }
}

