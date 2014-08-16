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
        /// Force Action
        /// </summary>
		bool Command339()
        {
			// Ignore if (not in battle
			if (!InGame.Temp.IsInBattle) return true;
			// Ignore if (number of turns = 0
			if (InGame.Temp.BattleTurn == 0) return true;
			// Process with iterator (For convenience, this process won't be repeated)
            // Npc
            if (intParams[0]==0)
            {
                return doForceAction(IterateEnemy(intParams[1]));
            }
            else
            {
                return doForceAction(IterateActor(intParams[1]));
            }
		}

        /// <summary>
        /// Apply Force Action on Enemy/NPC
        /// </summary>
        /// <param Name="list">List on Enemy/NPC</param>
        bool doForceAction(List<GameNpc> list)
        {
            for (int i=0;i<list.Count;i++)
            {
				// If Battler exists
                if (list[i].IsExist)
                {
					// Set action
                    list[i].CurrentAction.kind = intParams[2];
                    if (list[i].CurrentAction.kind == 0)
                    {
                        list[i].CurrentAction.basic = intParams[3];
                    }
					else
                    {
                        list[i].CurrentAction.SkillId = intParams[3];
					}
					// Set action target
					if (intParams[4] == -2)
                    {
                        list[i].CurrentAction.DecideLastTargetForEnemy();
                    }
					else if (intParams[4] == -1)
                    {
                        list[i].CurrentAction.DecideRandomTargetforEnemy();
					}
					else if (intParams[4] >= 0)
                    {
                        list[i].CurrentAction.TargetIndex = intParams[4];
					}
					// Set force flag
                    list[i].CurrentAction.IsForcing = true;
					// If action is valid and [run now]
                    if (list[i].CurrentAction.IsValid() && intParams[5] == 1)
                    {
						// Set Battler being forced into action
                        InGame.Temp.ForcingBattler = list[i];
						index += 1;
						return false;
					}
				}
			}
            return true;
        }

        /// <summary>
        /// Apply Force Action on Actors
        /// </summary>
        /// <param Name="list">List on Actors</param>
        bool doForceAction(List<GameActor> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
				// If Battler exists
				if (list[i].IsExist)
                {
					// Set action
                    list[i].CurrentAction.kind = intParams[2];
                    if (list[i].CurrentAction.kind == 0)
                    {
                        list[i].CurrentAction.basic = intParams[3];
                    }
					else
                    {
                        list[i].CurrentAction.SkillId = intParams[3];
					}
					// Set action target
					if (intParams[4] == -2)
                    {
                        list[i].CurrentAction.DecideLastTargetForActor();
                    }
					else if (intParams[4] == -1)
                    {
                        list[i].CurrentAction.DecideRandomTargetForActor();
					}
					else if (intParams[4] >= 0)
                    {
                        list[i].CurrentAction.TargetIndex = intParams[4];
					}
					// Set force flag
                    list[i].CurrentAction.IsForcing = true;
					// If action is valid and [run now]
                    if (list[i].CurrentAction.IsValid() && intParams[5] == 1)
                    {
						// Set Battler being forced into action
                        InGame.Temp.ForcingBattler = list[i];
						index += 1;
						return false;
					}
				}
			}
            return true;
        }
    }
}

