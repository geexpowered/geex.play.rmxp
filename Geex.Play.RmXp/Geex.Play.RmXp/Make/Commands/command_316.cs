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
		/// Change Level
		/// </summary>
		bool Command316()
		{
			// Get operate value
			int value = OperateValue(intParams[1], intParams[2], intParams[3]);
			// Process with iterator
			foreach (GameActor actor in IterateActor(intParams[0]))
			{
				// Change actor level
				actor.Level += value;
			}
			return true;
		}
	}
}

