using Geex.Edit;
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
		/// <summary>
		/// Show Picture
		/// </summary>	    
		bool Command231()
		{
			bool reComputePosition = false;      // because of resolution change
			int x;
			int y;
			// Get Picture number
			int number = intParams[0]/* + (InGame.Temp.IsInBattle ? 50 : 0)*/;
			// If appointment method is [direct appointment]
			if (intParams[2] == 0)
			{
				x = intParams[3];
				y = intParams[4];
			}
			// If appointment method is [appoint with variables]
			else
			{
				reComputePosition = false;      // If position chosen from variable do not change them
				x = InGame.Variables.Arr[intParams[3]];
				y = InGame.Variables.Arr[intParams[4]];
			}
			// Show Picture
            if (InGame.Temp.IsInBattle)
                InGame.Screen.BattlePictures[number].Show(stringParams[0], intParams[1], x, y, intParams[5], intParams[6], (byte)intParams[7], intParams[8], false, reComputePosition);
            else
			    InGame.Screen.Pictures[number].Show(stringParams[0], intParams[1], x, y, intParams[5], intParams[6], (byte)intParams[7], intParams[8], false,reComputePosition);
            return true;
		}

		/// <summary>
		/// Move Picture
		/// </summary>
		bool Command232()
		{
			int x;
			int y;
			bool reComputePosition = false;      // because of resolution change
			// Get Picture number
			int number = intParams[0] /*+ (InGame.Temp.IsInBattle ? 50 : 0)*/;
			// If appointment method is [direct appointment]
			if (intParams[3] == 0)
			{
				x = intParams[4];
				y = intParams[5];
			}
			// If appointment method is [appoint with variables]
			else
			{
				reComputePosition = false;      // If position chosen from variable do not change them
				x = InGame.Variables.Arr[intParams[4]];
				y = InGame.Variables.Arr[intParams[5]];
			}
			// Move Picture
            if (InGame.Temp.IsInBattle)
                InGame.Screen.BattlePictures[number].Move((int)(intParams[1] * 2 * GameOptions.AdjustFrameRate), intParams[2], x, y, intParams[6], intParams[7], (byte)intParams[8], intParams[9], reComputePosition);
            else
			    InGame.Screen.Pictures[number].Move((int)(intParams[1] * 2 * GameOptions.AdjustFrameRate), intParams[2], x, y, intParams[6], intParams[7], (byte)intParams[8], intParams[9],reComputePosition);
			return true;
		}

		/// <summary>
		/// Rotate Picture
		/// </summary>
		bool Command233()
		{
			// Get Picture number
			int number = intParams[0] /*+ (InGame.Temp.IsInBattle ? 50 : 0)*/;
			// Set rotation speed
            if (InGame.Temp.IsInBattle)
			    InGame.Screen.BattlePictures[number].Rotate(intParams[1]);
            else
                InGame.Screen.Pictures[number].Rotate(intParams[1]);
			return true;
		}

		/// <summary>
		/// Change Picture Color Tone
		/// </summary>
		bool Command234()
		{
			// Get Picture number
			int number = intParams[0] /*+ (InGame.Temp.IsInBattle ? 50 : 0)*/;
			// Start changing Color tone
            if (InGame.Temp.IsInBattle)
                InGame.Screen.BattlePictures[number].StartToneChange(new Tone(intParams[1], intParams[2], intParams[3], intParams[4]), intParams[5] * 2);
            else
			    InGame.Screen.Pictures[number].StartToneChange(new Tone(intParams[1],intParams[2],intParams[3],intParams[4]), intParams[5] * 2);
			return true;
		}

		/// <summary>
		/// Erase Picture
		/// </summary>	    
		bool Command235()
		{
			// Get Picture number
			int number = intParams[0] /*+ (InGame.Temp.IsInBattle ? 50 : 0)*/;
			// Erase Picture
            if (InGame.Temp.IsInBattle)
			    InGame.Screen.BattlePictures[number].Erase();
            else
                InGame.Screen.Pictures[number].Erase();
			return true;
		}
	}
}

