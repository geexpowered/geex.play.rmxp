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
        /// Change Fog Color Tone
        /// </summary>
		bool Command205()
        {
			// Start Color tone change
            Tone tone = new Tone(intParams[0], intParams[1], intParams[2], intParams[3]);
			InGame.Map.StartFogToneChange(tone, intParams[4] * 2);
			// Continue
            return true;
		}
    }
}

