using System;
using Geex.Play.Rpg;
using Geex.Play.Rpg.Game;
using Geex.Run;


namespace Geex.Play.Make
{
	/// <summary>
	/// This interpreter runs event commands. This class is used within the
	//  GameSystem class and the GameEvent class
	/// </summary>
	public sealed partial class Interpreter
	{
		const short EndOfCommandCode=-9999;
		/// <summary>
		/// Set Move Route
		/// </summary>   
		bool Command209()
		{
			if (intParams[0]==-1)
			{
				InGame.Player.ForceMoveRoute(getMoveRoute());
				return true;
			}
			// Get GameEvent
			GameCharacter character = GetCharacter(intParams[0]);
			// If no Character exists
			if (character == null) return true;
			// Force move route
			character.ForceMoveRoute(getMoveRoute());
			return true;
		}

		/// <summary>
		/// Uncode parameters to Get Move Route object
		/// </summary>
		MoveRoute getMoveRoute()
		{
			MoveRoute moveRoute = new MoveRoute();
			moveRoute.List = new MoveCommand[Math.Max(0, intParams.Length - 3)/5];
			moveRoute.Repeat = (intParams[1] == 1);
			moveRoute.Skippable = (intParams[2] == 1);
			int countString =0;
			int j = 0;
			for (int i = 3; i < intParams.Length; i += 5)
			{
				moveRoute.List[j++] = getMoveCommand(intParams[i], intParams[i + 1], intParams[i + 2], intParams[i + 3], intParams[i + 4], stringParams[countString++]);
			}
			return moveRoute;
		}

		/// <summary>
		///  Get Move Command
		/// </summary>
		MoveCommand getMoveCommand(int code, short param1, short param2, short param3, short param4, string param5)
		{
			MoveCommand moveCommand = new MoveCommand();
			moveCommand.Code = code;
			int countArraySize = (param1 != EndOfCommandCode ? 1 : 0) + (param2 != EndOfCommandCode ? 1 : 0) + (param3 != EndOfCommandCode ? 1 : 0) + (param4 != EndOfCommandCode ? 1 : 0);
			moveCommand.IntParams = new short[countArraySize];
			// Create String parameters
			if (param5 != "") moveCommand.StringParams = param5;
			// Create int parameters
			// -1 is the code for empty parameter
			countArraySize = 0;
			if (param1 != EndOfCommandCode) moveCommand.IntParams[countArraySize++]=param1;
			if (param2 != EndOfCommandCode) moveCommand.IntParams[countArraySize++] = param2;
			if (param3 != EndOfCommandCode) moveCommand.IntParams[countArraySize++] = param3;
			if (param4 != EndOfCommandCode) moveCommand.IntParams[countArraySize] = param4;
			return moveCommand;
		}
	}
}

