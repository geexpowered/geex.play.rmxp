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
        /// Change Enemy HP
        /// </summary>
		bool Command331()
        {
			// Get operate value
			int value = OperateValue(intParams[1], intParams[2], intParams[3]);
			// Process with iterator
			foreach (GameNpc enemy in IterateEnemy(intParams[0]))
            {
				// If HP is not 0
				if (enemy.Hp > 0)
                {
					// Change HP (if (death is not permitted then change HP to 1)
					if (intParams[4] == 0 && enemy.Hp + value <= 0)
                    {
						enemy.Hp = 1;
                    }
					else
                    {
						enemy.Hp += value;
					}
				}
			}
			return true;
		}
	    
	    /// <summary>
	    /// Change Enemy SP
	    /// </summary>
		bool Command332()
        {
			// Get operate value
			int value = OperateValue(intParams[1], intParams[2], intParams[3]);
			// Process with iterator
			foreach (GameNpc enemy in IterateEnemy(intParams[0]))
            {
                // Change SP
				enemy.Sp += value;
			}
			return true;
		}
	    
	    /// <summary>
	    /// Change Enemy State
	    /// </summary>
		bool Command333()
        {
			// Process with iterator
			foreach (GameNpc enemy in IterateEnemy(intParams[0]))
            {
				// If [regard HP 0] state option is valid
				if (Data.States[intParams[2]].ZeroHp)
                {
					// Clear immortal flag
					enemy.IsImmortal = false;
				}
				// Change
				if (intParams[1] == 0)
                {
					enemy.AddState(intParams[2]);
                }
				else
                {
					enemy.RemoveState(intParams[2]);
				}
			}
			return true;
		}
	    
	    /// <summary>
	    /// Enemy Recover All
	    /// </summary>
		bool Command334()
        {
			// Process with iterator
			foreach (GameNpc enemy in IterateEnemy(intParams[0]))
            {
				// Recover all
				enemy.RecoverAll();
			}
			return true;
		}
	    
	    /// <summary>
	    /// Enemy Appearance
	    /// </summary>
		bool Command335()
        {
			// Get enemy
            GameNpc enemy = InGame.Troops.Npcs[InGame.Troops.Npcs.Count - 1 - intParams[0]];
			// Clear hidden flag
			if (enemy != null)
            {
				enemy.IsHidden = false;
			}
			return true;
		}
	    
	    /// <summary>
	    /// Enemy Transform
	    /// </summary>
		bool Command336()
        {
			// Get enemy
            GameNpc enemy = InGame.Troops.Npcs[InGame.Troops.Npcs.Count - 1 - intParams[0]];
			// Transform processing
			if (enemy != null)
            {
				enemy.Transform(intParams[1]);
			}
            return true;
		}
    }
}

