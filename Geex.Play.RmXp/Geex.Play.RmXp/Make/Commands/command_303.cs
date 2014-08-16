using Geex.Play.Rpg;
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
		/// localName Input Processing
		/// </summary>
		bool Command303()
		{
			// If not invalid actors
			if (Data.Actors[intParams[0]] != null)
			{
				// Set battle abort flag
				InGame.Temp.BattleAbort = true;
				// Set Name input calling flag
				InGame.Temp.IsCallingName = true;
				InGame.Temp.NameActorId = intParams[0]-1;
				InGame.Temp.NameMaxChar = intParams[1];
			}
			// Advance index
			index += 1;
			return false;
		}
	}
}

