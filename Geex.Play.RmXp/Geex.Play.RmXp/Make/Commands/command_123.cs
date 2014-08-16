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
        /// Control Self Switch
        /// </summary>
        bool Command123()
        {
            // If event ID is valid
            if (eventId > 0)
            {
                // Make a self switch key
                GameSwitch key = new GameSwitch(InGame.Map.MapId, eventId, stringParams[0]);
                // Change self switches
                InGame.System.GameSelfSwitches[key] = (intParams[0] == 0);
                // Refresh map
                InGame.Map.IsNeedRefresh = true;
            }
            return true;
        }
    }
}