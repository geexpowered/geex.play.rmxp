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
        /// Show Animation
        /// </summary>
        bool Command207()
        {
            // if Game Player
            if (intParams[0] == -1)
            {
                InGame.Player.AnimationId = intParams[1];
                return true;
            }
            else
            {
                GameCharacter character = GetCharacter(intParams[0]);
                // If no Character exists
                if (character == null)
                {
                    return true;
                }
                // Set Animation ID
                character.AnimationId = intParams[1];
                return true;
            }
        }
    }
}

