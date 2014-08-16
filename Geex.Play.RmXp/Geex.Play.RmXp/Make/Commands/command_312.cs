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
        /// Change SP
        /// </summary>
		bool Command312()
        {
			// Get operate value
			int value = OperateValue(intParams[1], intParams[2], intParams[3]);
			// Process with iterator
			foreach (GameActor actor in IterateActor(intParams[0]))
            {
				// Change actor SP
				actor.Sp += value;
			}
            return true;
		}
    }
}

