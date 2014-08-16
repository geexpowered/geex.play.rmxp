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
		/// Change Transparent Flag
		/// </summary>   
		bool Command208()
		{
			// Change player transparent flag
			InGame.Player.IsTransparent = (intParams[0] == 0);
			// Continue
			return true;
		}
	}
}

