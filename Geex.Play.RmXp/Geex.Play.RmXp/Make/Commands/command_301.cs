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
		/// Battle Processing
		/// </summary>
		bool Command301()
		{
			// If not invalid troops
			if (Data.Troops[intParams[0]] != null)
			{
				// Set battle abort flag
				InGame.Temp.BattleAbort = true;
				// Set battle calling flag
				InGame.Temp.IsCallingBattle = true;
				InGame.Temp.BattleTroopId = intParams[0];
				InGame.Temp.IsBattleCanEscape = (intParams[1]==1);
				InGame.Temp.IsBattleCanLose = (intParams[2]==1);
				// Set callback
				int current_indent = list[index].Indent;
				InGame.Temp.BattleProc = new ProcInt(ProcAssignBranch);
			}
			// Advance index
			index += 1;
			// End
			return false;
		}    
	}
}

