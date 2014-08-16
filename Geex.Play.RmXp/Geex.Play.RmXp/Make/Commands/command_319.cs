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
		/// Change Equipment
		/// </summary>
		bool Command319()
		{
			// Get actor
			GameActor actor = InGame.Actors[intParams[0]-1];
			// Change Equipment
			if (actor != null)
			{
				actor.Equip(intParams[1], intParams[2]);
			}
			return true;
		}
	}
}

