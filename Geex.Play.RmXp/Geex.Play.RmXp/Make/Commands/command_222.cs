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
		/// Execute Transition
		/// </summary>	    
		bool Command222()
		{
			// If Transition processing flag is already set
			if (InGame.Temp.IsProcessingTransition) return false;
			// Set Transition processing flag
            InGame.Temp.IsProcessingTransition = true;
			InGame.Temp.TransitionName = stringParams[0];
			// Advance index
			index += 1;
			// End
			return false;
		}
	}
}

