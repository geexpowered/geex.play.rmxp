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
        /// Deal Damage
        /// </summary>
		bool Command338()
        {
			// Get operate value
			int value = OperateValue(0, intParams[2], intParams[3]);
			// Process with iterator
            // Npc 
            if (intParams[0]==0)
            {
                foreach (GameNpc npc in IterateEnemy(intParams[0]))
                {
                    dealDamage(value,npc);
                }
            }
            else
            {
                foreach (GameActor actor in IterateActor(intParams[0]))
                {
                    dealDamage(value,actor);
                }
            }
            return true;
        }

        /// <summary>
        /// Deal Damage for Npc
        /// </summary>
        /// <param Name="Battler">Npc</param>
        void dealDamage(int value,GameNpc battler)
        {
            // If Battler exists
            if (battler.IsExist)
            {
                // Change HP
                battler.Hp -= value;
                // If in battle
                if (InGame.Temp.IsInBattle)
                {
                    // Set damage
                    battler.Damage = value.ToString();
                    battler.IsDamagePop = true;
                }
            }
        }

        /// <summary>
        /// Deal Damage for Actor
        /// </summary>
        /// <param Name="Battler">Actor</param>
        void dealDamage(int value, GameActor battler)
        {
            // If Battler exists
			if (battler.IsExist)
            {
                // Change HP
				battler.Hp -= value;
                // If in battle
				if (InGame.Temp.IsInBattle)
                {
                    // Set damage
					battler.Damage = value.ToString();
					battler.IsDamagePop = true;
                }
            }
		}
    }
}

