using Geex.Play.Rpg;
using Geex.Run;
using Microsoft.Xna.Framework;
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
		/// Change Screen Color Tone
		/// </summary>	    
		bool Command223()
		{
			// Start changing Color tone (duration 0 is set to duration 1)
            InGame.Screen.StartToneChange(new Tone(intParams[0], intParams[1], intParams[2], intParams[3]), intParams[4] == 0 ? 1 : intParams[4] * 2);
			return true;
		}
		
		/// <summary>
		/// Screen Flash
		/// </summary>
		 bool Command224()
		 {
            // Start flash (duration 0 is set to duration 1)
            InGame.Screen.StartFlash(new Color((byte)intParams[0], (byte)intParams[1], (byte)intParams[2], (byte)intParams[3]), intParams[4] == 0 ? 1 : intParams[4] * 2);
		    return true;
		}
		
		/// <summary>
		/// Screen Shake
		/// </summary>
		bool Command225()
		{
            // Start shake (duration 0 is set to duration 1)
            InGame.Screen.StartShake(intParams[0], intParams[1], intParams[2] == 0 ? 1 : intParams[2] * 2);
			return true;
		}
	}
}

