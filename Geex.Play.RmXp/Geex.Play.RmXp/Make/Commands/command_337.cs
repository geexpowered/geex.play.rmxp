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
        /// Show Battle Animation
        /// </summary>
		bool Command337()
        {
			// Process with iterator
            if (intParams[0]==0)
            {
                foreach(GameNpc battler in IterateEnemy(intParams[1]))
                {
                     if (battler.IsExist) battler.AnimationId=intParams[2];
                }
			 }
			 else
			 {
                 foreach (GameActor battler in IterateActor(intParams[1]))
			     {
                     if (battler.IsExist) battler.AnimationId = intParams[2];
			     }
			 }
            return true;
		}
    }
}

