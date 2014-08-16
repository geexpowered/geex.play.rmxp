using System;
using System.Collections.Generic;
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
		/// Wait for Move's Completion
		/// </summary>	    
		bool Command210()
		{
			// If not in battle
			if (!InGame.Temp.IsInBattle)
			{
				// Set move route completion waiting flag
				isWaitingMoveRoute = true;
			}
			// Continue
			return true;
		}
	}
}

