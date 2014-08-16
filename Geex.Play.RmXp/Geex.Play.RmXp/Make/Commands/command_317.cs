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
		/// Change Parameters
		/// </summary>	    
		bool Command317()
		{
			// Get operate value
			int value = OperateValue(intParams[2], intParams[3], intParams[4]);
			// Get actor
			GameActor actor = InGame.Actors[intParams[0]-1];
			// Change parameters
			if (actor != null)
			{
				switch (intParams[1])
				{
					case 0:    // MaxHp
                        actor.MaxhpPlus+=value;//actor.MaxHp += value;
						break;
					case 1:    // MaxSp
                        actor.MaxspPlus += value;
						break;
					case 2:    // strength
						actor.Str += value;
						break;
					case 3:    // dexterity
						actor.Dex += value;
						break;
					case 4:    // agility
						actor.Agi += value;
						break;
					case 5:    // intelligence
						actor.Intel += value;
						break;
				}
			}
			return true;
		}
	}
}

