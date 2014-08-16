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
        /// Change Party Member
        /// </summary>
        bool Command129()
        {
            // Get actor
            GameActor actor = InGame.Actors[intParams[0]-1];
            // If actor is valid
            if (actor != null)
            {
                // Branch with control
                if (intParams[1] == 0)
                {
                    if (intParams[2] == 1)
                    {
                        InGame.Actors[intParams[0]-1].Setup(intParams[0]-1);
                    }
                    InGame.Party.AddActor(intParams[0]-1);
                }
                else
                {
                    InGame.Party.RemoveActor(intParams[0]-1);
                }
            }
            return true;
        }
    }
}