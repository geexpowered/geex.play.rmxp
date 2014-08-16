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
		/// Change Skills
		/// </summary>
		bool Command318()
		{
			// Get actor
			GameActor actor = InGame.Actors[intParams[0]-1];
			// Change skill
			if (actor != null)
			{
				if (intParams[1] == 0)
				{
					actor.LearnSkill(intParams[2]);
				}
				else
				{
					actor.ForgetSkill(intParams[2]);
				}
			}
			return true;
		}
	}
}

