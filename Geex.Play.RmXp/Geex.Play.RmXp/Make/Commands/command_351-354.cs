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
		/// Call Menu Screen
		/// </summary>
		bool Command351()
		{
			// Set battle abort flag
			InGame.Temp.BattleAbort = true;
			// Set menu calling flag
			InGame.Temp.IsCallingMenu = true;
			index += 1;
			return false;
		}
		
		/// <summary>
		/// Call Save Screen
		/// </summary>
		bool Command352()
		{
			// Set battle abort flag
			InGame.Temp.BattleAbort = true;
			// Set save calling flag
			InGame.Temp.IsCallingSave = true;
			// Advance index
			index += 1;
			return false;
		}
		
		/// <summary>
		/// Game Over
		/// </summary>
		bool Command353()
		{
			// Set game over flag
			InGame.Temp.IsGameover = true;
			return false;
		}
		
		/// <summary>
		/// Return to Title Screen
		/// </summary>
		bool Command354()
		{
			// Set return to title screen flag
			InGame.Temp.ToTitle = true;
			return false;
		}
	}
}

