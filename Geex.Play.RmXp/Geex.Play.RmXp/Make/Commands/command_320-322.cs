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
		/// Change Actor localName
		/// </summary>
		bool Command320()
		{
			// Get actor
			GameActor actor = InGame.Actors[intParams[0]-1];
			// Change Name
			if (actor != null)
			{
				actor.Name = stringParams[0];
			}
			return true;
		}
		
		/// <summary>
		/// Change Actor Class
		/// </summary>
		bool Command321()
		{
			// Get actor
			GameActor actor = InGame.Actors[intParams[0]-1];
			// Change class
			if (actor != null)
			{
				actor.ClassId = intParams[1];
			}
			return true;
		}
		
		/// <summary>
		/// Change Actor Graphic
		/// </summary>
		bool Command322()
		{
			// Get actor
			GameActor actor = InGame.Actors[intParams[0]-1];
			// Change graphic
			if (actor != null)
			{
				actor.SetGraphic(stringParams[0], intParams[1],stringParams[1], intParams[2]);
			}
			// Refresh player
			InGame.Player.Refresh();
			return true;
		}
	}
}

