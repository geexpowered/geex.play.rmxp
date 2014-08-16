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
        /// Recover All
        /// </summary>    
		bool Command314()
        {
			// Process with iterator
			foreach (GameActor actor in IterateActor(intParams[0]))
            {
				// Recover all for actor
				actor.RecoverAll();
			}
            return true;
		}
    }
}

