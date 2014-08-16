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
        /// Change State
        /// </summary>
		bool Command313()
        {
			// Process with iterator
			foreach (GameActor actor in IterateActor(intParams[0]))
            {
				// Change state
				if (intParams[1] == 0)
                {
					actor.AddState(intParams[2]);
                }
				else
                {
					actor.RemoveState(intParams[2]);
				}
			}
            return true;
		}
    }
}

