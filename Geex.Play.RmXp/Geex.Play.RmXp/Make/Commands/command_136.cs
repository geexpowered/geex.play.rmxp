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
        /// Change Encounter
        /// </summary>
        bool Command136()
        {
            // Change encounter flag
            InGame.System.IsEncounterDisabled = (intParams[0] == 0);
            // Make encounter count
            InGame.Player.MakeEncounterCount();
            return true;
        }
    }
}