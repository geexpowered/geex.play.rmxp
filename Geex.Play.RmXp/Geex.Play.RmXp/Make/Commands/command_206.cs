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
        /// Change Fog Opacity
        /// </summary>
		bool Command206()
        {
			// Start opacity level change
			InGame.Map.StartFogOpacityChange((byte)intParams[0], intParams[1] * 2);
			// Continue
            return true;
		}
    }
}

