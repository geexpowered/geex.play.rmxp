using System;
using System.Collections.Generic;
using Geex.Play.Rpg.Game;
using Geex.Play.Rpg;

namespace Geex.Play.Make
{
	/// <summary>
	/// This interpreter runs event commands. This class is used within the
	//  GameSystem class and the GameEvent class
	/// </summary>
	public sealed partial class Interpreter
	{
		/// <summary>
		/// Change HP
		/// </summary>
		bool Command311()
		{
			// Get operate value
			int value = OperateValue(intParams[1], intParams[2], intParams[3]);
			// Process with iterator
			foreach(GameActor actor in IterateActor(intParams[0]))
			{
				// If HP are not 0
				if (actor.Hp > 0)
				{
					// Change HP (if (death is not permitted, make HP 1)
					if (intParams[4] == 0 && actor.Hp + value <= 0)
					{
						actor.Hp = 1;
					}
					else
					{
						actor.Hp += value;
					}
				}
			}
			// Determine game over
			InGame.Temp.IsGameover = InGame.Party.IsAllDead;
			return true;
		}
	}
}

