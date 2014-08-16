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
        /// Change Weapons
        /// </summary>
        bool Command127()
        {
            // Get value to operate
            int value = OperateValue(intParams[1], intParams[2], intParams[3]);
            // Increase / decrease weapons
            InGame.Party.GainWeapon(intParams[0], value);
            return true;
        }
    }
}