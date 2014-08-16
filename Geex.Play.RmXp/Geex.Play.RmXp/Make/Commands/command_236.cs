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
		/// Set Weather Effects
		/// </summary>	    
		bool Command236()
		{
			// Set Weather Effects
			InGame.Screen.Weather(intParams[0], intParams[1], intParams[2]);
			return true;
		}
	}
}

